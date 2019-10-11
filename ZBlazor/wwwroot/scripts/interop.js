window.interopElement = {
    debug: false,
    focus: function (element) {
        if (this.debug) console.log("interop: focus()");
        element.focus();
    },
    scrollToBottom: function(element) {
        if (this.debug) console.log("interop: scrollToBottom");
        element.scrollTop = element.scrollHeight;
    },
    ensureVisible: function(element) {
        if (this.debug) console.log("interop: ensureVisible");
        element.ensureVisible();
    },
    click: function(element) {
        if (this.debug) console.log("interop: click");
        element.click();
    },
    setSelectionRange: function (element, start, end) {
        if (this.debug) console.log("interop: setSelectionRange", start, end);
        element.setSelectionRange(start, end);
    }
}
