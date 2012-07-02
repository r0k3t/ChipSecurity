using System;
using System.Collections.Generic;
using System.Windows.Documents;
using ChipSecurityCore.DataTypes;
using ChipSecurityCore.Interfaces;

namespace SecurityClient
{
    public class SecurityClientViewModel : BaseViewModel
    {
        private readonly IAccessService accessService;
        private List<string> results;
        string failureMsg = @"Cannot unlock master panel";

        private string securityKey;

        public SecurityClientViewModel(IAccessService accessService)
        {
            this.accessService = accessService;
            ValidateAccess = new DelegateCommand(OnValidateAccess);
        }

        private void OnValidateAccess()
        {
            results = new List<string>();
            if (!accessService.AccessInputIsValid(SecurityKey))
            {
                results.Add(failureMsg);
                OnPropertyChanged("Results");
                return;
            }
            
            AccessCodeSet = accessService.CreateAccessCodeSet(SecurityKey);
            var sortedList = accessService.OrderSecurityTokens(AccessCodeSet, new List<Tuple<string, string>>(), null);
            foreach (var tuple in sortedList)
                results.Add(tuple.Item1 + "," + tuple.Item2);

            OnPropertyChanged("Results");
        }

        public string SecurityKey
        {
            get { return securityKey; }
            set
            {
                securityKey = value;
                OnPropertyChanged("SecurityKey");
            }
        }

        public DelegateCommand ValidateAccess { get; set; }

        public List<string> Results
        {
            get { return results; }
        }

        public AccessCodeSet AccessCodeSet { get; set; }

    }
}