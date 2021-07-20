using Newtonsoft.Json;

namespace JumpCloudAssignment.Models
{
    public class ActionInfo
    {
        [JsonRequired]
        public string Action { get; set; }

        [JsonRequired]
        public string Time { get; set; }
    }
}