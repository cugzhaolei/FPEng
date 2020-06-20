using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Face.Web.Core.FaceAI
{
   public class FileUtil
    {
        /// <summary>
        /// 把byte保存成二进制文件
        /// </summary>
        /// <param name="file_name"></param>
        /// <param name="b"></param>
        /// <param name="len"></param>
        public static void byte2file(string file_name, byte[] b, int len)
        {
            FileStream fs = new FileStream(file_name, FileMode.OpenOrCreate);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(b, 0, len);
            bw.Close();
            fs.Close();

        }
    }
}
