using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using JumpCloudAssignment.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JumpCloudAssignment.Service
{
    public class ActionService
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            MissingMemberHandling = MissingMemberHandling.Error,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private ConcurrentDictionary<string, int[]> actionDictionary = new ConcurrentDictionary<string, int[]>();

        /// <summary>
        /// Adds action information in json format to the action service.
        /// </summary>
        /// <param name="input">Json string of action information</param>
        /// <returns>Status of adding the action to the service</returns>
        public string AddAction(string input)
        {
            ActionInfo actionInfo;

            //Verify the input has content
            if (string.IsNullOrEmpty(input))
            {
                return ActionMessages.EmptyAction;
            }

            if (!IsValidAction(input, out actionInfo))
            { 
                return ActionMessages.InvalidActionMessage(input);
            }

            //Attempts to add to the action dictionary.
            //If the key (action type) already exists, it appends the action's time to the end of the array for that action
            actionDictionary.AddOrUpdate(actionInfo.Action, new[] {actionInfo.Time},
                (key, value) => value.Append(actionInfo.Time).ToArray());

            return ActionMessages.SuccessMessage;
        }

        /// <summary>
        /// Retrieves the action statistics for each type of action from the action service.
        /// </summary>
        /// <returns>Action statistics</returns>
        public string GetStats()
        {
            var actionStats = new List<ActionStatistics>();

            //Loops through all entries in the dictionary
            foreach (var key in actionDictionary.Keys.OrderBy(x => x))
            {
                //Attempts to retrieve the value for the key
                if (actionDictionary.TryGetValue(key, out var value))
                {
                    var actionStat = new ActionStatistics()
                    {
                        Action = key,
                        Avg = (double)value.Sum() / value.Length
                    };
                    actionStats.Add(actionStat);
                }
            }
            return JsonConvert.SerializeObject(actionStats, _settings);
        }

        /// <summary>
        /// Verifies that the action info has the correct action type and time
        /// </summary>
        /// <param name="actionInfo">ActionInfo to be validated</param>
        /// <returns>true or false depending on validation results</returns>
        private bool IsValidAction(string action, out ActionInfo actionInfo)
        {
            actionInfo = null;
            try
            {
                actionInfo = JsonConvert.DeserializeObject<ActionInfo>(action, _settings);

                if (!Enum.TryParse(actionInfo.Action, true, out ActionTypes _))
                {
                    return false;
                }

                if (actionInfo.Time < 0)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
