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
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Tx.Impl;
using Seasar.Framework.Util;

namespace Seasar.Dao.Dbms
{
    public class MSSQLServer : Standard
    {
        public override string Suffix
        {
            get { return "_mssql"; }
        }

        public override string IdentitySelectString
        {
            get { return "select @@identity"; }
        }

        public override KindOfDbms Dbms
        {
            get
            {
                return KindOfDbms.MSSQLServer;
            }
        }

        public MSSQLServer(IDataSource dataSource, IDbConnection cn)
        {
            IList tableSet = GetTableSet(dataSource, cn);
                
            IDictionary primaryKeys = new Hashtable(CaseInsensitiveHashCodeProvider.Default,
                CaseInsensitiveComparer.Default);
            IDictionary columns = new Hashtable(CaseInsensitiveHashCodeProvider.Default,
                CaseInsensitiveComparer.Default);
            IEnumerator tables = tableSet.GetEnumerator();

            while(tables.MoveNext())
            {
                primaryKeys[tables.Current] = GetPrimaryKeySet(
                    dataSource, cn, (string) tables.Current);
                columns[tables.Current] = GetColumnSet(dataSource, cn, (string) tables.Current);
            }
                
            DatabaseMetaDataImpl dbMetaData = new DatabaseMetaDataImpl();
            dbMetaData.TableSet = tableSet;
            dbMetaData.PrimaryKeys = primaryKeys;
            dbMetaData.Columns = columns;
            this.dbMetadata = dbMetaData;
        }

        protected IList GetTableSet(IDataSource dataSource, IDbConnection cn)
        {
            IList list = new CaseInsentiveSet();
            string cmdText = "sp_tables @table_type=\"'TABLE'\"";
            
            using(IDbCommand cmd = dataSource.GetCommand(cmdText, cn))
            {
                DataSourceUtil.SetTransaction(dataSource, cmd);
                using(IDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        list.Add(reader["table_name"]);
                    }
                }
            }
            return list;
        }

        protected IList GetPrimaryKeySet(IDataSource dataSource, IDbConnection cn, string tableName)
        {
            IList list = new CaseInsentiveSet();
            string cmdText = "sp_pkeys";
            using(IDbCommand cmd = dataSource.GetCommand(cmdText, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(dataSource.GetParameter("@table_name", tableName));
                DataSourceUtil.SetTransaction(dataSource, cmd);
                using(IDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        list.Add(reader["column_name"]);
                    }
                }
            }
            return list;
        }

        protected IList GetColumnSet(IDataSource dataSource, IDbConnection cn, string tableName)
        {
            IList list = new CaseInsentiveSet();
            string cmdText = "sp_columns";
            using(IDbCommand cmd = dataSource.GetCommand(cmdText, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(dataSource.GetParameter("@table_name", tableName));
                DataSourceUtil.SetTransaction(dataSource, cmd);
                using(IDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        list.Add(reader["column_name"]);
                    }
                }
            }
            return list;
        }
    }
}
