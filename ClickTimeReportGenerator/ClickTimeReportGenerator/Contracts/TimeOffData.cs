using Newtonsoft.Json;

namespace ClickTimeReportGenerator
{
    public class TimeOffData
    {
        [JsonProperty("Date")]
        public string TimeOffDate { get; set; }
        public decimal Hours { get; set; }
        [JsonProperty("ID")]
        public string TimesheetId { get; set; }
        public string TimeOffRequestID { get; set; }
        public string TimeOffTypeID { get; set; }
        public string UserID { get; set; }
    }
}
