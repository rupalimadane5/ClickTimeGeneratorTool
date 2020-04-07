namespace ClickTimeReportGenerator
{
    public class Constants
    {
        #region Url 
        public const string GetMe = "https://api.clicktime.com/v2/me";
        public const string GetManagedUsers = "https://api.clicktime.com/v2/manage/users?offset=0&limit=1000";
        public const string GetTimesheet = "https://api.clicktime.com/v2/manage/timesheets/{0}/users/{1}";
        public const string GetTimesheetHours = "https://api.clicktime.com/v2/Timesheets/{0}/TimeEntries";
        public const string GetTimeOffByTimesheetId = "https://api.clicktime.com/v2/Manage/Timesheets/{0}/TimeOff";
        #endregion

        #region Appsettings
        public const string Token = "************";//Milind
        public const string Token_Abhijit = "*************";//Abhjit

        public const string LZDivisionId = "***************";

        public const string TimesheetApproverID_Milind = "**********";
        public const string TimesheetApproverID_Abhijit = "***********";

        public const double MinTimesheetHours = 42.5;
        #endregion
    }
}
