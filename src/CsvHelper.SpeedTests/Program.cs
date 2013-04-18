// Copyright 2009-2013 Josh Close
// This file is a part of CsvHelper and is licensed under the MS-PL
// See LICENSE.txt for details or visit http://www.opensource.org/licenses/ms-pl.html
// http://csvhelper.com
// *************************
// Forked Version 04/2013
// Git: https://github.com/thiscode/CsvHelper
// Documentation: https://github.com/thiscode/CsvHelper/Wiki
// Author: Thomas Miliopoulos (thiscode)
// *************************
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper.Configuration;

namespace CsvHelper.SpeedTests
{
	class Program
	{
        private const string FileName = "test.csv";
        private const string FileNameWithReference = "testRef.csv";

		static void Main( string[] args )
		{
            CreateCsvFile();
            CreateCsvFileForReferenceTest();
            SelectTest();
		}
		
		private static string ReadUntilValid( string question, string errorMessage, Func<string,bool> answerValidator )
		{
			for( ; ; )
			{
				Console.Write( question );
				var answer = Console.ReadLine();
				if( answerValidator( answer ) )
				{
					return answer;
				}
				Console.WriteLine( errorMessage );
			}
		}

		private static void CreateCsvFile()
		{
			var answer = ReadUntilValid( "Create CSV? ", "Not a valid answer.", a => new[] { "y", "yes", "1", "true", "n", "no", "0", "false" }.Contains( a, StringComparer.Create( CultureInfo.CurrentCulture, true ) ) );
			if( !new[] { "y", "yes", "1", "true" }.Contains( answer, StringComparer.Create( CultureInfo.CurrentCulture, true ) ) )
			{
				return;
			}

			int rowCount = 0;
			ReadUntilValid( "How many rows? ", "You need to specify a valid integer.", s => int.TryParse( s, NumberStyles.Number, CultureInfo.CurrentCulture, out rowCount ) );

			var rowsWrittenText = "Rows written: ";
			Console.Write( rowsWrittenText );
			using( var stream = File.Open( FileName, FileMode.Create ) )
			using( var writer = new StreamWriter( stream ) )
			using( var csv = new CsvWriter( writer ) )
			{
				var stopwatch = new Stopwatch();
				stopwatch.Start();

				for( var i = 1; i <= rowCount; i++ )
				{
                    var row = new TestClass
                    {
                        IntColumn = i,
                        StringColumn = string.Format("Row {0}", i),
                        DateColumn = DateTime.Now,
                        BoolColumn = i % 2 == 0,
                        GuidColumn = Guid.NewGuid()
                    };
                    Console.CursorLeft = rowsWrittenText.Length;
                    if (i == 1) csv.WriteHeader(typeof(TestClass));
					csv.WriteRecord( row );
					Console.Write( "{0:N0}", i );
				}
				stopwatch.Stop();
                Console.WriteLine();
                Console.WriteLine("Time: {0}", stopwatch.Elapsed);
			}
			Console.WriteLine();
		}

        private static void CreateCsvFileForReferenceTest()
        {
            var answer = ReadUntilValid("Create CSV for Reference Test? ", "Not a valid answer.", a => new[] { "y", "yes", "1", "true", "n", "no", "0", "false" }.Contains(a, StringComparer.Create(CultureInfo.CurrentCulture, true)));
            if (!new[] { "y", "yes", "1", "true" }.Contains(answer, StringComparer.Create(CultureInfo.CurrentCulture, true)))
            {
                return;
            }

            int rowCount = 0;
            ReadUntilValid("How many rows? ", "You need to specify a valid integer.", s => int.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out rowCount));

            var rowsWrittenText = "Rows written: ";
            Console.Write(rowsWrittenText);
            using (var stream = File.Open(FileNameWithReference, FileMode.Create))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer))
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                for (var i = 1; i <= rowCount; i++)
                {
                    var row = new TestClassWithReference
                    {
                        IntColumn = i,
                        StringColumn = string.Format("Row {0}", i),
                        DateColumn = DateTime.Now,
                        BoolColumn = i % 2 == 0,
                        GuidColumn = Guid.NewGuid(),
                        ReferenceColumn = new TestClassReference()
                        {
                            RefIntColumn = i,
                            RefStringColumn = string.Format("Ref Row {0}", i),
                            RefDateColumn = DateTime.Now.Subtract(new TimeSpan(1,0,0,0,0)),
                            RefBoolColumn = i % 2 == 0,
                            RefGuidColumn = Guid.NewGuid()
                        }
                    };
                    Console.CursorLeft = rowsWrittenText.Length;
                    if (i == 1) csv.WriteHeader(typeof(TestClassWithReference));
                    csv.WriteRecord(row);
                    Console.Write("{0:N0}", i);
                }
                stopwatch.Stop();
                Console.WriteLine();
                Console.WriteLine("Time: {0}", stopwatch.Elapsed);
            }
            Console.WriteLine();
        }

        private static void SelectTest()
		{
			while( true )
			{
				var question = new StringBuilder();
				question.AppendLine("1) Parse");
                question.AppendLine("2) Parse and count bytes");
                question.AppendLine("3) Test Reader and GetRecord");
                question.AppendLine("4) Test Reader and GetRecord with Reference");
                question.AppendLine("5) Compare new Reader to origin");
                question.AppendLine("q) Quit");
				question.Append( "Select test to run: " );
				var option = ReadUntilValid( question.ToString(), "Not a valid option.", s => new[] { "q", "1", "2", "3", "4", "5" }.Contains( s, StringComparer.Create( CultureInfo.CurrentCulture, true ) ) );

				switch( option )
				{
					case "1":
						ParseTest();
						break;
                    case "2":
                        ParseCountingBytesTest();
                        break;
                    case "3":
                        CsvReaderGetRecordTest();
                        break;
                    case "4":
                        CsvReaderGetRecordTestWithReference();
                        break;
                    case "5":
                        CompareToOrigin();
                        break;
                    case "q":
						return;
				}
                Console.WriteLine();
            }
		}

		private static void ParseTest()
		{
			using( var stream = File.OpenRead( FileName ) )
			using( var reader = new StreamReader( stream ) )
			using( var parser = new CsvParser( reader ) )
			{
				var stopwatch = new Stopwatch();
				stopwatch.Start();
				while( true )
				{
					var record = parser.Read();
					if( record == null )
					{
						break;
					}
				}
				stopwatch.Stop();
				Console.WriteLine( "Time: {0}", stopwatch.Elapsed );
			}
		}

		private static void ParseCountingBytesTest()
		{
			using( var stream = File.OpenRead( FileName ) )
			using( var reader = new StreamReader( stream ) )
			using( var parser = new CsvParser( reader ) )
			{
				parser.Configuration.CountBytes = true;
				var stopwatch = new Stopwatch();
				stopwatch.Start();
				while( true )
				{
					var record = parser.Read();
					if( record == null )
					{
						break;
					}
				}
				stopwatch.Stop();
				Console.WriteLine( "Time: {0}", stopwatch.Elapsed );
			}
		}

        private static void CsvReaderGetRecordTest()
        {
            using (var stream = File.OpenRead(FileName))
            using (var reader = new StreamReader(stream))
            using (var csvReader = new CsvReader(reader))
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                while (csvReader.Read())
                {
                    var record = csvReader.GetRecord<TestClass>();
                }
                stopwatch.Stop();
                Console.WriteLine("Time: {0}", stopwatch.Elapsed);
            }
        }

        private static void CsvReaderGetRecordTestWithReference()
        {
            using (var stream = File.OpenRead(FileNameWithReference))
            using (var reader = new StreamReader(stream))
            using (var csvReader = new CsvReader(reader))
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                while (csvReader.Read())
                {
                    var record = csvReader.GetRecord<TestClassWithReference>();
                }
                stopwatch.Stop();
                Console.WriteLine("Time: {0}", stopwatch.Elapsed);
            }
        }

        private static void CompareToOrigin()
        {
            Console.WriteLine("New");
            for (int x = 0; x < 20; x++)
            {
                using (var stream = File.OpenRead(FileNameWithReference))
                using (var reader = new StreamReader(stream))
                using (var csvReader = new CsvReader(reader))
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    while (csvReader.Read())
                    {
                        var record = csvReader.GetRecord<TestClassWithReference>();
                    }
                    stopwatch.Stop();
                    Console.WriteLine("Time: {0}", stopwatch.Elapsed);
                }
            }
            Console.WriteLine("Origin");
            for (int x = 0; x < 20; x++)
            {
                using (var stream = File.OpenRead(FileNameWithReference))
                using (var reader = new StreamReader(stream))
                using (var csvReader = new CsvReaderOrigin(reader))
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    while (csvReader.Read())
                    {
                        var record = csvReader.GetRecord<TestClassWithReference>();
                    }
                    stopwatch.Stop();
                    Console.WriteLine("Time: {0}", stopwatch.Elapsed);
                }
            }
        }

        private class TestClass
		{
			[CsvField( Name = "Int Column" )]
			public int IntColumn { get; set; }

			[CsvField( Name = "String Column" )]
			public string StringColumn { get; set; }

			[CsvField( Name = "Date Column" )]
			public DateTime DateColumn { get; set; }

			[CsvField( Name = "Bool Column" )]
			public bool BoolColumn { get; set; }

			[CsvField( Name = "Guid Column" )]
			public Guid GuidColumn { get; set; }
		}
        private class TestClassWithReference
        {
            [CsvField(Name = "Int Column")]
            public int IntColumn { get; set; }

            [CsvField(Name = "String Column")]
            public string StringColumn { get; set; }

            [CsvField(Name = "Date Column")]
            public DateTime DateColumn { get; set; }

            [CsvField(Name = "Bool Column")]
            public bool BoolColumn { get; set; }

            [CsvField(Name = "Guid Column")]
            public Guid GuidColumn { get; set; }

            [CsvField(ReferenceKey = "Reference Column")]
            public TestClassReference ReferenceColumn { get; set; }
        }
        private class TestClassReference
        {
            [CsvField(Name = "Ref Int Column")]
            public int RefIntColumn { get; set; }

            [CsvField(Name = "Ref String Column")]
            public string RefStringColumn { get; set; }

            [CsvField(Name = "Ref Date Column")]
            public DateTime RefDateColumn { get; set; }

            [CsvField(Name = "Ref Bool Column")]
            public bool RefBoolColumn { get; set; }

            [CsvField(Name = "Ref Guid Column")]
            public Guid RefGuidColumn { get; set; }
        }

	}
}
