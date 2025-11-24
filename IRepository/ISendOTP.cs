namespace OTPNumberPrototype.IRepository
{
    public interface ISendOTP
    {
        Task<string> SendOTPAsync(string mobile, int otp);
        bool ApiResponse(string response);


    }
}
