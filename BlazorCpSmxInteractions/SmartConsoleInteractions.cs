using Microsoft.JSInterop;
using System;
using System.Text.Json;
using System.Threading.Tasks;


namespace BlazorCpSmxInteractions
{
    // This class provides wrapped JavaScript functionality 
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.


    public class SmartConsoleInteractions : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        private IJSRuntime _jsRuntime;
        string _demoInteractionJSModulPath = string.Empty;


        public SmartConsoleInteractions(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/BlazorCpSmxInteractions/smartConsoleInteractionsJsInterop.js").AsTask());

            _jsRuntime = jsRuntime;
        }


        /// <summary>
        /// Gets or sets the Demo-Interaction JavaScript modul path
        /// </summary>
        /// <returns>The Demo-Interaction JavaScript modul path</returns>
        public string DemoInteractionJSModulPath
        {
            get { return _demoInteractionJSModulPath; }

            set
            {
                _demoInteractionJSModulPath = value;
            }
        }


        /// <summary>
        /// Execute query to get objects from the Security Management API.
        /// </summary>
        /// <param name="queryRequestId">String with query ID to execute</param>
        /// <param name="queryRequestParams">JSON String with parameters to execute</param>
        /// <returns>Query response</returns>
        public async ValueTask<JsonElement> Query(string queryRequestId, JsonElement queryRequestParams)
        {
            JsonElement smxJsonObject;

            try
            {
                var module = await moduleTask.Value;

                var subscriptionId = Guid.NewGuid().ToString();

                smxJsonObject = await module.InvokeAsync<JsonElement>("smxQuery", queryRequestId, queryRequestParams, subscriptionId, _demoInteractionJSModulPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Query-Exception: {0}", e.Message);

                throw;
            }

            return smxJsonObject;
        }


        /// <summary>
        /// Extension context provided by SmartConsole.
        /// </summary>
        /// <returns>JSON object of extension location context. See <a href="https://sc1.checkpoint.com/documents/SmartConsole/Extensions/#Extension%20Context">Extension Context</a>.</returns>
        public async ValueTask<JsonElement> GetContextObject()
        {
            JsonElement smxJsonObject;

            try
            {
                var module = await moduleTask.Value;

                var subscriptionId = Guid.NewGuid().ToString();

                smxJsonObject = await module.InvokeAsync<JsonElement>("smxGetContextObject", subscriptionId, _demoInteractionJSModulPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("GetContextObject-Exception: {0}", e.Message);

                throw;
            }

            return smxJsonObject;
        }


        /// <summary>
        /// Request SmartConsole user to execute list of commands. 
        /// Used by extensions to apply changes by SmartConsole user private session.
        /// </summary>
        /// <param name="commandsToCommit">
        /// JSON object constructed from list of SmartConsole CLI commands. 
        /// See Management API documentation. 
        /// The last argument is the name of the callback function to be called when the API commands have been executed.
        /// </param>
        /// <returns>JSON object of commands results.</returns>
        /// <remarks>
        /// Presents the user a dialog requesting to commit the specified API commands.
        /// Note - Most Session Management commands are not supported through SmartConsole CLI thus also through this interaction.
        /// See Management API documentation whether a command specifies the “--format JSON” option, 
        /// the corresponding response will also be a JSON structure.
        /// 
        /// Note, commands are prompt to user require user approval
        /// </remarks>
        public async ValueTask<JsonElement> RequestCommit(string[] commandsToCommit)
        {
            JsonElement smxJsonObject;

            try
            {
                var module = await moduleTask.Value;

                var subscriptionId = Guid.NewGuid().ToString();

                smxJsonObject = await module.InvokeAsync<JsonElement>("smxRequestCommit", commandsToCommit, subscriptionId, _demoInteractionJSModulPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("RequestCommit-Exception: {0}", e.Message);

                throw;
            }

            return smxJsonObject;
        }


        /// <summary>
        /// Request SmartConsole to navigate to a rule.
        /// </summary>
        /// <param name="uid">rule uid to navigate to in SmartConsole</param>
        public async ValueTask Navigate(string uid)
        {
            try
            {
                var module = await moduleTask.Value;

                var subscriptionId = Guid.NewGuid().ToString();

                await module.InvokeVoidAsync("smxNavigate", uid, subscriptionId, _demoInteractionJSModulPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Navigate-Exception: {0}", e.Message);

                throw;
            }
        }


        /// <summary>
        /// Request SmartConsole to close the extension window.
        /// </summary>
        public async ValueTask CloseExtensionWindow()
        {
            try
            {
                var module = await moduleTask.Value;

                var subscriptionId = Guid.NewGuid().ToString();

                await module.InvokeVoidAsync("smxCloseExtensionWindow", subscriptionId, _demoInteractionJSModulPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("CloseExtensionWindow-Exception: {0}", e.Message);

                throw;
            }
        }


        [JSInvokable]
        public static string DotNetUuid()
        {
            var uuid = Guid.NewGuid().ToString();

            return uuid;
        }


        /// <summary>
        /// Check if extension is runing inside SmartConsole
        /// </summary>
        /// <returns>Returns the user agent string for the current browser.</returns>
        public async ValueTask<bool> IsSmartConsoleMode()
        {
            return await _jsRuntime.InvokeAsync<bool>("window.hasOwnProperty", "smxProxy");
        }


        /// <summary>
        /// Get a user agent in Blazor WebAssembly using JavaScript Interop with the navigator.userAgent property.
        /// </summary>
        /// <returns>True if is running inside SmartConsole; otherwise, false.</returns>
        public async ValueTask<string> GetUserAgent()
        {
            string userAgent = "n/a";

            try
            {
                var module = await moduleTask.Value;

                userAgent = await module.InvokeAsync<string>("getUserAgent");
            }
            catch (Exception e)
            {
                Console.WriteLine("GetUserAgent-Exception: {0}", e.Message);

                throw;
            }

            return userAgent;
        }


        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
