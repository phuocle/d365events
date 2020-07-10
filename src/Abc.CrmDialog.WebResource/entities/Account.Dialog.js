"use strict";
var dialog = (function () {
    "use strict";
    async function onOpen(executionContext) {
        alert("HELLO WORLD");
    }
    return {
        OnOpen: onOpen
    };
})();
