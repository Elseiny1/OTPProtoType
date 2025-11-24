using System.ComponentModel.DataAnnotations;

namespace OTPNumberPrototype.ViewModels
{
    public record VerifyOTPRequest(string phone, int otp);
}
