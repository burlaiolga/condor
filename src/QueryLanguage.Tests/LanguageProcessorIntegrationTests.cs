using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueryLanguage.Models;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Common;
using QueryLanguage.Analysis;

namespace QueryLanguage.Tests
{
    [TestClass]
    public class LanguageProcessorIntegrationTests
    {
        LanguageProcessor languageProcessor = new LanguageProcessor();

        List<Country> allCountries = new List<Country>()
            {
                new Country()
                {
                    Population = 20,
                    Name = "NewZealand"
                },
                new Country()
                {
                    Population = 100,
                    Name = "Ukraine"
                },
                new Country()
                {
                    Population = 200,
                    Name = "Netherlands"
                },
                new Country()
                {
                    Population = 250,
                    Name = "Australia"
                }
            };

        [TestMethod]
        public void TestSimpleCondition()
        {
            string expression = "Name eq Australia";
            var countries = languageProcessor.Find(allCountries, expression);
            countries.Should().Contain(allCountries.Last());
            countries.Count.Should().IsSameOrEqualTo(1);

        }

        [TestMethod]
        public void TestConditionWithLogicalOperator()
        {
            string expression = "Name eq Australia or Population ge 100 and Population lt 201";
            var countries = languageProcessor.Find(allCountries, expression);
           countries.Count.Should().IsSameOrEqualTo(3);
        }

        //"Name eq Australia or Population", "or and eq",  "100 eq 201" "Australia", "Australia or Population"
        [TestMethod]
        [DataRow("or and eq")]
        [DataRow("Name eq Australia or Population")]
        [DataRow("Population like 20")]
        [DataRow("Name gt 1")]
        [ExpectedException(typeof(ExpressionInvalidException))]
        public void TestInvalidConditionThrowsException(string expression)
        {
            languageProcessor.Find(allCountries, expression);
        }

        //TODO add more tests
    }
}
