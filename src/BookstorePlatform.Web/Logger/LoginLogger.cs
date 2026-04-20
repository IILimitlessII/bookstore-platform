namespace BookstorePlatform.Web.Logger
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.IO;

    public sealed class LoginLogger
    {
        private static readonly Lazy<LoginLogger> instance = new Lazy<LoginLogger>(() => new LoginLogger());
        private readonly string logFilePath = "login_log.log";
        private string lastIpAddress = "Unknown";
        private string lastUserAgent = "Unknown";
        private string lastGeoLocation = "Unknown";

        private LoginLogger() { }

        public static LoginLogger Instance => instance.Value;

        public void LogLoginDetails(string username)
        {
            string logMessage = $"--------------------------------------------------------------------------\n" +
                $"{DateTime.Now} - User '{username}'\n" +
                $"IP address :'{lastIpAddress}'\n" +
                $"User-Agent :'{lastUserAgent}'\n" +
                $"Geolocation : '{lastGeoLocation}'\n" +
                $"\n--------------------------------------------------------------------------";

            try
            {
                using (StreamWriter writer = File.AppendText(logFilePath))
                {
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging login details: {ex.Message}");
            }
        }

        public async Task UpdateLoginDetails(string ipAddress, string userAgent)
        {
            lastIpAddress = ipAddress;
            lastUserAgent = userAgent;
            lastGeoLocation = await GetGeoLocation(ipAddress);
        }

        private async Task<string> GetGeoLocation(string ipAddress)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string apiUrl = $"http://ip-api.com/json/{ipAddress}";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(responseBody);
                    string country = json.Value<string>("country") ?? "Unknown";
                    string region = json.Value<string>("regionName") ?? "Unknown";
                    string city = json.Value<string>("city") ?? "Unknown";
                    string latitude = json["lat"]?.ToString() ?? "Unknown";
                    string longitude = json["lon"]?.ToString() ?? "Unknown";
                    return $"{city}, {region}, {country}, {latitude}, {longitude}";
                }
                catch (HttpRequestException)
                {
                    // Log the error or handle it as needed
                    return "Unknown";
                }
            }
        }
    }

}
