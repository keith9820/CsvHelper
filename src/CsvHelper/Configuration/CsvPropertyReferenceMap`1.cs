﻿// Copyright 2009-2013 Josh Close
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
using System.Reflection;

namespace CsvHelper.Configuration
{
	/// <summary>
	/// Mappinging info for a reference property mapping to a class.
	/// </summary>
	/// <typeparam name="TClassMap">The type of the class map.</typeparam>
	public class CsvPropertyReferenceMap<TClassMap> : CsvPropertyReferenceMap 
		where TClassMap : CsvClassMap
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CsvPropertyReferenceMap&lt;TClassMap&gt;"/> class.
		/// </summary>
		/// <param name="property">The property.</param>
        public CsvPropertyReferenceMap(PropertyInfo property)
            : base(typeof(TClassMap), property)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvPropertyReferenceMap&lt;TClassMap&gt;"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        public CsvPropertyReferenceMap(PropertyInfo property, params object[] typeConstructorParams)
            : base(typeof(TClassMap), property, typeConstructorParams)
        {
        }
    }
}
