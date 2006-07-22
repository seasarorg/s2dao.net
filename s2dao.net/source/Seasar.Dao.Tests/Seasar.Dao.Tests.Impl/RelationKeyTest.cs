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

using Seasar.Dao.Attrs;
using Seasar.Dao.Impl;
using Seasar.Dao.Unit;
using Seasar.Extension.Unit;
using MbUnit.Framework;

namespace Seasar.Dao.Tests.Impl
{
    [TestFixture]
    public class RelationKeyTest : S2DaoTestCase
    {
        [Test, S2]
        public void TestEquals() 
        {
            object[] values = new object[]{ "1", "2" };
            RelationKey pk = new RelationKey(values);
            Assert.AreEqual(pk, pk, "1");
            Assert.AreEqual(pk, new RelationKey(values), "2");
            Assert.AreEqual(false, new RelationKey(new object[]{ "1" }).Equals(pk), "3");
        }
        
        [Test, S2]
        public void TestHashCode() 
        {
            object[] values = new object[]{"1", "2"};
            RelationKey pk = new RelationKey(values);
            Assert.AreEqual("1".GetHashCode() + "2".GetHashCode(), pk.GetHashCode(), "1");
        }
        
        [Table("MyBean")]
        public class MyBean 
        {       
            private int aaa_;

            private string bbb_;

            private Ccc ccc_;

            private int ddd_;

            public int Aaa 
            {
                get { return aaa_; }
                set { aaa_ = value; }
            }
            
            [Column("myBbb")]
            public string Bbb
            {
                get { return bbb_; }
                set { bbb_ = value; }
            }
            
            [Relno(0), Relkeys("ddd:id")]
            public Ccc Cccc
            {
                get { return ccc_; }
                set { ccc_ = value; }
            }
            
            public int Ddd 
            {
                get { return ddd_; }
                set { ddd_ = value; }
            }
        }

        public class Ccc 
        {
            private int id_;

            public int Id
            {
                get { return id_; }
                set { id_ = value; }
            }
        }
    }
}
