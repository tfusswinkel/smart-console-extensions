// globalThis polyfill (see < https://github.com/ungap/global-this/blob/master/min.js >):
!function (t) { function e() { var e = this || self; e.globalThis = e, delete t.prototype._T_ } "object" != typeof globalThis && (this ? e() : (t.defineProperty(t.prototype, "_T_", { configurable: !0, get: e }), _T_)) }(Object);


// Blazor WebAssembly JS initializer:
export function beforeStart(options, extensions) {
    console.log("Blazor WebAssembly - beforeStart");
    console.info("navigator.userAgent = " + navigator.userAgent);
    console.debug("isSmartConsoleMode = " + window.hasOwnProperty("smxProxy"));
}

export function afterStarted(blazor) {
    console.log("Blazor WebAssembly - afterStarted");
}
