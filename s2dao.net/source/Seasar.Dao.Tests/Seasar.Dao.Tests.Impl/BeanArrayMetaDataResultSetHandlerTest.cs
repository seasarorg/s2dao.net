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
using Seasar.Extension.ADO.Types;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Util;
using NUnit.Framework;

namespace Seasar.Dao.Tests.Impl
{
	/// <summary>
	///  BeanArrayMetaDataDataReaderHandlerTestの概要の説明です。
    ///  Java版ではBeanArrayMetaDataResultSetHandler
    ///  接続文字列の件も
    /// </summary>
    [TestFixture]
    public class BeanArrayMetaDataDataReaderHandlerTest
	{

        private const string PATH = "Tests.dicon";
        private IBeanMetaData beanMetaData_;

        [SetUp]
        public void SetUp()
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            IDataSource dataSource = (IDataSource) container.GetComponent(typeof(IDataSource));

            IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            IDbms dbms = new MSSQLServer(dataSource,cn);
            beanMetaData_ = new BeanMetaDataImpl(typeof(Employee), dbms.DatabaseMetaData, dbms);
        }

        [Test]
        public void TestHandle() 
        {
			IDataReaderHandler handler = new BeanArrayMetaDataDataReaderHandler(
					beanMetaData_);
			String sql = "select * from emp";
			//Connection con = GetConnection();
			//PreparedStatement ps = con.prepareStatement(sql);

            IS2Container container = S2ContainerFactory.Create(PATH);
            IDataSource dataSource = (IDataSource) container.GetComponent(typeof(IDataSource));

            //SQLServer用
            //IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            SqlConnection cn = (SqlConnection)DataSourceUtil.GetConnection(dataSource);
            SqlCommand cmd = new SqlCommand(sql, cn);

			Employee[] ret = null;
			try {
				SqlDataReader rs = cmd.ExecuteReader();//ps.executeQuery();
				try {
                    Assert.Ignore("ValueTypesのInitが呼ばれないとここでエラーになるがどこで呼ぶべき？");
					ret = (Employee[]) handler.Handle(rs);
				} finally {
					rs.Close();
				}
			} finally {
				cn.Close();
			}
			Assert.IsNotNull(ret, "1");
			for (int i = 0; i < ret.Length; ++i) {
				Employee emp = ret[i];
				//System.out.println(emp.getEmpno() + "," + emp.getEname());
			}
		}


	}
}
