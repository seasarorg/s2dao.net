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
using System.Data;
using System.Data.SqlClient;
using Seasar.Extension.ADO;

namespace Seasar.Extension.ADO.Types
{
    public class StringType : BaseValueType, IValueType
    {

        public StringType(IDataSource dataSource)
            : base(dataSource)
        {
        }

        #region IValueType ÉÅÉìÉo

        public object GetValue(System.Data.IDataReader reader, int index)
        {
            return reader.GetString(index);
        }

        object Seasar.Extension.ADO.IValueType.GetValue(System.Data.IDataReader reader, string columnName)
        {
            return (string) reader[columnName];
        }

        public void BindValue(System.Data.IDbCommand cmd, string columnName, object value)
        {
            BindValue(cmd, columnName, value, DbType.String);
        }

        #endregion
    }
}
