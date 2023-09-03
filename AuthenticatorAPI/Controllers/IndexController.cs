using AuthenticatorAPI.Data.Models;
using AuthenticatorAPI.Data.Repositories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Newtonsoft.Json;
using AuthenticatorAPI.Convertors;
using Newtonsoft.Json.Linq;

namespace AuthenticatorAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        #region Variables
        private Log _log;
        private ILogRepository _logRepository;
        #endregion

        #region ctor
        public IndexController(Log log, ILogRepository logRepository)
        {
            _log = log;
            _logRepository = logRepository;
        }
        #endregion

        #region Insert a day viewer log into database

        [HttpPost("/sendLog")]
        public IActionResult SendLog(Log log)//string firstRemot, string secondeRemote, int remoteId, int remoteType)
        {
            ClaimsPrincipal user = this.HttpContext.User;
            Log _log = new Log()
            {
                RemoteOne = log.RemoteOne,
                RemoteTwo = log.RemoteTwo,
                RemoteType = log.RemoteType,
                DataId = Convert.ToInt32(this.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("UserId", StringComparison.OrdinalIgnoreCase)).Value),
                DataType = Convert.ToInt32(this.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("RemoteType", StringComparison.OrdinalIgnoreCase)).Value),
                Date = DateTime.UtcNow,
            };
            bool insert = _logRepository.InsertLog(_log);
            BoolToResult res = new BoolToResult();
            res.Result = insert;
            var keyValuePairs = JsonConvert.SerializeObject(res);

            if (insert)
                return Ok(keyValuePairs);
            else
                return BadRequest(keyValuePairs);
        }

        #endregion

    }

    public class BoolToResult
    {
        public bool Result { get; set; }
    }
}

