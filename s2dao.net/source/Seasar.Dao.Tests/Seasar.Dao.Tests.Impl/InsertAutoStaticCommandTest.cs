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

using System.Diagnostics;
using Seasar.Dao;
using Seasar.Dao.Dbms;
using Seasar.Dao.Unit;
using Seasar.Extension.Unit;
using MbUnit.Framework;

namespace Seasar.Dao.Tests.Impl
{
    [TestFixture]
    public class InsertAutoStaticCommandTest : S2DaoTestCase
    {
        [Test, S2(Tx.Rollback)]
        public void TestExecuteTx() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Insert");
            Employee emp = new Employee();
            emp.Empno = 99;
            emp.Ename = "hoge";
            int count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute2Tx() 
        {
            if (Dbms.IdentitySelectString == null) 
            {
                Assert.Ignore("IDENTITYをサポートしていないDBMS。");
            }

            IDaoMetaData dmd = CreateDaoMetaData(typeof(IIdentityTableAutoDao));

            Assert.Ignore("IDENTITYTABLEのIDテーブルをIdentityにすればInsertは可能になるが、@@IDENTITYを取得できていない");
            //AbstractAutoHandler#ExecuteでCommandUtil.Close(cmd)の後にPostUpdateBean(bean)をしているので、別セッションになってしまう

            ISqlCommand cmd = dmd.GetSqlCommand("Insert");
            IdentityTable table = new IdentityTable();
            table.IdName = "hoge";
            int count1 = (int) cmd.Execute(new object[] { table });
            Assert.AreEqual(1, count1, "1");
            int id1 = table.Myid;
            Trace.WriteLine(id1);
            int count2 = (int) cmd.Execute(new object[] { table });
            Assert.AreEqual(2, count2, "2");
            int id2 = table.Myid;
            Trace.WriteLine(id2);
            Assert.AreEqual(1, id2 - id1, "2");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute3_1Tx() 
        {
            if (Dbms.GetSequenceNextValString("dummy") == null) 
            {
                Assert.Ignore("SEQUENCEをサポートしていないDBMS。");
            }

            IDaoMetaData dmd = CreateDaoMetaData(typeof(SeqTableAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Insert");
            SeqTable table = new SeqTable();
            table.Name ="hoge";
            int count = (int) cmd.Execute(new object[] { table });
            Assert.AreEqual(1, count, "1");
            Trace.WriteLine(table.Id);
            Assert.IsTrue(table.Id > 0, "2");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute3_2Tx() 
        {
            if (Dbms.GetSequenceNextValString("dummy") == null) 
            {
                Assert.Ignore("SEQUENCEをサポートしていないDBMS。");
            }

            IDaoMetaData dmd = CreateDaoMetaData(typeof(SeqTableAuto2Dao));
            ISqlCommand cmd = dmd.GetSqlCommand("Insert");
            SeqTable2 table1 = new SeqTable2();
            table1.Name ="hoge";
            int count = (int) cmd.Execute(new object[] { table1 });
            Assert.AreEqual(1, count, "1");
            Trace.WriteLine(table1.Id);
            Assert.IsTrue(table1.Id > 0, "2");

            SeqTable2 table2 = new SeqTable2();
            table2.Name ="foo";
            cmd.Execute(new object[] { table2 });
            Trace.WriteLine(table2.Id);
            Assert.IsTrue(table2.Id > table1.Id, "3");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute4Tx() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Insert2");
            Employee emp = new Employee();
            emp.Empno = 99;
            emp.Ename = "hoge";
            int count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
        }

        [Test, S2(Tx.Rollback)]
        public void TestExecute5Tx() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Insert3");
            Employee emp = new Employee();
            emp.Empno = 99;
            emp.Ename = "hoge";
            emp.Deptno =10;
            int count = (int) cmd.Execute(new object[] { emp });
            Assert.AreEqual(1, count, "1");
        }
    }
}
