using System;
using System.IO;

namespace testface
{
    public class ImageUtil
    {
        // 图片文件转bytes
        public static byte[] img2byte(System.Drawing.Image img)
        {
            //将Image转换成流数据，并保存为byte[]
            MemoryStream mstream = new MemoryStream();
            img.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] byData = new Byte[mstream.Length];
            mstream.Position = 0;
            mstream.Read(byData, 0, byData.Length);
            mstream.Close();
            return byData;
        }
        // bytes转图片文件
        public static void byte2img(byte[] b, int len, string file_name)
        {
            MemoryStream ms = new MemoryStream(b);
            ms.Position = 0;
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            img.Save(file_name);
            ms.Close();
        }
    }
}
