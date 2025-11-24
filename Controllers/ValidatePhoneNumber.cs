using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using OTPNumberPrototype.Hub;
using OTPNumberPrototype.IRepository;
using OTPNumberPrototype.ViewModels;
using System.Threading.Tasks;

namespace OTPNumberPrototype.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidatePhoneNumber : ControllerBase
    {
        private readonly IMemoryCache _cash;
        private readonly IUserData _userData;
        private readonly ISendOTP _sendOTP;
        private readonly IHubContext<UserDataHub> _userDataHub;

        public ValidatePhoneNumber(IMemoryCache cash, 
            IUserData userData,
            ISendOTP sendOTP,
            IHubContext<UserDataHub> userDataHub)
        {
            _cash = cash;
            _userData = userData;
            _sendOTP = sendOTP;
            _userDataHub = userDataHub;
        }


        [HttpPost]
        [Route("Add Phone")]
        public async Task<IActionResult> AddPhoneNumber([FromBody] PhoneNumber phone)
        {
           if (!ModelState.IsValid) 
                return BadRequest("invalid phone number");

            var secession = new Secession
            {
                Phone = phone.Phone,
                OTP = new Random().Next(10000, 99999),
                Verified = false,
                Attempts = 3
            };

            _cash.Set(phone.Phone, secession, TimeSpan.FromMinutes(5));

            //add the Irepository OTP sender
            string response = await _sendOTP.SendOTPAsync(phone.Phone, secession.OTP);

            if (!_sendOTP.ApiResponse(response))
                return BadRequest(response);

            return Ok(new { Phone = phone.Phone, Message = "OTP sent" });
        }

        [HttpPost]
        [Route("Verify OTP")]
        public IActionResult ValidateOTP(VerifyOTPRequest model)
        {
            if (model.otp <= 100000 && model.otp >= 999999)
                return BadRequest("Invalid OTP Value");


            if (!_cash.TryGetValue(model.phone, out Secession? session) ||
                session is null)
                return BadRequest("Session expired or not found");

            if (session.Attempts <= 0)
            {
                _cash.Remove(model.phone);
                return BadRequest("too many attempts please try again latter");
            }

            if (model.otp != session.OTP)
            {
                session.Attempts--;
                _cash.Set(model.phone, session, TimeSpan.FromMinutes(3));
                return BadRequest($"error, remaining attempts {session.Attempts}");
            }

            session.Verified = true;
            _cash.Set(model.phone, session, TimeSpan.FromMinutes(5));
            _cash.Set("phone", model.phone, TimeSpan.FromMinutes(5));

            return RedirectToAction("GetUserStatus");

        }
        [HttpGet]
        [Route("Get User Data")]
        public async Task<IActionResult> GetUserStatus()
        {
            if (!_cash.TryGetValue("phone", out string? phone) ||
                phone == null)
                return BadRequest("Phone number dosn't exist");

            //if otp is valid or true
            if (!ModelState.IsValid ||
                !_cash.TryGetValue
                (phone, out Secession? secession) ||
                secession is null ||
                !secession.Verified)
                return BadRequest("Session expired");

            var userData = _userData.GenereateUserData();

            await _userDataHub.Clients.All.SendAsync("GetUserStatus", userData);

            return Ok(userData);
        }
    }
}
