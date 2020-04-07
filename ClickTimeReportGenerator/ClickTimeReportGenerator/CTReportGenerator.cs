using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace ClickTimeReportGenerator
{
    public class CTReportGenerator
    {
        string _timesheetDate = string.Empty;
        public CTReportGenerator(DateTime timesheetDate)
        {
            _timesheetDate = timesheetDate.ToString("yyyy-MM-dd");
        }

        public async Task<DataTable> GetReport()
        {
            await GetMeDetails().ConfigureAwait(false);
            var managedUserDetails = await GetManagedUsersDetails().ConfigureAwait(false);
            return await GenerateDataTable(managedUserDetails).ConfigureAwait(false);
        }

        private async Task<GetMeResponse> GetMeDetails()
        {
            var response = await HttpServices.GetMe(Constants.Token).ConfigureAwait(false);
            ValidateUser(response);
            return response;
        }

        private void ValidateUser(GetMeResponse response)
        {
            if (response?.Error?.Any() ?? false)
            {
                throw new Exception(response.Error[0].Message);
            }

            if (response.Data == null)
            {
                throw new Exception($"No data found for requested user");
            }

            if (response.Data != null && !response.Data.IsTimeApprover)
            {
                throw new Exception($"You are not authorized to get the ClickTime report");
            }

            //TODO : Currently commenting this condition as Milind and Abhijit both are not assigned to LZ Division.
            //if (!response.Data.DivisionID.Equals(Constants.LZDivisionId))
            //{
            //    throw new Exception($"You are not authorized to get the LZ ClickTime report as you are not from LegalZoom team");
            //}
        }

        private async Task<List<DataResponse>> GetManagedUsersDetails()
        {
            var response = await HttpServices.GetManagedUsers(Constants.Token).ConfigureAwait(false);

            if (response?.Error?.Any() ?? false)
            {
                throw new Exception(response.Error[0].Message);
            }

            if (!response.Data.Any())
            {
                throw new Exception($"No managed users found for requested user");
            }

            var managedUsers = response.Data
                .Where(x => (x.TimesheetApproverID == Constants.TimesheetApproverID_Milind || x.TimesheetApproverID == Constants.TimesheetApproverID_Abhijit)
                        && x.DivisionID == Constants.LZDivisionId
                        && x.IsActive)
                .ToList();

            if (!managedUsers.Any())
            {
                throw new Exception($"No managed users found for requested user");
            }
            Console.WriteLine(managedUsers.Select(x => x.Name));
            return managedUsers;
        }

        private async Task<DataTable> GenerateDataTable(List<DataResponse> users)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Sr No");
            dt.Columns.Add("Name");
            dt.Columns.Add("Id");
            dt.Columns.Add("Email");
            dt.Columns.Add("Start Date");
            dt.Columns.Add("End Date");
            dt.Columns.Add("Status");

            //dt.Columns.Add("WorkingHours");
            //dt.Columns.Add("LeaveHours");
            //dt.Columns.Add("TotalHours");

            //dt.Columns.Add("UserId");
            //dt.Columns.Add("TimesheetId");


            for (int i = 0; i < users.Count; i++)
            {
                var timesheetDetails = await HttpServices.GetTimesheetByUserAndDate(Constants.Token, users[i].ClickTimeId, _timesheetDate, Constants.Token_Abhijit)
                    .ConfigureAwait(false);

                var srNo = i + 1;
                var row = dt.NewRow();

                row["Sr No"] = srNo;
                row["Name"] = users[i].Name;
                row["Id"] = users[i].EmployeeId;
                row["Email"] = users[i].Email;
                row["Start Date"] = timesheetDetails?.Data?.StartDate;
                row["End Date"] = timesheetDetails?.Data?.EndDate;
                row["Status"] = timesheetDetails?.Data?.Status;

                //var timesheetHours = await GetTimesheetHoursByTimesheetId(timesheetDetails?.Data?.TimesheetID, timesheetDetails?.Data?.UserID);
                //var timeOffHours = await GetTimeOffHoursByTimesheetId(timesheetDetails?.Data?.TimesheetID, timesheetDetails?.Data?.UserID);

                //row["WorkingHours"] = timesheetHours;

                //row["LeaveHours"] = timeOffHours;

                //row["TotalHours"] = timesheetHours + timeOffHours;



                dt.Rows.Add(row);
            }

            return dt;
        }

        private async Task<decimal> GetTimesheetHoursByTimesheetId(string timsheetId, string userId)
        {
            var response = await HttpServices.GetTimesheetHoursByTimesheetId(Constants.Token, timsheetId);

            if (response?.Error?.Any() ?? false)
            {
                throw new Exception(response.Error[0].Message);
            }

            if (response?.Data.All(x => x.UserID.Equals(userId)) ?? false)
            {
                return response?.Data.Sum(y => y.Hours) ?? 0;
            }

            return 0;
        }

        private async Task<decimal> GetTimeOffHoursByTimesheetId(string timsheetId, string userId)
        {
            var response = await HttpServices.GetTimeOffByTimesheetId(Constants.Token, timsheetId);

            if (response?.Error?.Any() ?? false)
            {
                throw new Exception(response.Error[0].Message);
            }

            if (response?.Data.All(x => x.UserID.Equals(userId)) ?? false)
            {
                return response?.Data.Sum(y => y.Hours) ?? 0;
            }

            return 0;
        }
    }
}
