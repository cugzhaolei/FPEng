using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Face.Web.Core.FaceAI;
using Face.Web.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Face.Web.Core.Models.Models;

namespace Face.Web.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// user management
    /// </summary>
    public class UserController : ControllerBase
    {
        public FaceUtil _faceUtil;
        private readonly FaceManager faceManager = new FaceManager();
        private readonly FaceCompare faceCompare = new FaceCompare();
        private readonly FaceTrack faceTrack = new FaceTrack();
        /// <summary>
        /// user controller
        /// </summary>
        public UserController()
        {
            FaceUtil faceUtil = new FaceUtil();
            _faceUtil = faceUtil;
        }
        /// <summary>
        /// get user list info
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        // GET: api/User
        [Route("GetUserList")]
        [HttpGet]
        public IEnumerable<string> GetUserList(string groupid)
        {
            var result = faceManager.GetUserList(groupid);

            return new string[] { result };
        }
        /// <summary>
        /// get user info
        /// </summary>
        /// <param name="userid">user id</param>
        /// <param name="groupid">group id</param>
        /// <returns></returns>
        // GET: api/User/5
        [Route("GetUserInfo")]
        [HttpGet]
        public string GetUserInfo(string userid, string groupid)
        {
            var result = faceManager.GetUserInfo(userid, groupid);
            return result.ToString();
        }
        /// <summary>
        /// user add or update
        /// </summary>
        /// <param name="value"></param>
        // POST: api/User
        [Route("AddOrUpdate")]
        [HttpPost]
        public string Post(UserAdd value)
        {
            try
            {
                string result = "";
                // UserAdd userAdd = JsonConvert.DeserializeObject<UserAdd>(value.ToString());
                UserAdd userAdd = value;
                string time = new DateTime().ToString();
                string type = userAdd.Type == null ? "add" : userAdd.Type;
                string userId = userAdd.UserId == null ? Guid.NewGuid().ToString("N") : userAdd.UserId;
                string groupId = userAdd.GroupId == null ? "test_group" : userAdd.GroupId;
                string fileName = userAdd.FileName == null ? Guid.NewGuid().ToString("N") : userAdd.FileName;
                string userInfo = userAdd.UserInfo == null ? "test_user" : value.UserInfo;
                string filePath = userAdd.FilePath == null ? "G:\\Development\\Application\\testface\\img\\beckham\\2.jpg" : userAdd.FilePath;

                // post add user
                if (type == "add")
                {
                    result = faceManager.UserAdd(userId, groupId, filePath, userInfo);
                }
                // post update user
                else if (type == "update")
                {
                    result = faceManager.UserUpdate(userId, groupId, filePath, userInfo);
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// update info
        /// </summary>
        /// <param name="value"></param>
        // POST: api/User
        [Route("Update")]
        [HttpPost]
        public void Update(UserAdd value)
        {
            try
            {
                // UserAdd userAdd = JsonConvert.DeserializeObject<UserAdd>(value.ToString());
                UserAdd userAdd = value;
                string time = new DateTime().ToString();
                string type = userAdd.Type == null ? "add" : userAdd.Type;
                string userId = userAdd.UserId == null ? Guid.NewGuid().ToString("N") : userAdd.UserId;
                string groupId = userAdd.GroupId == null ? "test_group" : userAdd.GroupId;
                string fileName = userAdd.FileName == null ? Guid.NewGuid().ToString("N") : userAdd.FileName;
                string userInfo = userAdd.UserInfo == null ? "test_user" : value.UserInfo;
                string filePath = userAdd.FilePath == null ? "G:\\Development\\Application\\testface\\img\\beckham\\2.jpg" : userAdd.FilePath;

                // post add user
                if (type == "add")
                {
                    var result = faceManager.UserAdd(userId, groupId, filePath, userInfo);
                }
                // post update user
                else if (type == "update")
                {
                    var result = faceManager.UserUpdate(userId, groupId, filePath, userInfo);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// put 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        // PUT: api/User/5
        [Route("Put")]
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
        [Route("Delete")]
        [HttpDelete]
        public void Delete(string userid, string groupid)
        {
            var result = _faceUtil._faceManager.UserDelete(userid, groupid);
            Console.WriteLine("deleteFace", result);
        }
    }
}
