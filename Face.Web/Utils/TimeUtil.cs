using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Face.Web.Core.Utils
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
