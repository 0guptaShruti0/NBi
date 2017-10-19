﻿using NBi.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core
{
    public class CsvReaderTest
    {
        [Test]
        [TestCase(null, "")]
        [TestCase("(null)", null)] //Parse (null) to a real null value
        [TestCase("\"(null)\"", "(null)")] //Explicitly quoted (null) should be (null)
        [TestCase("null", "null")]
        [TestCase("", "")]
        [TestCase("a", "a")]
        [TestCase("\"", "\"")]
        [TestCase("\"a", "\"a")]
        [TestCase("ab", "ab")]
        [TestCase("\"ab\"", "ab")]
        [TestCase("abc", "abc")]
        [TestCase("\"abc\"", "abc")]
        [TestCase("\"a\"b\"", "a\"b")]
        [TestCase("\"\"a\"b\"\"", "\"a\"b\"")]
        public void RemoveTextQualifier_String_CorrectString(string item, string result)
        {
            var reader = new CsvReader();
            var value = reader.RemoveTextQualifier(item);
            Assert.That(value, Is.EqualTo(result));
        }

        public void SplitLine_Null_NotEmpty()
        {
            var reader = new CsvReader();
            var values = reader.SplitLine("a;(null)");
            Assert.That(values.ElementAt(1), Is.Null);
        }

        [Test]
        [TestCase("abc+abc+abc+abc", "+", 1, 4)]
        [TestCase("abc+abc+abc+abc", "+", 2, 4)]
        [TestCase("abc+abc+abc+abc", "+", 200, 4)]
        [TestCase("abc+@abc+@abc+@abc", "+@", 1, 4)]
        [TestCase("abc+@abc+@abc+@abc", "+@", 2, 4)]
        [TestCase("abc+@abc+@abc+@abc", "+@", 4, 4)]
        [TestCase("abc+@abc+@abc+@abc", "+@", 5, 4)]
        [TestCase("abc+@abc+@abc+@abc", "+@", 200, 4)]
        [TestCase("abc+@abc+abc+@abc", "+@", 1, 3)]
        [TestCase("abc+@abc+abc+@abc", "+@", 2, 3)]
        [TestCase("abc+@abc+abc+@abc", "+@", 4, 3)]
        [TestCase("abc+@abc+abc+@abc", "+@", 5, 3)]
        [TestCase("abc+@abc+abc+@abc", "+@", 200, 3)]
        [TestCase("abc+@abc+abc+@abc+@", "+@", 1, 3)]
        [TestCase("abc+@abc+abc+@abc+@", "+@", 2, 3)]
        [TestCase("abc+@abc+abc+@abc+@", "+@", 4, 3)]
        [TestCase("abc+@abc+abc+@abc+@", "+@", 5, 3)]
        [TestCase("abc+@abc+abc+@abc+@", "+@", 200, 3)]
        [TestCase("abc", "+@", 200, 1)]
        public void CountRecordSeparator_Csv_CorrectCount(string text, string recordSeparator, int bufferSize, int result)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write(text);
                writer.Flush();
                
                stream.Position = 0;

                var reader = new CsvReader();
                using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8, true))
                {
                    var value = reader.CountRecordSeparator(streamReader, recordSeparator, bufferSize);
                    Assert.That(value, Is.EqualTo(result));
                }
                writer.Dispose();
            }
            
        }

        [Test]
        [TestCase("abc+abc+abc+abc", "+", 1)]
        [TestCase("abc+abc+abc+abc", "+", 2)]
        [TestCase("abc+abc+abc+abc", "+", 200)]
        [TestCase("abc+@abc+@abc+@abc", "+@", 1)]
        [TestCase("abc+@abc+@abc+@abc", "+@", 2)]
        [TestCase("abc+@abc+@abc+@abc", "+@", 4)]
        [TestCase("abc+@abc+@abc+@abc", "+@", 5)]
        [TestCase("abc+@abc+@abc+@abc", "+@", 200)]
        [TestCase("abc+@abc+abc+@abc", "+@", 1)]
        [TestCase("abc+@abc+abc+@abc", "+@", 2)]
        [TestCase("abc+@abc+abc+@abc", "+@", 4)]
        [TestCase("abc+@abc+abc+@abc", "+@", 5)]
        [TestCase("abc+@abc+abc+@abc", "+@", 200)]
        [TestCase("abc+@abc+abc+@abc+@", "+@", 1)]
        [TestCase("abc+@abc+abc+@abc+@", "+@", 2)]
        [TestCase("abc+@abc+abc+@abc+@", "+@", 4)]
        [TestCase("abc+@abc+abc+@abc+@", "+@", 5)]
        [TestCase("abc+@abc+abc+@abc+@", "+@", 200)]
        [TestCase("abc", "+@", 200)]
        public void GetFirstRecord_Csv_CorrectResult(string text, string recordSeparator, int bufferSize)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write(text);
                writer.Flush();

                stream.Position = 0;

                var reader = new CsvReader();
                using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8, true))
                {
                    var value = reader.GetFirstRecord(streamReader, recordSeparator, bufferSize);
                    Assert.That(value, Is.EqualTo("abc" + recordSeparator).Or.EqualTo("abc"));
                }
                writer.Dispose();
            }

        }

        [Test]
        [TestCase("abc+abc++abc+abc", "++", 1)]
        public void GetFirstRecord_CsvWithSemiSeparator_CorrectResult(string text, string recordSeparator, int bufferSize)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write(text);
                writer.Flush();

                stream.Position = 0;

                var reader = new CsvReader();
                using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8, true))
                {
                    var value = reader.GetFirstRecord(streamReader, recordSeparator, bufferSize);
                    Assert.That(value, Is.EqualTo("abc+abc" + recordSeparator).Or.EqualTo("abc+abc"));
                }
                writer.Dispose();
            }
        }

        [Test]
        [TestCase("abc+abc+abc+abc", "+", 1)]
        [TestCase("abc+abc+abc+abc", "+", 2)]
        [TestCase("abc+abc+abc+abc", "+", 200)]
        [TestCase("abc+@abc+@abc+@abc", "+@", 1)]
        [TestCase("abc+@abc+@abc+@abc", "+@", 2)]
        [TestCase("abc+@abc+@abc+@abc", "+@", 4)]
        [TestCase("abc+@abc+@abc+@abc", "+@", 5)]
        [TestCase("abc+@abc+@abc+@abc", "+@", 200)]
        [TestCase("abc+@abc+abc+@abc", "+@", 1)]
        [TestCase("abc+@abc+abc+@abc", "+@", 2)]
        [TestCase("abc+@abc+abc+@abc", "+@", 4)]
        [TestCase("abc+@abc+abc+@abc", "+@", 5)]
        [TestCase("abc+@abc+abc+@abc", "+@", 200)]
        [TestCase("abc+@abc+abc+@abc+@", "+@", 1)]
        [TestCase("abc+@abc+abc+@abc+@", "+@", 2)]
        [TestCase("abc+@abc+abc+@abc+@", "+@", 4)]
        [TestCase("abc+@abc+abc+@abc+@", "+@", 5)]
        [TestCase("abc+@abc+abc+@abc+@", "+@", 200)]
        [TestCase("abc", "+@", 200)]
        public void NextRecords_Csv_CorrectResults(string text, string recordSeparator, int bufferSize)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write(text);
                writer.Flush();

                stream.Position = 0;

                var reader = new CsvReader();
                using (var streamReader = new StreamReader(stream, Encoding.UTF8, true))
                {
                    var extraRead = string.Empty;
                    var values = reader.GetNextRecords(streamReader, recordSeparator, bufferSize, string.Empty, out extraRead);
                    foreach (var value in values)
                    {
                        Assert.That(value, Does.StartWith("abc"));
                        Assert.That(value, Does.EndWith("abc").Or.EndWith("\0").Or.EndWith(recordSeparator));
                    }
                }
                writer.Dispose();
            }
        }

        [Test]
        [TestCase("a+b+c#a+b#a#a+b", '+', "#", "?")]
        public void NextRecords_CsvWithCsvProfileMissingCell_CorrectResults(string text, char fieldSeparator, string recordSeparator, string missingCell)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write(text);
                writer.Flush();

                stream.Position = 0;
                var profile = new CsvProfile(fieldSeparator, '\"', recordSeparator, false, "_", missingCell);
                var reader = new CsvReader(profile);
                var dataTable = reader.Read(stream, false);

                Assert.That(dataTable.Rows[0].ItemArray[0], Is.EqualTo("a"));
                Assert.That(dataTable.Rows[0].ItemArray[1], Is.EqualTo("b"));
                Assert.That(dataTable.Rows[0].ItemArray[2], Is.EqualTo("c"));

                Assert.That(dataTable.Rows[1].ItemArray[0], Is.EqualTo("a"));
                Assert.That(dataTable.Rows[1].ItemArray[1], Is.EqualTo("b"));
                Assert.That(dataTable.Rows[1].ItemArray[2], Is.EqualTo("?"));

                Assert.That(dataTable.Rows[2].ItemArray[0], Is.EqualTo("a"));
                Assert.That(dataTable.Rows[2].ItemArray[1], Is.EqualTo("?"));
                Assert.That(dataTable.Rows[2].ItemArray[2], Is.EqualTo("?"));

                Assert.That(dataTable.Rows[3].ItemArray[0], Is.EqualTo("a"));
                Assert.That(dataTable.Rows[3].ItemArray[1], Is.EqualTo("b"));
                Assert.That(dataTable.Rows[3].ItemArray[2], Is.EqualTo("?"));


                writer.Dispose();
            }
        }

        [Test]
        [TestCase("a+b+c#a++c#+b+c#+b+", '+', "#", "?")]
        public void NextRecords_CsvWithCsvProfileEmptyCell_CorrectResults(string text, char fieldSeparator, string recordSeparator, string emptyCell)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write(text);
                writer.Flush();

                stream.Position = 0;
                var profile = new CsvProfile(fieldSeparator, '\"', recordSeparator, false, emptyCell, "_");
                var reader = new CsvReader(profile);
                var dataTable = reader.Read(stream, false);

                Assert.That(dataTable.Rows[0].ItemArray[0], Is.EqualTo("a"));
                Assert.That(dataTable.Rows[0].ItemArray[1], Is.EqualTo("b"));
                Assert.That(dataTable.Rows[0].ItemArray[2], Is.EqualTo("c"));

                Assert.That(dataTable.Rows[1].ItemArray[0], Is.EqualTo("a"));
                Assert.That(dataTable.Rows[1].ItemArray[1], Is.EqualTo("?"));
                Assert.That(dataTable.Rows[1].ItemArray[2], Is.EqualTo("c"));

                Assert.That(dataTable.Rows[2].ItemArray[0], Is.EqualTo("?"));
                Assert.That(dataTable.Rows[2].ItemArray[1], Is.EqualTo("b"));
                Assert.That(dataTable.Rows[2].ItemArray[2], Is.EqualTo("c"));

                Assert.That(dataTable.Rows[3].ItemArray[0], Is.EqualTo("?"));
                Assert.That(dataTable.Rows[3].ItemArray[1], Is.EqualTo("b"));
                Assert.That(dataTable.Rows[3].ItemArray[2], Is.EqualTo("?"));


                writer.Dispose();
            }
        }

        [Test]
        [TestCase("abc", "+@", "abc")]
        [TestCase("abc+@", "+@", "abc")]
        [TestCase("abc\0\0\0", "+@", "abc")]
        [TestCase("", "+@", "")]
        public void CleanRecord_Record_CorrectResult(string text, string recordSeparator, string result)
        {
            var reader = new CsvReader();
            var value = reader.CleanRecord(text, recordSeparator);
            Assert.That(value, Is.EqualTo(result));
        }

        [Test]
        [TestCase("abc", false)]
        [TestCase("abc+@", false)]
        [TestCase("abc\0\0\0", true)]
        [TestCase("", true)]
        [TestCase("\0\0\0", true)]
        public void IsLastRecord_Record_CorrectResult(string record, bool result)
        {
            var reader = new CsvReader();
            var value = reader.IsLastRecord(record);
            Assert.That(value, Is.EqualTo(result));
        }

        [Test]
        [TestCase("abc\r\ndef\r\nghl\r\nijk", 1, 1)]
        [TestCase("abc\r\ndef\r\nghl\r\nijk", 17, 1)]
        [TestCase("abc\r\ndef\r\nghl\r\nijk", 18, 1)]
        [TestCase("abc\r\ndef\r\nghl\r\nijk", 19, 1)]
        [TestCase("abc\r\ndef\r\nghl\r\nijk", 512, 1)]
        [TestCase("abc;xyz\r\ndef;xyz\r\nghl\r\n;ijk", 1, 2)]
        [TestCase("abc;xyz\r\ndef;xyz\r\nghl\r\n;ijk", 512, 2)]
        public void Read_Csv_CorrectResult(string text, int bufferSize, int columnCount)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write(text);
                writer.Flush();

                stream.Position = 0;

                var reader = new CsvReader(bufferSize);
                var dataTable = reader.Read(stream, false);
                Assert.That(dataTable.Rows, Has.Count.EqualTo(4));
                Assert.That(dataTable.Columns, Has.Count.EqualTo(columnCount));
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (var cell in row.ItemArray)
                        Assert.That(cell.ToString(), Has.Length.EqualTo(3).Or.EqualTo("(empty)").Or.EqualTo("(null)"));
                }
                writer.Dispose();
            }
        }

        [Test]
        [TestCase("abc", "123", 0)]
        [TestCase("abc1", "123", 1)]
        [TestCase("abc12", "123", 2)]
        [TestCase("abc12a", "123", 0)]
        [TestCase("", "123", 0)]
        [TestCase("", "#", 0)]
        [TestCase("abc", "#", 0)]
        public void IdentifyPartialRecordSeparator_Csv_CorrectResult(string text, string recordSeparator, int result)
        {
            var reader = new CsvReader(20);
            var value = reader.IdentifyPartialRecordSeparator(text, recordSeparator);
            Assert.That(value, Is.EqualTo(result));
        }

        [Test]
        [TestCase("a;b;c\r\nd;e;f;g\r\n", 1, 1)]
        [TestCase("a;b;c\r\nd;e;f\r\ng;h;i;j\r\n", 2, 1)]
        [TestCase("a;b;c\r\nd;e;f\r\ng;h;i;j;k\r\n", 2, 2)]
        public void Read_MoreFieldThanExpected_ExceptionMessage(string text, int rowNumber, int moreField)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(text);
                    writer.Flush();

                    stream.Position = 0;

                    var reader = new CsvReader(1024);

                    var ex = Assert.Throws<InvalidDataException>(delegate { reader.Read(stream, true); });
                    Assert.That(ex.Message, Does.Contain(string.Format("row {0} ", rowNumber+1)));
                    Assert.That(ex.Message, Does.Contain(string.Format("{0} more", moreField)));
                }
            }
        }
    }
}
