//存储全局公共的数据
const usePublicStore = defineStore('public', {
    state: () => ({
        lang: localStorage.getItem("lang") ?? 'zh',
    }),
    getters: {},
    actions: {},
})
export default usePublicStore