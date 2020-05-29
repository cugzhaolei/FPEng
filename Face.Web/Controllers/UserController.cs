using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Face.Web.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testface;

namespace Face.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public FaceUtil _faceUtil;

        public UserController(FaceUtil faceUtil)
        {
            _faceUtil = faceUtil;
        }
        // GET: api/User
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/User
        [HttpPost]
        public void Post([FromBody] string value)
        {
            string type = "add";
            string userId = "";
            string groupId = "";
            string fileName = "";
            string userInfo = "";
            string filePath = "";
            
            // post add user
            if(type == "add")
            {
                var result = _faceUtil._faceManager.UserAdd(userId,groupId,fileName,userInfo);
            }
            // post update user
            else if(type == "update")
            {
                var result = _faceUtil._faceManager.UserAddByBuffer(userId, groupId, userInfo);
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
