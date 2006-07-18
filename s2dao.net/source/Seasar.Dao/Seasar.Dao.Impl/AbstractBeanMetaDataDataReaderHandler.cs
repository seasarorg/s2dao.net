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
using System.Data;
using System.Reflection;
using Seasar.Extension.ADO;
using Seasar.Framework.Util;

namespace Seasar.Dao.Impl
{
    public abstract class AbstractBeanMetaDataDataReaderHandler 
        : IDataReaderHandler
    {
        private IBeanMetaData beanMetaData;

        public AbstractBeanMetaDataDataReaderHandler(IBeanMetaData beanMetaData)
        {
            this.beanMetaData = beanMetaData;
        }

        public IBeanMetaData BeanMetaData
        {
            get { return beanMetaData; }
        }

        protected object CreateRow(IDataReader reader, IList columnNames)
        {
            object row = ClassUtil.NewInstance(beanMetaData.BeanType);
            for(int i = 0; i < beanMetaData.PropertyTypeSize; ++i)
            {
                IPropertyType pt = beanMetaData.GetPropertyType(i);
                if(columnNames.Contains(pt.ColumnName))
                {
                    IValueType valueType = pt.ValueType;
                    PropertyInfo pi = pt.PropertyInfo;
                    object value = valueType.GetValue(reader, pt.ColumnName);
                    pi.SetValue(row, value, null);
                }
                else if(columnNames.Contains(pt.PropertyName))
                {
                    IValueType valueType = pt.ValueType;
                    PropertyInfo pi = pt.PropertyInfo;
                    object value = valueType.GetValue(reader, pt.PropertyName);
                    pi.SetValue(row, value, null);
                }
                else if(!pt.IsPersistent)
                {
                    for(IEnumerator enu = columnNames.GetEnumerator(); enu.MoveNext();)
                    {
                        string columnName = (string) enu.Current;
                        string columnName2 = columnName.Replace("_", "");
                        if(string.Compare(columnName2, pt.ColumnName, true) == 0)
                        {
                            IValueType valueType = pt.ValueType;
                            PropertyInfo pi = pt.PropertyInfo;
                            object value = valueType.GetValue(reader, columnName);
                            pi.SetValue(row, value, null);
                            break;
                        }
                    }
                }
            }
            return row;
        }

        protected object CreateRelationRow(IDataReader reader, IRelationPropertyType rpt,
            IList columnNames, Hashtable relKeyValues)
        {
            object row = null;
            IBeanMetaData bmd = rpt.BeanMetaData;
            for(int i = 0; i < rpt.KeySize; ++i)
            {
                string columnName = rpt.GetMyKey(i);
                if(columnNames.Contains(columnName))
                {
                    if(row == null) row = CreateRelationRow(rpt);
                    if(relKeyValues != null && relKeyValues.ContainsKey(columnName))
                    {
                        object value = relKeyValues[columnName];
                        IPropertyType pt = bmd.GetPropertyTypeByColumnName(rpt.GetYourKey(i));
                        PropertyInfo pi = pt.PropertyInfo;
                        if(value != null) pi.SetValue(row, value, null);
                    }
                }
                continue;
            }
            for(int i = 0; i < bmd.PropertyTypeSize; ++i)
            {
                IPropertyType pt = bmd.GetPropertyType(i);
                string columnName = pt.ColumnName + "_" + rpt.RelationNo;
                if(!columnNames.Contains(columnName)) continue;
                if(row == null) row = CreateRelationRow(rpt);
                object value = null;
                PropertyInfo pi = pt.PropertyInfo;
                if(relKeyValues != null && relKeyValues.ContainsKey(columnName))
                {
                    value = relKeyValues[columnName];
                }
                else
                {
                    IValueType valueType = pt.ValueType;
                    value = valueType.GetValue(reader, columnName);
                }
                if(value != null) pi.SetValue(row, value, null);
            }
            return row;
        }

        protected object CreateRelationRow(IRelationPropertyType rpt)
        {
            return ClassUtil.NewInstance(rpt.PropertyInfo.PropertyType);
        }

        protected IList CreateColumnNames(DataTable dt)
        {
            IList columnNames = new CaseInsentiveSet();
            foreach(DataRow row in dt.Rows)
            {
                string columnName = (string) row["ColumnName"];
                columnNames.Add(columnName);
            }
            return columnNames;
        }

        #region IDataReaderHandler ƒƒ“ƒo

        public virtual object Handle(System.Data.IDataReader dataReader)
        {
            return null;
        }

        #endregion
    }
}
