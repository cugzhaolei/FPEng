using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Face.Web.Models.Models;

namespace Face.Web.Repository
{
    interface IFaceDataRepository
    {
        int Insert(FaceInfo faceInfo);

        int Insert(List<FaceInfo> faceInfos);

        FaceInfo Query(FaceInfo faceInfo);

        bool IsExist(FaceInfo faceInfo);
    }
}
