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
using Seasar.Dao;
using Seasar.Dao.Dbms;
using Seasar.Dao.Impl;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.ADO.Types;
using Seasar.Extension.Unit;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Util;
using MbUnit.Framework;

namespace Seasar.Dao.Tests.Impl
{

    [TestFixture]
    public class BeanArrayMetaDataDataReaderHandlerTest : S2TestCase
	{
        private IBeanMetaData beanMetaData_;

        [Test, S2()]
        public void TestHandle() 
        {
            IDataSource dataSource = (IDataSource) GetComponent(typeof(IDataSource));
            IDbms dbms = new MSSQLServer();
            beanMetaData_ = new BeanMetaDataImpl(typeof(Employee), new DatabaseMetaDataImpl(dataSource), dbms);

			IDataReaderHandler handler = new BeanArrayMetaDataDataReaderHandler(beanMetaData_);

			String sql = "select * from emp";
            SqlCommand cmd = new SqlCommand(sql, this.Connection as SqlConnection);

			Employee[] ret = null;

			SqlDataReader reader = cmd.ExecuteReader();
			try {
				ret = (Employee[]) handler.Handle(reader);
			} finally {
				reader.Close();
			}

			Assert.IsNotNull(ret, "1");
			for (int i = 0; i < ret.Length; ++i) {
				Employee emp = ret[i];
                Console.WriteLine(emp.Empno + "," + emp.Ename);
			}

		}


	}
}
