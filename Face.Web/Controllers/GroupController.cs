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
    public class GroupController : ControllerBase
    {
        public FaceUtil _faceUtil;
        public FaceManager faceManager = new FaceManager();

        public GroupController()
        {
            //FaceUtil faceUtil = new FaceUtil();
            //faceUtil = _faceUtil;
        }
        /// <summary>
        /// 组列表查询
        /// </summary>
        /// <returns></returns>
        // GET: api/Group
        [Route("GetGroupList")]
        [HttpGet]
        public string GetGroupList()
        {
            var result = faceManager.GetGroupList();
            return  result ;
        }

        /// <summary>
        /// 用户组列表查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Group/5
        [Route("GetGroupUsers/{id}")]
        [HttpGet]
        public string GetGroupUsers(string id)
        {
            var result = faceManager.GetUserList(id);
            
            return result.ToString();
        }
        /// <summary>
        /// 创建用户组
        /// </summary>
        /// <param name="value">用户组id，标识一组用户（由数字、字母、下划线组成），长度限制128B</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public string Add(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var result = faceManager.GroupAdd(value);
                return result;
            }
            else
            {
                return "no group id";
            }
            // return "add group success";
        }
        /// <summary>
        /// 创建用户组
        /// </summary>
        /// <param name="value">用户组id，标识一组用户（由数字、字母、下划线组成），长度限制128B</param>
        /// <returns></returns>
        // POST: api/Group
        [HttpPost]
        [Route("GroupAdd")]
        public string GroupAdd([FromBody] string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                faceManager.GroupAdd(value);
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
        /// 用户组删除
        /// </summary>
        /// <param name="id">用户组id，标识一组用户（由数字、字母、下划线组成），长度限制128B</param>
        // DELETE: api/ApiWithActions/5
        [Route("GroupDelete")]
        [HttpDelete]
        public void Delete(string id)
        {
            var result = faceManager.GroupDelete(id);
            Console.WriteLine("groupDelete", result);
        }
    }
}
