import request from '@/utils/request'

// 查询操作日志列表
export function list(query) {
    return request({
        url: '/monitor/oper-log',
        method: 'get',
        params: query
    })
}

// 删除操作日志
export function delOperlog(operId) {
    return request({
        url: '/monitor/oper-log',
        method: 'delete',
        data: "string" == typeof (operId) ? [operId] : operId
    })
}
