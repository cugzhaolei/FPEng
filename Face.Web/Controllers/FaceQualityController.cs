using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Face.Web.Core.FaceAI;
using Face.Web.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Face.Web.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceQualityController : ControllerBase
    {
        private readonly FaceQuality faceQuality = new FaceQuality();

        public FaceQualityController()
        {
        }
    }
}
