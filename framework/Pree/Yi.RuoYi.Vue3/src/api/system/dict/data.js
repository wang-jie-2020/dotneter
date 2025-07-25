import request from '@/utils/request'

// 查询字典数据列表
export function listData(query) {
    return request({
        url: '/system/dictionary',
        method: 'get',
        params: query
    })
}

// 查询字典数据详细
export function getData(dictCode) {
    return request({
        url: '/system/dictionary/' + dictCode,
        method: 'get'
    })
}

// 根据字典类型查询字典数据信息
export function getDicts(dictType) {
    return request({
        url: '/system/dictionary/dic-type/' + dictType,
        method: 'get'
    })
}

// 新增字典数据
export function addData(data) {
    return request({
        url: '/system/dictionary',
        method: 'post',
        data: data
    })
}

// 修改字典数据
export function updateData(data) {
    return request({
        url: `/system/dictionary/${data.id}`,
        method: 'put',
        data: data
    })
}

// 删除字典数据
export function delData(dictCode) {
    console.log(dictCode, "dictCode")
    return request({
        url: `/system/dictionary`,
        method: 'delete',
        params: {id: dictCode}
    })
}
