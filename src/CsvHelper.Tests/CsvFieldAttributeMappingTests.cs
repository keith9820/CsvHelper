﻿// Copyright 2009-2013 Josh Close
// This file is a part of CsvHelper and is licensed under the MS-PL
// See LICENSE.txt for details or visit http://www.opensource.org/licenses/ms-pl.html
// http://csvhelper.com
using System;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
#if WINRT_4_5
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CsvHelper.Tests
{
	[TestClass]
	public class CsvFieldAttributeMappingTests
	{
		[TestMethod]
		public void FieldAttributeNameTest()
		{
			var config = new CsvConfiguration();
			config.AttributeMapping<TestClass>();

			Assert.AreEqual( 4, config.Mapping.PropertyMaps.Count );

            Assert.AreEqual("Guid Column", config.Mapping.PropertyMaps[0].NameValue);
            Assert.AreEqual("Int Column", config.Mapping.PropertyMaps[1].NameValue);
            Assert.AreEqual("String Column", config.Mapping.PropertyMaps[2].NameValue);
            Assert.AreEqual("NotUsedColumn", config.Mapping.PropertyMaps[3].NameValue);
		}

		[TestMethod]
		public void FieldAttributeIndexTest()
		{
			var config = new CsvConfiguration();
			config.AttributeMapping<TestClass>();

			Assert.AreEqual( 4, config.Mapping.PropertyMaps.Count );

            Assert.AreEqual(1, config.Mapping.PropertyMaps[0].IndexValue);
            Assert.AreEqual(2, config.Mapping.PropertyMaps[1].IndexValue);
            Assert.AreEqual(3, config.Mapping.PropertyMaps[2].IndexValue);
            Assert.AreEqual(-1, config.Mapping.PropertyMaps[3].IndexValue);
		}

		[TestMethod]
		public void FieldAttributeTypeConverterTest()
		{
			var config = new CsvConfiguration();
			config.AttributeMapping<TestClass>();

            Assert.AreEqual(4, config.Mapping.PropertyMaps.Count);

            Assert.IsInstanceOfType(config.Mapping.PropertyMaps[0].TypeConverterValue, typeof(StringConverter));
            Assert.IsInstanceOfType(config.Mapping.PropertyMaps[1].TypeConverterValue, typeof(Int32Converter));
            Assert.IsInstanceOfType(config.Mapping.PropertyMaps[2].TypeConverterValue, typeof(Int16Converter));
            Assert.IsInstanceOfType(config.Mapping.PropertyMaps[3].TypeConverterValue, typeof(StringConverter));
		}

		[TestMethod]
		public void FieldAttributeIgnoreTest()
		{
			var config = new CsvConfiguration();
			config.AttributeMapping<TestClass>();

            Assert.AreEqual(4, config.Mapping.PropertyMaps.Count);

            Assert.IsFalse(config.Mapping.PropertyMaps[0].IgnoreValue);
            Assert.IsTrue(config.Mapping.PropertyMaps[1].IgnoreValue);
            Assert.IsFalse(config.Mapping.PropertyMaps[2].IgnoreValue);
            Assert.IsFalse(config.Mapping.PropertyMaps[3].IgnoreValue);
		}

		[TestMethod]
		public void FieldAttributeMultipleNamesTest()
		{
			var config = new CsvConfiguration();
			config.AttributeMapping<TestMultipleNamesClass>();

            Assert.AreEqual(2, config.Mapping.PropertyMaps.Count);
            Assert.AreEqual(2, config.Mapping.PropertyMaps[0].NamesValue.Length);
            Assert.AreEqual("Id1", config.Mapping.PropertyMaps[0].NamesValue[0]);
            Assert.AreEqual("Id2", config.Mapping.PropertyMaps[0].NamesValue[1]);
            Assert.AreEqual("Name1", config.Mapping.PropertyMaps[1].NamesValue[0]);
            Assert.AreEqual("Name2", config.Mapping.PropertyMaps[1].NamesValue[1]);
		}

		private class TestClass
		{
			[TypeConverter( typeof( Int16Converter ) )]
			[CsvField( Index = 3, Name = "String Column" )]
			public string StringColumn { get; set; }

			[CsvField( Index = 2, Name = "Int Column", Ignore = true )]
			public int IntColumn { get; set; }

			[TypeConverter( typeof( StringConverter ) )]
			[CsvField( Index = 1, Name = "Guid Column" )]
			public Guid GuidColumn { get; set; }

			public string NotUsedColumn { get; set; }
		}

		private class TestMultipleNamesClass
		{
			[CsvField( Names = new [] { "Id1", "Id2" } )]
			public int Id { get; set; }

			[CsvField( Names = new [] { "Name1", "Name2" } )]
			public string Name { get; set; }
		}
	}
}
