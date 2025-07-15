import {defineConfig, loadEnv} from 'vite'
import path from 'path'
import createVitePlugins from './vite/plugins'

// https://vitejs.dev/config/
export default defineConfig(({mode, command}) => {
    const env = loadEnv(mode, process.cwd())
    const {VITE_APP_ENV, VITE_APP_BASE_URL, VITE_APP_BASE_URL_WS} = env
    return {
        // 部署生产环境和开发环境下的URL。
        // 默认情况下，vite 会假设你的应用是被部署在一个域名的根路径上
        // 例如 https://www.ruoyi.vip/。如果应用被部署在一个子路径上，你就需要用这个选项指定这个子路径。例如，如果你的应用被部署在 https://www.ruoyi.vip/admin/，则设置 baseUrl 为 /admin/。
        base: VITE_APP_ENV === 'production' ? '/' : '/',
        plugins: createVitePlugins(env, command === 'build'),
        resolve: {
            // https://cn.vitejs.dev/config/#resolve-alias
            alias: {
                // 设置路径
                '~': path.resolve(__dirname, './'),
                // 设置别名
                '@': path.resolve(__dirname, './src')
            },
            // https://cn.vitejs.dev/config/#resolve-extensions
            extensions: ['.mjs', '.js', '.ts', '.jsx', '.tsx', '.json', '.vue']
        },
        // vite 相关配置
        server: {
            port: 18000,
            host: true,
            open: true,


            proxy: {
                // https://cn.vitejs.dev/config/#server-proxy
                '/dev-api': {
                    target: VITE_APP_BASE_URL,
                    changeOrigin: true,
                    rewrite: (p) => p.replace(/^\/dev-api/, ''),
                },

                '/dev-ws': {
                    target: VITE_APP_BASE_URL_WS,
                    changeOrigin: true,
                    rewrite: (p) => p.replace(/^\/dev-ws/, ''),
                    ws: true
                }

            }
        },
        //fix:error:stdin>:7356:1: warning: "@charset" must be the first rule in the file
        css: {
            postcss: {
                plugins: [
                    {
                        postcssPlugin: 'internal:charset-removal',
                        AtRule: {
                            charset: (atRule) => {
                                if (atRule.name === 'charset') {
                                    atRule.remove();
                                }
                            }
                        }
                    }
                ]
            }
        },

        // 20250715 之后再整理过
        build: {
            outDir: 'dist', //指定打包输出路径
            assetsDir: 'assets', //指定静态资源存放路径
            cssCodeSplit: true, //css代码拆分,禁用则所有样式保存在一个css里面
            // minify: 'terser',// 混淆器，默认esbuild 比 terser 快 20-40 倍，压缩率只差 1%-2%
            chunkSizeWarningLimit: 800, // chunk 大小警告的限制
            // terserOptions: {
            //     compress: {
            //         // minify为terser才有效  生产环境取消 console、debugger
            //         drop_console: true,
            //         drop_debugger: true
            //     }
            // },
            // esbuild: {
            //   drop: ['console', 'debugger']
            // },
            //会打包出 css js 等文件夹 目录显得清晰
            rollupOptions: {
                output: {
                    chunkFileNames: 'js/[name]-[hash].js',
                    entryFileNames: 'js/[name]-[hash].js',
                    assetFileNames: '[ext]/[name]-[hash].[ext]'
                }
            }
        }
    }
})
