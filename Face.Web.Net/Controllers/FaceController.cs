using Dapper;
using Face.Web.App.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Face.Web.Net.Controllers
{
    /// <summary>
    /// face management
    /// </summary>
    public class FaceController : ApiController
    {
        /// <summary>
        /// face util
        /// </summary>
        public FaceUtil _faceUtil;

        /// <summary>
        /// face management controller
        /// </summary>
        public FaceController()
        {
            FaceUtil faceUtil = new FaceUtil();
            _faceUtil = faceUtil;
        }
        private static string ConnString = "server=127.0.0.1;user id=root;pwd=159357;database=`face-app`;SslMode=none;allowuservariables=True;";

        /// <summary>
        /// get user list 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        // GET: api/Face
        [Route("api/Face/GetUserList")]
        [HttpGet]
        public IEnumerable<string> GetUserList(string groupId)
        {
            var result = _faceUtil._faceManager.GetUserList(groupId);
            _faceUtil._auth.SDK_Destory();
            return new string[] { result };
        }
        /// <summary>
        /// get user info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Face/5
        [Route("api/Face/GetUser")]
        [HttpGet]
        public IHttpActionResult GetUser(string id)
        {
            try
            {
                MySqlConnection mySqlConnection = new MySqlConnection(ConnString);
                dynamic data = new DynamicParameters();
                if (id != null)
                {
                    data = mySqlConnection.Query("select * from `face-app`.faceinfo where FaceId = " + id);
                }
                else
                {
                    data = mySqlConnection.Query("select * from `face-app`.faceinfo Limit 0,100");
                }
                data = _faceUtil._faceManager.GetUserInfo(id, "test_group");
                _faceUtil._auth.SDK_Destory();

                return data;// Json(data);
            }
            catch (Exception err)
            {
                throw err;
            }

            // return "value";
        }
        /// <summary>
        /// post face info
        /// </summary>
        /// <param name="value"></param>
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

        /// <summary>
        /// delete user info
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <param name="faceToken"></param>
        // DELETE: api/ApiWithActions/5
        [Route("api/Face/Delete")]
        [HttpDelete]
        public void Delete(string userId, string groupId, string faceToken)
        {

            var result = _faceUtil._faceManager.UserFaceDelete(userId, groupId, faceToken);

        }

    }
}
