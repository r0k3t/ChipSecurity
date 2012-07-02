using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ChipSecurityCore.DataTypes;
using ChipSecurityCore.Interfaces;

namespace ChipSecurityCore
{
    public class AccessService : IAccessService
    {
        private const string ValidInputSetRegEx = @"(\w+,\s\w+)";

        public AccessCodeSet CreateAccessCodeSet(string input)
        {
            var codeSet = new AccessCodeSet();
            var list = GetInputSets(input);
            codeSet.StartColor = list.First().Split(',').First().Trim();
            codeSet.EndColor = list.First().Split(',').Skip(1).First().Trim();
            foreach (var token in list.Skip(1))
                codeSet.TokenList.Add(CreateSingleCodeToken(token));

            return codeSet;
        }

        public List<Tuple<string, string>> OrderSecurityTokens(AccessCodeSet accessCodeSet, List<Tuple<string, string>> sortedList, string newStartColor)
        {
            var startColor = accessCodeSet.StartColor;
            if (!string.IsNullOrEmpty(newStartColor))
                startColor = newStartColor;
            if (accessCodeSet.TokenList.Count(x => x.Item1.Equals(startColor)) == 1)
            {
                sortedList.Add(accessCodeSet.TokenList.Single(x => x.Item1.Equals(startColor)));
                accessCodeSet.TokenList = accessCodeSet.TokenList.ToList().Except(new List<Tuple<string, string>> { sortedList.Last() }).ToList();
            }
            else
            {
                var listOfTokensWithSameStartColor = accessCodeSet.TokenList.Where(x => x.Item1.Equals(startColor));
                if (!listOfTokensWithSameStartColor.Any())
                    return sortedList;
                foreach (var token in listOfTokensWithSameStartColor)
                {
                    if (accessCodeSet.TokenList.Count(x => x.Item1.Equals(token.Item2)) == 1)
                    {
                        sortedList.Add(token);
                        accessCodeSet.TokenList = accessCodeSet.TokenList.ToList().Except(new List<Tuple<string, string>> { token }).ToList();
                        sortedList.Add(accessCodeSet.TokenList.Single(x => x.Item1.Equals(token.Item2)));
                        accessCodeSet.TokenList = accessCodeSet.TokenList.ToList().Except(new List<Tuple<string, string>> { sortedList.Last() }).ToList();
                    }
                }
            }
            if (accessCodeSet.TokenList.Any())
                OrderSecurityTokens(accessCodeSet, sortedList, sortedList.Last().Item2);
            return sortedList;
        }

        private Tuple<string, string> CreateSingleCodeToken(string input)
        {
            var item1 = input.Split(',').First().Trim();
            var item2 = input.Split(',').Skip(1).First().Trim();
            return new Tuple<string, string>(item1, item2);
        }

        public bool AccessInputIsValid(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            IEnumerable<string> inputSets = GetInputSets(input);
            if (inputSets.Any(x => !Regex.Match(x, ValidInputSetRegEx).Success))
                return false;

            return true;
        }

        private IEnumerable<String> GetInputSets(string input)
        {
            var list = Regex.Split(input, ValidInputSetRegEx);
            return list.Where(x => !string.IsNullOrEmpty(x.Trim()));
        }
    }
}