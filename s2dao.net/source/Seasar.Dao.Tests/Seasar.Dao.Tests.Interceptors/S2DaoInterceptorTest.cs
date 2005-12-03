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
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using NUnit.Framework;

namespace Seasar.Dao.Tests.Interceptors
{
	/// <summary>
	/// S2DaoInterceptorTest の概要の説明です。
	/// </summary>
    [TestFixture]
    public class S2DaoInterceptorTest
	{

        private const string PATH = "Seasar.Dao.Tests.Interceptors/IEmployeeDao.dicon";
        private IEmployeeDao employeeDao;

        [SetUp]
        public void SetUp()
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            employeeDao = (IEmployeeDao) container.GetComponent(typeof(IEmployeeDao));
        }

        [Test]
        public void TestSelectBeanList() {
	        IList employees = employeeDao.GetAllEmployees();
	        for (int i = 0; i < employees.Count; ++i) {
		        //System.out.println(employees.get(i));
	        }
	        Assert.AreEqual(true, employees.Count > 0, "1");
        }

        [Test]
        public void TestSelectBean() {
	        Employee employee = employeeDao.GetEmployee(7788);
	        //System.out.println(employee);
	        Assert.AreEqual("SCOTT", employee.Ename, "1");
        }

        [Test]
        public void TestSelectObject() {
            Assert.Ignore("未作成");
            int count = employeeDao.GetCount();
	        //System.out.println("count:" + count);
	        Assert.AreEqual(true, count > 0, "1");
        }

        [Test]
        public void TestUpdateTx() {
	        Employee employee = employeeDao.GetEmployee(7788);
            Assert.Ignore("未作成");
	        //Assert.AreEqual(1, employeeDao.Update(employee), "1");
        }

        [Test]
        public void TestEntityManager() {
            Assert.Ignore("「System.InvalidCastException : 引数の戻り値の型が無効です。」が発生してしまう");
            Employee[] employees = employeeDao.GetEmployeesByDeptno(10);
	        Assert.AreEqual(3, employees.Length, "1");
        }

        [Test]
        public void TestInsertTx() {
	        Assert.Ignore("未作成");
	        //employeeDao.Insert(9999, "hoge");
        }

    }
}
