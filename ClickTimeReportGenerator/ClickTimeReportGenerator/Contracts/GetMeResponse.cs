using Newtonsoft.Json;
using System.Collections.Generic;

namespace ClickTimeReportGenerator
{
    public class GetMeResponse
    {
        [JsonProperty("data")]
        public DataResponse Data { get; set; }

        [JsonProperty("errors")]
        public List<ErrorResponse> Error { get; set; }
    }
}
