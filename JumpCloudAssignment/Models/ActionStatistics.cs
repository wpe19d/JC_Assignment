using Newtonsoft.Json;

namespace JumpCloudAssignment.Models
{
    public class ActionStatistics
    {
        [JsonRequired]
        public string Action { get; set; }
        [JsonRequired]
        public double Avg { get; set; }
    }
}