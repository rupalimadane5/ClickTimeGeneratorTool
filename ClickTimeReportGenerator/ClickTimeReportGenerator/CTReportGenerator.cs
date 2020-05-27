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
                        && x.IsActive
                        && (x.Name.Contains("Rupali") || x.Name.Contains("Vaishnavi") || x.Name.Contains("Yogesh")
                        || x.Name.Contains("Dhanashree") || x.Name.Contains("Girish")))
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
            //dt.Columns.Add("Id");
            //dt.Columns.Add("Email");
            //dt.Columns.Add("Start Date");
            //dt.Columns.Add("End Date");
            //dt.Columns.Add("Status");

            dt.Columns.Add("TimesheetDate");
            dt.Columns.Add("StatusReporting");
            dt.Columns.Add("Development");
            dt.Columns.Add("UnitTesting");
            dt.Columns.Add("CodeReview");
            dt.Columns.Add("DefectFixing");
            dt.Columns.Add("DevelopmentUnbillable");
            dt.Columns.Add("Onboarding-KT");
            dt.Columns.Add("TotalTimesheetHours");

            for (int i = 0; i < users.Count; i++)
            {
                var timesheetDetails = await HttpServices.GetTimesheetByUserAndDate(Constants.Token, users[i].ClickTimeId, _timesheetDate, Constants.Token_Abhijit)
                    .ConfigureAwait(false);

                var timesheetHours = await GetTimesheetHoursByTimesheetId(timesheetDetails?.Data?.TimesheetID, timesheetDetails?.Data?.UserID);

                var timesheetDate = Convert.ToDateTime(timesheetDetails?.Data?.StartDate).AddDays(2);

                for (int j = 0; j < 5; j++)
                {
                    var statusReportingHours = timesheetHours.Where(x => x.TimesheetDate == timesheetDate.ToString("yyyy-MM-dd")
                    && (x.TaskId == "4LAIuDKFMsTqVfv6eUiETeQ2" || x.TaskId == "4O3GMEq-raPBXTwy1SbMfCw2"))?.FirstOrDefault()?.Hours ?? 0;

                    var devHours = timesheetHours.Where(x => x.TimesheetDate == timesheetDate.ToString("yyyy-MM-dd")
                    && (x.TaskId == "4Yq1dSSGY9dUIG9B6uJYwAQ2" || x.TaskId == "4GgxFznnRGZfj1HljViuZxQ2"))?.FirstOrDefault()?.Hours ?? 0;

                    var unitTestsHours = timesheetHours.Where(x => x.TimesheetDate == timesheetDate.ToString("yyyy-MM-dd")
                    && x.TaskId == "4_Hhldajki0u8yQakLH_X2g2")?.FirstOrDefault()?.Hours ?? 0;

                    var codeReviewsHours = timesheetHours.Where(x => x.TimesheetDate == timesheetDate.ToString("yyyy-MM-dd")
                    && (x.TaskId == "4uqvfyYwHEai-_BsUD5unMg2" || x.TaskId == "4fhj7wvpJNfsH1P4REvfu0Q2"))?.FirstOrDefault()?.Hours ?? 0;

                    var defectFixingHours = timesheetHours.Where(x => x.TimesheetDate == timesheetDate.ToString("yyyy-MM-dd")
                     && x.TaskId == "4RrmchWnZgxp7ZvUwyKu1oA2")?.FirstOrDefault()?.Hours ?? 0;

                    var devUnbillableHours = timesheetHours.Where(x => x.TimesheetDate == timesheetDate.ToString("yyyy-MM-dd")
                    && (x.TaskId == "4KX_Pe17ImqRZIlZi5T16dw2"))?.FirstOrDefault()?.Hours ?? 0;

                    var onboardingKTHours = timesheetHours.Where(x => x.TimesheetDate == timesheetDate.ToString("yyyy-MM-dd")
                    && (x.TaskId == "41CpoNMprvKtl0qDyHHX8Bw2"))?.FirstOrDefault()?.Hours ?? 0;

                    var srNo = j + 1;
                    var row = dt.NewRow();

                    row["Sr No"] = srNo;
                    row["Name"] = users[i].Name;
                    //row["Id"] = users[i].EmployeeId;
                    //row["Email"] = users[i].Email;
                    //row["Start Date"] = timesheetDetails?.Data?.StartDate;
                    //row["End Date"] = timesheetDetails?.Data?.EndDate;
                    //row["Status"] = timesheetDetails?.Data?.Status;

                    row["TimesheetDate"] = timesheetDate.ToString("dd-MM-yyyy");
                    row["StatusReporting"] = statusReportingHours;
                    row["Development"] = devHours;
                    row["UnitTesting"] = unitTestsHours;
                    row["CodeReview"] = codeReviewsHours;
                    row["DefectFixing"] = defectFixingHours;
                    row["DevelopmentUnbillable"] = devUnbillableHours;
                    row["Onboarding-KT"] = onboardingKTHours;
                    row["TotalTimesheetHours"] = statusReportingHours + devHours + unitTestsHours + codeReviewsHours
                        + defectFixingHours + devUnbillableHours + onboardingKTHours;

                    dt.Rows.Add(row);
                    timesheetDate = timesheetDate.AddDays(1);
                }
                dt.Rows.Add(dt.NewRow());

            }

            return dt;
        }

        private async Task<List<TimesheetHourData>> GetTimesheetHoursByTimesheetId(string timsheetId, string userId)
        {
            var response = await HttpServices.GetTimesheetHoursByTimesheetId(Constants.Token, timsheetId);

            if (response?.Error?.Any() ?? false)
            {
                throw new Exception(response.Error[0].Message);
            }

            //if (response?.Data.All(x => x.UserID.Equals(userId)) ?? false)
            //{
            //    return response?.Data.Sum(y => y.Hours) ?? 0;
            //}


            return response?.Data;
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
