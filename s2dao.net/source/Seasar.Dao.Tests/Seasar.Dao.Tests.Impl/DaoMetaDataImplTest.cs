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
using System.Data;
using Seasar.Dao.Dbms;
using Seasar.Dao.Impl;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;
using MbUnit.Framework;

namespace Seasar.Dao.Tests.Impl
{
    [TestFixture]
    public class DaoMetaDataImplTest : S2TestCase
    {
        protected IDaoMetaData CreateDaoMetaData(Type daoType) 
        {
            return CreateDaoMetaData(daoType, null);
        }

        protected IDaoMetaData CreateDaoMetaData(Type daoType, string sqlFileEncoding) 
        {
            DaoMetaDataImpl dmd = new DaoMetaDataImpl();
            dmd.DaoType = daoType;
            dmd.DataSource = DataSource;
            dmd.CommandFactory = BasicCommandFactory.INSTANCE;
            dmd.DataReaderFactory = BasicDataReaderFactory.INSTANCE;
            dmd.DatabaseMetaData = new DatabaseMetaDataImpl(DataSource);
            if (sqlFileEncoding != null) 
            {
                dmd.SqlFileEncoding = sqlFileEncoding;
            }
            dmd.Initialize();
            return dmd;
        }

        [Test, S2]
        public void TestSelectBeanList() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeDao));
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetAllEmployees");
            Assert.IsNotNull(cmd, "1");
            Assert.AreEqual("SELECT * FROM emp", cmd.Sql, "2");
            BeanListMetaDataDataReaderHandler rsh = (BeanListMetaDataDataReaderHandler) cmd.DataReaderHandler;
            Assert.AreEqual(typeof(Employee), rsh.BeanMetaData.BeanType, "3");
        }

        [Test, S2]
        public void TestSelectBeanArray() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeDao));
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetAllEmployeeArray");
            Assert.IsNotNull(cmd, "1");
            BeanListMetaDataDataReaderHandler rsh = (BeanListMetaDataDataReaderHandler) cmd.DataReaderHandler;
            Assert.AreEqual(typeof(Employee), rsh.BeanMetaData.BeanType, "2");
        }

        [Test, S2]
        public void TestSelectBean() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeDao));
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployee");
            Assert.IsNotNull(cmd, "1");
            Assert.AreEqual(typeof(BeanMetaDataDataReaderHandler), cmd.DataReaderHandler.GetType(), "2");
            Assert.AreEqual("empno", cmd.ArgNames[0], "3");
        }

        [Test, S2]
        public void TestSelectObject() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeDao));
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetCount");
            Assert.IsNotNull(cmd, "1");
            Assert.AreEqual(typeof(ObjectDataReaderHandler), cmd.DataReaderHandler.GetType(), "2");
            Assert.AreEqual("SELECT count(*) FROM emp", cmd.Sql, "3");
        }

        [Test, S2]
        public void TestUpdate() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeDao));
            UpdateDynamicCommand cmd = (UpdateDynamicCommand) dmd.GetSqlCommand("Update");
            Assert.IsNotNull(cmd, "1");
            Assert.AreEqual("employee", cmd.ArgNames[0], "2");
        }

        [Test, S2(Tx.Rollback)]
        public void TestInsertAutoTx() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Insert");
            Assert.IsNotNull(cmd, "1");
            Employee emp = new Employee();
            emp.Empno= 99;
            emp.Ename = "hoge";
            cmd.Execute(new object[] { emp });
        }

        [Test, S2(Tx.Rollback)]
        public void TestUpdateAutoTx() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Update");
            Assert.IsNotNull(cmd, "1");
            SelectDynamicCommand cmd2 = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployee");
            Employee emp = (Employee) cmd2.Execute(new object[] { 7788 });
            emp.Ename = "hoge2";
            cmd.Execute(new object[] { emp });
        }

        [Test, S2(Tx.Rollback)]
        public void TestDeleteAutoTx() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Delete");
            Assert.IsNotNull(cmd, "1");
            SelectDynamicCommand cmd2 = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployee");
            Employee emp = (Employee) cmd2.Execute(new object[] { 7788 });
            cmd.Execute(new object[] { emp });
        }

        [Test, S2, ExpectedException(typeof(IllegalSignatureRuntimeException))]
        public void TestIllegalAutoUpdateMethod() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IIllegalEmployeeAutoDao));
        }

        [Test, S2]
        public void TestSelectAuto() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeeByDeptno");
            Console.Out.WriteLine(cmd.Sql);
        }

        [Test, S2]
        public void TestCreateFindCommand() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.CreateFindCommand(null);
            IList employees = (IList) cmd.Execute(null);
            Console.Out.WriteLine(employees);
            Assert.IsTrue(employees.Count > 0, "1");
        }

        [Test, S2]
        public void TestCreateFindCommand2() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.CreateFindCommand(null);
            IList employees = (IList) cmd.Execute(null);
            Console.Out.WriteLine(employees);
            Assert.IsTrue(employees.Count > 0, "1");
        }

        [Test, S2]
        public void TestCreateFindCommand3() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.CreateFindCommand("select * from emp");
            IList employees = (IList) cmd.Execute(null);
            Console.Out.WriteLine(employees);
            Assert.IsTrue(employees.Count > 0, "1");
        }

        [Test, S2]
        public void TestCreateFindCommand4() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.CreateFindCommand("order by empno");
            IList employees = (IList) cmd.Execute(null);
            Console.Out.WriteLine(employees);
            Assert.IsTrue(employees.Count > 0, "1");
        }

        [Test, S2]
        public void TestCreateFindCommand5() 
        {
            DaoMetaDataImpl dmd = (DaoMetaDataImpl) CreateDaoMetaData(typeof(IEmployeeAutoDao));
            dmd.Dbms = new Oracle();
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.CreateFindCommand("EMPNO = ?");
            Console.Out.WriteLine(cmd.Sql);
            Assert.IsTrue(cmd.Sql.EndsWith(" EMPNO = ?"), "1");
        }

        [Test, S2]
        public void TestCreateFindBeanCommand() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.CreateFindBeanCommand("empno = ?");
            Employee employee = (Employee) cmd.Execute(new object[] { 7788});
            Console.Out.WriteLine(employee);
            Assert.IsNotNull(employee, "1");
        }

        [Test, S2]
        public void TestCreateObjectBeanCommand() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.CreateFindObjectCommand("select count(*) from emp");
            Int32 count = (Int32) cmd.Execute(null);
            Assert.AreEqual(14, count, "1");
        }

        [Test, S2]
        public void TestSelectAutoByQuery() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("GetEmployeesBySal");
            IList employees = (IList) cmd.Execute(new object[] { 0, 1000 });
            Console.Out.WriteLine(employees);
            Assert.AreEqual(2, employees.Count, "1");
        }

        [Test, S2]
        public void TestSelectAutoByQueryMultiIn() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeesByEnameJob");
            Console.Out.WriteLine(cmd.Sql);
            IList enames = new ArrayList();
            enames.Add("SCOTT");
            enames.Add("MARY");
            IList jobs = new ArrayList();
            jobs.Add("ANALYST");
            jobs.Add("FREE");
            IList employees = (IList) cmd.Execute(new object[] { enames, jobs });
            Console.Out.WriteLine(employees);
            Assert.AreEqual(1, employees.Count, "MARYはいないので");
        }

        [Test, S2]
        public void TestRelation1() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployee2Dao));
            ISqlCommand cmd = dmd.GetSqlCommand("GetAllEmployees");
            IList emps = (IList) cmd.Execute(null);
            Console.Out.WriteLine(emps);
            Assert.IsTrue(emps.Count > 0, "1");
            foreach (Employee2 emp in emps) 
            {
                Assert.IsNotNull(emp.Department2);
            }
        }

        [Test, S2]
        public void TestRelation2() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployee2Dao));
            ISqlCommand cmd = dmd.GetSqlCommand("GetAllEmployeesOnly");
            IList emps = (IList) cmd.Execute(null);
            Console.Out.WriteLine(emps);
            Assert.IsTrue(emps.Count > 0, "1");
            foreach (Employee2 emp in emps) 
            {
                Assert.IsNotNull(emp.Department2);
            }
        }

        [Test, S2(Tx.Rollback)]
        public void TestRelation3() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            ISqlCommand cmd = dmd.GetSqlCommand("Insert");
            Employee emp = new Employee();
            emp.Empno = 9999;
            emp.Ename = "test";
            // Department:50 does not exist.
            emp.Deptno = 50;
            cmd.Execute(new object[] { emp });

            ISqlCommand cmd2 = dmd.GetSqlCommand("GetEmployee");
            Employee emp2 = (Employee) cmd2.Execute(new object[] { 7369 });
            Console.Out.WriteLine(emp2);
            Assert.IsNotNull(emp2.Department);

            Employee emp3 = (Employee) cmd2.Execute(new object[] { 9999 });
            Console.Out.WriteLine(emp3);
            Assert.IsNotNull(emp2.Department);
        }

        [Test, S2]
        public void TestGetDaoInterface() 
        {
            Assert.AreEqual(typeof(IEmployeeDao), DaoMetaDataImpl.GetDaoInterface(typeof(IEmployeeDao)), "1");
            //Assert.AreEqual(typeof(IEmployeeDao), DaoMetaDataImpl.getDaoInterface(typeof(EmployeeDaoImpl)), "2");
        }

        [Test, S2]
        public void TestAutoSelectSqlByDto() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeesBySearchCondition");
            Assert.IsNotNull(cmd, "1");
            Console.Out.WriteLine(cmd.Sql);
            EmployeeSearchCondition dto = new EmployeeSearchCondition();
            dto.Dname = "RESEARCH";
            IList employees = (IList) cmd.Execute(new object[] { dto });
            Assert.IsTrue(employees.Count > 0, "2");
        }

        [Test, S2]
        public void TestAutoSelectSqlByDto2() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeesByEmployee");
            Assert.IsNotNull(cmd, "1");
            Console.Out.WriteLine(cmd.Sql);
            Employee dto = new Employee();
            dto.Job = "MANAGER";            
            IList employees = (IList) cmd.Execute(new object[] { dto });
            Console.Out.WriteLine(employees);
            //Assert.IsTrue(employees.Count > 0, "2");
        }

        [Test, S2]
        public void TestAutoSelectSqlByDto3() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployee3Dao));
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployees");
            Assert.IsNotNull(cmd, "1");
            Console.Out.WriteLine(cmd.Sql);
            Employee3 dto = new Employee3();
            dto.Mgr = 7902;
            IList employees = (IList) cmd.Execute(new object[] { dto });
            Console.Out.WriteLine(employees);
            Assert.IsTrue(employees.Count > 0, "2");
        }

        [Test, S2]
        public void TestAutoSelectSqlByDto4() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployee3Dao));
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployees2");
            Assert.IsNotNull(cmd, "1");
            Console.Out.WriteLine(cmd.Sql);
            Assert.IsTrue(cmd.Sql.EndsWith(" ORDER BY empno"));
        }

        [Test, S2]
        public void TestAutoSelectSqlByDto5() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeesBySearchCondition2");
            Assert.IsNotNull(cmd, "1");
            Console.Out.WriteLine(cmd.Sql);
            EmployeeSearchCondition dto = new EmployeeSearchCondition();
            Department department = new Department();
            department.Dname = "RESEARCH";
            dto.Department = department;
            IList employees = (IList) cmd.Execute(new object[] { dto });
            Assert.IsTrue(employees.Count > 0, "2");
        }

        [Test, S2]
        public void TestAutoSelectSqlByDto6() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeesBySearchCondition2");
            Assert.IsNotNull(cmd, "1");
            Console.Out.WriteLine(cmd.Sql);
            EmployeeSearchCondition dto = new EmployeeSearchCondition();
            dto.Department = null;
            IList employees = (IList) cmd.Execute(new object[] { dto });
            Assert.AreEqual(0, employees.Count, "2");
        }

        [Test, S2]
        public void TestSelfReference() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployee4Dao));
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployee");
            Assert.IsNotNull(cmd, "1");
            Console.Out.WriteLine(cmd.Sql);
            Employee4 employee = (Employee4) cmd.Execute(new object[] { 7788 });
            Console.Out.WriteLine(employee);
            Assert.AreEqual(7566, employee.Parent.Empno, "2");
        }

        [Test, S2]
        public void TestSelfMultiPk() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployee5Dao));
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployee");
            Assert.IsNotNull(cmd, "1");
            Console.Out.WriteLine(cmd.Sql);
        }

        [Test, S2]
        public void TestNotHavePrimaryKey() 
        {
            Assert.Ignore("エラーになる。");

            IDaoMetaData dmd = CreateDaoMetaData(typeof(IDepartmentTotalSalaryDao));
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetTotalSalaries");
            Assert.IsNotNull(cmd, "1");
            Console.Out.WriteLine(cmd.Sql);
            IList result = (IList) cmd.Execute(null);
            Console.Out.WriteLine(result);
        }

        [Test, S2]
        public void TestSelectAutoFullColumnName() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeAutoDao));
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployee");
            Console.Out.WriteLine(cmd.Sql);
        }

        [Test, S2]
        public void TestStartsWithOrderBy() 
        {
            Assert.Ignore("SQL文の末尾に余計なWHEREが付く。");

            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployee6Dao));
            EmployeeSearchCondition condition = new EmployeeSearchCondition();
            condition.Dname = "RESEARCH";
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployees");
            Console.Out.WriteLine(cmd.Sql);
            Employee[] results = (Employee[]) cmd.Execute(new object[] { condition });
            condition.OrderByString = "ENAME";
            results = (Employee[]) cmd.Execute(new object[] { condition });
        }

        // TODO
        //public void TestStartsWithBeginComment()

        // TODO
        //public void TestQueryAnnotationTx()

        // TODO
        //public void TestDaoExtend1()

        // TODO
        //public void TestDaoExtend2()

        // TODO
        //public void TestUsingColumnAnnotationForSql_Insert()

        // TODO
        //public void TestUsingColumnAnnotationForSql_Update()

        // TODO
        //public void TestUsingColumnAnnotationForSql_Select()

        // TODO
        //public void TestUsingColumnAnnotationForSql_SelectDto()

        [Test, S2(Tx.Rollback)]
        public void TestSqlFileEncodingDefault() 
        {
            IDaoMetaData dmd = CreateDaoMetaData(typeof(IEmployeeDao));
            ISqlCommand cmd = dmd.GetSqlCommand("UpdateSqlFileEncodingDefault");
            Employee emp = new Employee();
            emp.Empno= 7788;
            cmd.Execute(new object[] { emp });

            ISqlCommand cmd2 = dmd.GetSqlCommand("GetEmployee");
            Employee ret = (Employee) cmd2.Execute(new object[] { 7788 });
            Assert.AreEqual("日本語", ret.Ename);
        }

        [Test, S2(Tx.Rollback)]
        public void TestSqlFileEncodingUTF8() 
        {
            DaoMetaDataImpl dmd = (DaoMetaDataImpl) CreateDaoMetaData(typeof(IEmployeeDao), "utf-8");
            ISqlCommand cmd = dmd.GetSqlCommand("UpdateSqlFileEncodingUTF8");
            Employee emp = new Employee();
            emp.Empno= 7788;
            cmd.Execute(new object[] { emp });

            ISqlCommand cmd2 = dmd.GetSqlCommand("GetEmployee");
            Employee ret = (Employee) cmd2.Execute(new object[] { 7788 });
            Assert.AreEqual("日本語", ret.Ename);
        }
    }
}
