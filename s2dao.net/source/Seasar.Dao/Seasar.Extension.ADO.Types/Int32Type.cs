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
using Seasar.Extension.ADO;

namespace Seasar.Extension.ADO.Types
{
    public class Int32Type :BaseValueType, IValueType
    {
        public Int32Type(IDataSource dataSource)
            : base(dataSource)
        {
        }

        #region IValueType �����o

        public object GetValue(System.Data.IDataReader reader, int index)
        {
            return Convert.ToInt32(reader.GetValue(index));
        }

        object Seasar.Extension.ADO.IValueType.GetValue(System.Data.IDataReader reader, string columnName)
        {
            return Convert.ToInt32(reader[columnName]);
        }

        public void BindValue(System.Data.IDbCommand cmd, string columnName, object value)
        {
            BindValue(cmd, columnName, value, DbType.Int32);
        }

        #endregion
    }
}
