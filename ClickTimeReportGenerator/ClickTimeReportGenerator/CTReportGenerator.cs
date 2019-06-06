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

            if (!response.Data.DivisionID.Equals(Constants.LZDivisionId))
            {
                throw new Exception($"You are not authorized to get the LZ ClickTime report as you are not from LegalZoom team");
            }
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
                .Where(x => x.TimesheetApproverID == Constants.TimesheetApproverID
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
            

            for (int i = 0; i < users.Count; i++)
            {

                var timesheetDetails = await HttpServices.GetTimesheetByUserAndDate(Constants.Token, users[i].ClickTimeId, _timesheetDate)
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

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
