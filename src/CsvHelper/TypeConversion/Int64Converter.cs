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
using System.Globalization;

namespace CsvHelper.TypeConversion
{
	/// <summary>
	/// Converts an Int64 to and from a string.
	/// </summary>
	public class Int64Converter : DefaultTypeConverter
	{
        private NumberStyles UsingNumberStyles = NumberStyles.Integer;
        private CultureInfo UsingCultureInfo;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Int64Converter()
        {
        }

        /// <summary>
        /// Customizable constructor
        /// </summary>
        public Int64Converter(NumberStyles UseNumberStyles)
        {
            UsingNumberStyles = UseNumberStyles;
        }

        /// <summary>
        /// Customizable constructor
        /// </summary>
        public Int64Converter(CultureInfo UseCultureInfo, NumberStyles UseNumberStyles)
        {
            UsingNumberStyles = UseNumberStyles;
            UsingCultureInfo = UseCultureInfo;
        }

        /// <summary>
        /// Customizable constructor
        /// </summary>
        public Int64Converter(CultureInfo UseCultureInfo)
        {
            UsingCultureInfo = UseCultureInfo;
        }

		/// <summary>
		/// Converts the string to an object.
		/// </summary>
		/// <param name="culture">The culture used when converting.</param>
		/// <param name="text">The string to convert to an object.</param>
		/// <returns>The object created from the string.</returns>
		public override object ConvertFromString( CultureInfo culture, string text )
		{
			long l;
            if (long.TryParse(text, UsingNumberStyles, UsingCultureInfo ?? culture, out l))
			{
				return l;
			}

            return base.ConvertFromString(UsingCultureInfo ?? culture, text);
		}

		/// <summary>
		/// Determines whether this instance [can convert from] the specified type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		///   <c>true</c> if this instance [can convert from] the specified type; otherwise, <c>false</c>.
		/// </returns>
		public override bool CanConvertFrom( System.Type type )
		{
			// We only care about strings.
			return type == typeof( string );
		}
	}
}
