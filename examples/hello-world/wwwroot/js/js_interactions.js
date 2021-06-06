var smxInterop = function () {
    return {
        globalCallbackFunctionName: null,
        smxInvoker: function (smxInteraction, smxParameter, smxCallbackFunctionName, dotNetCallbackReference) {
            this.globalCallbackFunctionName = smxCallbackFunctionName;

            window[this.globalCallbackFunctionName] = (result) => {
                dotNetCallbackReference.invokeMethod("SmxResultCallback", result);
            };

            smxProxy.sendRequest(smxInteraction, smxParameter, this.globalCallbackFunctionName);
        },
        smxDispose: function () {
            if (window.hasOwnProperty(this.globalCallbackFunctionName)) {
                delete window[this.globalCallbackFunctionName];
            }
        }
    }
}
