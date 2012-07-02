using System.Collections.Generic;
using System.Linq;
using ChipSecurityCore;
using NUnit.Framework;
using SecurityClient;

namespace ChipSecurityUnitTests
{
    [TestFixture]
    internal class SecurityClientViewModelTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            vm = new SecurityClientViewModel(new AccessService());
        }

        #endregion

        private SecurityClientViewModel vm;

        [Test]
        public void ProcessSecurityKey_Will_Set_Results_List_To_Cannot_unlock_master_panel_If_Input_Is_Bad()
        {
            string failureMsg = @"Cannot unlock master panel";
            vm.SecurityKey = "Opps!";
            vm.ValidateAccess.Execute();
            Assert.AreEqual(failureMsg, vm.Results.First());
        }

        [Test]
        public void ProcessSecurityKey_Calls_OnPropertyChanged_On_Results()
        {
            var onPropertyChangedWasCalledWithStringResults = false;
            vm.PropertyChanged += (e, a) =>
                                      {
                                          if (a.PropertyName.Equals("Results"))
                                              onPropertyChangedWasCalledWithStringResults = true;
                                      };
            vm.SecurityKey = "Opps!";
            vm.ValidateAccess.Execute();
            Assert.IsTrue(onPropertyChangedWasCalledWithStringResults);
        }

        [Test]
        public void Can_Set_SecurityKey()
        {
            vm.SecurityKey = "";
            Assert.IsNotNull(vm.SecurityKey);
        }

        [Test]
        public void SecurityClientViewModel_Can_Be_Constructed_With_Instance_Of_AccessService()
        {
            Assert.IsNotNull(vm);
        }

        [Test]
        public void ValidateAccess_Will_Create_And_Set_AccessCodeSet()
        {
            vm.SecurityKey = "blue, green yellow, red orange, purple";
            vm.ValidateAccess.Execute();
            Assert.AreEqual("blue", vm.AccessCodeSet.StartColor);
            Assert.AreEqual("green", vm.AccessCodeSet.EndColor);
            Assert.AreEqual(2, vm.AccessCodeSet.TokenList.Count());
        }

        [Test]
        public void ValidateAccess_Will_Set_Results_To_List_Of_AccessTokens_If_Success()
        {
            vm.SecurityKey = @"blue, green blue, yellow red, orange red, green yellow, red orange, red";
            vm.ValidateAccess.Execute();
            Assert.AreEqual(5, vm.Results.Count());
        }

        [Test]
        public void ValidateAccess_Will_Set_Results_To_List_To_Cannot_unlock_master_panel_If_It_Cant_Order_The_Entire_List()
        {
            string failureMsg = @"Cannot unlock master panel";
            vm.SecurityKey = @"blue, green blue, yellow red, orange red, green yellow, red black, yellow orange, red";
            vm.ValidateAccess.Execute();
            Assert.AreEqual(failureMsg, vm.Results.First());
        }


    }
}