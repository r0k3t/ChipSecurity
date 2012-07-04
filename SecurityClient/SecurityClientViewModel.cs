using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ChipSecurityCore.DataTypes;
using ChipSecurityCore.Interfaces;

namespace SecurityClient
{
    public class SecurityClientViewModel : BaseViewModel
    {
        private readonly IAccessService accessService;
        private readonly List<string> predefinedKeys;
        private readonly List<string> results;
        private const string FailureMsg = @"Cannot unlock master panel";

        private string securityKey;

        public SecurityClientViewModel(IAccessService accessService)
        {
            this.accessService = accessService;
            ValidateAccess = new DelegateCommand(OnValidateAccess, x => !string.IsNullOrEmpty(securityKey));
            results = new List<string>();
            predefinedKeys = new List<string>
                                 {
                                     "blue, green blue, yellow red, orange red, green yellow, red orange, purple",
                                     "blue, green blue, yellow red, orange red, green yellow, red orange, red",
                                     "blue, green blue, green blue, yellow green, yellow orange, red red, green red, orange yellow, blue yellow, red",
                                 };
            UsePredefinedKeyOneCommand = new DelegateCommand(() => SecurityKey = predefinedKeys[0]);
            UsePredefinedKeyTwoCommand = new DelegateCommand(() => SecurityKey = predefinedKeys[1]);
            UsePredefinedKeyThreeCommand = new DelegateCommand(() => SecurityKey = predefinedKeys[2]);
        }

        public string SecurityKey
        {
            get { return securityKey; }
            set
            {
                securityKey = value;
                OnPropertyChanged("SecurityKey");
                ValidateAccess.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand ValidateAccess { get; set; }

        public DelegateCommand UsePredefinedKeyOneCommand { get; set; }
        public DelegateCommand UsePredefinedKeyTwoCommand { get; set; }
        public DelegateCommand UsePredefinedKeyThreeCommand { get; set; }

        public ObservableCollection<string> Results
        {
            get { return new ObservableCollection<string>(results); }
        }

        public AccessCodeSet AccessCodeSet { get; set; }

        private void OnValidateAccess()
        {
            results.Clear();
            int originalTokenListCount = 0;
            if (!accessService.AccessInputIsValid(SecurityKey))
            {
                SetFailureMessege();
                return;
            }
            AccessCodeSet = accessService.CreateAccessCodeSet(SecurityKey);
            originalTokenListCount = AccessCodeSet.TokenList.Count;
            List<Tuple<string, string>> sortedList = accessService.OrderSecurityTokens(AccessCodeSet,
                                                                                       new List<Tuple<string, string>>(),
                                                                                       null);
            if (originalTokenListCount > sortedList.Count)
            {
                SetFailureMessege();
                return;
            }
            AccessCodeSet.TokenList = sortedList;
            if (!AccessCodeSet.TokenList.Last().Item2.Equals(AccessCodeSet.EndColor))
            {
                SetFailureMessege();
                return;
            }
            foreach (var tuple in AccessCodeSet.TokenList)
                results.Add(tuple.Item1 + "," + tuple.Item2);
            
            OnPropertyChanged("Results");
        }

        private void SetFailureMessege()
        {
            results.Add(FailureMsg);
            OnPropertyChanged("Results");
        }
    }
}