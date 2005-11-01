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
using NUnit.Framework;

namespace Seasar.Dao.Tests.Dbms
{
	/// <summary>
	/// StandardTest の概要の説明です。
	/// </summary>
	[TestFixture]
	public class StandardTest {

		[Test,Ignore("StandardのDatabaseMetaDataはnullを返しています。実装予定がないならここはコメントアウトしてください。")]
		public void TestCreateAutoSelectList() {
			IDbms dbms = new Standard();
			IBeanMetaData bmd = new BeanMetaDataImpl(typeof(Employee),
					dbms.DatabaseMetaData, dbms);
			String sql = dbms.GetAutoSelectSql(bmd);
			//System.out.println(sql);
		}
	}
}