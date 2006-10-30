#region Copyright
/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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

using System.Diagnostics;
using Seasar.Dao;
using Seasar.Dao.Unit;
using Seasar.Extension.Unit;
using MbUnit.Framework;

namespace Seasar.Dao.Tests.Impl
{
    [TestFixture]
    public class UpdateAutoStaticCommandTest : S2DaoTestCase
    {
        [Test, S2(Tx.Rollback)]
        public void TestExecuteTx() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Update");
            ISqlCommand cmd2 = dmd.GetSqlCommand("GetEmployee");
            Employee emp = (Employee) cmd2.Execute(new object[] { 7788 });
            int count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
        }

        [Test, S2(Tx.Rollback)]
        public void TestUpdateNullableTx()
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Update");
            ISqlCommand cmd2 = dmd.GetSqlCommand("GetEmployee");
            {
                Employee emp = (Employee)cmd2.Execute(new object[] { 7788 });
                emp.NullableNextRestDate = null;
                int count = (int)cmd.Execute(new object[] { emp });
                Assert.AreEqual(1, count, "1");
            }
            {
                Employee emp = (Employee)cmd2.Execute(new object[] { 7788 });
                emp.NullableNextRestDate = Nullables.NullableDateTime.Parse("2006/01/01");
                int count = (int)cmd.Execute(new object[] { emp });
                Assert.AreEqual(1, count, "2");
            }
        }


        [Test, S2(Tx.Rollback)]
        public void TestExecute2Tx() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IDepartmentAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Update");
            Department dept = new Department();
            dept.Deptno =10;
            int count = (int) cmd.Execute(new object[] { dept });
            Assert.AreEqual(1, count, "1");
            Assert.AreEqual(1, dept.VersionNo, "2");
        }

        [Test, S2(Tx.Rollback)]
        [ExpectedException(typeof(NotSingleRowUpdatedRuntimeException))]
        public void TestExecute3Tx() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IDepartmentAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Update");
            Department dept = new Department();
            dept.Deptno = 10;
            dept.VersionNo = -1;

            cmd.Execute(new object[] { dept });
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute4Tx() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Update2");
            ISqlCommand cmd2 = dmd.GetSqlCommand("GetEmployee");
            Employee emp = (Employee) cmd2.Execute(new object[] { 7788 });
            int count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute5Tx() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Update3");
            ISqlCommand cmd2 = dmd.GetSqlCommand("GetEmployee");
            Employee emp = (Employee) cmd2.Execute(new object[] { 7788 });
            int count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
        }
    }
}
