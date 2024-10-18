<template>
    <div v-html="divContent"></div>
</template>

<script setup>
import axios from 'axios';

const props = defineProps({
    id: {
        type: String,
        defalut: 'mini-profiler'
    },
    scriptSrc: {
        type: String,
        defalut: ''
    },
    cssSrc: {
        type: String,
        defalut: ''
    },
    dataPath: {
        type: String,
        defalut: ''
    },
    dataVersion: {
        type: String,
        defalut: ''
    },
    dataPosition: {
        type: String,
        defalut: 'right'
    },
    dataChildren: {
        type: Boolean,
        defalut: 'true'
    },
    dataMaxTraces: {
        type: Number,
        default: 35,
    },
    dataAuthorized: {
        type: Boolean,
        defalut: 'true'
    },
    dataStartHidden: {
        type: String,
        defalut: 'false'
    },
    dataToggleShortcut: {
        type: String,
        defalut: 'Alt+P'
    },
    dataTrivialMilliseconds: {
        type: Number,
        default: 35,
    },
    dataTrivial: {
        type: Boolean,
        defalut: 'true'
    },
    dataControls: {
        type: Boolean,
        defalut: 'true'
    },
    dataCurrentId: {
        type: String,
        defalut: ''
    },
    dataIds: {
        type: String,
        defalut: ''
    },
    scriptAsync: {
        type: Boolean,
        defalut: 'true'
    },
    innerHTML: {
        type: String,
        defalut: ''
    },
});

const profiler = {

}

const divContent = ref('');

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

    axios.interceptors.response.use(res => {
        const miniProfiler = window[key];
        const miniProfilerIds = JSON.parse(res.headers['x-miniprofiler-ids']) as string[];
        miniProfiler.fetchResults(miniProfilerIds);
        return Promise.resolve(res);
    },
        error => {


            return Promise.reject(error)
        }
    )

    profiler.src = "http://localhost:5000/mini-profiler-resources";
    profiler.scriptSrc = 'http://localhost:19001/profiler/includes.min.js?v=4.3.8+1120572909';
    profiler.cssSrc = 'http://localhost:19001/profiler/includes.min.css?v=4.3.8+1120572909';

    console.log(profiler.src);
};

function appendDivElement() {

    divContent.value = document.querySelector('div').innerHTML;

    const body = document.body; //as HTMLDivElement;
    const script = document.createElement('script');
    //script.innerHTML = this.innerHTML;
    script.src = profiler.scriptSrc;
    script.setAttribute('data-version', props.dataVersion);
    script.setAttribute('data-path', props.dataPath);
    script.setAttribute('data-position', props.dataPosition);
    script.setAttribute('id', props.id);
    script.setAttribute('data-current-id', props.dataCurrentId);
    script.setAttribute('data-ids', props.dataIds);
    script.setAttribute('data-trivial', props.dataTrivial);
    script.setAttribute('data-children', props.dataChildren);
    script.setAttribute('data-max-traces', props.dataMaxTraces);
    script.setAttribute('data-controls', props.dataControls);
    script.setAttribute('data-authorized', props.dataAuthorized);
    script.setAttribute('data-start-hidden', props.dataStartHidden);
    script.setAttribute('data-toggle-shortcut', props.dataToggleShortcut);
    script.setAttribute('data-trivial-milliseconds', props.dataTrivialMilliseconds);
    script.async = props.scriptAsync;

    body.appendChild(script);
}

function appendCssLink() {
    const body = document.body; // as HTMLDivElement;
    const css = document.createElement('link');
    css.href = profiler.cssSrc;
    css.rel = 'stylesheet';

    body.appendChild(css);
}

axiosSetUp();
appendDivElement();
appendCssLink();
</script>