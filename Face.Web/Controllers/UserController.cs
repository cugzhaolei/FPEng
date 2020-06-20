using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Face.Web.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using static Face.Web.Models.Models;

namespace Face.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    /// <summary>
    /// user management
    /// </summary>
    public class UserController : ControllerBase
    {
        public FaceUtil _faceUtil;
        /// <summary>
        /// user controller
        /// </summary>
        public UserController()
        {
            //FaceUtil faceUtil = new FaceUtil();
            //_faceUtil = faceUtil;
        }
        /// <summary>
        /// get user list info
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        // GET: api/User
        [Route("api/User/GetUserList")]
        [HttpGet]
        public IEnumerable<string> GetUserList(string groupid)
        {
            var result = _faceUtil._faceManager.GetUserList(groupid);

            return new string[] { result };
        }
        /// <summary>
        /// get user info
        /// </summary>
        /// <param name="userid">user id</param>
        /// <param name="groupid">group id</param>
        /// <returns></returns>
        // GET: api/User/5
        [Route("api/User/GetUserInfo")]
        [HttpGet]
        public string GetUserInfo(string userid, string groupid)
        {
            var result = _faceUtil._faceManager.GetUserInfo(userid, groupid);
            return result.ToString();
        }
        /// <summary>
        /// user add or update
        /// </summary>
        /// <param name="value"></param>
        // POST: api/User
        [Route("api/User/Add")]
        [HttpPost]
        public void Post([FromBody] UserAdd value)
        {
            // UserAdd user = JsonConvert.DeserializeObject<UserAdd>(value);
            string time = new DateTime().ToString();
            string type = value.Type == null ? "add" : value.Type;
            string userId = time;
            string groupId = "test";
            string fileName = value.FileName;
            string userInfo = value.UserInfo == null ? "test user" : value.UserInfo;
            string filePath = value.FilePath == null ? "G:\\Development\\Application\\testface\\img\\beckham\\2.jpg" : value.FilePath;

            // post add user
            if (type == "add")
            {
                var result = _faceUtil._faceManager.UserAdd(userId, groupId, fileName, userInfo);
            }
            // post update user
            else if (type == "update")
            {
                var result = _faceUtil._faceManager.UserAddByBuffer(userId, groupId, userInfo);
            }
        }
        /// <summary>
        /// update info
        /// </summary>
        /// <param name="value"></param>
        // POST: api/User
        [Route("api/User/Update")]
        [HttpPost]
        public void Update([FromBody] string value)
        {
            string type = "add";
            string userId = "";
            string groupId = "";
            string fileName = "";
            string userInfo = "";
            string filePath = "";

            var result = _faceUtil._faceManager.UserUpdate(userId, groupId, fileName, userInfo);
            Console.WriteLine("Update", result);
        }
        /// <summary>
        /// put 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        // PUT: api/User/5
        [HttpPut]
        public void Put(string id, [FromBody] string value)
        {
        }
        /// <summary>
        /// delete user info
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="groupid"></param>
        // DELETE: api/ApiWithActions/5
        [Route("api/User/Delete")]
        [HttpDelete]
        public void Delete(string userid, string groupid)
        {
            var result = _faceUtil._faceManager.UserDelete(userid, groupid);
            Console.WriteLine("deleteFace", result);
        }
    }
}
