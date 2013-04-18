CsvHelper
=========

A library for reading and writing CSV files. Extremely fast, flexible, and easy to use. Supports reading and writing of custom class objects.

Modifications of this Fork
==========================

**IMPORTANT: I DID ONLY UPDATE THE MAIN VERSION OF THE CsvHelper. VARIANTS FOR EARLIER OR SPECIAL VERSIONS OF .NET WAS NOT TOUCHED OR TESTED (CsvHelper20, CsvHelper35, CsvHelperRt45, CsvHelperSl4, CsvHelperSl5)**

The intention of the modifications of this fork:

- Use the library efficient for a project reading csv files in many different national standards
- Use the library in a .NET 4.5 runtime
- Use this library to match Entity Framework 5 POCO Entities (especially 1:N entitie structures)

Beyond this, there was some improvements done. Here the modifications in detail:

- Using specific instances of TypeConverters and set CultureInfo to use
- Bugfix to work wit .NET 4.5 (prevent exception "Operation could destabalize the Runtime")
- New Property-Mapping-Features "ConvertFieldUsing<T>" and "SetConstantValue"
- New Methods to define references as collections "SetCollectionOf"
- Some minor modifications relating to exception handling
- Reference mapping and constructors of references can now be done recursively
- Some basic modifications done in the GetRecord-logic

All Modification are explained in the Wiki!

Install
=======

To install CsvHelper, run the following command in the Package Manager Console

    PM> Install-Package CsvHelper

Documentation
=======

http://github.com/JoshClose/CsvHelper/wiki

Discussion
=======
csvhelper@googlegroups.com

https://groups.google.com/forum/?fromgroups#!forum/csvhelper

License
=======

Microsoft Public License (Ms-PL)

http://www.opensource.org/licenses/MS-PL
