#region Copyright
/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */
#endregion

using System;
using System.Collections;
using System.Reflection;
using Seasar.Dao.Attrs;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.ADO.Types;
using Seasar.Framework.Beans;

namespace Seasar.Dao.Impl
{
    public class DtoMetaDataImpl : IDtoMetaData
    {
        private Type beanType;

#if NET_1_1
        private Hashtable propertyTypes = new Hashtable(
            CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
#else
        private Hashtable propertyTypes = new Hashtable(
            StringComparer.OrdinalIgnoreCase);
#endif

        protected IBeanAnnotationReader beanAnnotationReader;

        protected DtoMetaDataImpl()
        {
        }

        public DtoMetaDataImpl(Type beanType, IBeanAnnotationReader beanAnnotationReader)
        {
            BeanType = beanType;
            BeanAnnotationReader = beanAnnotationReader;
            Initialize();
        }

        public void Initialize()
        {
            SetupPropertyType();
        }

        #region IDtoMetaData ÉÅÉìÉo

        public Type BeanType
        {
            get
            {
                return beanType;
            }
            set
            {
                beanType = value; 
            }
        }

        public int PropertyTypeSize
        {
            get
            {
                return propertyTypes.Count;
            }
        }

        public IBeanAnnotationReader BeanAnnotationReader
        {
            set { beanAnnotationReader = value; }
        }

        public Seasar.Extension.ADO.IPropertyType GetPropertyType(int index)
        {
            IEnumerator enu = propertyTypes.Keys.GetEnumerator();
            for(int i = -1; i < index; ++i) enu.MoveNext();
            return (IPropertyType) propertyTypes[enu.Current];
        }

        public IPropertyType GetPropertyType(string propertyName)
        {
            IPropertyType propertyType = (IPropertyType) propertyTypes[propertyName];
            if(propertyType == null)
                throw new PropertyNotFoundRuntimeException(beanType, propertyName);
            return propertyType;
        }

        public bool HasPropertyType(string propertyName)
        {
            return propertyTypes.Contains(propertyName);
        }

        #endregion

        protected void SetupPropertyType()
        {
            foreach(PropertyInfo pi in beanType.GetProperties())
            {
                IPropertyType pt = CreatePropertyType(pi);
                AddPropertyType(pt);
            }
        }

        protected IPropertyType CreatePropertyType(PropertyInfo pi)
        {
            string columnName = beanAnnotationReader.GetColumn(pi);
            if (columnName == null)
            {
                columnName = pi.Name;
            }
            IValueType valueType = ValueTypes.GetValueType(pi.PropertyType);
            IPropertyType pt = new PropertyTypeImpl(pi, valueType, columnName);
            return pt;
        }

        protected void AddPropertyType(IPropertyType propertyType)
        {
            propertyTypes.Add(propertyType.PropertyName, propertyType);
        }
    }
}
