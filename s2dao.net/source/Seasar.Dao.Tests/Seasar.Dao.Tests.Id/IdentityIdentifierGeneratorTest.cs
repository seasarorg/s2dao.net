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

using MbUnit.Framework;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;
using Seasar.Dao.Attrs;
using Seasar.Dao.Dbms;
using Seasar.Dao.Id;
using Seasar.Dao.Unit;

namespace Seasar.Dao.Tests.Id
{
    [TestFixture]
    public class IdentityIdentifierGeneratorTest : S2DaoTestCase 
    {
        [Test, S2(Tx.Rollback)]
        public void TestGetGeneratedValue() 
        {
            if (Dbms.IdentitySelectString == null) 
            {
                Assert.Ignore("IDENTITYをサポートしていないDBMS。");
            }

            BasicUpdateHandler updateHandler = new BasicUpdateHandler(
                DataSource, "insert into identitytable(name) values('hoge')");
            updateHandler.Execute(null);

            IdentityIdentifierGenerator generator = new IdentityIdentifierGenerator("Id", Dbms);
            Hoge hoge = new Hoge();
            generator.SetIdentifier(hoge, DataSource);
            Assert.IsTrue(hoge.Id > 0);
        }
    }
}