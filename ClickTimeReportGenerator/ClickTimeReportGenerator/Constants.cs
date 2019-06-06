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
        //public const string Token = "YOnW48KdUemifVA9aTH5e9fx1hBKymKa25xiTnv-BXlS3o7gftrBSw2";//Rupali
        public const string Token = "cnSzAqrjvcQ1_hwuUAp2hcgaYGiPF-v2cZ1hkhBa7Uiy01Kq9lZViQ2";//Nikhil
        public const string LZDivisionId = "4aqE7xoEXgW0wN4BGl98OUw2";
        public const string TimesheetApproverID = "4ul5rVj-ogGXmw61jUGCDWA2";
        #endregion
    }
}
