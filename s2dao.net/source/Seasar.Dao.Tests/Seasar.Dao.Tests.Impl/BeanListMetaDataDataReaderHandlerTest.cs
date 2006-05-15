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
using System.Data.SqlClient;
using Seasar.Dao;
using Seasar.Dao.Dbms;
using Seasar.Dao.Impl;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.ADO.Types;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Util;
using MbUnit.Framework;

namespace Seasar.Dao.Tests.Impl
{

    [TestFixture]
    public class BeanListMetaDataDataReaderHandlerTest
	{
        private const string PATH = "Tests.dicon";
        private IBeanMetaData beanMetaData_;

        [SetUp]
        public void SetUp()
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            IDataSource dataSource = (IDataSource) container.GetComponent(typeof(IDataSource));

            IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            IDbms dbms = new MSSQLServer();
            beanMetaData_ = new BeanMetaDataImpl(typeof(Employee), new DatabaseMetaDataImpl(dataSource), dbms);
        }

        [Test]
        public void TestHandle()
        {
            IDataReaderHandler handler = new BeanListMetaDataDataReaderHandler(
                beanMetaData_);
            String sql = "select * from emp";

            IS2Container container = S2ContainerFactory.Create(PATH);
            IDataSource dataSource = (IDataSource) container.GetComponent(typeof(IDataSource));

            //SQLServer—p
            SqlConnection cn = (SqlConnection)DataSourceUtil.GetConnection(dataSource);
            SqlCommand cmd = new SqlCommand(sql, cn);

            IList ret = null;
            try 
            {
                SqlDataReader rs = cmd.ExecuteReader();
                try 
                {
                    ret = (IList) handler.Handle(rs);
                } 
                finally 
                {
                    rs.Close();
                }
            } 
            finally 
            {
                cn.Close();
            }
            Assert.IsNotNull(ret, "1");
            //for (int i = 0; i < ret.Count; ++i) 
            //{
            //    Employee emp = (Employee) ret.get(i);
            //    ////System.out.println(emp.getEmpno() + "," + emp.getEname());
            //}
            foreach (object itm in ret) 
            {
                Employee emp = (Employee) itm;
                ////System.out.println(emp.getEmpno() + "," + emp.getEname());
            }

        }

        [Test]
        public void TestHandle2()
        {
            IDataReaderHandler handler = new BeanListMetaDataDataReaderHandler(
                beanMetaData_);
            String sql = "select emp.*, dept.dname as dname_0 from emp, dept where emp.deptno = dept.deptno and emp.deptno = 20";

            IS2Container container = S2ContainerFactory.Create(PATH);
            IDataSource dataSource = (IDataSource) container.GetComponent(typeof(IDataSource));

            //SQLServer—p
            SqlConnection cn = (SqlConnection)DataSourceUtil.GetConnection(dataSource);
            SqlCommand cmd = new SqlCommand(sql, cn);

            IList ret = null;
            try 
            {
                SqlDataReader rs = cmd.ExecuteReader();
                try 
                {
                    ret = (IList) handler.Handle(rs);
                } 
                finally 
                {
                    rs.Close();
                }
            } 
            finally 
            {
                cn.Close();
            }
            Assert.IsNotNull(ret, "1");
            foreach (object itm in ret) 
            {
                Employee emp = (Employee) itm;
                ////System.out.println(emp);
                Department dept = emp.Department;
                Assert.IsNotNull(dept,"2");
                Assert.AreEqual(emp.Deptno, dept.Deptno, "3");
                Assert.IsNotNull(dept.Dname,"4");
            }

        }

        [Test]
        public void TestHandle3()
        {
            IDataReaderHandler handler = new BeanListMetaDataDataReaderHandler(
                beanMetaData_);
            String sql = "select emp.*, dept.deptno as deptno_0, dept.dname as dname_0 from emp, dept where dept.deptno = 20 and emp.deptno = dept.deptno";
            IS2Container container = S2ContainerFactory.Create(PATH);
            IDataSource dataSource = (IDataSource) container.GetComponent(typeof(IDataSource));

            //SQLServer—p
            SqlConnection cn = (SqlConnection)DataSourceUtil.GetConnection(dataSource);
            SqlCommand cmd = new SqlCommand(sql, cn);

            IList ret = null;
            try 
            {
                SqlDataReader rs = cmd.ExecuteReader();
                try 
                {
                    ret = (IList) handler.Handle(rs);
                } 
                finally 
                {
                    rs.Close();
                }
            } 
            finally 
            {
                cn.Close();
            }

            IEnumerator employees = ret.GetEnumerator();
            employees.MoveNext();
            Employee emp = (Employee) employees.Current;
            employees.MoveNext();
            Employee emp2 = (Employee) employees.Current;
            Assert.AreSame(emp.Department, emp2.Department,"1");
        }

	}
}
