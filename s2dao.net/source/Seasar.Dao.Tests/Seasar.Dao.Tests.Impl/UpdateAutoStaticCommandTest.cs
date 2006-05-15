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
using Seasar.Dao;
using Seasar.Dao.Impl;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Util;
using NUnit.Framework;

namespace Seasar.Dao.Tests.Impl
{
	/// <summary>
	/// UpdateAutoStaticCommandTest の概要の説明です。
	/// </summary>
    [TestFixture]
    public class UpdateAutoStaticCommandTest : S2TestCase
	{
        private const string PATH = "Tests.dicon";
        private IDataSource dataSource;

        [SetUp]
        public void SetUp()
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            dataSource = (IDataSource) container.GetComponent(typeof(IDataSource));
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecuteTx() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(EmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, new DatabaseMetaDataImpl(dataSource));

            ISqlCommand cmd = dmd.GetSqlCommand("Update");
            ISqlCommand cmd2 = dmd.GetSqlCommand("GetEmployee");
            Employee emp = (Employee) cmd2.Execute(new Object[] { 7788 });
            Int32 count = (Int32) cmd.Execute(new Object[] { emp });
            Assert.AreEqual(1, count, "1");

        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute2Tx() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(DepartmentAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, new DatabaseMetaDataImpl(dataSource));
            
            ISqlCommand cmd = dmd.GetSqlCommand("Update");
            Department dept = new Department();
            dept.Deptno =10;
            Int32 count = (Int32) cmd.Execute(new Object[] { dept });
            Assert.AreEqual(1, count, "1");
            Assert.AreEqual(1, dept.VersionNo, "2");

            
            //更新したVersionNoを戻しておく
            string cmdText = "Update [dbo].[DEPT] SET VERSIONNO = 0 WHERE DEPTNO = 10";
            System.Data.IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            System.Data.IDbCommand dbcmd = dataSource.GetCommand(cmdText,cn);
            CommandUtil.ExecuteNonQuery(dataSource,dbcmd);

        }

        [Test, S2(Tx.Rollback)]
        [ExpectedException(typeof(NotSingleRowUpdatedRuntimeException))]
        public void TestExecute3Tx() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(DepartmentAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, new DatabaseMetaDataImpl(dataSource));
            
            ISqlCommand cmd = dmd.GetSqlCommand("Update");
            Department dept = new Department();
            dept.Deptno = 10;
            dept.VersionNo = -1;
//            try 
//            {
                cmd.Execute(new Object[] { dept });
//                fail("1");
//            } 
//                catch (UpdateFailureRuntimeException ex) 
//            {
//                //System.out.println(ex);
//            }
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute4Tx() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(EmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, new DatabaseMetaDataImpl(dataSource));
            
            ISqlCommand cmd = dmd.GetSqlCommand("Update2");
            ISqlCommand cmd2 = dmd.GetSqlCommand("GetEmployee");
            Employee emp = (Employee) cmd2.Execute(new Object[] { 7788 });
            Int32 count = (Int32) cmd.Execute(new Object[] { emp });
            Assert.AreEqual(1, count, "1");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute5Tx() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(EmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, new DatabaseMetaDataImpl(dataSource));

            ISqlCommand cmd = dmd.GetSqlCommand("Update3");
            ISqlCommand cmd2 = dmd.GetSqlCommand("GetEmployee");
            Employee emp = (Employee) cmd2.Execute(new Object[] { 7788 });
            Int32 count = (Int32) cmd.Execute(new Object[] { emp });
            Assert.AreEqual(1, count, "1");
        }
	}
}
