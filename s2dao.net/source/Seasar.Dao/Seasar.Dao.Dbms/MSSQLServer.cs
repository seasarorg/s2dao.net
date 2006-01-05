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

using System.Collections;
using System.Data;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
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
			base.SetupDatabaseMetaData(GetTableSet(dataSource, cn), dataSource, cn);
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
    }
}
