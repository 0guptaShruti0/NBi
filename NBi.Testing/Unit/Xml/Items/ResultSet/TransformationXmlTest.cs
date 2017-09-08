﻿#region Using directives
using System.IO;
using System.Reflection;
using NBi.Core.ResultSet;
using NBi.Xml;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
using NBi.Core.Transformation;
using NBi.Xml.Items.ResultSet;
#endregion

namespace NBi.Testing.Unit.Xml.Items.ResultSet
{
    [TestFixture]
    public class TransformationXmlTest
    {

        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
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

        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.TransformationXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            return manager.TestSuite;
        }

        [Test]
        public void Deserialize_CSharp_CSharpAndCode()
        {
            int testNr = 0;
            
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EqualToXml>());
            var transfo = ((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[0].Transformation;



            Assert.That(transfo.Language, Is.EqualTo(LanguageType.CSharp));
            Assert.That(transfo.Code.Trim, Is.EqualTo("value.Trim().ToUpper();"));
        }

        [Test]
        public void Deserialize_Native_NativeAndNativeTransfo()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            Assert.That(ts.Tests[testNr].Constraints[0], Is.TypeOf<EqualToXml>());
            var transfo = ((EqualToXml)ts.Tests[testNr].Constraints[0]).ColumnsDef[0].Transformation;



            Assert.That(transfo.Language, Is.EqualTo(LanguageType.Native));
            Assert.That(transfo.Code, Is.EqualTo("empty-to-null"));
        }

        [Test]
        public void Serialize_CSharp_CodeTransfo()
        {
            var def = new ColumnDefinitionXml();
            def.TransformationInner = new TransformationXml();
            def.TransformationInner.Language = LanguageType.CSharp;
            def.TransformationInner.Code = "value.Trim().ToUpper();";


            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<ColumnDefinitionXml>(def);

            Assert.That(xml, Is.StringContaining(">value.Trim().ToUpper();<"));
        }

        [Test]
        public void Serialize_Format_CodeTransfo()
        {
            var def = new ColumnDefinitionXml();
            def.TransformationInner = new TransformationXml();
            def.TransformationInner.Language = LanguageType.Format;
            def.TransformationInner.Code = "##.000";


            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<ColumnDefinitionXml>(def);

            Assert.That(xml, Is.StringContaining("format"));
            Assert.That(xml, Is.StringContaining(">##.000<"));
        }

        [Test]
        public void Serialize_Native_CodeTransfo()
        {
            var def = new ColumnDefinitionXml();
            def.TransformationInner = new TransformationXml();
            def.TransformationInner.Language = LanguageType.Native;
            def.TransformationInner.Code = "empty-to-null";


            var manager = new XmlManager();
            var xml = manager.XmlSerializeFrom<ColumnDefinitionXml>(def);

            Assert.That(xml, Is.StringContaining("native"));
            Assert.That(xml, Is.StringContaining(">empty-to-null<"));
        }

    }
}
