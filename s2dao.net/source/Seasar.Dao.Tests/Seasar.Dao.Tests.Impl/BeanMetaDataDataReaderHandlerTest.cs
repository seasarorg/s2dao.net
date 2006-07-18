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
using Seasar.Extension.Unit;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Util;
using MbUnit.Framework;

namespace Seasar.Dao.Tests.Impl
{
    [TestFixture]
    public class BeanMetaDataDataReaderHandlerTest : S2TestCase
	{

        public IBeanMetaData CreateBeanMetaData()
        {
            IDbms dbms = new MSSQLServer();
            
            IBeanMetaData beanMetaData = new BeanMetaDataImpl(typeof(Employee), 
                new DatabaseMetaDataImpl(DataSource), dbms);

            return beanMetaData;
        }

	    [Test, S2]
        public void TestHandle() 
        {
		    IDataReaderHandler handler = new BeanMetaDataDataReaderHandler(
				    CreateBeanMetaData());
		    String sql = "select emp.*, dept.deptno as deptno_0, dept.dname as dname_0 " +
                "from emp, dept where empno = 7788 and emp.deptno = dept.deptno";
            Employee ret = null;
            using(IDbCommand cmd = DataSource.GetCommand(sql,Connection))
            {
                using(IDataReader rs = cmd.ExecuteReader())
			    {
                    ret = (Employee) handler.Handle(rs);
			    }
		    }
		    Assert.IsNotNull(ret, "1");
		    Department dept = ret.Department;
		    Assert.IsNotNull(dept, "2");
		    Assert.AreEqual(20, dept.Deptno, "3");
		    Assert.AreEqual("RESEARCH", dept.Dname, "4");
	    }

	    [Test, S2]
        public void TestHandle2() {
		    IDataReaderHandler handler = new BeanMetaDataDataReaderHandler(
				    CreateBeanMetaData());
		    String sql = "select ename, job from emp where empno = 7788";
            Employee ret = null;
            using(IDbCommand cmd = DataSource.GetCommand(sql,Connection))
            {
                using(IDataReader rs = cmd.ExecuteReader())
                {
                    ret = (Employee) handler.Handle(rs);
			    }
		    }
		    Assert.IsNotNull(ret, "1");
		    Department dept = ret.Department;
		    Assert.IsNull(dept, "2");
	    }

	    [Test, S2]
        public void TestHandle3() {
		    IDataReaderHandler handler = new BeanMetaDataDataReaderHandler(
				    CreateBeanMetaData());
		    String sql = "select ename, dept.dname as dname_0 " +
                "from emp, dept where empno = 7788 and emp.deptno = dept.deptno";
            Employee ret = null;
            using(IDbCommand cmd = DataSource.GetCommand(sql,Connection))
		    {
                using(IDataReader rs = cmd.ExecuteReader())
                {
                    ret = (Employee) handler.Handle(rs);
			    }
		    }
            Assert.IsNotNull(ret, "1");
		    Department dept = ret.Department;
            Assert.IsNotNull(dept, "2");
            Assert.AreEqual("RESEARCH", dept.Dname, "3");
	    }
	}
}
