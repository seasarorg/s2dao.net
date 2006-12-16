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
using System.Text;
using Seasar.Dao.Attrs;
using Seasar.Dao.Id;
using Seasar.Dao.Impl;
using Seasar.Extension.ADO;
using Seasar.Framework.Beans;
using Seasar.Framework.Log;

namespace Seasar.Dao.Impl
{
    public class BeanMetaDataImpl : DtoMetaDataImpl, IBeanMetaData
    {
        private static Logger logger = Logger.GetLogger(typeof(BeanMetaDataImpl));
        protected string tableName;

#if NET_1_1
        private Hashtable propertyTypesByColumnName = new Hashtable(
            CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
#else
        protected Hashtable propertyTypesByColumnName =
            new Hashtable(StringComparer.OrdinalIgnoreCase);
#endif

        protected ArrayList relationProeprtyTypes = new ArrayList();
        protected string[] primaryKeys = new string[0];
        protected string autoSelectList;
        protected bool relation;
        protected IIdentifierGenerator identifierGenerator;
        protected string versionNoPropertyName = "VersionNo";
        protected string timestampPropertyName = "Timestamp";
        protected string versionNoBindingName;
        protected string timestampBindingName;
        private IAnnotationReaderFactory annotationReaderFactory;

        public BeanMetaDataImpl()
        {
        }

        [Obsolete]
        public BeanMetaDataImpl(Type beanType)
            : this(beanType, false)
        {
        }

        [Obsolete]
        public BeanMetaDataImpl(Type beanType, bool relation)
        {
            this.BeanType = beanType;
            this.relation = relation;
        }

        [Obsolete]
        public BeanMetaDataImpl(Type beanType, IDatabaseMetaData dbMetaData, IDbms dbms)
            : this(beanType, dbMetaData, dbms, false)
        {
        }

        [Obsolete]
        public BeanMetaDataImpl(Type beanType, IDatabaseMetaData dbMetaData, IDbms dbms, bool relation)
            : this(beanType, dbMetaData, dbms, new FieldAnnotationReaderFactory(), relation)
        {
        }

        public BeanMetaDataImpl(Type beanType, IDatabaseMetaData dbMetaData,
            IDbms dbms, IAnnotationReaderFactory annotationReaderFactory)
            : this(beanType, dbMetaData, dbms, annotationReaderFactory, false)
        {
        }

        public BeanMetaDataImpl(Type beanType, IDatabaseMetaData dbMetaData,
            IDbms dbms, IAnnotationReaderFactory annotationReaderFactory, bool relation)
        {
            BeanType = beanType;
            this.relation = relation;
            AnnotationReaderFactory = annotationReaderFactory;
            Initialize(dbMetaData, dbms);
        }

        public void Initialize(IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            beanAnnotationReader = AnnotationReaderFactory.CreateBeanAnnotationReader(BeanType);
            SetupTableName(this.BeanType);
            SetupVersionNoPropertyName(this.BeanType);
            SetupTimestampPropertyName(this.BeanType);
            SetupProperty(this.BeanType, dbMetaData, dbms);
            SetupDatabaseMetaData(this.BeanType, dbMetaData, dbms);
            SetupPropertiesByColumnName();
        }

        protected IAnnotationReaderFactory AnnotationReaderFactory
        {
            get { return annotationReaderFactory; }
            set { annotationReaderFactory = value; }
        }

        #region IBeanMetaData ÉÅÉìÉo

        public string TableName
        {
            get
            {
                return tableName;
            }
        }

        public Seasar.Extension.ADO.IPropertyType VersionNoPropertyType
        {
            get
            {
                return GetPropertyType(versionNoPropertyName);
            }
        }

        public string VersionNoPropertyName
        {
            get
            {
                return versionNoPropertyName;
            }
        }

        public string VersionNoBindingName
        {
            get
            {
                return versionNoBindingName;
            }
        }

        public bool HasVersionNoPropertyType
        {
            get
            {
                return HasPropertyType(versionNoPropertyName);
            }
        }

        public Seasar.Extension.ADO.IPropertyType TimestampPropertyType
        {
            get
            {
                return GetPropertyType(timestampPropertyName);
            }
        }

        public string TimestampPropertyName
        {
            get
            {
                return timestampPropertyName;
            }
        }

        public string TimestampBindingName
        {
            get
            {
                return timestampBindingName;
            }
        }

        public bool HasTimestampPropertyType
        {
            get
            {
                return HasPropertyType(timestampPropertyName);
            }
        }

        public string ConvertFullColumnName(string alias)
        {
            if(HasPropertyTypeByColumnName(alias))
                return tableName + "." + alias;
            int index = alias.LastIndexOf('_');
            if(index < 0)
                throw new ColumnNotFoundRuntimeException(tableName, alias);
            string columnName = alias.Substring(0, index);
            string relnoStr = alias.Substring(index + 1);
            int relno = -1;
            try
            {
                relno = int.Parse(relnoStr);
            }
            catch(Exception)
            {
                throw new ColumnNotFoundRuntimeException(tableName, alias);
            }
            IRelationPropertyType rpt = GetRelationPropertyType(relno);
            if(!rpt.BeanMetaData.HasPropertyTypeByColumnName(columnName))
                throw new ColumnNotFoundRuntimeException(tableName, alias);
            return rpt.PropertyName + "." + columnName;
        }

        public Seasar.Extension.ADO.IPropertyType GetPropertyTypeByAliasName(string aliasName)
        {
            if(HasPropertyTypeByColumnName(aliasName))
                return GetPropertyTypeByColumnName(aliasName);
            int index = aliasName.LastIndexOf('_');
            if(index < 0)
                throw new ColumnNotFoundRuntimeException(tableName, aliasName);
            string columnName = aliasName.Substring(0, index);
            string relnoStr = aliasName.Substring(index + 1);
            int relno = -1;
            try
            {
                relno = int.Parse(relnoStr);
            }
            catch(Exception)
            {
                throw new ColumnNotFoundRuntimeException(tableName, columnName);
            }
            IRelationPropertyType rpt = GetRelationPropertyType(relno);
            if(!rpt.BeanMetaData.HasPropertyTypeByColumnName(columnName))
                throw new ColumnNotFoundRuntimeException(tableName, aliasName);
            return rpt.BeanMetaData.GetPropertyTypeByAliasName(columnName);
        }

        public Seasar.Extension.ADO.IPropertyType GetPropertyTypeByColumnName(string columnName)
        {
            IPropertyType propertyType = (IPropertyType) propertyTypesByColumnName[columnName];
            if(propertyType == null)
                throw new ColumnNotFoundRuntimeException(tableName, columnName);

            return propertyType;
        }

        public bool HasPropertyTypeByColumnName(string columnName)
        {
            return propertyTypesByColumnName[columnName] != null;
        }

        public bool HasPropertyTypeByAliasName(string aliasName)
        {
            if(HasPropertyTypeByColumnName(aliasName)) return true;
            int index = aliasName.LastIndexOf('_');
            if(index < 0) return false;
            string columnName = aliasName.Substring(0, index);
            string relnoStr = aliasName.Substring(index + 1);
            int relno = -1;
            try
            {
                relno = int.Parse(relnoStr);
            }
            catch(Exception)
            {
                return false;
            }
            if(relno >= RelationPropertyTypeSize) return false;
            IRelationPropertyType rpt = GetRelationPropertyType(relno);
            return rpt.BeanMetaData.HasPropertyTypeByColumnName(columnName);
        }

        public int RelationPropertyTypeSize
        {
            get
            {
                return relationProeprtyTypes.Count;
            }
        }

        public IRelationPropertyType GetRelationPropertyType(int index)
        {
            return (IRelationPropertyType) relationProeprtyTypes[index];
        }

        IRelationPropertyType Seasar.Dao.IBeanMetaData.GetRelationPropertyType(string propertyName)
        {
            for(int i = 0; i < RelationPropertyTypeSize; ++i)
            {
                IRelationPropertyType rpt = (IRelationPropertyType) relationProeprtyTypes[i];
                if(rpt != null
                        && string.Compare(rpt.PropertyName, propertyName, true) == 0)
                    return rpt;
            }
            throw new PropertyNotFoundRuntimeException(BeanType, propertyName);
        }

        public int PrimaryKeySize
        {
            get
            {
                return primaryKeys.Length;
            }
        }

        public string GetPrimaryKey(int index)
        {
            return primaryKeys[index];
        }

        public IIdentifierGenerator IdentifierGenerator
        {
            get
            {
                return identifierGenerator;
            }
        }

        public string AutoSelectList
        {
            get
            {
                lock(this)
                {
                    if(autoSelectList != null)
                        return autoSelectList;
                    SetupAutoSelectList();
                    return autoSelectList;
                }
            }
        }

        public bool IsRelation
        {
            get
            {
                return relation;
            }
        }

        #endregion

        protected virtual void SetupTableName(Type beanType)
        {
            string ta = beanAnnotationReader.GetTable();
            if(ta != null)
            {
                tableName = ta;
            }
            else
            {
                tableName = beanType.Name;
            }
        }

        protected virtual void SetupVersionNoPropertyName(Type beanType)
        {
            string vna = beanAnnotationReader.GetVersionNoProteryName();
            if (vna != null)
            {
                versionNoPropertyName = vna;
            }

            int i = 0;
            do
            {
                versionNoBindingName = versionNoPropertyName + i++;
            } while (HasPropertyType(versionNoBindingName));
        }

        protected virtual void SetupTimestampPropertyName(Type beanType)
        {
            string tsa = beanAnnotationReader.GetTimestampPropertyName();
            if (tsa != null)
            {
                timestampPropertyName = tsa;
            }

            int i = 0;
            do
            {
                timestampBindingName = timestampPropertyName + i++;
            } while (HasPropertyType(timestampBindingName));

            if (timestampBindingName.Equals(versionNoBindingName))
            {
                timestampBindingName = timestampPropertyName + i++;
            }
        }

        protected virtual void SetupProperty(Type beanType, IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            foreach(PropertyInfo pi in beanType.GetProperties())
            {
                IPropertyType pt = null;
                RelnoAttribute relnoAttr = beanAnnotationReader.GetRelnoAttribute(pi);
                if(relnoAttr != null)
                {
                    if(!relation)
                    {
                        IRelationPropertyType rpt = CreateRelationPropertyType(
                            beanType, pi, relnoAttr, dbMetaData, dbms);
                        AddRelationPropertyType(rpt);
                    }
                }
                else
                {
                    pt = CreatePropertyType(pi);
                    AddPropertyType(pt);
                }
                if(IdentifierGenerator == null)
                {
                    IDAttribute idAttr = beanAnnotationReader.GetIdAttribute(pi);
                    if (idAttr != null)
                    {
                        identifierGenerator = IdentifierGeneratorFactory.CreateIdentifierGenerator(
                            pi.Name, dbms, idAttr);
                        primaryKeys = new string[] { pt.ColumnName };
                        pt.IsPrimaryKey = true;
                    }
                }
            }
        }

        protected virtual void SetupDatabaseMetaData(Type beanType,
            IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            SetupPropertyPersistentAndColumnName(beanType, dbMetaData);
            SetupPrimaryKey(dbMetaData, dbms);
        }

        protected virtual void SetupPrimaryKey(IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            if(IdentifierGenerator == null)
            {
                ArrayList pkeyList = new ArrayList();
                IList primaryKeySet = dbMetaData.GetPrimaryKeySet(tableName);
                for(int i = 0; i < PropertyTypeSize; ++i)
                {
                    IPropertyType pt = GetPropertyType(i);
                    if(primaryKeySet != null && primaryKeySet.Contains(pt.ColumnName))
                    {
                        pt.IsPrimaryKey = true;
                        pkeyList.Add(pt.ColumnName);
                    }
                    else
                    {
                        pt.IsPrimaryKey = false;
                    }
                }
                primaryKeys = (string[]) pkeyList.ToArray(typeof(string));
                identifierGenerator = IdentifierGeneratorFactory
                    .CreateIdentifierGenerator(null, dbms);
            }
        }

        protected virtual void SetupPropertyPersistentAndColumnName(Type beanType,
            IDatabaseMetaData dbMetaData)
        {
            IList columnSet = dbMetaData.GetColumnSet(tableName);
            if(columnSet == null || columnSet.Count == 0)
            {
                logger.Log("WDAO0002", new object[] { tableName });
            }
            else
            {
                for(IEnumerator enu = columnSet.GetEnumerator(); enu.MoveNext();)
                {
                    string columnName = (string)enu.Current;
                    string noUnderscoreColumnName = columnName.Replace("_", "");
                    bool hasProperty = false;
                    for (int i = 0; i < PropertyTypeSize; ++i)
                    {
                        IPropertyType pt = GetPropertyType(i);
                        if (string.Compare(pt.ColumnName, columnName, true) == 0)
                        {
                            hasProperty = true;
                        } else
                        {
                            if (string.Compare(pt.ColumnName, noUnderscoreColumnName, true) == 0)
                            {
                                hasProperty = true;
                            }
                        }
                        if (hasProperty)
                        {
                            pt.ColumnName = columnName;
                            break;
                        }
                    }
                }
            }

            string[] props = beanAnnotationReader.GetNoPersisteneProps();
            if (props != null)
            {
                foreach(string prop in props)
                {
                    IPropertyType pt = GetPropertyType(prop.Trim());
                    pt.IsPersistent = false;
                }
            }
            for(int i = 0; i < PropertyTypeSize; ++i)
            {
                IPropertyType pt = GetPropertyType(i);
                if(columnSet == null || !columnSet.Contains(pt.ColumnName))
                {
                    pt.IsPersistent = false;
                }
            }
        }

        protected virtual void SetupPropertiesByColumnName()
        {
            for(int i = 0; i < PropertyTypeSize; ++i)
            {
                IPropertyType pt = GetPropertyType(i);
                propertyTypesByColumnName[pt.ColumnName] = pt;
            }
        }

        protected virtual IRelationPropertyType CreateRelationPropertyType(Type beanType,
            PropertyInfo propertyInfo, RelnoAttribute relnoAttr,
            IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            string[] myKeys = new string[0];
            string[] yourKeys = new string[0];
            string relkeys = beanAnnotationReader.GetRelationKey(propertyInfo);
            if(relkeys != null)
            {
                ArrayList myKeyList = new ArrayList();
                ArrayList yourKeyList = new ArrayList();
                foreach(string token in relkeys.Split(
                    '\t', '\n', '\r', '\f', ','))
                {
                    int index = token.IndexOf(':');
                    if(index > 0)
                    {
                        myKeyList.Add(token.Substring(0, index));
                        yourKeyList.Add(token.Substring(index + 1));
                    }
                    else
                    {
                        myKeyList.Add(token);
                        yourKeyList.Add(token);
                    }
                }
                myKeys = (string[]) myKeyList.ToArray(typeof(string));
                yourKeys = (string[]) yourKeyList.ToArray(typeof(string));
            }
            IRelationPropertyType rpt = CreateRelationPropertyType(propertyInfo,
                                          relnoAttr, myKeys, yourKeys, dbMetaData, dbms);
            return rpt;
        }

        protected virtual IRelationPropertyType CreateRelationPropertyType(PropertyInfo propertyInfo,
            RelnoAttribute relnoAttr, string[] myKeys, string[] yourKeys,
            IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            IBeanMetaData relationBmd = CreateRelationBeanMetaData(propertyInfo, dbMetaData, dbms);
            return new RelationPropertyTypeImpl(propertyInfo, relnoAttr.Relno, myKeys, yourKeys, relationBmd);
        }

        protected virtual IBeanMetaData CreateRelationBeanMetaData(PropertyInfo propertyInfo, IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            BeanMetaDataImpl bmdImpl = new BeanMetaDataImpl(propertyInfo.PropertyType, dbMetaData, dbms, AnnotationReaderFactory, true);
            return bmdImpl;
        }

        protected virtual void AddRelationPropertyType(IRelationPropertyType rpt)
        {
            for(int i = relationProeprtyTypes.Count; i <= rpt.RelationNo; ++i)
            {
                relationProeprtyTypes.Add(null);
            }
            relationProeprtyTypes[rpt.RelationNo] = rpt;
        }

        protected virtual void SetupAutoSelectList()
        {
            StringBuilder buf = new StringBuilder(100);
            buf.Append("SELECT ");
            for(int i = 0; i < PropertyTypeSize; ++i)
            {
                IPropertyType pt = GetPropertyType(i);
                if(pt.IsPersistent)
                {
                    buf.Append(tableName);
                    buf.Append(".");
                    buf.Append(pt.ColumnName);
                    buf.Append(", ");
                }
            }
            foreach(IRelationPropertyType rpt in relationProeprtyTypes)
            {
                IBeanMetaData bmd = rpt.BeanMetaData;
                for(int i = 0; i < bmd.PropertyTypeSize; ++i)
                {
                    IPropertyType pt = bmd.GetPropertyType(i);
                    if(pt.IsPersistent)
                    {
                        string columnName = pt.ColumnName;
                        buf.Append(rpt.PropertyName);
                        buf.Append(".");
                        buf.Append(columnName);
                        buf.Append(" AS ");
                        buf.Append(pt.ColumnName).Append("_");
                        buf.Append(rpt.RelationNo);
                        buf.Append(", ");
                    }
                }
            }
            buf.Length = buf.Length - 2;
            autoSelectList = buf.ToString();
        }
    }
}
