<template>

</template>

<script setup>
import axios from 'axios';
import { reactive } from 'vue';
import request from '@/utils/request'

const { proxy } = getCurrentInstance();

function appendMiniProfiler() {
    const key = 'MiniProfiler';

    request.interceptors.response.use(res => {
        const miniProfiler = window[key];
        const miniProfilerIds = JSON.parse(res.headers['x-miniprofiler-ids']);
        miniProfiler.fetchResults(miniProfilerIds);
        return Promise.resolve(res);
    },
    error => {
        return Promise.reject(error)
    })

    const s = document.createElement("script");
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
    s.setAttribute('data-max-traces', 15);
    s.setAttribute('data-controls', true);
    document.head.appendChild(s);

    const css = document.createElement('link');
    css.href = profiler.cssSrc;
    css.rel = 'stylesheet';
    document.head.appendChild(css);
};

appendMiniProfiler();
</script>