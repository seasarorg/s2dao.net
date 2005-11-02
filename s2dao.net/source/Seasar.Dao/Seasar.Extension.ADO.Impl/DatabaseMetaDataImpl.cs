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

namespace Seasar.Extension.ADO.Impl
{
    public class DatabaseMetaDataImpl : IDatabaseMetaData
    {
        private IList tableSet;
        private IDictionary primaryKeys;
        private IDictionary columns;

        public DatabaseMetaDataImpl()
        {
        }

        public IList TableSet
        {
            set { tableSet = value; }
        }

        public IDictionary PrimaryKeys
        {
            set { primaryKeys = value; }
        }

        public IDictionary Columns
        {
            set { columns = value; }
        }

        #region IDatabaseMetaData ÉÅÉìÉo

        public System.Collections.IList GetPrimaryKeySet(string tableName)
        {
            return (IList) primaryKeys[tableName];
        }

        public IList GetTableSet()
        {
            return tableSet;
        }

        public IList GetColumnSet(string tableName)
        {
            return (IList) columns[tableName];
        }

        #endregion
    }
}
