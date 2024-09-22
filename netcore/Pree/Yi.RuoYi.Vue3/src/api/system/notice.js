import request from '@/utils/request'

// 查询公告列表
export function listNotice(query) {
  return request({
    url: '/system/notice',
    method: 'get',
    params: query
  })
}

// 查询公告详细
export function getNotice(id) {
  return request({
    url: `/system/notice/${id}`,
    method: 'get'
  })
}

// 新增公告
export function addNotice(data) {
  return request({
    url: '/system/notice',
    method: 'post',
    data: data
  })
}

// 修改公告
export function updateNotice(id,data) {
  return request({
    url: `/system/notice/${id}`,
    method: 'put',
    data: data
  })
}

// 删除公告
export function delNotice(ids) {
  return request({
    url: `/system/notice`,
    method: 'delete',
    params:{id:ids}
  })
}

// 发送在线公告
export function sendOnlineNotice(id) {
  return request({
    url: '/system/notice/online/'+id,
    method: 'post',
  })
}
// 发送离线公告
export function sendOfflineNotice(id) {
  return request({
    url: '/system/notice/offline/'+id,
    method: 'post',
  })
}