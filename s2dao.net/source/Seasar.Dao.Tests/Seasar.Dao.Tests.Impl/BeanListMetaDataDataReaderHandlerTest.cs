using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Seasar.Dao;
using Seasar.Dao.Dbms;
using Seasar.Dao.Impl;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Types;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Util;
using NUnit.Framework;

namespace Seasar.Dao.Tests.Impl
{
	/// <summary>
	/// BeanListMetaDataDataReaderHandlerTest の概要の説明です。
	/// </summary>
    [TestFixture]
    public class BeanListMetaDataDataReaderHandlerTest
	{
        private const string PATH = "Tests.dicon";
        private IBeanMetaData beanMetaData_;

        [SetUp]
        public void SetUp()
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            IDataSource dataSource = (IDataSource) container.GetComponent(typeof(IDataSource));

            IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            IDbms dbms = new MSSQLServer(dataSource,cn);
            beanMetaData_ = new BeanMetaDataImpl(typeof(Employee), dbms.DatabaseMetaData, dbms);
        }

        [Test]
        public void TestHandle()
        {
            IDataReaderHandler handler = new BeanListMetaDataDataReaderHandler(
                beanMetaData_);
            String sql = "select * from emp";

            IS2Container container = S2ContainerFactory.Create(PATH);
            IDataSource dataSource = (IDataSource) container.GetComponent(typeof(IDataSource));

            //SQLServer用
            //IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
            SqlConnection cn = (SqlConnection)DataSourceUtil.GetConnection(dataSource);
            SqlCommand cmd = new SqlCommand(sql, cn);

            IList ret = null;
            try 
            {
                //ResultSet rs = ps.executeQuery();
                SqlDataReader rs = cmd.ExecuteReader();
                try 
                {
                    Assert.Ignore("ValueTypesのInitが呼ばれないとここでエラーになるがどこで呼ぶべき？");
                    ret = (IList) handler.Handle(rs);
                } 
                finally 
                {
                    rs.Close();
                }
            } 
            finally 
            {
                cn.Close();
            }
            Assert.IsNotNull(ret, "1");
            //for (int i = 0; i < ret.Count; ++i) 
            //{
            //    Employee emp = (Employee) ret.get(i);
            //    //System.out.println(emp.getEmpno() + "," + emp.getEname());
            //}
            foreach (object itm in ret) 
            {
                Employee emp = (Employee) itm;
                //System.out.println(emp.getEmpno() + "," + emp.getEname());
            }

        }

        [Test,Ignore("ValueTypesの問題を確認してから実装")]
        public void TestHandle2()
        {
//            ResultSetHandler handler = new BeanListMetaDataResultSetHandler(
//                beanMetaData_);
//            String sql = "select emp.*, dept.dname as dname_0 from emp, dept where emp.deptno = dept.deptno and emp.deptno = 20";
//            Connection con = getConnection();
//            PreparedStatement ps = con.prepareStatement(sql);
//            List ret = null;
//            try 
//            {
//                ResultSet rs = ps.executeQuery();
//                try 
//                {
//                    ret = (List) handler.handle(rs);
//                } 
//                finally 
//                {
//                    rs.close();
//                }
//            } 
//            finally 
//            {
//                ps.close();
//            }
//            assertNotNull("1", ret);
//            for (int i = 0; i < ret.size(); ++i) 
//            {
//                Employee emp = (Employee) ret.get(i);
//                System.out.println(emp);
//                Department dept = emp.getDepartment();
//                assertNotNull("2", dept);
//                assertEquals("3", emp.getDeptno(), dept.getDeptno());
//                assertNotNull("4", dept.getDname());
//            }        
        }

        [Test,Ignore("ValueTypesの問題を確認してから実装")]
        public void TestHandle3()
        {
//            ResultSetHandler handler = new BeanListMetaDataResultSetHandler(
//                beanMetaData_);
//            String sql = "select emp.*, dept.deptno as deptno_0, dept.dname as dname_0 from emp, dept where dept.deptno = 20 and emp.deptno = dept.deptno";
//            Connection con = getConnection();
//            PreparedStatement ps = con.prepareStatement(sql);
//            List ret = null;
//            try 
//            {
//                ResultSet rs = ps.executeQuery();
//                try 
//                {
//                    ret = (List) handler.handle(rs);
//                } 
//                finally 
//                {
//                    rs.close();
//                }
//            } 
//            finally 
//            {
//                ps.close();
//            }
//            Employee emp = (Employee) ret.get(0);
//            Employee emp2 = (Employee) ret.get(1);
//            assertSame("1", emp.getDepartment(), emp2.getDepartment());
        }

	}
}
