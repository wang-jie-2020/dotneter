import {createI18n} from 'vue-i18n'
import {getLanguageMessages} from '@/api/system/language'
import zh from './lang/zh'
import en from './lang/en'

const localMessages = {
    en,
    zh,
}

const fallbackLocale = 'en'
const loadedLocales = new Set()

export function getDefaultLocale() {
    return localStorage.getItem('lang') || 'zh'
}

function getFallbackMessageKey(locale) {
    const currentLocale = (locale || '').toLowerCase()
    if (currentLocale.startsWith('zh')) {
        return 'zh'
    }
    if (currentLocale.startsWith('en')) {
        return 'en'
    }
    return fallbackLocale
}

function normalizeLocaleMessages(payload) {
    if (!payload) {
        return {}
    }
    if (Array.isArray(payload)) {
        return payload.reduce((result, item) => {
            const key = item?.key ?? item?.name ?? item?.code
            const value = item?.value ?? item?.text ?? item?.content
            if (key !== undefined) {
                result[key] = value ?? ''
            }
            return result
        }, {})
    }
    if (payload.data && typeof payload.data === 'object' && !Array.isArray(payload.data)) {
        return normalizeLocaleMessages(payload.data)
    }
    if (payload.items && Array.isArray(payload.items)) {
        return normalizeLocaleMessages(payload.items)
    }
    if (payload.resources && typeof payload.resources === 'object' && !Array.isArray(payload.resources)) {
        return payload.resources
    }
    if (payload.texts && typeof payload.texts === 'object' && !Array.isArray(payload.texts)) {
        return payload.texts
    }
    if (typeof payload === 'object') {
        return payload
    }
    return {}
}

function buildLocaleMessages(locale, remoteMessages = {}) {
    const fallbackKey = getFallbackMessageKey(locale)
    return {
        ...(localMessages[fallbackKey] || {}),
        ...remoteMessages,
    }
}

const i18n = createI18n({
    locale: getDefaultLocale(),
    fallbackLocale,
    messages: {},
    legacy: false,
})

export async function loadLocaleMessages(locale = getDefaultLocale(), force = false) {
    const currentLocale = locale || getDefaultLocale()
    if (!force && loadedLocales.has(currentLocale)) {
        return i18n.global.getLocaleMessage(currentLocale)
    }

    let remoteMessages = {}
    try {
        const response = await getLanguageMessages(currentLocale)
        remoteMessages = normalizeLocaleMessages(response?.data)
    } catch {
        remoteMessages = {}
    }

    const messages = buildLocaleMessages(currentLocale, remoteMessages)
    i18n.global.setLocaleMessage(currentLocale, messages)
    loadedLocales.add(currentLocale)
    return messages
}

export async function setI18nLanguage(locale) {
    const currentLocale = locale || getDefaultLocale()
    await loadLocaleMessages(currentLocale)
    i18n.global.locale.value = currentLocale
    localStorage.setItem('lang', currentLocale)
    return currentLocale
}

export async function setupI18n(app) {
    const currentLocale = getDefaultLocale()
    await loadLocaleMessages(currentLocale)
    i18n.global.locale.value = currentLocale
    app.use(i18n)
    return i18n
}

const elementPlusLocaleLoaders = {
    zh: () => import('element-plus/dist/locale/zh-cn.mjs'),
    en: () => import('element-plus/dist/locale/en.mjs'),
}

export async function getElementPlusLocale(locale = getDefaultLocale()) {
    const fallbackKey = getFallbackMessageKey(locale)
    const loader = elementPlusLocaleLoaders[fallbackKey] || elementPlusLocaleLoaders.en
    const module = await loader()
    return module.default
}

export default i18n
