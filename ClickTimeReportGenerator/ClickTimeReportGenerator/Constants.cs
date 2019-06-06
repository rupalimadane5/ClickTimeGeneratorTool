namespace ClickTimeReportGenerator
{
    public class Constants
    {
        #region Url 
        public const string GetMe = "https://api.clicktime.com/v2/me";
        public const string GetManagedUsers = "https://api.clicktime.com/v2/manage/users?offset=0&limit=1000";
        public const string GetTimesheet = "https://api.clicktime.com/v2/manage/timesheets/{0}/users/{1}";
        #endregion

        #region Appsettings
        //public const string Token = "";
        public const string Token = "";
        public const string LZDivisionId = "";
        public const string TimesheetApproverID = "";
        #endregion
    }
}
