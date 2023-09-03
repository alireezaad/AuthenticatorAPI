using AuthenticatorAPI.Convertors;
using AuthenticatorAPI.Data.Models;
using AuthenticatorAPI.Data.Repositories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthenticatorAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILockRepository _lockRepository;

        public ValuesController(ILockRepository lockRepository)
        {
            _lockRepository = lockRepository;
        }

        [HttpGet("/test")]
        public string test(string pass)
        {
            var Encpass = Encription.Encrypt(pass);// ("TlGVjlm5nkgHYtGzBoRu/cla225wrSu0iV1Exsnnsl4=");
            //var unEncpass = Encription.Decrypt(Encpass.ToString());
            return $"encpass = {Encpass} and unencpass = {pass}";
        }

        [HttpGet("/api/akbare2")]
        //[Route("akbare2")]
        public async Task<ActionResult<Table_lock>> Akbare2(string serial)
        {
            var _lock = await _lockRepository.GetLocksBySerial(serial);

            if (_lock == null)
                return NotFound();

            return Ok(_lock);

        }

        [HttpGet("/WhoIsSignedIn")]
        public  string SubjectId()
        {
            ClaimsPrincipal user = this.HttpContext.User;
            return user?.Claims?.FirstOrDefault(c => c.Type.Equals("DisplayName", StringComparison.OrdinalIgnoreCase))?.Value;

        }

    }
}
