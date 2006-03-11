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
using Seasar.Dao;
using Seasar.Dao.Dbms;
using Seasar.Dao.Impl;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.ADO.Types;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Util;
using NUnit.Framework;

namespace Seasar.Dao.Tests.Impl
{
	/// <summary>
	/// BeanMetaDataDataReaderHandlerTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
    [TestFixture]
    public class BeanMetaDataDataReaderHandlerTest
	{
        private const string PATH = "Tests.dicon";
        private IBeanMetaData beanMetaData_;
        private IDataSource dataSource;

        [SetUp]
        public void SetUp()
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            dataSource = (IDataSource) container.GetComponent(typeof(IDataSource));

            IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            IDbms dbms = new MSSQLServer();
            beanMetaData_ = new BeanMetaDataImpl(typeof(Employee), new DatabaseMetaDataImpl(dataSource), dbms);
        }

	    [Test]
        public void TestHandle() 
        {
		    IDataReaderHandler handler = new BeanMetaDataDataReaderHandler(
				    beanMetaData_);
		    String sql = "select emp.*, dept.deptno as deptno_0, dept.dname as dname_0 from emp, dept where empno = 7788 and emp.deptno = dept.deptno";
            IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            //PreparedStatement ps = con.prepareStatement(sql);
            IDbCommand cmd = dataSource.GetCommand(sql,cn);
		    Employee ret = null;
		    try {
			    //ResultSet rs = ps.executeQuery();
                IDataReader rs = cmd.ExecuteReader();
			    try {
                    
                    Assert.Ignore("AbstractBeanMetaDataDataReaderHandler#CreateRowÇ≈valueType.GetValueÇ…é∏îsÇ∑ÇÈ");
                    
                    ret = (Employee) handler.Handle(rs);
			    } finally {
				    //rs.Close();
			    }
		    } finally {
			    //ps.close();
		    }
		    Assert.IsNotNull(ret, "1");
		    //System.out.println(ret.getEmpno() + "," + ret.getEname());
		    Department dept = ret.Department;
		    Assert.IsNotNull(dept, "2");
		    Assert.AreEqual(20, dept.Deptno, "3");
		    Assert.AreEqual("RESEARCH", dept.Dname, "4");
	    }

	    [Test]
        public void TestHandle2() {
		    IDataReaderHandler handler = new BeanMetaDataDataReaderHandler(
				    beanMetaData_);
		    String sql = "select ename, job from emp where empno = 7788";
            IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            //PreparedStatement ps = con.prepareStatement(sql);
            IDbCommand cmd = dataSource.GetCommand(sql,cn);
            Employee ret = null;
		    try {
                //ResultSet rs = ps.executeQuery();
                IDataReader rs = cmd.ExecuteReader();
                try 
                {
                    
                    Assert.Ignore("AbstractBeanMetaDataDataReaderHandler#CreateRowÇ≈valueType.GetValueÇ…é∏îsÇ∑ÇÈ");
                    
                    ret = (Employee) handler.Handle(rs);
			    } finally {
				    //rs.Close();
			    }
		    } finally {
			    //ps.close();
		    }
		    Assert.IsNotNull(ret, "1");
		    //System.out.println(ret.getEmpno() + "," + ret.getEname());
		    Department dept = ret.Department;
		    Assert.IsNull(dept, "2");
	    }

	    [Test]
        public void TestHandle3() {
		    IDataReaderHandler handler = new BeanMetaDataDataReaderHandler(
				    beanMetaData_);
		    String sql = "select ename, dept.dname as dname_0 from emp, dept where empno = 7788 and emp.deptno = dept.deptno";
            IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            //PreparedStatement ps = con.prepareStatement(sql);
            IDbCommand cmd = dataSource.GetCommand(sql,cn);
            Employee ret = null;
		    try {
                //ResultSet rs = ps.executeQuery();
                IDataReader rs = cmd.ExecuteReader();
                try 
                {
                    
                    Assert.Ignore("AbstractBeanMetaDataDataReaderHandler#CreateRowÇ≈valueType.GetValueÇ…é∏îsÇ∑ÇÈ");
                    
                    ret = (Employee) handler.Handle(rs);
			    } finally {
				    //rs.Close();
			    }
		    } finally {
			    //ps.close();
		    }
            Assert.IsNotNull(ret, "1");
            //System.out.println(ret.getEname());
		    Department dept = ret.Department;
            Assert.IsNull(dept, "2");
            Assert.AreEqual("RESEARCH", dept.Dname, "3");
	    }
	}
}
