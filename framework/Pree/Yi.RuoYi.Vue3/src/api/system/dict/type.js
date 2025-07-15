import request from '@/utils/request'

// 查询字典类型列表
export function listType(query) {
    return request({
        url: '/system/dictionary-type',
        method: 'get',
        params: query
    })
}

// 查询字典类型详细
export function getType(dictId) {
    return request({
        url: '/system/dictionary-type/' + dictId,
        method: 'get'
    })
}

// 新增字典类型
export function addType(data) {
    return request({
        url: '/system/dictionary-type',
        method: 'post',
        data: data
    })
}

// 修改字典类型
export function updateType(data) {
    return request({
        url: `/system/dictionary-type/${data.id}`,
        method: 'put',
        data: data
    })
}

// 删除字典类型
export function delType(dictId) {
    return request({
        url: `/system/dictionary-type`,
        method: 'delete',
        params: {id: dictId}
    })
}

// 获取字典选择框列表
export function optionselect() {
    return request({
        url: '/system/dictionary-type',
        method: 'get'
    })
}
