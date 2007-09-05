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
    public class BeanDataSetMetaDataDataReaderHandlerTest : S2DaoTestCase
    {
        [Test, S2]
        public void TestHandle()
        {
            IDataReaderHandler handler = new BeanDataSetMetaDataDataReaderHandler(
                CreateBeanMetaData(typeof(Employee)), typeof(EmployeeDataSet));

            string sql = "select emp.empno,emp.ename,dept.deptno, dept.dname from emp left outer join dept on emp.deptno = dept.deptno";
            using ( IDbConnection con = Connection )
            {
                using ( IDbCommand cmd = con.CreateCommand() )
                {
                    cmd.CommandText = sql;

                    DataSet ret;

                    using ( IDataReader reader = cmd.ExecuteReader() )
                    {
                        ret = (DataSet)handler.Handle(reader);
                    }

                    Assert.IsNotNull(ret, "1");
                    Assert.IsTrue(ret is EmployeeDataSet);
                    foreach ( DataTable table in ret.Tables )
                    {
                        Trace.WriteLine(table.TableName);
                        foreach ( DataRow row in table.Rows )
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
        }

        [Test, S2]
        public void TestHandle_Where() {
            IDataReaderHandler handler = new BeanDataSetMetaDataDataReaderHandler(
                CreateBeanMetaData(typeof(Employee)), typeof(EmployeeDataSet));
            const int EMP_NO = 7499;
            const int DEPT_NO = 30;

            string sql = "select emp.empno,emp.ename,dept.deptno, dept.dname from emp left outer join dept on emp.deptno = dept.deptno where emp.empno = " + EMP_NO.ToString();
            using ( IDbConnection con = Connection ) {
                using ( IDbCommand cmd = con.CreateCommand() ) {
                    cmd.CommandText = sql;

                    DataSet ret;

                    using ( IDataReader reader = cmd.ExecuteReader() ) {
                        ret = (DataSet)handler.Handle(reader);
                    }

                    Assert.IsNotNull(ret, "1");
                    Assert.IsTrue(ret is EmployeeDataSet);
                    Assert.IsNotNull(ret, "1");
                    Assert.IsTrue(ret is EmployeeDataSet);
                    Assert.AreEqual(1, ret.Tables.Count);
                    DataTable actualTable = ret.Tables[0];
                    Assert.IsTrue(actualTable is EmployeeDataSet.EmpAndDeptDataTable);
                    Assert.AreEqual("EmpAndDept", actualTable.TableName);
                    Assert.AreEqual(1, actualTable.Rows.Count);
                    DataRow actualRow = actualTable.Rows[0];
                    Assert.AreEqual(EMP_NO, actualRow["EMPNO"]);
                    Assert.AreEqual("ALLEN", actualRow["ENAME"]);
                    Assert.AreEqual(DEPT_NO, actualRow["DEPTNO"]);
                    Assert.AreEqual("SALES", actualRow["DNAME"]);
                }
            }
        }

        [Test, S2]
        public void TestHandle_NoType() {
            IDataReaderHandler handler = new BeanDataSetMetaDataDataReaderHandler(
                CreateBeanMetaData(typeof(Employee)), typeof(DataSet));

            string sql = "select emp.empno,emp.ename,dept.deptno, dept.dname from emp left outer join dept on emp.deptno = dept.deptno";
            using ( IDbConnection con = Connection ) {
                using ( IDbCommand cmd = con.CreateCommand() ) {
                    cmd.CommandText = sql;

                    DataSet ret;

                    try {
                        using ( IDataReader reader = cmd.ExecuteReader() ) {
                            ret = (DataSet)handler.Handle(reader);
                        }
                        Assert.Fail("VS‚ÅŽ©“®¶¬‚µ‚Ä‚¢‚È‚¢DataSet‚Í¡‚Ì‚Æ‚±‚ëŽg‚¤‚±‚Æ‚Í‚Å‚«‚È‚¢B");
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
