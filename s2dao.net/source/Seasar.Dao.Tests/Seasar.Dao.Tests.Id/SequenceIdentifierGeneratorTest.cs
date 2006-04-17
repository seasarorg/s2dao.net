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
using Seasar.Dao.Dbms;
using Seasar.Extension;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;
using Seasar.Dao.Id;
using Seasar.Dao.Attrs;
using MbUnit.Framework;

namespace Seasar.Dao.Tests.Id
{
	[TestFixture]
	public class SequenceIdentifierGeneratorTest : S2TestCase
    {

        public void SetUpGenerate()
        {
            Include("Seasar.Dao.Tests.Id.PostgreSQLEx.dicon");
        }

		[Test, S2(Tx.Rollback)]
		public void TestGenerate()
        {
            
            IDataSource dataSource = (IDataSource) GetComponent("PostgreSQLEx.DataSource");

            SequenceIdentifierGenerator generator = new SequenceIdentifierGenerator("Id", new PostgreSQL());
            generator.SequenceName = "\"sequencetable_seqcol_seq\"";
            Hoge hoge = new Hoge();
            generator.SetIdentifier(hoge, dataSource);
            Assert.IsTrue(hoge.Id > 0);
		}
	}
}