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
using System.Data;
using System.Reflection;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Log;
using Seasar.Framework.Util;

namespace Seasar.Dao.Impl
{
    public abstract class AbstractAutoHandler : BasicHandler, IUpdateHandler
    {
        private static Logger logger = Logger.GetLogger(typeof(AbstractAutoHandler));
        private IBeanMetaData beanMetaData;
        private object[] bindVariables;
        private Type[] bindVariableTypes;
        private DateTime timestamp = DateTime.MinValue;
        private Int32 versionNo = Int32.MinValue;
        private IPropertyType[] propertyTypes;

        public AbstractAutoHandler(IDataSource dataSource, ICommandFactory commandFactory,
            IBeanMetaData beanMetaData, IPropertyType[] propertyTypes)
        {
            this.DataSource = dataSource;
            this.CommandFactory = commandFactory;
            this.beanMetaData = beanMetaData;
            this.propertyTypes = propertyTypes;
        }

        public new IDataSource DataSource
        {
            get { return base.DataSource; }
            set 
            {
                if (value is ConnectionHolderDataSource) 
                {
                    base.DataSource = value;
                } 
                else 
                {
                    base.DataSource = new ConnectionHolderDataSource(value);
                }
            }
        }

        public IBeanMetaData BeanMetaData
        {
            get { return beanMetaData; }
        }

        protected static Logger Logger
        {
            get { return logger; }
        }

        protected object[] BindVariables
        {
            get { return bindVariables; }
            set { bindVariables = value; }
        }

        protected Type[] BindVariableTypes
        {
            get { return bindVariableTypes; }
            set { bindVariableTypes = value; }
        }

        protected DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        protected int VersionNo
        {
            get { return versionNo; }
            set { versionNo = value; }
        }

        protected IPropertyType[] PropertyTypes
        {
            get { return propertyTypes; }
            set { propertyTypes = value; }
        }

        #region IUpdateHandler ÉÅÉìÉo

        public int Execute(object[] args)
        {
            IDbConnection connection = this.Connection;

            try
            {
                return Execute(connection, args[0]);
            }
            finally
            {
                ConnectionHolderDataSource holderDataSoure = DataSource as ConnectionHolderDataSource;
                holderDataSoure.ReleaseConnection();
                DataSourceUtil.CloseConnection(holderDataSoure, connection);
            }
        }

        public int Execute(object[] args, Type[] argTypes)
        {
            return Execute(args);
        }

        public int Execute(object[] args, Type[] argTypes, string[] argNames)
        {
            return Execute(args);
        }

        #endregion

        protected int Execute(IDbConnection connection, object bean)
        {
            PreUpdateBean(bean);
            SetupBindVariables(bean);
            if(logger.IsDebugEnabled) logger.Debug(GetCompleteSql(bindVariables));
            IDbCommand cmd = Command(connection);
            int ret = -1;
            try
            {
                BindArgs(cmd, bindVariables, bindVariableTypes);
                ret = CommandUtil.ExecuteNonQuery(this.DataSource, cmd);
            }
            finally
            {
                CommandUtil.Close(cmd);
            }
            PostUpdateBean(bean);
            return ret;
        }

        protected virtual void PreUpdateBean(object bean)
        {
        }

        protected virtual void PostUpdateBean(object bean)
        {
        }

        protected abstract void SetupBindVariables(object bean);

        protected void SetupInsertBindVariables(object bean)
        {
            ArrayList varList = new ArrayList();
            ArrayList varTypeList = new ArrayList();
            for (int i = 0; i < propertyTypes.Length; ++i)
            {
                IPropertyType pt = propertyTypes[i];
                if (string.Compare(pt.PropertyName, BeanMetaData.TimestampPropertyName, true) == 0)
                {
                    this.Timestamp = DateTime.Now;
                    SetupTimestampVariableList(varList, pt);
                }
                else if (pt.PropertyName.Equals(this.BeanMetaData.VersionNoPropertyName))
                {
                    this.VersionNo = 0;
                    varList.Add(this.VersionNo);
                }
                else
                {
                    varList.Add(pt.PropertyInfo.GetValue(bean, null));
                }
                varTypeList.Add(pt.PropertyInfo.PropertyType);
            }
            BindVariables = varList.ToArray();
            BindVariableTypes = (Type[])varTypeList.ToArray(typeof(Type));
        }

        protected void SetupUpdateBindVariables(object bean)
        {
            ArrayList varList = new ArrayList();
            ArrayList varTypeList = new ArrayList();
            for(int i = 0; i < propertyTypes.Length; ++i)
            {
                IPropertyType pt = propertyTypes[i];
                if(string.Compare(pt.PropertyName, BeanMetaData.TimestampPropertyName, true) == 0)
                {
                    this.Timestamp = DateTime.Now;
                    SetupTimestampVariableList(varList, pt);
                }
                else if(string.Compare(pt.PropertyName, BeanMetaData.VersionNoPropertyName, true) ==0)
                {
                    object value = pt.PropertyInfo.GetValue(bean, null);
                    int intValue = Convert.ToInt32(value) + 1;
                    this.VersionNo = intValue;
                    varList.Add(this.VersionNo);
                }
                else
                {
                    varList.Add(pt.PropertyInfo.GetValue(bean, null));
                }
                varTypeList.Add(pt.PropertyInfo.PropertyType);
            }
            AddAutoUpdateWhereBindVariables(varList, varTypeList, bean);
            BindVariables = varList.ToArray();
            BindVariableTypes = (Type[]) varTypeList.ToArray(typeof(Type));
        }

        protected void SetupDeleteBindVariables(object bean)
        {
            ArrayList varList = new ArrayList();
            ArrayList varTypeList = new ArrayList();
            AddAutoUpdateWhereBindVariables(varList, varTypeList, bean);
            BindVariables = varList.ToArray();
            BindVariableTypes = (Type[]) varTypeList.ToArray(typeof(Type));
        }

        protected void AddAutoUpdateWhereBindVariables(ArrayList varList, ArrayList varTypeList,
            object bean)
        {
            IBeanMetaData bmd = this.BeanMetaData;
            for(int i = 0; i < bmd.PrimaryKeySize; ++i)
            {
                IPropertyType pt = bmd.GetPropertyTypeByColumnName(bmd.GetPrimaryKey(i));
                PropertyInfo pi = pt.PropertyInfo;
                varList.Add(pi.GetValue(bean, null));
                varTypeList.Add(pi.PropertyType);
            }
            if(bmd.HasVersionNoPropertyType)
            {
                IPropertyType pt = bmd.VersionNoPropertyType;
                PropertyInfo pi = pt.PropertyInfo;
                varList.Add(pi.GetValue(bean, null));
                varTypeList.Add(pi.PropertyType);
            }
            if(bmd.HasTimestampPropertyType)
            {
                IPropertyType pt = bmd.TimestampPropertyType;
                PropertyInfo pi = pt.PropertyInfo;
                varList.Add(pi.GetValue(bean, null));
                varTypeList.Add(pi.PropertyType);
            }
        }

        protected void UpdateTimestampIfNeed(object bean)
        {
            if(Timestamp != DateTime.MinValue)
            {
                PropertyInfo pi = BeanMetaData.TimestampPropertyType.PropertyInfo;
                SetupTimestampPropertyInfo(pi, bean);
            }
        }

        protected void UpdateVersionNoIfNeed(object bean)
        {
            if(VersionNo != Int32.MinValue)
            {
                PropertyInfo pi = BeanMetaData.VersionNoPropertyType.PropertyInfo;
                pi.SetValue(bean, VersionNo, null);
            }
        }

        protected void SetupTimestampVariableList(IList varList, IPropertyType pt)
        {
            if (pt.PropertyType == typeof(DateTime))
            {
                varList.Add(this.Timestamp);
            }
            else if (pt.PropertyType == typeof(Nullables.NullableDateTime))
            {
                varList.Add(new Nullables.NullableDateTime(this.Timestamp));
            }
            else
            {
                String msg = "The property type is wrong: name=" + pt.PropertyName + " type=" + pt.PropertyType;
                throw new SystemException(msg);
            }
        }

        protected void SetupTimestampPropertyInfo(PropertyInfo pi, object bean)
        {
            if (pi.PropertyType == typeof(DateTime))
            {
                pi.SetValue(bean, Timestamp, null);
            }
            else if (pi.PropertyType == typeof(Nullables.NullableDateTime))
            {
                pi.SetValue(bean, new Nullables.NullableDateTime(this.Timestamp), null);
            }
            else
            {
                String msg = "The property type is wrong: name=" + pi.Name + " type=" + pi.PropertyType;
                throw new SystemException(msg);
            }
        }
    }
}
