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

using MbUnit.Framework;
using Seasar.Dao.Pager;
using Seasar.Extension.ADO;
using Seasar.Extension.Unit;

namespace Seasar.Dao.Tests.Pager
{
    [TestFixture]
    public class PagerDataReaderFactoryRowNumberWrapperTest : S2TestCase
    {
        private MockDataReaderFactory _original;
        private MockPagerDataReaderFactoryRowNumberWrapper _wrapper;

        [SetUp]
        public void SetUp()
        {
            _original = new MockDataReaderFactory();
            _wrapper = new MockPagerDataReaderFactoryRowNumberWrapper(_original, null);
        }

        [Test]
        public void MakeRowNumberSql()
        {
            Assert.AreEqual(
                "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY empno, ename ASC) AS pagerrownumber,  * FROM emp ) AS a WHERE pagerrownumber BETWEEN 1 AND 2",
                _wrapper.MockMakeRowNumberSql("SELECT * FROM emp ORDER BY empno, ename ASC", 2, 0)
                );
        }

        [Test]
        public void MakeCountSql()
        {
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT"),
                "count(*)�őS�������擾����SQL�𐶐�"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT ) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT order by id"),
                "count(*)�őS�������擾����SQL�𐶐�(order by ����)"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT ) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT ORDER BY id"),
                "count(*)�őS�������擾����SQL�𐶐�(ORDER BY ����)"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT\n) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT\norder by\n    id"),
                "count(*)�őS�������擾����SQL�𐶐�(whitespace�t��order by ����)"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT WHERE name like '%order by%' ) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT WHERE name like '%order by%' order by id"),
                "count(*)�őS�������擾����SQL�𐶐�(�r����order by�͏������Ȃ�)"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT WHERE name='aaa'/*order by*/) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT WHERE name='aaa'/*order by*/order by id"),
                "count(*)�őS�������擾����SQL�𐶐�(�r����order by�͏������Ȃ�)"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT WHERE\n--order by\nname=1\n) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT WHERE\n--order by\nname=1\norder by id"),
                "count(*)�őS�������擾����SQL�𐶐�(�r����order by�͏������Ȃ�)"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT ) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT order by �m�n"),
                "count(*)�őS�������擾����SQL�𐶐�(order by���� UNICODE)"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT ) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT order by ���O, �g�D_ID"),
                "count(*)�őS�������擾����SQL�𐶐�(order by���� UNICODE)"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT ) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT order by ���O ASC\n, �g�D_ID DESC"),
                "count(*)�őS�������擾����SQL�𐶐�(order by���� ASC,DESC)"
                );
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT ) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT order\n\tby\n\n ���O \n\tASC \n\n\n, �g�D_ID \n\tDESC \n"),
                "count(*)�őS�������擾����SQL�𐶐�(order by���� ASC,DESC+��s)"
                );
        }

        [Test]
        public void ChopOrderByAndMakeCountSql()
        {
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT ) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT order by id"),
                "count(*)�őS�������擾����SQL�𐶐�(chopOrderBy=true, order by ����)"
                );
            _wrapper.ChopOrderBy = false;
            Assert.AreEqual(
                "SELECT COUNT(*) FROM (SELECT * FROM DEPARTMENT order by id) AS total",
                _wrapper.MockMakeCountSql("SELECT * FROM DEPARTMENT order by id"),
                "count(*)�őS�������擾����SQL�𐶐�(chopOrderBy=false, order by ����)"
                );
        }
    }

    internal class MockPagerDataReaderFactoryRowNumberWrapper : PagerDataReaderFactoryRowNumberWrapper
    {
        public MockPagerDataReaderFactoryRowNumberWrapper(
            IDataReaderFactory dataReaderFactory,
            ICommandFactory commandFactory
            )
            : base(dataReaderFactory, commandFactory)
        {
        }

        public string MockMakeRowNumberSql(string baseSql, int limit, int offset)
        {
            return MakeRowNumberSql(baseSql, limit, offset);
        }

        public string MockMakeCountSql(string baseSql)
        {
            return MakeCountSql(baseSql);
        }
    }
}