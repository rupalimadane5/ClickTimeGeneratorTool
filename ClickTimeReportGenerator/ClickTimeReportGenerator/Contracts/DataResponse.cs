using Newtonsoft.Json;

namespace ClickTimeReportGenerator
{
    public class DataResponse
    {
        [JsonProperty("ID")]
        public string ClickTimeId { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }

        [JsonProperty("EmployeeNumber")]
        public string EmployeeId { get; set; }

        public string DivisionID { get; set; }
        public string TimesheetApproverID { get; set; }
        public bool IsTimeApprover { get; set; }
        public bool IsActive { get; set; }
    }
}
