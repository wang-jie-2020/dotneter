import request from '@/utils/request'

// 查询语言列表
export function listLanguage(query) {
    return request({
        url: '/system/language',
        method: 'get',
        params: query
    })
}

// 查询语言资源
export function getLanguageMessages(locale) {
    return request({
        url: '/system/language/messages',
        method: 'get',
        params: {culture: locale}
    })
}

// 查询语言详细
export function getLanguage(id) {
    return request({
        url: `/system/language/${id}`,
        method: 'get'
    })
}

// 新增语言
export function addLanguage(data) {
    return request({
        url: '/system/language',
        method: 'post',
        data: data
    })
}

// 修改语言
export function updateLanguage(id, data) {
    return request({
        url: `/system/language/${id}`,
        method: 'put',
        data: data
    })
}

// 删除语言
export function delLanguage(ids) {
    return request({
        url: '/system/language',
        method: 'delete',
        params: {id: ids}
    })
}
