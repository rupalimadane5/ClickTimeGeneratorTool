using Newtonsoft.Json;

namespace ClickTimeReportGenerator
{
    public class TimesheetData
    {
        public string ApprovedByUserID { get; set; }
        public string EndDate { get; set; }
        [JsonProperty("ID")]
        public string TimesheetID { get; set; }
        public string RejectedByUserID { get; set; }
        public string StartDate { get; set; }
        public string Status { get; set; }
        public string SubmittedByUserID { get; set; }
        public string UserID { get; set; }
    }
}
