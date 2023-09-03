using AuthenticatorAPI.Convertors;
using AuthenticatorAPI.Data.DataAcces;
using AuthenticatorAPI.Data.Models;
using AuthenticatorAPI.Data.Repositories.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;

namespace AuthenticatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region Variables

        public IConfiguration _configuration;
        private IUserRepository _userRepository;
        private ILockRepository _lockRepository;
        private AuthenticationResponse _authenticationResponse;

        #endregion

        #region Ctor

        public AuthenticationController(IConfiguration config, IUserRepository userRepository, ILockRepository lockRepository, AuthenticationResponse authenticationResponse)
        {
            _configuration = config;
            _userRepository = userRepository;
            _lockRepository = lockRepository;
            _authenticationResponse = authenticationResponse;
        }

        #endregion

        #region Authenticate user

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate(UserInfo _userData)
        {
            if (_userData != null && _userData.Fld_User_UserName != null && _userData.Fld_User_Password != null && !string.IsNullOrEmpty(_userData.Fld_User_UserName) && !string.IsNullOrEmpty(_userData.Fld_User_Password))
            {
                var user = await GetUser(_userData.Fld_User_UserName, _userData.Fld_User_Password);

                if (user != null)
                {
                    #region Make JWT Token
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Fld_User_ID.ToString()),
                        new Claim("DisplayName", user.Fld_User_NameL1),
                        new Claim("UserName", user.Fld_User_UserName),
                        new Claim("RemoteType", "1")
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddHours(2),
                        signingCredentials: signIn);

                    var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(token);

                    #endregion

                    _authenticationResponse.UserInfo = user;
                    _authenticationResponse.UserInfo.Fld_User_Password = _userData.Fld_User_Password;
                    _authenticationResponse.Token = tokenToReturn;
                    _authenticationResponse.Status = true;
                    _authenticationResponse.Message = "Succeed";
                    _authenticationResponse.UserKind = 1;

                    var authenticationResponseAsJson = JsonConvert.SerializeObject(_authenticationResponse);

                    return Ok(authenticationResponseAsJson);
                }
                else if (user == null)
                {
                    var _lock = await GetLock(_userData.Fld_User_UserName, _userData.Fld_User_Password);
                    if (_lock != null && _lock.Fld_Lock_BlackList == false)
                    {
                        DateTime lockSupportEndDate = _lockRepository.GetLockSupportEndDate(_lock.Fld_Lock_ID);
                        if (lockSupportEndDate >= DateTime.Now)
                        {
                            #region Make JWT Token
                            //create claims details based on the user information
                            var claims = new[]
                            {
                                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                                new Claim("UserId", _lock.Fld_Lock_ID.ToString()),
                                new Claim("DisplayName", _lock.Fld_Lock_Serial),
                                new Claim("UserName", _lock.Fld_Lock_Model),
                                new Claim("RemoteType", "2")
                            };

                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            var token = new JwtSecurityToken(
                                _configuration["Jwt:Issuer"],
                                _configuration["Jwt:Audience"],
                                claims,
                                expires: DateTime.UtcNow.AddHours(2),
                                signingCredentials: signIn);
                            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(token);

                            #endregion
                            string customerName = "";
                            if (_lock.Fld_Moshtarian_ID != null)
                                customerName = _lockRepository.GetLockCustomerName(_lock.Fld_Moshtarian_ID);
                            else if (_lock.Fld_Moshtarian_ID == null)
                                customerName = _lockRepository.GetLockCustomerName(_lock.Fld_Moshtarian_IDNamayandeh);
                            else
                                customerName = "نا مشخص";

                            _authenticationResponse.LockInfo = _lock;
                            _authenticationResponse.Message = "احراز هویت با موفقیت انجام شد";
                            _authenticationResponse.Status = true;
                            _authenticationResponse.Token = tokenToReturn;
                            _authenticationResponse.UserKind = 2;
                            _authenticationResponse.CustomerName = customerName;

                            var authenticationResponseAsJson = JsonConvert.SerializeObject(_authenticationResponse);

                            return Ok(authenticationResponseAsJson);
                        }
                        else
                        {
                            _authenticationResponse.Status = false;
                            _authenticationResponse.Message = string.Empty;
                            _authenticationResponse.Token = string.Empty;
                            _authenticationResponse.Message = "خدمات این قفل به اتمام رسیده";
                            var authenticationResponseAsJson = JsonConvert.SerializeObject(_authenticationResponse);
                            return NotFound(authenticationResponseAsJson);
                        }
                    }
                    else
                    {
                        _authenticationResponse.Status = false;
                        _authenticationResponse.Message = string.Empty;
                        _authenticationResponse.Token = string.Empty;
                        _authenticationResponse.Message = "اطلاعات وارد شده صحیح نمی باشد یا قفل در لیست سیاه است";
                        var authenticationResponseAsJson = JsonConvert.SerializeObject(_authenticationResponse);
                        return NotFound(authenticationResponseAsJson);
                    }
                }
                else
                {
                    _authenticationResponse.Status = false;
                    _authenticationResponse.Message = string.Empty;
                    _authenticationResponse.Token = string.Empty;
                    _authenticationResponse.Message = "اطلاعات نا معتبر";
                    var authenticationResponseAsJson = JsonConvert.SerializeObject(_authenticationResponse);
                    return NotFound(authenticationResponseAsJson);
                }

            }
            else
            {
                _authenticationResponse.Status = false;
                _authenticationResponse.Message = string.Empty;
                _authenticationResponse.Token = string.Empty;
                _authenticationResponse.Message = "اطلاعات وارد شده صحیح نمی باشد";
                var authenticationResponseAsJson = JsonConvert.SerializeObject(_authenticationResponse);
                return BadRequest(authenticationResponseAsJson);

            }
        }

        private async Task<UserInfo> GetUser(string userName, string password)
        {
            bool isUserExist = _userRepository.IsUserExist(userName, Encription.Encrypt(password));

            if (isUserExist)
                return await _userRepository.GetUser(userName, Encription.Encrypt(password));
            else
                return null;
        }

        private async Task<Table_lock> GetLock(string userName, string password)
        {
            bool isSerialExist = _lockRepository.IsSerialExist(userName);
            if (isSerialExist)
                return await _lockRepository.GetLocksBySerial(userName);
            else
                return null;

        }

        #endregion

    }
}
