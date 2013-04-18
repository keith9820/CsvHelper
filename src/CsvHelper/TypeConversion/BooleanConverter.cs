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
using System.Collections.Generic;

namespace CsvHelper.TypeConversion
{
	/// <summary>
	/// Converts a Boolean to and from a string.
	/// </summary>
	public class BooleanConverter : DefaultTypeConverter
	{
        /// <summary>
        /// Gets a <see cref="List"/> of strings which represents a value of "true"
        /// </summary>
        /// <value>
        /// The <see cref="List"/> of strings which represents a value of "true"
        /// </value>
        public List<string> TrueRepresentation { get; private set; }

        /// <summary>
        /// Gets a <see cref="List"/> of strings which represents a value of "false"
        /// </summary>
        /// <value>
        /// The <see cref="List"/> of strings which represents a value of "false"
        /// </value>
        public List<string> FalseRepresentation { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BooleanConverter()
            : this(new List<string>() { "yes", "y" }, new List<string>() { "no", "n" })
        {
        }

        /// <summary>
        /// Customizable constructor
        /// </summary>
        public BooleanConverter(List<string> trueStrings, List<string> falseStrings)
        {
            TrueRepresentation = trueStrings;
            FalseRepresentation = falseStrings;
        }

        /// <summary>
		/// Converts the string to an object.
		/// </summary>
		/// <param name="culture">The culture used when converting.</param>
		/// <param name="text">The string to convert to an object.</param>
		/// <returns>The object created from the string.</returns>
		public override object ConvertFromString( CultureInfo culture, string text )
		{
			bool b;
			if( bool.TryParse( text, out b ) )
			{
				return b;
			}

			short sh;
			if( short.TryParse( text, out sh ) )
			{
				if( sh == 0 )
				{
					return false;
				}
				if( sh == 1 )
				{
					return true;
				}
			}

			var t = ( text ?? string.Empty ).Trim();
            if( TrueRepresentation.Exists(m => culture.CompareInfo.Compare( m, t, CompareOptions.IgnoreCase ) == 0) )
			{
				return true;
			}

            if (FalseRepresentation.Exists(m => culture.CompareInfo.Compare(m, t, CompareOptions.IgnoreCase) == 0))
            {
				return false;
			}

			return base.ConvertFromString( culture, text );
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
