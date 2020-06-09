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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public FaceUtil _faceUtil;

        public UserController(FaceUtil faceUtil)
        {
            _faceUtil = faceUtil;
        }
        // GET: api/User
        //[Route("api/[controller]/GetUserList/{id}")]
        [HttpGet("{id}")]
        public IEnumerable<string> GetUserList(string groupid)
        {
            var result = _faceUtil._faceManager.GetUserList(groupid);

            return new string[] { result };
        }

        // GET: api/User/5
        //[Route("api/[controller]/GetUserInfo")]
        [HttpGet("{id}")]
        public string GetUserInfo(string userid,string groupid)
        {
            var result = _faceUtil._faceManager.GetUserInfo(userid,groupid);
            return result.ToString();
        }

        // POST: api/User
        //[Route("api/[controller]/Add")]
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
        // POST: api/User
        //[Route("api/[controller]/Update")]
        [HttpPost]
        public void Update([FromBody] string value)
        {
            string type = "add";
            string userId = "";
            string groupId = "";
            string fileName = "";
            string userInfo = "";
            string filePath = "";

            var result = _faceUtil._faceManager.UserUpdate(userId,groupId,fileName,userInfo);
            Console.WriteLine("Update", result);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        //[Route("api/[controller]/Delete/{userid}&{groupdid}")]
        [HttpDelete("{id}")]
        public void Delete(string userid,string groupid)
        {
            var result = _faceUtil._faceManager.UserDelete(userid, groupid);
            Console.WriteLine("deleteFace", result);
        }
    }
}
