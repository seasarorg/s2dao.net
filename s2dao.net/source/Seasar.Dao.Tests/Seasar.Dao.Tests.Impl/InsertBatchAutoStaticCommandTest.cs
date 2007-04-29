#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
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
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Seasar.Dao.Tests.Impl
{
    [TestFixture]
    public class InsertBatchAutoStaticCommandTest : S2DaoTestCase
    {

        [Test, S2(Tx.Rollback)]
        public void TestExecuteTx()
        {
            const int GEN_TEST_DATA_COUNT = 50000;

            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("InsertBatch");
            List<Employee> empList = new List<Employee>();
            Employee emp1 = new Employee();
            Random rand = new Random();
            for (long no = 1; no <= GEN_TEST_DATA_COUNT; no++) {
                Employee emp = new Employee();
                emp.Empno = no;
                string noStr = no.ToString("".PadLeft(GEN_TEST_DATA_COUNT.ToString().Length, '0'));
                emp.Ename = "emp" + noStr;
                emp.Comm = rand.Next(100);
                emp.JobName = "Job" + noStr;
                emp.Mgr = (System.Data.SqlTypes.SqlInt16)rand.Next(100);
                emp.Sal = (System.Data.SqlTypes.SqlSingle)rand.Next(100);
                empList.Add(emp);
            }

            int count = (int)cmd.Execute(new object[] { empList });
            Assert.AreEqual(empList.Count, count);

            //SqlCommand com = new SqlCommand();
            //com.Connection = (SqlConnection)DataSource.GetConnection();
            //com.Transaction = (SqlTransaction)DataSource.GetTransaction();
            //com.CommandText = "INSERT INTO EMP (EMPNO, ENAME) VALUES(@0, @1)";
            //com.Parameters.Add(new SqlParameter("@0", System.Data.SqlDbType.Decimal, 16, System.Data.ParameterDirection.Input, false,
            //    10, 0, "EMPNO", System.Data.DataRowVersion.Current, 10));
            //com.Parameters.Add(new SqlParameter("@1", System.Data.SqlDbType.VarChar, 10, System.Data.ParameterDirection.Input, true,
            //    0, 0, "ENAME", System.Data.DataRowVersion.Current, "TEST"));
            ////  Not Prepare = 33s
            //  //Prepared = 30s
            ////com.Prepare();
            //for (int i = 0; i < GEN_TEST_DATA_COUNT; i++) {
            //    //if ((i + 1) % 1000 == 0) {
            //    //    Console.WriteLine(i + 1);
            //    //}
            //    com.Parameters[0].Value = 8000 + i;
            //    com.Parameters[1].Value = "TEST" + i.ToString("00000");
            //    com.ExecuteNonQuery();
            //}
        }
    }
}
