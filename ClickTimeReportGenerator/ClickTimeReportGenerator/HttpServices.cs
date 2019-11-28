using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClickTimeReportGenerator
{
    public class HttpServices
    {

        public static HttpClient Client { get; set; }
        public static async Task<GetMeResponse> GetMe(string token)
        {
            InitializeClient();

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Authorization", $"Token {token}");

            var response = await Client.GetAsync(new Uri(Constants.GetMe)).ConfigureAwait(false);
            if (response != null)
            {
                return JsonConvert.DeserializeObject<GetMeResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            return null;
        }

        public static async Task<GetManagedUsersResponse> GetManagedUsers(string token)
        {
            InitializeClient();

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Authorization", $"Token {token}");

            var response = await Client.GetAsync(new Uri(Constants.GetManagedUsers)).ConfigureAwait(false);
            if (response != null)
            {
                return JsonConvert.DeserializeObject<GetManagedUsersResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            return null;
        }

        public static async Task<GetTimesheetResponse> GetTimesheetByUserAndDate(string token, string userId, string date)
        {
            InitializeClient();

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Authorization", $"Token {token}");

            string url = string.Format(Constants.GetTimesheet, date, userId);

            var response = await Client.GetAsync(new Uri(url)).ConfigureAwait(false);
            if (response != null)
            {
                return JsonConvert.DeserializeObject<GetTimesheetResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            return null;
        }

        public static async Task<GetTimesheetHoursResponse> GetTimesheetHoursByTimesheetId(string token, string timesheetId)
        {
            InitializeClient();

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Authorization", $"Token {token}");

            string url = string.Format(Constants.GetTimesheetHours, timesheetId);

            var response = await Client.GetAsync(new Uri(url)).ConfigureAwait(false);
            if (response != null)
            {
                return JsonConvert.DeserializeObject<GetTimesheetHoursResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            return null;
        }

        public static async Task<GetTimeOffResponse> GetTimeOffByTimesheetId(string token, string timesheetId)
        {
            InitializeClient();

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("Authorization", $"Token {token}");

            string url = string.Format(Constants.GetTimeOffByTimesheetId, timesheetId);

            var response = await Client.GetAsync(new Uri(url)).ConfigureAwait(false);
            if (response != null)
            {
                return JsonConvert.DeserializeObject<GetTimeOffResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            return null;
        }

        private static HttpClient InitializeClient()
        {
            if (Client == null)
            {
                Client = new HttpClient();
            }
            return Client;
        }

    }
}
