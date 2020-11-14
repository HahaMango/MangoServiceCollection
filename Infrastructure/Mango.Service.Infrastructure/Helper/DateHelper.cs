using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Service.Infrastructure.Helper
{
    public class DateHelper
    {
        /// <summary>
        /// 时间间隔秒数
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static int SecondOfTimeSpan(TimeSpan timeSpan)
        {
            return (int)timeSpan.TotalSeconds;
        }

        /// <summary>
        /// 每天秒数
        /// </summary>
        /// <returns></returns>
        public static int SecondOfDay(int day)
        {
            return SecondOfTimeSpan(new TimeSpan(1, 0, 0, 0));
        }
    }
}
