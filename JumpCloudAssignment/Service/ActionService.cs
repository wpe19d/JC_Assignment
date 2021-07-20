using System;
using System.Collections.Concurrent;
using System.Linq;
using JumpCloudAssignment.Models;
using Newtonsoft.Json;

namespace JumpCloudAssignment.Service
{
    public class ActionService
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            MissingMemberHandling = MissingMemberHandling.Error
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

            try
            {
                //Verify the input has content
                if (string.IsNullOrEmpty(input))
                {
                    throw new Exception(ActionMessages.EmptyAction);
                }

                //Converts the input into an ActionInfo Model.
                actionInfo = JsonConvert.DeserializeObject<ActionInfo>(input, _settings);

                if (!IsValidAction(actionInfo))
                {
                    throw new Exception(ActionMessages.InvalidAction);
                }
            }
            catch(Exception e)
            {
                return e.Message;
            }

            //Attempts to add to the action dictionary.
            //If the key (action type) already exists, it appends the action's time to the end of the array for that action
            _ = actionDictionary.AddOrUpdate(actionInfo.Action, new[] {actionInfo.Time},
                (key, value) => value.Append(actionInfo.Time).ToArray());

            return ActionMessages.SuccessMessage;
        }

        private bool IsValidAction(ActionInfo actionInfo)
        {
            if (!Enum.TryParse(actionInfo.Action, true, out ActionTypes _))
            {
                return false;
            }

            if (actionInfo.Time < 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Retrieves the action statistics for each type of action from the action service.
        /// </summary>
        /// <returns>Action statistics</returns>
        public string GetStats()
        {
            throw new NotImplementedException();
        }
    }
}
