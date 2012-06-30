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
        public void AccessInputIsValidWillReturnFalseIfInputIsInvalid()
        {
            string input = "this is NOT valid input";
            bool result = service.AccessInputIsValid(input);
            Assert.IsFalse(result);
        }

        [Test]
        public void AccessInputIsValidWillReturnTrueIfInputIsInvalid()
        {
            string input =
                "blue, green blue, green blue, yellow green, yellow orange, red red, green red, orange yellow, blue yellow, red";
            bool result = service.AccessInputIsValid(input);
            Assert.IsTrue(result);
        }


        [Test]
        public void CanCreateAccessCodeSet()
        {
            string input =
                "blue, green blue, green blue, yellow green, yellow orange, red red, green red, orange yellow, blue yellow, red";
            AccessCodeSet codeSet = service.CreateAccessCodeSet(input);
            Assert.AreEqual("blue", codeSet.StartColor);
            Assert.AreEqual("green", codeSet.EndColor);
            Assert.AreEqual(8, codeSet.TokenList.Count());
        }

        [Test]
        public void CanInitialNewSecurityService()
        {
            Assert.IsNotNull(service);
        }

        [Test]
        public void OrderSecurityTokensReturnsListShorterThanOriginalIfItCantOrderAllTokens()
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
            var list = service.OrderSecurityTokens(accessCodeSet, new List<Tuple<string, string>>());
            Assert.IsTrue(list.Count() < accessCodeSet.TokenList.Count());
        }

        [Test]
        public void OrderSecurityTokensReturnsListOfSameLengthAsOriginalIfCanOrderAllTheTokens()
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
            var list = service.OrderSecurityTokens(accessCodeSet, new List<Tuple<string, string>>());
            Assert.IsTrue(list.Count() < accessCodeSet.TokenList.Count());
        }
    }
}