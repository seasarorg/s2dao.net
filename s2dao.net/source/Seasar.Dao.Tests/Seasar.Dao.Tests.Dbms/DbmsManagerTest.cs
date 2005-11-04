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
using Seasar.Dao.Impl;
using Seasar.Dao.Dbms;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Util;
using NUnit.Framework;

namespace Seasar.Dao.Tests.Dbms
{

    /// <summary>
    /// MSSQLServerTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
    /// </summary>
    [TestFixture]
	public class DbmsManagerTest {

        private const string PATH = "Tests.dicon";

        [Test]
        public void TestCreateAutoSelectList() 
        {
            //Stringà¯êîÇÃé¿ëïÇ»Çµ
			//Assert.IsNotNull(DbmsManager.getDbms(""),"1");
			//Assert.IsNotNull(DbmsManager.getDbms("HSQL Database Engine"),"2");

            //DataProvider sqlClient = new DataProvider();
            //sqlClient.ConnectionType = "System.Data.SqlClient.SqlConnection";
            //sqlClient.CommandType = "System.Data.SqlClient.SqlCommand";
            //sqlClient.ParameterType = "System.Data.SqlClient.SqlParameter";
            //sqlClient.DataAdapterType = "System.Data.SqlClient.SqlDataAdapter";
            //IDataSource dataSource = new DataSourceImpl(sqlClient,"Server=127.0.0.1;database=s2dotnetdemo;Password=demopass;User ID=demouser");

            IS2Container container = S2ContainerFactory.Create(PATH);
            IDataSource dataSource = (IDataSource) container.GetComponent(typeof(IDataSource));

            Assert.IsNotNull(DbmsManager.GetDbms(dataSource),"3");
            
            IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            Assert.IsNotNull(DbmsManager.GetDbms(dataSource,cn),"4");


		}
	}
}