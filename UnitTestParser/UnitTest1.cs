using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using UnitTestParser;
using JSONTest1;

namespace UnitTestParser
{
    [TestClass]
    public class UnitTest1
    {
        const string datasA = "<root type=\"object\">\r\n  <vehicle type=\"string\">0014</vehicle>\r\n  <report type=\"string\">CheckIn</report>\r\n  <gps type=\"string\">not set</gps>\r\n  <DOP type=\"string\">-1</DOP>\r\n  <inventory type=\"object\">\r\n    <tags type=\"array\">\r\n      <item type=\"string\">00000035 0 45.553238,-73.735322</item>\r\n      <item type=\"string\">00001339 0 45.553238,-73.735322</item>\r\n      <item type=\"string\">000013D5 0 45.553238,-73.735322</item>\r\n      <item type=\"string\">000012E6 1</item>\r\n      <item type=\"string\">00001078 0 45.553238,-73.735322</item>\r\n      <item type=\"string\">00001372 1</item>\r\n      <item type=\"string\">000012DB 0 45.553238,-73.735322</item>\r\n      <item type=\"string\">00001060 0 45.553238,-73.735322</item>\r\n      <item type=\"string\">0000001B 0 45.553238,-73.735322</item>\r\n      <item type=\"string\">0000105C 0 45.553238,-73.735322</item>\r\n      <item type=\"string\">00001369 1</item>\r\n      <item type=\"string\">0000136F 0 45.553238,-73.735322</item>\r\n    </tags>\r\n  </inventory>\r\n</root>";
        const string datasB = "<root type=\"object\">\r\n  <vehicle type=\"string\">0014</vehicle>\r\n  \r\n  <inventory type=\"object\">\r\n    <tags type=\"array\">\r\n      <item type=\"string\">00000035 0 45.553238,-73.735322</item>\r\n      <item type=\"string\">00001339 0 45.553238,-73.735322</item>\r\n      <item type=\"string\">000013D5 0 45.553238,-73.735322</item>\r\n      <item type=\"string\">000012E6 1</item>\r\n      <item type=\"string\">00001078 0 45.553238,-73.735322</item>\r\n      <item type=\"string\">00001372 1</item>\r\n      <item type=\"string\">000012DB 0 45.553238,-73.735322</item>\r\n      <item type=\"string\">00001060 0 45.553238,-73.735322</item>\r\n      <item type=\"string\">0000001B 0 45.553238,-73.735322</item>\r\n      <item type=\"string\">0000105C 0 45.553238,-73.735322</item>\r\n      <item type=\"string\">00001369 1</item>\r\n      <item type=\"string\">0000136F 0 45.553238,-73.735322</item>\r\n    </tags>\r\n  </inventory>\r\n <report type=\"string\">CheckIn</report>\r\n  <gps type=\"string\">not set</gps>\r\n  <DOP type=\"string\">-1</DOP>\r\n</root>";
        
        [TestMethod]
        public void TestVehiculeInfosA()
        {
            Parser checkMe = new Parser(datasA);
            Assert.IsTrue(checkMe.getProperty("vehicle").Equals("0014"));
            Assert.IsTrue(checkMe.getProperty("report").Equals("CheckIn"));
            Assert.IsTrue(checkMe.getProperty("gps").Equals("not set"));
            Assert.IsTrue(checkMe.getProperty("DOP").Equals("-1"));
        }

        [TestMethod]
        public void TestTagsA()
        {
            Parser checkMe = new Parser(datasA);
            Assert.IsTrue(checkMe.getTagByID("00000035") != null);
            Assert.IsTrue(checkMe.getTagByID("00001339") != null);
            Assert.IsTrue(checkMe.getTagByID("000013D5") != null);
            Assert.IsTrue(checkMe.getTagByID("000012E6") != null);
            Assert.IsTrue(checkMe.getTagByID("00001078") != null);
            Assert.IsTrue(checkMe.getTagByID("00001372") != null);
            Assert.IsTrue(checkMe.getTagByID("000012DB") != null);
            Assert.IsTrue(checkMe.getTagByID("00001060") != null);
            Assert.IsTrue(checkMe.getTagByID("0000001B") != null);
            Assert.IsTrue(checkMe.getTagByID("0000105C") != null);
            Assert.IsTrue(checkMe.getTagByID("00001369") != null);
            Assert.IsTrue(checkMe.getTagByID("0000136F") != null);
            Assert.IsTrue(checkMe.getTagByID("1234136F") == null);
        }

        [TestMethod]
        public void TestVehiculeInfosB()
        {
            Parser checkMe = new Parser(datasB);
            Assert.IsTrue(checkMe.getProperty("vehicle").Equals("0014"));
            Assert.IsTrue(checkMe.getProperty("report").Equals("CheckIn"));
            Assert.IsTrue(checkMe.getProperty("gps").Equals("not set"));
            Assert.IsTrue(checkMe.getProperty("DOP").Equals("-1"));
        }

        [TestMethod]
        public void TestTagsB()
        {
            Parser checkMe = new Parser(datasB);
            Assert.IsTrue(checkMe.getTagByID("00000035") != null);
            Assert.IsTrue(checkMe.getTagByID("00001339") != null);
            Assert.IsTrue(checkMe.getTagByID("000013D5") != null);
            Assert.IsTrue(checkMe.getTagByID("000012E6") != null);
            Assert.IsTrue(checkMe.getTagByID("00001078") != null);
            Assert.IsTrue(checkMe.getTagByID("00001372") != null);
            Assert.IsTrue(checkMe.getTagByID("000012DB") != null);
            Assert.IsTrue(checkMe.getTagByID("00001060") != null);
            Assert.IsTrue(checkMe.getTagByID("0000001B") != null);
            Assert.IsTrue(checkMe.getTagByID("0000105C") != null);
            Assert.IsTrue(checkMe.getTagByID("00001369") != null);
            Assert.IsTrue(checkMe.getTagByID("0000136F") != null);
            Assert.IsTrue(checkMe.getTagByID("1234136F") == null);
        }
    }
}