using System;
using Seasar.Dao;
using Seasar.Dao.Impl;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using NUnit.Framework;

namespace Seasar.Dao.Tests.Impl
{
	/// <summary>
	/// UpdateBatchAutoStaticCommandTest の概要の説明です。
	/// </summary>
    [TestFixture]
    public class UpdateBatchAutoStaticCommandTest
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
                BasicDataReaderFactory.INSTANCE, new DatabaseMetaDataImpl(dataSource));

            Assert.Ignore("UpdateBatchを有効にすると例外");

            ISqlCommand cmd = dmd.GetSqlCommand("UpdateBatch");
            Employee emp = new Employee();
            emp.Empno = 7788;
            emp.Ename = "hoge";
            Employee emp2 = new Employee();
            emp2.Empno = 7369;
            emp2.Ename = "hoge2";
            Int32 count = (Int32) cmd.Execute(new Object[] { new Employee[] { emp, emp2 } });
            Assert.AreEqual(2, count, "1");
        }
	}
}
