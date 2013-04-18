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
using System.Linq.Expressions;
using System.Reflection;

namespace CsvHelper.Configuration
{
	/// <summary>
	/// Mappinging info for a reference property mapping to a class.
	/// </summary>
	public class CsvPropertyReferenceMap
	{
		private readonly PropertyInfo property;
        private CsvClassMap mapping;
        private bool isCollection = false;
        private Type collectionType;

        /// <summary>
        /// Gets the property.
        /// </summary>
        public PropertyInfo Property
        {
            get { return property; }
        }

        /// <summary>
        /// Gets the info if it is a collection.
        /// </summary>
        public bool IsCollection
        {
            get { return isCollection; }
        }

        /// <summary>
        /// Gets the type of collection objects (if this is a collection).
        /// </summary>
        public Type CollectionType
        {
            get { return collectionType; }
        }

        /// <summary>
        /// Gets or sets the <see cref="CsvClassMap"/> with the mapping definition.
        /// </summary>
        /// <value>
        /// The <see cref="CsvClassMap"/>
        /// </value>
        public CsvClassMap Mapping
        {
            get { return mapping; }
            set { mapping = value; }
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="CsvPropertyReferenceMap"/> class.
		/// </summary>
		/// <param name="property">The property.</param>
        public CsvPropertyReferenceMap(PropertyInfo property)
            : this(typeof(CsvClassMap), property, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvPropertyReferenceMap"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        public CsvPropertyReferenceMap(Type type, PropertyInfo property)
            : this(type, property, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvPropertyReferenceMap"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <param name="typeConstructorParams">Parameters for type constructor.</param>
        public CsvPropertyReferenceMap(Type type, PropertyInfo property, params object[] typeConstructorParams)
        {
            if (!type.IsSubclassOf(typeof(CsvClassMap)))
            {
                throw new ArgumentException("The type is not a CsvClassMap.");
            }

            this.property = property;
            mapping = (CsvClassMap)Activator.CreateInstance(type, typeConstructorParams);
        }

        /// <summary>
        /// Define this property as collection.
        /// New items will be added into the collection instead
        /// assigning them to this reference property.
        /// </summary>
        /// <param name="type">The type of the collection objects</param>
        /// <returns></returns>
        public CsvPropertyReferenceMap IsCollectionOf(Type type)
        {
            this.isCollection = true;
            this.collectionType = type;
            return this;
        }

    }
}
