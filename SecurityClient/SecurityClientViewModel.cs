using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            ValidateAccess = new DelegateCommand(OnValidateAccess, x => !string.IsNullOrEmpty(securityKey));
            results = new List<string>();
        }

        private void OnValidateAccess()
        {
            results.Clear();
            var originalTokenListCount = 0;
            if (!accessService.AccessInputIsValid(SecurityKey))
            {
                SetFailureMessege();
                return;
            }
            AccessCodeSet = accessService.CreateAccessCodeSet(SecurityKey);
            originalTokenListCount = AccessCodeSet.TokenList.Count;
            var sortedList = accessService.OrderSecurityTokens(AccessCodeSet, new List<Tuple<string, string>>(), null);
            if(originalTokenListCount > sortedList.Count)
            {
                SetFailureMessege();
                return;
            }
            AccessCodeSet.TokenList = sortedList;
            foreach (var tuple in AccessCodeSet.TokenList)
                results.Add(tuple.Item1 + "," + tuple.Item2);

            OnPropertyChanged("Results");
        }

        private void SetFailureMessege()
        {
            results.Add(failureMsg);
            OnPropertyChanged("Results");
        }

        public string SecurityKey
        {
            get { return securityKey; }
            set
            {
                securityKey = value;
                //OnPropertyChanged("SecurityKey");
                ValidateAccess.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand ValidateAccess { get; set; }

        public ObservableCollection<string> Results
        {
            get { return new ObservableCollection<string>(results); }
        }

        public AccessCodeSet AccessCodeSet { get; set; }

    }
}