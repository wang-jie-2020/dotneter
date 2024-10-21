<template>

</template>

<script setup>
import axios from 'axios';
import { reactive } from 'vue';
import request from '@/utils/request'

const { proxy } = getCurrentInstance();

// const props = defineProps({
//     id: {
//         type: String,
//         defalut: 'mini-profiler'
//     },
//     scriptSrc: {
//         type: String,
//         defalut: ''
//     },
//     cssSrc: {
//         type: String,
//         defalut: ''
//     },
//     dataPath: {
//         type: String,
//         defalut: ''
//     },
//     dataVersion: {
//         type: String,
//         defalut: ''
//     },
//     dataPosition: {
//         type: String,
//         defalut: 'right'
//     },
//     dataChildren: {
//         type: Boolean,
//         defalut: 'true'
//     },
//     dataMaxTraces: {
//         type: Number,
//         default: 35,
//     },
//     dataAuthorized: {
//         type: Boolean,
//         defalut: 'true'
//     },
//     dataStartHidden: {
//         type: String,
//         defalut: 'false'
//     },
//     dataToggleShortcut: {
//         type: String,
//         defalut: 'Alt+P'
//     },
//     dataTrivialMilliseconds: {
//         type: Number,
//         default: 35,
//     },
//     dataTrivial: {
//         type: Boolean,
//         defalut: 'true'
//     },
//     dataControls: {
//         type: Boolean,
//         defalut: 'true'
//     },
//     dataCurrentId: {
//         type: String,
//         defalut: ''
//     },
//     dataIds: {
//         type: String,
//         defalut: ''
//     },
//     scriptAsync: {
//         type: Boolean,
//         defalut: 'true'
//     },
//     innerHTML: {
//         type: String,
//         defalut: ''
//     },
// });

const profiler = reactive({
    id: 'mini-profiler',
    src: 'http://localhost:19001/profiler/',
    scriptSrc: 'http://localhost:19001/profiler/includes.min.js?v=4.3.8+1120572909',
    cssSrc: 'http://localhost:19001/profiler/includes.min.css?v=4.3.8+1120572909',
    dataPath: '',
    dataVersion: '',
    dataPosition: 'right',
    dataChildren: true,
    dataMaxTraces: 35,
    dataAuthorized: false,
    dataStartHidden: 'false',
    dataToggleShortcut: 'Alt+P',
    dataTrivialMilliseconds: 35,
    dataTrivial: true,
    dataControls: true,
    dataCurrentId: '',
    dataIds: '',
    scriptAsync: true,
    innerHTML: '',
});

function axiosSetUp() {
    const key = 'MiniProfiler';
    // axios.interceptors.response.use(function success(config) {
    //     const miniProfiler = window[key];
    //     const miniProfilerIds = JSON.parse(config.headers['x-miniprofiler-ids']) as string[];
    //     miniProfiler.fetchResults(miniProfilerIds);
    //     return config;
    // }, function bug(error) {
    //     return Promise.reject(error);
    // });

    request.interceptors.response.use(res => {
        console.log(res);
        const miniProfiler = window[key];
        const miniProfilerIds = JSON.parse(res.headers['x-miniprofiler-ids']);
        miniProfiler.fetchResults(miniProfilerIds);
        return Promise.resolve(res);
    },
        error => {


            return Promise.reject(error)
        }
    )

    // profiler.src = "http://localhost:5000/mini-profiler-resources";
    // profiler.scriptSrc = 'http://localhost:19001/profiler/includes.min.js?v=4.3.8+1120572909';
    // profiler.cssSrc = 'http://localhost:19001/profiler/includes.min.css?v=4.3.8+1120572909';

    console.log(profiler.src);
};

function appendDivElement() {
    const body = document.head; //as HTMLDivElement;
    const script1 = document.createElement('script');

    // script1.innerHTML = proxy.value.innerHTML;
    // script1.src = profiler.scriptSrc;
    // script1.setAttribute('data-version', profiler.dataVersion);
    // script1.setAttribute('data-path', profiler.dataPath);
    // script1.setAttribute('data-position', profiler.dataPosition);
    //script1.setAttribute('id', profiler.id);
    //script1.setAttribute('data-ids', "");
    // script1.setAttribute('data-current-id', profiler.dataCurrentId);
    // script1.setAttribute('data-ids', profiler.dataIds);
    // script1.setAttribute('data-trivial', profiler.dataTrivial);
    // script1.setAttribute('data-children', profiler.dataChildren);
    // script1.setAttribute('data-max-traces', profiler.dataMaxTraces);
    // script1.setAttribute('data-controls', profiler.dataControls);
    // script1.setAttribute('data-authorized', profiler.dataAuthorized);
    // script1.setAttribute('data-start-hidden', profiler.dataStartHidden);
    // script1.setAttribute('data-toggle-shortcut', profiler.dataToggleShortcut);
    // script1.setAttribute('data-trivial-milliseconds', profiler.dataTrivialMilliseconds);
    // script1.async = profiler.scriptAsync;

    //body.appendChild(script1);

    var s = document.createElement("script");
    s.id = "mini-profiler";
    s.async = "async";
    s.src = "http://localhost:19001/profiler/includes.min.js?v=4.3.8+1120572909";
    s.setAttribute('data-version', "4.3.8+1120572909");
    s.setAttribute('data-path', "http://localhost:19001/profiler/");
    s.setAttribute('data-position', "Right");
    s.setAttribute('data-scheme', "Light");
    s.setAttribute('data-authorized', "true");
    s.setAttribute('data-trivial', "2.0");
    s.setAttribute('data-trivial-milliseconds', "2.0");
    s.setAttribute('data-ignored-duplicate-execute-types', "Open,OpenAsync,Close,CloseAsync");
    s.setAttribute('data-max-traces',15);
    document.head.appendChild(s);
}

function appendCssLink() {
    const css = document.createElement('link');
    css.href = profiler.cssSrc;
    css.rel = 'stylesheet';

    document.head.appendChild(css);
}

axiosSetUp();
appendDivElement();
appendCssLink();

// const script2 = document.createElement('script');
//     script2.innerHTML = "window.MiniProfiler.listInit({path: '/profiler/', version: '4.3.8+1120572909', colorScheme: 'Light'});";
//     document.body.appendChild(script2);

</script>