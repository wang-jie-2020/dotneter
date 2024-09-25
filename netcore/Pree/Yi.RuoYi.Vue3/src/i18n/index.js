// index.js
import { createI18n } from 'vue-i18n'
import zh from './lang/zh'
import en from './lang/en'

const messages = {
  en,
  zh,
}
const i18n = createI18n({
  locale: localStorage.getItem('lang') ||  'zh',
  fallbackLocale: 'zh', // 设置备用语言
  messages,
  legacy:false,
})

export default i18n