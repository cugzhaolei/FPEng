using Face.Web.Core.FaceAI;
using System;

namespace Face.Web.Core.Utils
{

    public class FaceUtil
    {
        public FaceCompare _faceCompare { get; set; }
        public FaceManager _faceManager { get; set; }
        public FaceQuality _faceQuality { get; set; }
        public FaceSetting _faceSetting { get; set; } 
        public FaceAttr _faceAttr { get; set; }
        public FaceTrack _faceTrack { get; set; }
        public FaceLiveness _faceLiveness { get; set; }
    }
}

