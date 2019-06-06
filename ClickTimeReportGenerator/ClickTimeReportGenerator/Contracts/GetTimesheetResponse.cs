using Newtonsoft.Json;
using System.Collections.Generic;

namespace ClickTimeReportGenerator
{
    public class GetTimesheetResponse
    {
        [JsonProperty("data")]
        public TimesheetData Data { get; set; }

        [JsonProperty("errors")]
        public List<ErrorResponse> Error { get; set; }
    }
}
