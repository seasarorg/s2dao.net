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

using System;
using Seasar.Dao.Dbms;
using Seasar.Dao.Impl;
using Seasar.Dao.Unit;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;
using MbUnit.Framework;

namespace Seasar.Dao.Tests.Dbms
{
    [TestFixture]
    public class StandardTest : S2DaoTestCase 
    {
        [Test, S2]
        public void TestCreateAutoSelectList() 
        {
            IDbms dbms = new Standard();
            IBeanMetaData bmd = CreateBeanMetaData(typeof(Employee), dbms);
            string sql = dbms.GetAutoSelectSql(bmd);
            Assert.AreEqual("SELECT EMP2.DEPTNUM, EMP2.ENAME, EMP2.EMPNO FROM EMP2", sql);
        }
    }
}