using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace BlazorCpSmxInteractions
{
    public enum DetailsLevel
    {
        Standard,  // = "standard";
        Full,      // = "full";
        UID        // = "uid";
    }


    public enum TaskStatus
    {
        InProgress,  // = "in progress";
        Succeeded,   // = "succeeded";
        Failed       // = "failed";
    }


    internal class GetObjectsByQueryParams
    {
        public List<string> @in { get; set; }
        public string type { get; set; }

        [JsonPropertyName("details-level")]
        public string DetailsLevel { get; set; }
    }


    public class QueryManager
    {
        private SmartConsoleInteractions _interactions;
        private IJSRuntime _jsRuntime;

        private const string getObjectQueryId = "show-object";
        private const string getObjectsQueryId = "show-objects";
        private const string getObjectsQueryByName = "name";
        private const string getObjectsQueryByTag = "tags";
        private const string getTagsQueryId = "show-tags";
        private const string getTaskQueryId = "show-task";

        private static Dictionary<DetailsLevel, string> detailsLevelParamStrings = new Dictionary<DetailsLevel, string>()
        {
            { DetailsLevel.Full, "standard" },
            { DetailsLevel.Standard, "full" },
            { DetailsLevel.UID, "uid" }
        };


        private static Dictionary<TaskStatus, string> taskStatusReturnStrings = new Dictionary<TaskStatus, string>()
        {
            { TaskStatus.InProgress, "in progress" },
            { TaskStatus.Succeeded, "succeeded" },
            { TaskStatus.Failed, "failed" }
        };


        public const string defaultObjectType = "object";


        public QueryManager(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;

            _interactions = new SmartConsoleInteractions(jsRuntime);
        }


        /// <summary>
        /// Gets or sets the Demo-Interaction JavaScript modul path
        /// </summary>
        /// <returns>The Demo-Interaction JavaScript modul path</returns>
        public string DemoInteractionJSModulPath
        {
            get { return _interactions.DemoInteractionJSModulPath; }

            set
            {
                _interactions.DemoInteractionJSModulPath = value;
            }
        }


        /// <summary>
        /// Retrieve object by object uid
        /// </summary>
        /// <param name="uid">object uid</param>
        /// <param name="detailsLevel">details level</param>
        /// <returns>Requested object</returns>
        public async ValueTask<JsonElement?> GetObject(string uid, DetailsLevel detailsLevel = DetailsLevel.Standard)
        {
            var queryParams = new Dictionary<string, string>()
            {
                { "uid", uid},
                { "details-level", detailsLevelParamStrings[detailsLevel] },
            };

            string queryRequestParamsJsonString = JsonSerializer.Serialize(queryParams);

            using var queryRequestParamsJsonDocument = JsonDocument.Parse(queryRequestParamsJsonString);

            var queryRequestParamsJsonElement = queryRequestParamsJsonDocument.RootElement;


            try
            {
                var queryResponse = await _interactions.Query(getObjectQueryId, queryRequestParamsJsonElement);

                return queryResponse;
            }
            catch (Exception error)
            {
                Console.Error.WriteLine("Failed to get object uid(" + uid + ") - " + error);

                return null;
            }
        }


        /// <summary>
        /// Retrieve objects by object name
        /// </summary>
        /// <param name="name">object name</param>
        /// <param name="type">object type</param>
        /// <param name="detailsLevel">details level</param>
        /// <returns>List of objects</returns>
        public async ValueTask<JsonElement?> GetObjectsByName(string name, string type, DetailsLevel detailsLevel = DetailsLevel.Standard)
        {
            var queryParams = new GetObjectsByQueryParams()
            {
                @in = new List<string> { getObjectsQueryByName, name },
                type = type,
                DetailsLevel = detailsLevelParamStrings[detailsLevel]
            };


            string queryRequestParamsJsonString = JsonSerializer.Serialize(queryParams);

            using var queryRequestParamsJsonDocument = JsonDocument.Parse(queryRequestParamsJsonString);

            var queryRequestParamsJsonElement = queryRequestParamsJsonDocument.RootElement;


            try
            {
                var queryResponse = await _interactions.Query(getObjectsQueryId, queryRequestParamsJsonElement);

                return queryResponse;
            }
            catch (Exception error)
            {
                Console.Error.WriteLine("Failed to get objects - " + error);

                return null;
            }
        }


        /// <summary>
        /// Retrieve objects tagged by tag uid
        /// </summary>
        /// <param name="tag">tag uid</param>
        /// <param name="type">object type</param>
        /// <param name="detailsLevel">details level</param>
        /// <returns>List of objects</returns>
        public async ValueTask<JsonElement?> GetObjectsByTag(string tag, string type, DetailsLevel detailsLevel = DetailsLevel.Standard)
        {
            var queryParams = new GetObjectsByQueryParams()
            {
                @in = new List<string> { getObjectsQueryByTag, tag },
                type = type,
                DetailsLevel = detailsLevelParamStrings[detailsLevel]
            };


            string queryRequestParamsJsonString = JsonSerializer.Serialize(queryParams);

            using var queryRequestParamsJsonDocument = JsonDocument.Parse(queryRequestParamsJsonString);

            var queryRequestParamsJsonElement = queryRequestParamsJsonDocument.RootElement;


            try
            {
                var queryResponse = await _interactions.Query(getObjectsQueryId, queryRequestParamsJsonElement);

                return queryResponse;
            }
            catch (Exception error)
            {
                Console.Error.WriteLine("Failed to get objects - " + error);

                return null;
            }
        }


        /// <summary>
        /// Retrieve all tags
        /// </summary>
        /// <param name="detailsLevel">details level</param>
        /// <returns>List of tags</returns>
        public async ValueTask<JsonElement?> GetTags(DetailsLevel detailsLevel = DetailsLevel.Standard)
        {
            var queryParams = new Dictionary<string, string>()
            {
                { "details-level", detailsLevelParamStrings[detailsLevel] },
            };

            string queryRequestParamsJsonString = JsonSerializer.Serialize(queryParams);

            using var queryRequestParamsJsonDocument = JsonDocument.Parse(queryRequestParamsJsonString);

            var queryRequestParamsJsonElement = queryRequestParamsJsonDocument.RootElement;


            try
            {
                var queryResponse = await _interactions.Query(getTagsQueryId, queryRequestParamsJsonElement);

                return queryResponse;
            }
            catch (Exception error)
            {
                Console.Error.WriteLine("Failed to get tags - " + error);

                return null;
            }
        }


        /// <summary>
        /// Retrieve task by query request
        /// </summary>
        /// <param name="queryRequest">{QueryRequest} queryRequest</param>
        /// <returns>Task object</returns>
        internal async ValueTask<JsonElement?> GetTaskInternal(JsonElement queryRequestParamsJsonElement)
        {
            try
            {
                var queryResponse = await _interactions.Query(getTaskQueryId, queryRequestParamsJsonElement);

                return queryResponse;
            }
            catch (Exception error)
            {
                Console.Error.WriteLine("Failed to get task - " + error);

                return null;
            }
        }


        private JsonElement? CheckTaskResult(JsonElement? taskResult, out string status)
        {
            status = string.Empty;

            if ((taskResult != null) && (taskResult.HasValue))
            {
                if (taskResult.Value.TryGetProperty("tasks", out JsonElement taskJsonElement))
                {
                    status = taskJsonElement[0].GetProperty("status").GetString();

                    return taskJsonElement[0];
                }
            }

            return null;
        }


        /// <summary>
        /// Retrieve task by task uid
        /// </summary>
        /// <param name="uid">object uid</param>
        /// <param name="detailsLevel">details level</param>
        /// <param name="wait">wait</param>
        /// <returns>Task object</returns>
        public async ValueTask<JsonElement?> GetTask(string uid, DetailsLevel detailsLevel = DetailsLevel.Standard, bool wait = true)
        {
            var result = await GetTaskWithTaskStatus(uid, detailsLevel, wait);

            return result.TaskObject;
        }


        /// <summary>
        /// Retrieve task by task uid
        /// </summary>
        /// <param name="uid">object uid</param>
        /// <param name="detailsLevel">details level</param>
        /// <param name="wait">wait</param>
        /// <returns>A Tuple with Task object and TaskStatus</returns>
        public async ValueTask<(JsonElement? TaskObject, TaskStatus? TaskStatusCode)> GetTaskWithTaskStatus(string uid, DetailsLevel detailsLevel = DetailsLevel.Standard, bool wait = true)
        {
            var queryParams = new Dictionary<string, string>()
            {
                { "task-id", uid},
                { "details-level", detailsLevelParamStrings[detailsLevel] },
            };

            string queryRequestParamsJsonString = JsonSerializer.Serialize(queryParams);

            using var queryRequestParamsJsonDocument = JsonDocument.Parse(queryRequestParamsJsonString);

            var queryRequestParamsJsonElement = queryRequestParamsJsonDocument.RootElement;

            try
            {
                string status;

                var taskResult = CheckTaskResult(await GetTaskInternal(queryRequestParamsJsonElement), out status);

                while (wait && (status == taskStatusReturnStrings[TaskStatus.InProgress]))
                {
                    await Task.Delay(5000);

                    taskResult = CheckTaskResult(await GetTaskInternal(queryRequestParamsJsonElement), out status);
                }


                TaskStatus? taskStatusCode = null;

                foreach (var taskStatusKey in taskStatusReturnStrings.Keys)
                {
                    if (status == taskStatusReturnStrings[taskStatusKey])
                    {
                        taskStatusCode = taskStatusKey;
                        break;
                    }
                }


                await Done.InvokeAsync();  // Notify the subscriber

                return (taskResult, taskStatusCode);
            }
            catch (Exception error)
            {
                Console.Error.WriteLine("Failed to get task (" + uid + ") - " + error);

                return (null, null);
            }
        }


        [Parameter]
        public EventCallback Done { get; set; }


        /// <summary>
        /// Request SmartConsole user to execute passed commands
        /// </summary>
        /// <param name="commands">commands</param>
        /// <returns>List of commands result in a matching order</returns>
        public async ValueTask<JsonElement> RequestCommit(string[] commands)
        {
            try
            {
                var result = await _interactions.RequestCommit(commands);

                return result;
            }
            catch (Exception error)
            {
                Console.Error.WriteLine("RequestCommit error: " + error);

                throw;
            }
        }


        /// <summary>
        /// Retrieve SmartConsole context
        /// </summary>
        /// <returns>Context received from SmartConsole</returns>
        public async ValueTask<JsonElement> GetContextObject()
        {
            try
            {
                var result = await _interactions.GetContextObject();

                Console.WriteLine(result.ToString());

                return result;
            }
            catch (Exception error)
            {
                Console.Error.WriteLine("GetContextObject error: " + error);

                throw;
            }
        }


        /// <summary>
        /// Check if extension is runing inside SmartConsole
        /// </summary>
        /// <returns>True if is running inside SmartConsole; otherwise, false.</returns>
        public async ValueTask<bool> IsSmartConsoleMode()
        {
            return await _interactions.IsSmartConsoleMode();
        }
    }
}
