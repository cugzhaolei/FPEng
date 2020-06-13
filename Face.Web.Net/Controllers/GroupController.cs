using Face.Web.App.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Face.Web.Net.Controllers
{
    /// <summary>
    /// Group management 
    /// </summary>
    public class GroupController : ApiController
    {
        public FaceUtil _faceUtil;
        /// <summary>
        /// group management
        /// </summary>
        public GroupController()
        {
            FaceUtil faceUtil = new FaceUtil();
            faceUtil = _faceUtil;
        }

        /// <summary>
        /// get group list
        /// </summary>
        /// <returns></returns>
        // GET: api/Group
        [Route("api/Group/GetGroupList")]
        [HttpGet]
        public IEnumerable<string> GetGroupList()
        {
            // var result = _faceUtil._faceManager.GetGroupList();
            testface.FaceManager faceManager = new testface.FaceManager();
            var result = faceManager.GetGroupList();

            return new string[] { result };
        }

        /// <summary>
        /// get group users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Group/5
        [Route("api/Group/GetGroupUsers/{id}")]
        [HttpGet]
        public string GetGroupUsers(string id)
        {
            var result = _faceUtil._faceManager.GetGroupList();
            foreach (var face in result)
            {
            }
            return result.ToString();
        }

        /// <summary>
        /// add user
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Group/Add")]
        public string Add(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                _faceUtil._faceManager.GroupAdd(value);
            }
            else
            {
                return "no group id";
            }
            return "add group success";
        }
        
        /// <summary>
        /// add group
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        // POST: api/Group
        [HttpPost]
        public string Post([FromBody] string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                _faceUtil._faceManager.GroupAdd(value);
            }
            else
            {
                return "no group id";
            }
            return "add group success";
        }

        // PUT: api/Group/5
        [HttpPut]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>
        /// delete group
        /// </summary>
        /// <param name="id"></param>
        // DELETE: api/ApiWithActions/5
        [Route("api/Group/Delete/{id}")]
        [HttpDelete]
        public void Delete(string id)
        {
            var result = _faceUtil._faceManager.GroupDelete(id);
            Console.WriteLine("groupDelete", result);
        }
    }
}

