using System;
using System.Collections.Generic;
using System.Linq;
using ChipSecurityCore;
using ChipSecurityCore.DataTypes;
using ChipSecurityCore.Interfaces;
using NUnit.Framework;

namespace ChipSecurityUnitTests
{
    [TestFixture]
    public class AccessServiceTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            service = new AccessService();
        }

        #endregion

        private IAccessService service;

        [Test]
        public void AccessInputIsValid_Will_Return_False_If_Input_Is_Invalid()
        {
            string input = "this is NOT valid input";
            bool result = service.AccessInputIsValid(input);
            Assert.IsFalse(result);
        }

        [Test]
        public void AccessInputIsValid_Will_Return_True_If_Input_Is_Valid()
        {
            string input =
                "blue, green blue, green blue, yellow green, yellow orange, red red, green red, orange yellow, blue yellow, red";
            bool result = service.AccessInputIsValid(input);
            Assert.IsTrue(result);
        }


        [Test]
        public void Can_Create_Access_Code_Set()
        {
            string input =
                "blue, green blue, green blue, yellow green, yellow orange, red red, green red, orange yellow, blue yellow, red";
            AccessCodeSet codeSet = service.CreateAccessCodeSet(input);
            Assert.AreEqual("blue", codeSet.StartColor);
            Assert.AreEqual("green", codeSet.EndColor);
            Assert.AreEqual(8, codeSet.TokenList.Count());
        }

        [Test]
        public void Can_Initialize_New_SecurityService()
        {
            Assert.IsNotNull(service);
        }

        [Test]
        public void Order_Security_Tokens_Returns_List_Shorter_Than_Original_If_It_Cant_Order_All_Tokens()
        {
            var accessCodeSet = new AccessCodeSet
                                    {
                                        StartColor = "blue",
                                        EndColor = "green",
                                        TokenList = new List<Tuple<string, string>>
                                                        {
                                                            new Tuple<string, string>("blue", "green"),
                                                            new Tuple<string, string>("green", "yellow"),
                                                            new Tuple<string, string>("orange", "red"),
                                                            new Tuple<string, string>("red", "green"),
                                                            new Tuple<string, string>("grey", "white"),
                                                            new Tuple<string, string>("red", "orange"),
                                                            new Tuple<string, string>("yellow", "blue"),
                                                            new Tuple<string, string>("black", "yellow"),
                                                            new Tuple<string, string>("yellow", "black"),
                                                            new Tuple<string, string>("yellow", "grey"),
                                                            new Tuple<string, string>("yellow", "red"),
                                                            new Tuple<string, string>("blue", "yellow"),
                                                        }
                                    };
            var orginalCount = accessCodeSet.TokenList.Count();
            var list = service.OrderSecurityTokens(accessCodeSet, new List<Tuple<string, string>>(), null);
            Assert.IsTrue(orginalCount > accessCodeSet.TokenList.Count());
        }

        [Test]
        public void Order_Security_Tokens_Returns_List_Of_Same_Length_As_Original_If_Can_Order_All_The_Tokens()
        {
            var accessCodeSet = new AccessCodeSet
            {
                StartColor = "blue",
                EndColor = "green",
                TokenList = new List<Tuple<string, string>>
                                                        {
                                                            new Tuple<string, string>("blue", "green"),
                                                            new Tuple<string, string>("green", "yellow"),
                                                            new Tuple<string, string>("orange", "red"),
                                                            new Tuple<string, string>("red", "green"),
                                                            new Tuple<string, string>("red", "orange"),
                                                            new Tuple<string, string>("yellow", "blue"),
                                                            new Tuple<string, string>("black", "yellow"),
                                                            new Tuple<string, string>("yellow", "black"),
                                                            new Tuple<string, string>("yellow", "red"),
                                                            new Tuple<string, string>("blue", "yellow"),
                                                        }
            };
            var orginalCount = accessCodeSet.TokenList.Count();
            accessCodeSet.TokenList = service.OrderSecurityTokens(accessCodeSet, new List<Tuple<string, string>>(), null);
            Assert.AreEqual(orginalCount, accessCodeSet.TokenList.Count());
        }
    }
}