using System;
using System.Collections.Generic;
using Dapper;
using Face.Web.Face;
using Face.Web.Utils;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Face.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    /// <summary>
    /// face management
    /// </summary>
    public class FaceController : ControllerBase
    {
        public FaceUtil _faceUtil;
        public FaceManager faceManager = new FaceManager();

        /// <summary>
        /// face management controller
        /// </summary>
        /// <param name="faceUtil"></param>
        public FaceController()
        {
            // Face.Auth auth = new Face.Auth();
            // FaceUtil faceUtil = new FaceUtil();
            //_faceUtil = faceUtil;
        }
        private static string ConnString = "server=127.0.0.1;user id=root;pwd=159357;database=`face-app`;SslMode=none;allowuservariables=True;";

        // GET: api/Face
        [Route("api/Face/GetUserList")]
        [HttpGet]
        public IEnumerable<string> GetUserList(string groupId)
        {
            // FaceManager faceManager = new FaceManager();
            var result = faceManager.GetUserList(groupId);
            return new string[] { result };
        }

        // GET: api/Face/5
        [Route("api/Face/GetUser")]
        [HttpGet]
        public IActionResult GetUser(string id)
        {
            try
            {
                MySqlConnection mySqlConnection = new MySqlConnection(ConnString);
                dynamic data = new DynamicParameters();
                //if (id != null)
                //{
                //    data = mySqlConnection.Query("select * from `face-app`.faceinfo where FaceId = " + id);
                //}
                //else
                //{
                //    data = mySqlConnection.Query("select * from `face-app`.faceinfo Limit 0,100");
                //}
                data = faceManager.GetUserInfo(id, "test_group");
                return data;// Json(data);
            }
            catch (Exception err)
            {
                throw err;
            }

            // return "value";
        }

        // POST: api/Face
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Face/5
        [HttpPut]
        public void Put(string id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [Route("api/Face/Delete")]
        [HttpDelete]
        public void Delete(string userId, string groupId, string faceToken)
        {

            var result = faceManager.UserFaceDelete(userId, groupId, faceToken);

        }

    }
}
