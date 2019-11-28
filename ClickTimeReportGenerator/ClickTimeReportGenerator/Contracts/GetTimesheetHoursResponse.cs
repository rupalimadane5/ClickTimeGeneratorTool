using Newtonsoft.Json;
using System.Collections.Generic;

namespace ClickTimeReportGenerator
{
    public class GetTimesheetHoursResponse
    {
        [JsonProperty("data")]
        public List<TimesheetHourData> Data { get; set; }

        [JsonProperty("errors")]
        public List<ErrorResponse> Error { get; set; }
    }
}
