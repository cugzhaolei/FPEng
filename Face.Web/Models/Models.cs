using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Face.Web.Core.Models
{
    public class Models
    {
        public class FaceInfo
        {
            public FaceInfo()
            {
                // FaceId = new Guid().ToString("N");
                FaceLogTime = DateTime.Now;
            }
            [Key]
            public string FaceId { get; set; }
            public string FaceGroup { get; set; }
            public string FaceName { get; set; }
            public string FaceDetectionInfo { get; set; }
            public DateTime FaceLogTime { get; set; }
        }

        public class UseInfo
        {
            public string UserId { get; set; }
            public string GroupId { get; set; }
            public string FileName { get; set; }
            public string UserInfo { get; set; }
        }

        public class Info
        {
            public string Title { get; set; }
            public string Version { get; set; }
        }

        public class PostType
        {
            public string Type { get; set; }
        }

        public class UserAdd:PostType
        {
            public string UserId { get; set; }
            public string GroupId { get; set; }
            public string FileName { get; set; }
            public string UserInfo { get; set; }
            public string FilePath { get; set; }
        }
    }
}
