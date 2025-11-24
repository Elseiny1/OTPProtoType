namespace OTPNumberPrototype.Repository
{
    public class SendOTP : ISendOTP
    {
        //send OTP and check if it's success by returning true or false
        private readonly HttpClient _httpClient;
        private readonly string? _apiKey;

        public SendOTP(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = Environment.GetEnvironmentVariable("ApiKey");
        }

        public async Task<string> SendOTPAsync(string mobile, int otp)
        {
            string message = $"رمز التأكيد هو {otp}";

            if (_apiKey == null) 
                return "didn't find api key";

            var url = $"https://api-server14.com/api/send.aspx" +
                  $"?apikey={_apiKey}" +
                  $"&language=1" +
                  $"&sender=TEST" +
                  $"&mobile={mobile}" +
                  $"&message={Uri.EscapeDataString(message)}";
            try
            {
                var response = await _httpClient.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                
                return result;
            }
            catch (Exception e)
            {
                return "Exception occured";
            }
        }
        public bool ApiResponse(string response)
        {
            var result = response.Split(',');
            if (result[0] == "error")
                return false;

            return true;
        }

    }
   
}
