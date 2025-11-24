using System.ComponentModel.DataAnnotations;

namespace OTPNumberPrototype.ViewModels
{
    public class PhoneNumber
    {
        [Required]
        [RegularExpression
            (@"^(?:050|053|054|055|056|057|058|059)\d{7}$",
            ErrorMessage ="Invalid Phone Number or doesn't exist")]
        public required string Phone { get; set; }

    }
}
