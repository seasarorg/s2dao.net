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
using Seasar.Dao.Attrs;
using Seasar.Dao.Dbms;
using Seasar.Dao.Impl;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Util;
using MbUnit.Framework;

namespace Seasar.Dao.Tests.Impl
{
    [TestFixture]
    public class DaoMetaDataImplTest
    {
        private const string PATH = "Tests.dicon";
        private IDataSource dataSource;
        private IDatabaseMetaData dbMetaData;

        [SetUp]
        public void SetUp()
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            dataSource = (IDataSource) container.GetComponent(typeof(IDataSource));
            dbMetaData = new DatabaseMetaDataImpl(dataSource);

            IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            IDbms dbms = new MSSQLServer();
        }

        [Test]
        public void TestSelectBeanList() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetAllEmployees");
            Assert.IsNotNull(cmd, "1");
            //TODO
            //Assert.AreEqual("SELECT * FROM emp", cmd.Sql, "2");
            BeanListMetaDataDataReaderHandler rsh = (BeanListMetaDataDataReaderHandler) cmd.DataReaderHandler;
            Assert.AreEqual(typeof(Employee), rsh.BeanMetaData.BeanType, "3");
        }

        [Test]
        public void TestSelectBeanArray() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetAllEmployeeArray");
            Assert.IsNotNull(cmd, "1");
            BeanListMetaDataDataReaderHandler rsh = (BeanListMetaDataDataReaderHandler) cmd.DataReaderHandler;
            Assert.AreEqual(typeof(Employee), rsh.BeanMetaData.BeanType, "2");
        }

        [Test]
        public void TestSelectBean() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployee");
            Assert.IsNotNull(cmd, "1");
            Assert.AreEqual(typeof(BeanMetaDataDataReaderHandler), cmd.DataReaderHandler.GetType(), "2");
            Assert.AreEqual("empno", cmd.ArgNames[0], "3");
        }

        [Test]
        public void TestSelectObject() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetCount");
            Assert.IsNotNull(cmd, "1");
            Assert.AreEqual(typeof(ObjectDataReaderHandler), cmd.DataReaderHandler.GetType(), "2");
            Assert.Ignore("SetupMethodのdbmsPath・standardPath両方とも、IsExistと判断されない");
            Assert.AreEqual("SELECT count(*) FROM emp", cmd.Sql, "3");
        }

        [Test]
        public void TestUpdate() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            Assert.Ignore("sqlファイルを取得できていないので、UpdateAutoStaticCommandが返っている");
            UpdateDynamicCommand cmd = (UpdateDynamicCommand) dmd.GetSqlCommand("Update");
            Assert.IsNotNull(cmd, "1");
            Assert.AreEqual("employee", cmd.ArgNames[0], "2");
        }

//        [Test, S2(Tx.Rollback)]
        [Test]
        public void TestInsertAutoTx() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            InsertAutoStaticCommand cmd = (InsertAutoStaticCommand) dmd.GetSqlCommand("Insert");
            Assert.IsNotNull(cmd, "1");
            Employee emp = new Employee();
            emp.Empno= 99;
            emp.Ename = "hoge";
            cmd.Execute(new Object[] { emp });

            //TODO:本当はTx.Rollbackしたい
            //更新したVersionNoを戻しておく
            string cmdText = "DELETE [dbo].[EMP] WHERE EMPNO = 99";
            System.Data.IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            System.Data.IDbCommand dbcmd = dataSource.GetCommand(cmdText,cn);
            CommandUtil.ExecuteNonQuery(dataSource,dbcmd);

        }

//        [Test]
//        public void TestInsertAuto() 
//        {
//            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IFormUseHistoryDao),
//                dataSource, BasicCommandFactory.INSTANCE,
//                BasicDataReaderFactory.INSTANCE, dbMetaData);
//
//            InsertAutoStaticCommand cmd = (InsertAutoStaticCommand) dmd.GetSqlCommand("Insert");
//            Assert.IsNotNull(cmd, "1");
//        }

        [Test]
        public void TestUpdateAutoTx() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            ISqlCommand cmd = dmd.GetSqlCommand("Update");
            Assert.IsNotNull(cmd, "1");
            SelectDynamicCommand cmd2 = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployee");
            Employee emp = (Employee) cmd2.Execute(new Object[] { 7788 });
            emp.Ename = "hoge2";
            cmd.Execute(new Object[] { emp });

            //TODO:本当はTx.Rollbackしたい
            string cmdText = "UPDATE [dbo].[EMP] SET ENAME = 'ANALYST' WHERE EMPNO = 7788";
            System.Data.IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            System.Data.IDbCommand dbcmd = dataSource.GetCommand(cmdText,cn);
            CommandUtil.ExecuteNonQuery(dataSource,dbcmd);

        }

        [Test]
        public void TestDeleteAutoTx() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            ISqlCommand cmd = dmd.GetSqlCommand("Delete");
            Assert.IsNotNull(cmd, "1");
            SelectDynamicCommand cmd2 = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployee");
            Employee emp = (Employee) cmd2.Execute(new Object[] { 7788 });
            cmd.Execute(new Object[] { emp });

            //TODO:本当はTx.Rollbackしたい
            string cmdText = "INSERT INTO [dbo].[EMP] VALUES(7788,'SCOTT','ANALYST',7566,CONVERT(datetime,'1982-12-09'),3000.0,NULL,20,CONVERT(datetime,'2005-01-18 13:09:32.213'))";
            System.Data.IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            System.Data.IDbCommand dbcmd = dataSource.GetCommand(cmdText,cn);
            CommandUtil.ExecuteNonQuery(dataSource,dbcmd);
        }

        [Test]
        public void TestIllegalAutoUpdateMethod() 
        {
            try 
            {
    //            new DaoMetaDataImpl(Illegaltypeof(IEmployeeAutoDao), getDataSource(),
    //                    BasicStatementFactory.INSTANCE,
    //                    BasicResultSetFactory.INSTANCE);
                IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IIllegalEmployeeAutoDao),
                    dataSource, BasicCommandFactory.INSTANCE,
                    BasicDataReaderFactory.INSTANCE, dbMetaData);

                Assert.Ignore("例外発生しない");
                Assert.Fail("1");
            } 
            catch (IllegalSignatureRuntimeException ex) 
            {
                //System.out.println(ex.getSignature());
                //System.out.println(ex);
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void TestSelectAuto() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeeByDeptno");
            //System.out.println(cmd.Sql);
        }

//    [Test]
//    public void TestInsertBatchAuto() 
//    {
//        IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeAutoDao),
//            dataSource, BasicCommandFactory.INSTANCE,
//            BasicDataReaderFactory.INSTANCE, dbMetaData);
//
//        InsertBatchAutoStaticCommand cmd = (InsertBatchAutoStaticCommand) dmd
//            .GetSqlCommand("InsertBatch");
//        //Assert.IsNotNull("1", cmd);
//    }
//
//    [Test]
//    public void TestUpdateBatchAuto() 
//    {
//    IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeAutoDao),
//        getDataSource(), BasicStatementFactory.INSTANCE,
//        BasicResultSetFactory.INSTANCE);
//        UpdateBatchAutoStaticCommand cmd = (UpdateBatchAutoStaticCommand) dmd
//            .GetSqlCommand("updateBatch");
//        Assert.IsNotNull("1", cmd);
//    }
//
//    [Test]
//    public void TestDeleteBatchAuto() 
//    {
//    IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeAutoDao),
//        getDataSource(), BasicStatementFactory.INSTANCE,
//        BasicResultSetFactory.INSTANCE);
//        DeleteBatchAutoStaticCommand cmd = (DeleteBatchAutoStaticCommand) dmd
//            .GetSqlCommand("deleteBatch");
//        Assert.IsNotNull("1", cmd);
//    }
//
        [Test]
        public void TestCreateFindCommand() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            ISqlCommand cmd = dmd.CreateFindCommand(null);
            IList employees = (IList) cmd.Execute(null);
            //System.out.println(employees);
            Assert.IsTrue(employees.Count > 0, "1");
        }

        [Test]
        public void TestCreateFindCommand2() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            ISqlCommand cmd = dmd.CreateFindCommand(null);
            IList employees = (IList) cmd.Execute(null);
            //System.out.println(employees);
            Assert.IsTrue(employees.Count > 0, "1");
        }

        [Test]
        public void TestCreateFindCommand3() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            ISqlCommand cmd = dmd.CreateFindCommand("select * from emp");
            IList employees = (IList) cmd.Execute(null);
            //System.out.println(employees);
            Assert.IsTrue(employees.Count > 0, "1");
        }

        [Test]
        public void TestCreateFindCommand4() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            ISqlCommand cmd = dmd.CreateFindCommand("order by empno");
            IList employees = (IList) cmd.Execute(null);
            //System.out.println(employees);
            Assert.IsTrue(employees.Count > 0, "1");
        }

        [Test]
        public void TestCreateFindCommand5() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            //dmd.setDbms(new Oracle());
            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.CreateFindCommand("EMPNO = ?");
            //System.out.println(cmd.Sql);
            Assert.IsTrue(cmd.Sql.EndsWith(" EMPNO = ?"), "1");
        }

        [Test]
        public void TestCreateFindBeanCommand() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            ISqlCommand cmd = dmd.CreateFindBeanCommand("empno = ?");
            Employee employee = (Employee) cmd.Execute(new Object[] { 7788});
            //System.out.println(employee);
            Assert.IsNotNull(employee, "1");
        }

        [Test]
        public void TestCreateObjectBeanCommand() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            ISqlCommand cmd = dmd.CreateFindObjectCommand("select count(*) from emp");
            Int32 count = (Int32) cmd.Execute(null);
            Assert.AreEqual(14, count, "1");
        }

        [Test]
        public void TestSelectAutoByQuery() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            ISqlCommand cmd = dmd.GetSqlCommand("GetEmployeesBySal");
            IList employees = (IList) cmd.Execute(new Object[] { 0,1000 });
            Assert.Ignore("BindVariablesに入っていない");
            Assert.AreEqual(2, employees.Count, "1");
        }

        [Test]
        public void TestSelectAutoByQueryMultiIn() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeesByEnameJob");
            //System.out.println(cmd.Sql);
            IList enames = new ArrayList();
            enames.Add("SCOTT");
            enames.Add("MARY");
            IList jobs = new ArrayList();
            jobs.Add("ANALYST");
            jobs.Add("FREE");
            IList employees = (IList) cmd.Execute(new Object[] { enames, jobs });
            //System.out.println(employees);
            //MARYはいないので
            Assert.AreEqual(1, employees.Count, "1");
        }

        [Test]
        public void TestRelation() 
        {
            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployee2Dao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            ISqlCommand cmd = dmd.GetSqlCommand("GetAllEmployees");
            IList emps = (IList) cmd.Execute(null);
            //System.out.println(emps);
            Assert.IsTrue(emps.Count > 0, "1");
        }

        [Test]
        public void TestGetDaoInterface() 
        {
            Assert.AreEqual(typeof(IEmployeeDao), DaoMetaDataImpl.GetDaoInterface(typeof(IEmployeeDao)), "1");
            //Assert.AreEqual(typeof(IEmployeeDao), DaoMetaDataImpl.getDaoInterface(typeof(EmployeeDaoImpl)), "2");
        }

        [Test]
        public void TestAutoSelectSqlByDto() 
        {
            //TODO
            Assert.Ignore("この書き方ではSetupSelectMethodByAutoの条件判断でargNamesに「dto」がセットされていると判断されてしまう");

            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeesBySearchCondition");
            Assert.IsNotNull(cmd, "1");
            //System.out.println(cmd.Sql);
            EmployeeSearchCondition dto = new EmployeeSearchCondition();
            dto.Dname = "RESEARCH";
            IList employees = (IList) cmd.Execute(new Object[] { dto });
            Assert.IsTrue(employees.Count > 0, "2");
        }

        [Test]
        public void TestAutoSelectSqlByDto2() 
        {
            //TODO
            Assert.Ignore("この書き方ではSetupSelectMethodByAutoの条件判断でargNamesに「dto」がセットされていると判断されてしまう");

            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeAutoDao),
                dataSource, BasicCommandFactory.INSTANCE,
                BasicDataReaderFactory.INSTANCE, dbMetaData);

            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("GetEmployeesByEmployee");
            Assert.IsNotNull(cmd, "1");
            //System.out.println(cmd.Sql);
            Employee dto = new Employee();
            dto.Job = "MANAGER";
            
            IList employees = (IList) cmd.Execute(new Object[] { dto });
            //System.out.println(employees);
            Assert.IsTrue(employees.Count > 0, "2");
        }

//        [Test]
//        public void TestAutoSelectSqlByDto3() 
//        {
//            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(Employee3Dao),
//                            getDataSource(), BasicStatementFactory.INSTANCE,
//                            BasicResultSetFactory.INSTANCE);
//
//            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("getEmployees");
//            Assert.IsNotNull("1", cmd);
//            //System.out.println(cmd.Sql);
//            Employee3 dto = new Employee3();
//            dto.setManager(new Short((short) 7902));
//            List employees = (List) cmd.Execute(new Object[] { dto });
//            //System.out.println(employees);
//            Assert.IsTrue("2", employees.size() > 0);
//        }
//
//        [Test]
//        public void TestAutoSelectSqlByDto4() 
//        {
//            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(Employee3Dao),
//                        getDataSource(), BasicStatementFactory.INSTANCE,
//                        BasicResultSetFactory.INSTANCE);
//
//            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("getEmployees2");
//            Assert.IsNotNull("1", cmd);
//            //System.out.println(cmd.Sql);
//            Assert.IsTrue("2", cmd.Sql.endsWith(" ORDER BY empno"));
//        }
//
//        [Test]
//        public void TestSelfReference() 
//        {
//            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(Employee4Dao),
//                dataSource, BasicCommandFactory.INSTANCE,
//                BasicDataReaderFactory.INSTANCE, dbMetaData);
//
//            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("getEmployee");
//            Assert.IsNotNull("1", cmd);
//            Employee4 employee = (Employee4) cmd.execute(new Object[] { new Int32(7788) });
//            Assert.AreEqual(new Long(7566), employee.getParent().getEmpno(), "2");
//        }
//
//        [Test]
//        public void TestSelfMultiPk() 
//        {
//            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(Employee5Dao),
//                getDataSource(), BasicStatementFactory.INSTANCE,
//                BasicResultSetFactory.INSTANCE);
//            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("getEmployee");
//            Assert.IsNotNull("1", cmd);
//            //System.out.println(cmd.Sql);
//        }
//
//        [Test]
//        public void TestNotHavePrimaryKey() 
//        {
//            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(DepartmentTotalSalaryDao),
//            getDataSource(), BasicStatementFactory.INSTANCE,
//            BasicResultSetFactory.INSTANCE);
//
//            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("getTotalSalaries");
//            Assert.IsNotNull("1", cmd);
//            //System.out.println(cmd.Sql);
//            List result = (List) cmd.Execute(null);
//            //System.out.println(result);
//        }
//
//        [Test]
//        public void TestSelectAutoFullColumnName() 
//        {
//            IDaoMetaData dmd = new DaoMetaDataImpl(typeof(IEmployeeAutoDao),
//            getDataSource(), BasicStatementFactory.INSTANCE,
//            BasicResultSetFactory.INSTANCE);
//
//            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("getEmployee");
//            //System.out.println(cmd.Sql);
//        }
//
//        [Test]
//        public void TestStartsWithOrderBy() 
//        {
//        DaoMetaDataImpl dmd = new DaoMetaDataImpl(typeof(Employee6Dao),
//                        getDataSource(), BasicStatementFactory.INSTANCE,
//                        BasicResultSetFactory.INSTANCE);
//
//            EmployeeSearchCondition condition = new EmployeeSearchCondition();
//            condition.setDname("RESEARCH");
//            SelectDynamicCommand cmd = (SelectDynamicCommand) dmd.GetSqlCommand("getEmployees");
//            //System.out.println(cmd.Sql);
//            Employee[] results = (Employee[]) cmd.Execute(new Object[]{condition});
//            condition.setOrderByString("ENAME");
//            results = (Employee[]) cmd.Execute(new Object[]{condition});
//        }
//        [Test]
//        public void TestQueryAnnotationTx() 
//        {
//            DaoMetaDataImpl dmd = new DaoMetaDataImpl(typeof(Employee7Dao),
//            getDataSource(), BasicStatementFactory.INSTANCE,
//            BasicResultSetFactory.INSTANCE);
//
//            SelectDynamicCommand cmd1 = (SelectDynamicCommand) dmd.GetSqlCommand("getCount");
//            UpdateDynamicCommand cmd2 = (UpdateDynamicCommand) dmd.GetSqlCommand("deleteEmployee");
//            //System.out.println(cmd1.Sql);
//            //System.out.println(cmd2.Sql);
//            assertEquals(new Int32(14),cmd1.execute(null));
//            assertEquals(1,cmd2.execute(new Object[]{new Int32(7369)}));
//            assertEquals(new Int32(13),cmd1.execute(null));
//        }

    }
}
