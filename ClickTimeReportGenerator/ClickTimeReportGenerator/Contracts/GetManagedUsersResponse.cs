using Newtonsoft.Json;
using System.Collections.Generic;

namespace ClickTimeReportGenerator
{
    public class GetManagedUsersResponse
    {
        [JsonProperty("data")]
        public List<DataResponse> Data { get; set; }

        [JsonProperty("errors")]
        public List<ErrorResponse> Error { get; set; }
    }
}
