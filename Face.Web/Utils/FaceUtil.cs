using Face.Web.Face;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Face.Web.Utils
{

    public class FaceUtil
    {

        public FaceUtil()
        {
            Auth auth = new Auth();
            // auth.SDK_Init();
            Console.WriteLine("sdkinit: ");
        }
        public Auth _auth = new Auth();// { get; set; }
        public FaceCompare _faceCompare = new FaceCompare(); // { get; set; }
        public FaceManager _faceManager = new FaceManager(); // { get; set; }
        public FaceQuality _faceQuality = new FaceQuality(); // { get; set; }
        public FaceSetting _faceSetting = new FaceSetting(); // { get; set; } 
        public FaceAttr _faceAttr = new FaceAttr(); // { get; set; }
        public FaceTrack _faceTrack = new FaceTrack(); //{ get; set; }
        public FaceLiveness _faceLiveness = new FaceLiveness(); // { get; set; }
    }
}

