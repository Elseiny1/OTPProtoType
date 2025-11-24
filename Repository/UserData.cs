using OTPNumberPrototype.IRepository;
using OTPNumberPrototype.ViewModels;
using System.Drawing;

namespace OTPNumberPrototype.Repository
{
    public class UserData : IUserData
    {
        private static readonly Random _random = new();


        #region Generate User Demo Data
        public User GenereateUserData()
        {
            User user = new User
            {
                Code = GenerateCode(),
                ColorCode = GenerateColorCode()
            };
            return user;
        }
        private Colors GenerateColorCode()
        {
            Colors[] colors =
            {
                new Colors { R = 255, G = 255, B = 255 }, // White
                new Colors { R = 0,   G = 0,   B = 255 }, // Blue
                new Colors { R = 255, G = 0,   B = 0   }, // Red
                new Colors { R = 0,   G = 255, B = 0   }, // Green
                new Colors { R = 255, G = 255, B = 0   }  // Yellow
            };
                
            int index = _random.Next(colors.Length);
            return colors[index];
        }
        private string GenerateCode()
        {
            // Generate 3 random uppercase letters
            string letters = new string(Enumerable.Range(0, 3)
                .Select(_ => (char)('A' + _random.Next(0, 26)))
                .ToArray());

            // Generate 3 random digits
            string numbers = new string(Enumerable.Range(0, 3)
                .Select(_ => (char)('0' + _random.Next(0, 10)))
                .ToArray());

            return letters + numbers;
        }
        #endregion

    }
}
