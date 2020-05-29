using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace testface
{
   public class FileUtil
    {
        // 把byte保存成二进制文件
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
