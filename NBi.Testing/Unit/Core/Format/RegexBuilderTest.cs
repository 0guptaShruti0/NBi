﻿using System;
using System.Data;
using System.Linq;
using NBi.Core.Format;
using NBi.Core.Query;
using NBi.Xml.Items.Format;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.Format
{
    [TestFixture]
    public class RegexBuilderTest
    {
        
        #region SetUp & TearDown
        //Called only at instance creation
        [SetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        [Test]
        public void Build_NumericFormat_CorrectRegex()
        {
            var builder = new RegexBuilder();
            var result = builder.Build(
                    new NumericFormatXml()
                        {
                            DecimalDigits=2,
                            DecimalSeparator=".",
                            GroupSeparator=","
                        }
                );

            Assert.That(result, Is.EqualTo(@"^?[0-9]{1,3}(?:\,?[0-9]{3})*\.[0-9]{2}$"));
            Assert.That("1,125,125.21", Does.Match(result));
        }

        [Test]
        public void Build_NumericFormatWithoutGroupSeparator_CorrectRegex()
        {
            var builder = new RegexBuilder();
            var result = builder.Build(
                    new NumericFormatXml()
                    {
                        DecimalDigits = 3,
                        DecimalSeparator = ",",
                        GroupSeparator = ""
                    }
                );

            Assert.That(result, Is.EqualTo(@"^?[0-9]*\,[0-9]{3}$"));
            Assert.That("1125125,215", Does.Match(result));
        }

        [Test]
        public void Build_NumericFormatWithoutDecimal_CorrectRegex()
        {
            var builder = new RegexBuilder();
            var result = builder.Build(
                    new NumericFormatXml()
                    {
                        DecimalDigits = 0,
                        GroupSeparator = ""
                    }
                );

            Assert.That(result, Is.EqualTo(@"^?[0-9]*$"));
            Assert.That("1125", Does.Match(result));
        }

        [Test]
        public void Build_NumericFormatWithoutDecimalByWithDefaultGroupSeparator_CorrectRegex()
        {
            var builder = new RegexBuilder();
            var result = builder.Build(
                    new NumericFormatXml()
                    {
                        DecimalDigits = 0
                    }
                );

            Assert.That(result, Is.EqualTo(@"^?[0-9]{1,3}(?:\,?[0-9]{3})*$"));
            Assert.That("1,125", Does.Match(result));
        }

        [Test]
        public void Build_CurrencyFormat_CorrectRegex()
        {
            var builder = new RegexBuilder();
            var result = builder.Build(
                    new CurrencyFormatXml()
                    {
                        DecimalDigits = 2,
                        DecimalSeparator = ".",
                        GroupSeparator = ",",
                        CurrencySymbol = "$",
                        CurrencyPattern=CurrencyPattern.Prefix
                    }
                );

            Assert.That(result, Is.EqualTo(@"^\$?[0-9]{1,3}(?:\,?[0-9]{3})*\.[0-9]{2}$"));
            Assert.That("$1,125,125.21", Does.Match(result));
        }

        [Test]
        public void Build_CurrencyFormatSpaceEuro_CorrectRegex()
        {
            var builder = new RegexBuilder();
            var result = builder.Build(
                    new CurrencyFormatXml()
                    {
                        DecimalDigits = 2,
                        DecimalSeparator = ",",
                        GroupSeparator = " ",
                        CurrencySymbol = "€",
                        CurrencyPattern = CurrencyPattern.SuffixSpace
                    }
                );

            Assert.That(result, Is.EqualTo(@"^?[0-9]{1,3}(?:\s?[0-9]{3})*\,[0-9]{2}\s\€$"));
            Assert.That("1 125 125,21 €", Does.Match(result));
        }

        [Test]
        public void Build_CurrencyFormatSpaceKiloEuro_CorrectRegex()
        {
            var builder = new RegexBuilder();
            var result = builder.Build(
                    new CurrencyFormatXml()
                    {
                        DecimalDigits = 2,
                        DecimalSeparator = ",",
                        GroupSeparator = " ",
                        CurrencySymbol = "k€",
                        CurrencyPattern = CurrencyPattern.SuffixSpace
                    }
                );

            Assert.That(result, Is.EqualTo(@"^?[0-9]{1,3}(?:\s?[0-9]{3})*\,[0-9]{2}\sk\€$"));
            Assert.That("1 125 125,21 k€", Does.Match(result));
        }

        [Test]
        public void Build_CurrencyFormatSpaceKiloEuroinLetter_CorrectRegex()
        {
            var builder = new RegexBuilder();
            var result = builder.Build(
                    new CurrencyFormatXml()
                    {
                        DecimalDigits = 2,
                        DecimalSeparator = ",",
                        GroupSeparator = " ",
                        CurrencySymbol = "Kilo Euro (k€)",
                        CurrencyPattern = CurrencyPattern.SuffixSpace
                    }
                );

            Assert.That(result, Is.EqualTo(@"^?[0-9]{1,3}(?:\s?[0-9]{3})*\,[0-9]{2}\sKilo\sEuro\s\(k\€\)$"));
            Assert.That("1 125 125,21 Kilo Euro (k€)", Does.Match(result));
        }
    }
}
