#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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
        private string tableName;
        private Hashtable propertyTypesByColumnName = new Hashtable(
            CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
        private ArrayList relationProeprtyTypes = new ArrayList();
        private string[] primaryKeys = new string[0];
        private string autoSelectList;
        private bool relation;
        private IIdentifierGenerator identifierGenerator;
        private string versionNoPropertyName = "versionNo";
        private string timestampPropertyName = "timestamp";

        public BeanMetaDataImpl(Type beanType, IDatabaseMetaData dbMetaData,
            IDbms dbms) : this(beanType, dbMetaData, dbms, false)
        {
        }

        public BeanMetaDataImpl(Type beanType, IDatabaseMetaData dbMetaData,
            IDbms dbms, bool relation)
        {
            BeanType = beanType;
            this.relation = relation;
            SetupTableName(beanType);
            SetupVersionNoPropertyName(beanType);
            SetupTimestampPropertyName(beanType);
            SetupProperty(beanType, dbMetaData, dbms);
            SetupDatabaseMetaData(beanType, dbMetaData, dbms);
            SetupPropertiesByColumnName();
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

        protected void SetupTableName(Type beanType)
        {
            TableAttribute attr = AttributeUtil.GetTableAttribute(beanType);
            if(attr != null)
                tableName = attr.TableName;
            else
                tableName = beanType.Name;
        }

        protected void SetupVersionNoPropertyName(Type beanType)
        {
            VersionNoPropertyAttribute attr = AttributeUtil.GetVersionNoPropertyAttribute(beanType);
            if(attr != null) versionNoPropertyName = attr.PropertyName;
        }

        protected void SetupTimestampPropertyName(Type beanType)
        {
            TimestampPropertyAttribute attr = AttributeUtil.GetTimestampPropertyAttribute(beanType);
            if(attr != null) timestampPropertyName = attr.PropertyName;
        }

        protected void SetupProperty(Type beanType, IDatabaseMetaData dbMetaData,IDbms dbms)
        {
            foreach(PropertyInfo pi in beanType.GetProperties())
            {
                IPropertyType pt = null;
                RelnoAttribute relnoAttr = AttributeUtil.GetRelnoAttribute(pi);
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
                    IDAttribute idAttr = AttributeUtil.GetIDAttribute(pi);
                    if(idAttr != null)
                    {
                        identifierGenerator = IdentifierGeneratorFactory.CreateIdentifierGenerator(
                            pi.Name, dbms, idAttr);
                        primaryKeys = new string[] { pt.ColumnName };
                        pt.IsPrimaryKey = true;
                    }
                }
            }
        }

        protected void SetupDatabaseMetaData(Type beanType,
            IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            SetupPropertyPersistentAndColumnName(beanType, dbMetaData);
            SetupPrimaryKey(dbMetaData, dbms);
        }

        protected void SetupPrimaryKey(IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            if(IdentifierGenerator == null)
            {
                ArrayList pkeyList = new ArrayList();
                IList primaryKeySet = dbMetaData.GetPrimaryKeySet(tableName);
                for(int i = 0; i < PropertyTypeSize; ++i)
                {
                    IPropertyType pt = GetPropertyType(i);
                    if(primaryKeySet.Contains(pt.ColumnName))
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

        protected void SetupPropertyPersistentAndColumnName(Type beanType,
            IDatabaseMetaData dbMetaData)
        {
            IList columnSet = dbMetaData.GetColumnSet(tableName);
            if(columnSet == null || columnSet.Count == 0)
            {
                logger.Log("WDAO0002", new object[] { tableName });
            } else {
                for(IEnumerator enu = columnSet.GetEnumerator(); enu.MoveNext();)
                {
                    string columnName = (string) enu.Current;
                    string columnName2 = columnName.Replace("_", "");
                    for(int i = 0; i < PropertyTypeSize; ++i)
                    {
                        IPropertyType pt = GetPropertyType(i);
                        if(string.Compare(pt.ColumnName, columnName2, true) == 0)
                        {
                            pt.ColumnName = columnName;
                            break;
                        }
                    }
                }
            }
            NoPersistentPropsAttribute noPersistentPropsAttr =
                AttributeUtil.GetNoPersistentPropsAttribute(beanType);
            if(noPersistentPropsAttr != null)
            {
                foreach(string prop in noPersistentPropsAttr.Props)
                {
                    IPropertyType pt = GetPropertyType(prop);
                    pt.IsPersistent = false;
                }
            }
            else
            {
                for(int i = 0; i < PropertyTypeSize; ++i)
                {
                    IPropertyType pt = GetPropertyType(i);
                    if(!columnSet.Contains(pt.ColumnName))
                        pt.IsPersistent = false;
                }
            }
        }

        protected void SetupPropertiesByColumnName()
        {
            for(int i = 0; i < PropertyTypeSize; ++i)
            {
                IPropertyType pt = GetPropertyType(i);
                propertyTypesByColumnName[pt.ColumnName] = pt;
            }
        }

        protected IRelationPropertyType CreateRelationPropertyType(Type beanType,
            PropertyInfo propertyInfo, RelnoAttribute relnoAttr,
            IDatabaseMetaData dbMetaData, IDbms dbms)
        {
            string[] myKeys = new string[0];
            string[] yourKeys = new string[0];
            RelkeysAttribute relkeysAttr = 
                AttributeUtil.GetRelkeysAttribute(propertyInfo);
            if(relkeysAttr != null)
            {
                ArrayList myKeyList = new ArrayList();
                ArrayList yourKeyList = new ArrayList();
                foreach(string token in relkeysAttr.Relkeys.Split(
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
            IRelationPropertyType rpt = new RelationPropertyTypeImpl(propertyInfo,
                relnoAttr.Relno, myKeys, yourKeys, dbMetaData, dbms);
            return rpt;
        }

        protected void AddRelationPropertyType(IRelationPropertyType rpt)
        {
            for(int i = relationProeprtyTypes.Count; i <= rpt.RelationNo; ++i)
            {
                relationProeprtyTypes.Add(null);
            }
            relationProeprtyTypes[rpt.RelationNo] = rpt;
        }

        protected void SetupAutoSelectList()
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
