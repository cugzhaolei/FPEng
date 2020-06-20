using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Face.Web.Core.FaceAI;
using Face.Web.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Face.Web.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceSettingController : ControllerBase
    {
        private readonly FaceSetting faceSetting = new FaceSetting();

        public FaceSettingController()
        {
        }
        // GET: api/FaceSetting
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
