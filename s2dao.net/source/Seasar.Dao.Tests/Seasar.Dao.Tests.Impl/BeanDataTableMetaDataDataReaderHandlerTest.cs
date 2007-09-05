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

using System.Collections;
using System.Data;
using System.Diagnostics;
using Seasar.Dao.Impl;
using Seasar.Dao.Unit;
using Seasar.Extension.ADO;
using Seasar.Extension.Unit;
using MbUnit.Framework;

namespace Seasar.Dao.Tests.Impl
{
    [TestFixture]
    public class BeanDataTableMetaDataDataReaderHandlerTest : S2DaoTestCase
    {
        [Test, S2]
        public void TestHandle()
        {
            IDataReaderHandler handler = new BeanDataTableMetaDataDataReaderHandler(
                CreateBeanMetaData(typeof(Employee)), typeof(EmployeeDataSet.EmpAndDeptDataTable));

            string sql = "select emp.empno,emp.ename,dept.deptno, dept.dname from emp left outer join dept on emp.deptno = dept.deptno";
            using ( IDbConnection con = Connection )
            {
                using ( IDbCommand cmd = con.CreateCommand() )
                {
                    cmd.CommandText = sql;

                    DataTable ret;

                    using ( IDataReader reader = cmd.ExecuteReader() )
                    {
                        ret = (DataTable)handler.Handle(reader);
                    }

                    Assert.IsNotNull(ret, "1");
                    Assert.IsTrue(ret is EmployeeDataSet.EmpAndDeptDataTable);
                    foreach ( DataRow row in ret.Rows )
                    {
                        for ( int i = 0; i < row.Table.Columns.Count; i++ )
                        {
                            Trace.Write(row.Table.Columns[i].ColumnName + " = ");
                            Trace.WriteLine(row[i]);
                        }
                    }
                }
            }
        }

        [Test, S2]
        public void TestHandle_Where() {
            IDataReaderHandler handler = new BeanDataTableMetaDataDataReaderHandler(
                CreateBeanMetaData(typeof(Employee)), typeof(EmployeeDataSet.EmpAndDeptDataTable));

            const int EMP_NO = 7499;
            const int DEPT_NO = 30;
            string sql = "select emp.empno,emp.ename,dept.deptno, dept.dname from emp left outer join dept on emp.deptno = dept.deptno where emp.empno = " + EMP_NO.ToString();
            using ( IDbConnection con = Connection ) {
                using ( IDbCommand cmd = con.CreateCommand() ) {
                    cmd.CommandText = sql;

                    DataTable ret;

                    using ( IDataReader reader = cmd.ExecuteReader() ) {
                        ret = (DataTable)handler.Handle(reader);
                    }

                    Assert.IsNotNull(ret, "1");
                    Assert.IsTrue(ret is EmployeeDataSet.EmpAndDeptDataTable);
                    Assert.AreEqual(1, ret.Rows.Count);
                    DataRow actualRow = ret.Rows[0];
                    Assert.AreEqual(EMP_NO, actualRow["EMPNO"]);
                    Assert.AreEqual("ALLEN", actualRow["ENAME"]);
                    Assert.AreEqual(DEPT_NO, actualRow["DEPTNO"]);
                    Assert.AreEqual("SALES", actualRow["DNAME"]);
                }
            }
        }

        [Test, S2]
        public void TestHandle_NoType() {
            IDataReaderHandler handler = new BeanDataTableMetaDataDataReaderHandler(
                CreateBeanMetaData(typeof(Employee)), typeof(DataTable));

            string sql = "select emp.empno,emp.ename,dept.deptno, dept.dname from emp left outer join dept on emp.deptno = dept.deptno";
            using ( IDbConnection con = Connection ) {
                using ( IDbCommand cmd = con.CreateCommand() ) {
                    cmd.CommandText = sql;

                    DataTable ret;

                    try {
                        using ( IDataReader reader = cmd.ExecuteReader() ) {
                            ret = (DataTable)handler.Handle(reader);
                        }
                        Assert.Fail("VS‚ÅŽ©“®¶¬‚µ‚Ä‚¢‚È‚¢DataTable‚Í¡‚Ì‚Æ‚±‚ëŽg‚¤‚±‚Æ‚Í‚Å‚«‚È‚¢B");
                    }
                    catch ( System.Exception e ) {
                        Trace.WriteLine(e.Message);
                        Trace.WriteLine(e.StackTrace);
                    }
                }
            }
        }
    }
}
