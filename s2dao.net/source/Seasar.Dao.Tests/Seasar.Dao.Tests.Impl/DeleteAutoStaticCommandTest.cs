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
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Util;
using NUnit.Framework;

namespace Seasar.Dao.Tests.Impl
{
	/// <summary>
	/// DeleteAutoStaticCommandTest の概要の説明です。
	/// </summary>
    [TestFixture]
    public class DeleteAutoStaticCommandTest
	{

        private const string PATH = "Tests.dicon";

        [Test]
        public void TestExecuteTx() 
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            IDataSource dataSource = (IDataSource) container.GetComponent(typeof(IDataSource));

            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(EmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE,new DatabaseMetaDataImpl(dataSource));
            ISqlCommand cmd = dmd.GetSqlCommand("Delete");

            ISqlCommand cmd2 = dmd.GetSqlCommand("GetEmployee");
            Employee emp = (Employee) cmd2.Execute(new Object[] { 7788 });
            int count = (int) cmd.Execute(new Object[] { emp });
            Assert.AreEqual(1, count, "1");

            //消したデータを入れておく
            string cmdText = "INSERT INTO [dbo].[EMP] VALUES(7788,'SCOTT','ANALYST',7566,CONVERT(datetime,'1982-12-09'),3000.0,NULL,20,CONVERT(datetime,'2005-01-18 13:09:32.213'))";
            System.Data.IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            System.Data.IDbCommand dbcmd = dataSource.GetCommand(cmdText,cn);
            CommandUtil.ExecuteNonQuery(dataSource,dbcmd);

        }

        [Test]
        public void TestExecute2Tx() 
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            IDataSource dataSource = (IDataSource) container.GetComponent(typeof(IDataSource));

            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(DepartmentAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, new DatabaseMetaDataImpl(dataSource));
            ISqlCommand cmd = dmd.GetSqlCommand("Delete");
            Department dept = new Department();
            dept.Deptno = 10;
            int count = (int) cmd.Execute(new Object[] { dept });
            Assert.AreEqual(1, count, "1");

            //消したデータを入れておく
            string cmdText = "INSERT INTO [dbo].[DEPT] VALUES(10,'ACCOUNTING','NEW YORK',0,1)";
            System.Data.IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            System.Data.IDbCommand dbcmd = dataSource.GetCommand(cmdText,cn);
            CommandUtil.ExecuteNonQuery(dataSource,dbcmd);
        }

        //[ExpectedException(typeof(UpdateFailureRuntimeException))]でなくてよい？
        [Test]
        [ExpectedException(typeof(NotSingleRowUpdatedRuntimeException))]
        public void TestExecute3Tx() 
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            IDataSource dataSource = (IDataSource) container.GetComponent(typeof(IDataSource));

            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(DepartmentAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, new DatabaseMetaDataImpl(dataSource));
            DeleteAutoStaticCommand cmd = (DeleteAutoStaticCommand) dmd
            .GetSqlCommand("Delete");
            Department dept = new Department();
            dept.Deptno = 10;
            dept.VersionNo = -1;
//            try 
//            {
                cmd.Execute(new Object[] { dept });
                Assert.Fail("1");
//            } 
//            catch (UpdateFailureRuntimeException ex) 
//            {
//                //System.out.println(ex);
//            }
        }
	}
}
