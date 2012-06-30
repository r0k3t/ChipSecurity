using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChipSecurityCore.DataTypes
{
    public class AccessCodeSet
    {
        public AccessCodeSet()
        {
            TokenList = new List<Tuple<string, string>>();
        }
        public string StartColor { get; set; }
        public string EndColor { get; set; }
        public List<Tuple<string, string>> TokenList { get; set; } 
    }
}
