using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NblClassLibrary.Models
{
    public static class Calculator
    {
        public static string TimeDuration(DateTime startDateTime, DateTime endDateTime)
        {
            TimeSpan timeSpan = endDateTime - startDateTime;
            return $"{timeSpan}";
        }
    }
}
