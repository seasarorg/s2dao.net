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
using System.Reflection;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Exceptions;

namespace Seasar.Dao.Id
{
    public abstract class AbstractIdentifierGenerator : IIdentifierGenerator
    {
        private static IDataReaderHandler dataReaderHandler = new ObjectDataReaderHandler();
        private string propertyName;
        private IDbms dbms;

        public AbstractIdentifierGenerator(string propertyName, IDbms dbms)
        {
            this.propertyName = propertyName;
            this.dbms = dbms;
        }

        public string PropertyName
        {
            get { return this.propertyName; }
        }

        public IDbms Dbms
        {
            get { return this.dbms; }
        }

        protected object ExecuteSql(IDataSource ds, string sql, object[] args)
        {
            BasicSelectHandler handler = new BasicSelectHandler(ds, sql, dataReaderHandler);
            return handler.Execute(args);
        }

        protected void SetIdentifier(object bean, object value)
        {
            if(propertyName == null) throw new EmptyRuntimeException("propertyName");
            PropertyInfo propertyInfo = bean.GetType().GetProperty(propertyName);
            
            if (propertyInfo.PropertyType == typeof(Nullables.NullableDecimal))
            {
                propertyInfo.SetValue(bean, Nullables.NullableDecimal.Parse(value.ToString()), null);
            }
            else
            {
                propertyInfo.SetValue(bean, Convert.ChangeType(value, propertyInfo.PropertyType), null);
            }
        }

        #region IIdentifierGenerator ÉÅÉìÉo

        public virtual bool IsSelfGenerate
        {
            get { throw new NotImplementedException(); }
        }

        public virtual void SetIdentifier(object bean, Seasar.Extension.ADO.IDataSource ds)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
