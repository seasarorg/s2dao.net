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
using Seasar.Dao.Impl;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.ADO.Types;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using NUnit.Framework;

namespace Seasar.Dao.Tests.Impl
{
	/// <summary>
	/// UpdateDynamicCommandTest の概要の説明です。
	/// </summary>
    [TestFixture]
    public class UpdateDynamicCommandTest
	{

        private const string PATH = "Tests.dicon";

        [Test]
        public void TestExecuteTx()
        {

            IS2Container container = S2ContainerFactory.Create(PATH);
            IDataSource dataSource = (IDataSource) container.GetComponent(typeof(IDataSource));
            ValueTypes.Init(dataSource);//これがないと処理できない

            UpdateDynamicCommand cmd = new UpdateDynamicCommand(dataSource,
                BasicCommandFactory.INSTANCE);
            //cmd.setSql("UPDATE emp SET ename = /*employee.ename*/'HOGE' WHERE empno = /*employee.empno*/1234");
            cmd.Sql = "UPDATE emp SET ename = /*employee.Ename*/'HOGE' WHERE empno = /*employee.Empno*/1234";
            cmd.ArgNames = new String[] { "employee" };

            Employee emp = new Employee();
            emp.Empno=7788;
            emp.Ename="SCOTT";
            int count = (int) cmd.Execute(new Object[] { emp });
            Assert.AreEqual(1, count, "1");
        }
    }
}
