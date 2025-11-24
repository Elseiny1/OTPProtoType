using System.ComponentModel.DataAnnotations;

namespace OTPNumberPrototype.ViewModels
{
    public class Secession
    {
        [Required]
        public required string Phone { get; set; }

        [Required]
        public int OTP { get; set; }

        [Required]
        public required bool Verified { get; set; }

        [Required]
        public int Attempts { get; set; }

    }
}
