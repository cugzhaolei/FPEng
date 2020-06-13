using System;
using System.Collections.Generic;
using System.Text;

namespace Face.Web.App.Utils
{
    public class TimeUtil
    {
        public static long get_time_stamp()
        {
            TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
            return (long)ts.TotalMilliseconds;
        }
    }
}
