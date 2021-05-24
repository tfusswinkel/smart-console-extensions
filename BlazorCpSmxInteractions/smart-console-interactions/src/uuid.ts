declare var DotNet: any


export default function uuidv2() {
    return DotNet.invokeMethod('BlazorCpSmxInteractions', 'DotNetUuid');
}
