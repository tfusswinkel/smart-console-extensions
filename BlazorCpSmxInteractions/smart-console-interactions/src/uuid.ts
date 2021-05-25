declare var DotNet: any


export default function uuidv4() {
    return DotNet.invokeMethod('BlazorCpSmxInteractions', 'DotNetUuid');
}
