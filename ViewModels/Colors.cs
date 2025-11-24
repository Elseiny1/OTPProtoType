using System.ComponentModel.DataAnnotations;

namespace OTPNumberPrototype.ViewModels
{
    public class Colors
    {
        [Range(0, 255)]
        public int R { get; set; }

        [Range(0, 255)]
        public int G { get; set; }

        [Range(0, 255)]
        public int B { get; set; }

    }
}
