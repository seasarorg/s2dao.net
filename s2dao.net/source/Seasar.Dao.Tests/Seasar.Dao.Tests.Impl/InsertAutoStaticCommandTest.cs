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
	/// InsertAutoStaticCommandTest の概要の説明です。
	/// </summary>
    [TestFixture]
    public class InsertAutoStaticCommandTest
	{
        private const string PATH = "Tests.dicon";
        private IDataSource dataSource;

        [SetUp]
        public void SetUp()
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            dataSource = (IDataSource) container.GetComponent(typeof(IDataSource));
        }

        [Test]
        public void TestExecuteTx() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(EmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE);

            ISqlCommand cmd = dmd.GetSqlCommand("Insert");
            Employee emp = new Employee();
            emp.Empno =99;
            emp.Ename = "hoge";
            Int32 count = (Int32) cmd.Execute(new Object[] { emp });
            Assert.AreEqual(1, count, "1");

            //挿入したデータを消しておく
            string cmdText = "DELETE [dbo].[EMP] WHERE Empno = 99";
            System.Data.IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            System.Data.IDbCommand dbcmd = dataSource.GetCommand(cmdText,cn);
            CommandUtil.ExecuteNonQuery(dataSource,dbcmd);

        }

        [Test]
        public void TestExecute2Tx() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IdentityTableAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE);

            Assert.Ignore("IDENTITYTABLEへのINSERT文が正しく生成されない");

            ISqlCommand cmd = dmd.GetSqlCommand("Insert");
            IdentityTable table = new IdentityTable();
            table.IdName = "hoge";
            Int32 count = (Int32) cmd.Execute(new Object[] { table });
            Assert.AreEqual(1, count, "1");
            //System.out.println(table.getMyid());
            Assert.IsTrue(table.Myid > 0,"2");
        }

        [Test]
        public void TestExecute3Tx() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IdentityTableAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE);

            Assert.Ignore("IDENTITYTABLEへのINSERT文が正しく生成されない");

            ISqlCommand cmd = dmd.GetSqlCommand("Insert");
            SeqTable table = new SeqTable();
            table.Name ="hoge";
            Int32 count = (Int32) cmd.Execute(new Object[] { table });
            Assert.AreEqual(1, count, "1");
            //System.out.println(table.getId());
            Assert.IsTrue(table.ID > 0,"2");
        }

        [Test]
        public void TestExecute4Tx() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(EmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE);

            ISqlCommand cmd = dmd.GetSqlCommand("Insert2");
            Employee emp = new Employee();
            emp.Empno =99;
            emp.Ename = "hoge";
            Int32 count = (Int32) cmd.Execute(new Object[] { emp });
            Assert.AreEqual(1, count, "1");

            //挿入したデータを消しておく
            string cmdText = "DELETE [dbo].[EMP] WHERE Empno = 99";
            System.Data.IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            System.Data.IDbCommand dbcmd = dataSource.GetCommand(cmdText,cn);
            CommandUtil.ExecuteNonQuery(dataSource,dbcmd);
        }

        [Test]
        public void TestExecute5Tx() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(EmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE);
            ISqlCommand cmd = dmd.GetSqlCommand("Insert3");
            Employee emp = new Employee();
            emp.Empno =99;
            emp.Ename = "hoge";
            emp.Deptno =10;
            Int32 count = (Int32) cmd.Execute(new Object[] { emp });
            Assert.AreEqual(1, count, "1");
        
            //挿入したデータを消しておく
            string cmdText = "DELETE [dbo].[EMP] WHERE Empno = 99";
            System.Data.IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            System.Data.IDbCommand dbcmd = dataSource.GetCommand(cmdText,cn);
            CommandUtil.ExecuteNonQuery(dataSource,dbcmd);
        }

    }
}
