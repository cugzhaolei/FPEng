using Dapper;
using Face.Web.Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static Face.Web.Core.Models.Models;

namespace Face.Web.Core.Repository
{
    public class FaceDataRepository:IFaceDataRepository
    {
        public int Insert(FaceInfo faceInfo)
        {
            using (IDbConnection connection = DBConfig.GetSqlConnection())
            {
                string sql = "insert into `face-app`.faceinfo(FaceId,FaceGroup,FaceName,FaceDetectionInfo,FaceLogTime" +
                    ") values(@FaceId,@FaceGroup,@FaceName,@FaceDetectionInfo,@FaceLogTime)";
                return connection.
                    Execute(sql, faceInfo);
            }
        }

        public int Insert(List<FaceInfo> faceInfoes)
        {
            using (IDbConnection connection = DBConfig.GetSqlConnection())
            {
                string sql = "insert into `face-app`.faceinfo(FaceId,FaceGroup,FaceName,FaceDetectionInfo,FaceLogTime" +
                    ") values(@FaceId,@FaceGroup,@FaceName,@FaceDetectionInfo,@FaceLogTime)";
                return connection.
                    Execute(sql, faceInfoes);
            }
        }

        public IEnumerable<FaceInfo> Query()
        {
            using (IDbConnection connection = DBConfig.GetSqlConnection())
            {
                return connection.Query<FaceInfo>("select * from `face-app`.faceinfo").ToList();
            }
        }

        public FaceInfo Query(FaceInfo faceInfo)
        {
            using (IDbConnection connection = DBConfig.GetSqlConnection())
            {
                return connection.Query<FaceInfo>("select * from `face-app`.faceinfo where FaceId=@FaceId", faceInfo).SingleOrDefault();
            }
        }

        public bool IsExist(FaceInfo faceInfo)
        {
            using (IDbConnection connection = DBConfig.GetSqlConnection())
            {
                var data = connection.Query<FaceInfo>("select * from `face-app`.faceinfo where FaceId=@FaceId", faceInfo).FirstOrDefault();
                return data.FaceId != null;
            }
        }

    }
}
