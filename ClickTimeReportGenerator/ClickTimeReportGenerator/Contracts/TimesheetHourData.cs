using Newtonsoft.Json;

namespace ClickTimeReportGenerator
{
    public class TimesheetHourData
    {
        [JsonProperty("Date")]
        public string TimesheetDate { get; set; }
        public decimal Hours { get; set; }
        [JsonProperty("ID")]
        public string TimesheetEntryId { get; set; }
        public string JobId { get; set; }
        public string TaskId { get; set; }
        public string UserID { get; set; }
    }
}
