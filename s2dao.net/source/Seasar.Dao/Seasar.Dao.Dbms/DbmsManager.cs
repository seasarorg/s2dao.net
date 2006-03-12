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
using System.Data.OleDb;
using System.Reflection;
using System.Resources;
using Seasar.Extension.ADO;

namespace Seasar.Dao.Dbms
{
    public sealed class DbmsManager
    {
        private static ResourceManager resourceManager;

        static DbmsManager()
        {
            resourceManager = new ResourceManager(
                "Dbms", Assembly.GetExecutingAssembly());
        }

        private DbmsManager()
        {
        }

        public static IDbms GetDbms(IDataSource dataSource)
        {
            IDbms dbms = null;
            IDbConnection cn = dataSource.GetConnection();
            if(cn is OleDbConnection)
            {
                OleDbConnection oleDbCn = cn as OleDbConnection;
                dbms = GetDbms(cn.GetType().Name + "_" + oleDbCn.Provider);
            }
            else
            {
                dbms = GetDbms(cn.GetType().Name);
            }
            if(dbms == null)
                dbms = GetDbms("");
            return dbms;
        }

        private static IDbms GetDbms(string name)
        {
            return (IDbms) Activator.CreateInstance(Type.GetType(
                resourceManager.GetString(name)), false);
        }
    }
}
