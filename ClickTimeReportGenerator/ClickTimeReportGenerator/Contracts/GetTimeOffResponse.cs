using Newtonsoft.Json;
using System.Collections.Generic;

namespace ClickTimeReportGenerator
{
    public class GetTimeOffResponse
    {
        [JsonProperty("data")]
        public List<TimeOffData> Data { get; set; }

        [JsonProperty("errors")]
        public List<ErrorResponse> Error { get; set; }
    }
}