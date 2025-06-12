(function () {
    var s = document.createElement("script");
    s.id = "mini-profiler";
    s.async = "async";
    s.src = "/profiler/includes.min.js?v=4.3.8+1120572909";
    s.setAttribute('data-version', "4.3.8+1120572909");
    s.setAttribute('data-path', "/profiler/");
    s.setAttribute('data-position', "Left");
    s.setAttribute('data-scheme', "Light");
    s.setAttribute('data-authorized', "true");
    s.setAttribute('data-trivial', "2.0");
    s.setAttribute('data-trivial-milliseconds', "2.0");
    s.setAttribute('data-ignored-duplicate-execute-types', "Open,OpenAsync,Close,CloseAsync");
    s.setAttribute('data-max-traces',15);
    s.setAttribute('data-controls', true);
    document.head.appendChild(s);
})();