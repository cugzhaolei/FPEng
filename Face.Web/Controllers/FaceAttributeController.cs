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
    public class FaceAttributeController : ControllerBase
    {
        private readonly FaceAttr faceAttr = new FaceAttr();
        FaceUtil _faceUtil;
        public FaceAttributeController(FaceUtil faceUtil)
        {
            _faceUtil = faceUtil;
        }

        [HttpGet]
        [Route("GetFaceAttr")]
        public string GetFaceAttr(string file)
        {
            try
            {
                var result = faceAttr.GetFaceAttr(file);
                // var data = JsonConvert.DeserializeObject<JObject>(result);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [HttpGet]
        [Route("GetFaceAttrByBuffer")]
        public string GetFaceAttrByBuffer(byte[] file)
        {
            try
            {
                var result = faceAttr.GetFaceAttrByBuffer(file);
                // var data = JsonConvert.DeserializeObject<JObject>(result);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // GET: api/FaceAttribute/5
        [HttpGet("{id}", Name = "GetByFile")]
        public JObject Get(string file)
        {
            try
            {
                var result = faceAttr.GetFaceAttrByFace(file);
                var data = JsonConvert.DeserializeObject<JObject>(result);
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // POST: api/FaceAttribute
        [HttpPost]
        public JObject Post([FromBody] string file)
        {
            try
            {
                var result = faceAttr.GetFaceAttrByFace(file);
                var data = JsonConvert.DeserializeObject<JObject>(result);
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
