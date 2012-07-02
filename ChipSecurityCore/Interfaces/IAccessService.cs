using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChipSecurityCore.DataTypes;

namespace ChipSecurityCore.Interfaces
{
    public interface IAccessService
    {
        AccessCodeSet CreateAccessCodeSet(string input);
        List<Tuple<string, string>> OrderSecurityTokens(AccessCodeSet accessCodeSet, List<Tuple<string, string>> sortedList, string newStartColor); 
        bool AccessInputIsValid(string input);
    }
}
