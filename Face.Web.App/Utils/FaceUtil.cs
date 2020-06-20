using System;
using System.Collections.Generic;
using System.Text;
using testface;

namespace Face.Web.App.Utils
{
    /// <summary>
    /// face recognition utils
    /// </summary>
    public class FaceUtil
    {
        public FaceUtil()
        {
            Auth auth = new Auth();
            auth.SDK_Init();
            Console.WriteLine("sdkinit: ");
        }
        public Auth _auth { get; set; }
        public FaceCompare _faceCompare { get; set; }
        public FaceManager _faceManager { get; set; }
        public FaceQuality _faceQuality { get; set; }
        public FaceSetting _faceSetting { get; set; }
        public FaceAttr _faceAttr { get; set; }
        public FaceTrack _faceTrack { get; set; }
        public FaceLiveness _faceLiveness { get; set; }
    }
}
