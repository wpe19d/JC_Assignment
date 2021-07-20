namespace JumpCloudAssignment.Models
{
    public static class ActionMessages
    {
        public const string SuccessMessage = "";
        public const string InvalidAction = "ActionInfo provided is not valid";
        public const string EmptyAction = "ActionInfo cannot be empty";

        /// <summary>
        /// Returns a formatted invalid action message with the action input.
        /// </summary>
        /// <param name="action">Action string</param>
        /// <returns>Invalid action message</returns>
        public static string InvalidActionMessage(string action)
        {
            return $"{InvalidAction}. Invalid ActionInfo: {action}";
        }
    }
}