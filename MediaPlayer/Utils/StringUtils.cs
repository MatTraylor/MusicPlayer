using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Utils
{
    public static class StringUtils
    {
        public static bool Contains(this String initialString, string comparisonString, StringComparison comparisonType) => initialString?.IndexOf(comparisonString, comparisonType) >= 0;        
    }
}
