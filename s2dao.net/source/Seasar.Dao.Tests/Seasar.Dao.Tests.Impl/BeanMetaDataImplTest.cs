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
using System.Data;
using Seasar.Dao.Attrs;
using Seasar.Dao.Dbms;
using Seasar.Dao.Impl;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;
using Seasar.Framework.Util;
using MbUnit.Framework;

namespace Seasar.Dao.Tests.Impl
{
	/// <summary>
	/// BeanMetaDataImplTest ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	[TestFixture]
	public class BeanMetaDataImplTest
	{
        private const string PATH = "Tests.dicon";
//        private IBeanMetaData beanMetaData_;
        private IDataSource dataSource;
    
        [SetUp]
        public void SetUp()
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            dataSource = (IDataSource) container.GetComponent(typeof(IDataSource));

//            IDbConnection cn = DataSourceUtil.GetConnection(dataSource);
//            IDbms dbms = new MSSQLServer();
//            beanMetaData_ = new BeanMetaDataImpl(typeof(Employee), new DatabaseMetaDataImpl(dataSource), dbms);
        }

//	    [Test]
//	    public void TestSetup() {
//		    IBeanMetaData bmd = new BeanMetaDataImpl(typeof(MyBean), new DatabaseMetaDataImpl(dataSource), new MSSQLServer());
//		    Assert.AreEqual("MyBean", bmd.TableName, "1");
//		    Assert.AreEqual(3, bmd.PropertyTypeSize, "2");
//		    IPropertyType aaa = bmd.GetPropertyType("aaa");
//		    Assert.AreEqual("aaa", aaa.ColumnName, "3");
//		    IPropertyType bbb = bmd.GetRelationPropertyType("bbb");
//		    Assert.AreEqual("myBbb", bbb.ColumnName, "4");
//		    Assert.AreEqual(1, bmd.RelationPropertyTypeSize, "5");
//		    IRelationPropertyType rpt = bmd.GetRelationPropertyType(0);
//		    Assert.AreEqual(1, rpt.KeySize, "6");
//		    Assert.AreEqual("ddd", rpt.GetMyKey(0), "7");
//		    Assert.AreEqual("id", rpt.GetYourKey(0), "8");
//		    Assert.IsNotNull(bmd.IdentifierGenerator ,"9");
//		    Assert.AreEqual(1, bmd.PrimaryKeySize, "10");
//		    Assert.AreEqual("aaa", bmd.GetPrimaryKey(0), "11");
//	    }
    	
	    [Test]
	    public void TestSetupDatabaseMetaData() {
		    IBeanMetaData bmd = new BeanMetaDataImpl(typeof(Employee), new DatabaseMetaDataImpl(dataSource), new MSSQLServer());
		    IPropertyType empno = bmd.GetPropertyType("empno");
		    Assert.AreEqual(true, empno.IsPrimaryKey, "1");
		    Assert.AreEqual(true, empno.IsPersistent, "2");
		    IPropertyType ename = bmd.GetPropertyType("ename");
		    Assert.AreEqual(false, ename.IsPrimaryKey, "3");
		    IPropertyType dummy = bmd.GetPropertyType("dummy");
		    Assert.AreEqual(false, dummy.IsPersistent, "4");
	    }
    	
	    [Test]
	    public void TestSetupAutoSelectList() {
		    IBeanMetaData bmd = new BeanMetaDataImpl(typeof(Department), new DatabaseMetaDataImpl(dataSource), new MSSQLServer());
		    IBeanMetaData bmd2 = new BeanMetaDataImpl(typeof(Employee), new DatabaseMetaDataImpl(dataSource), new MSSQLServer());
		    string sql = bmd.AutoSelectList;
		    string sql2 = bmd2.AutoSelectList;
		    //System.out.println(sql);
		    //System.out.println(sql2);
    		
		    Assert.IsTrue(sql2.IndexOf("EMP.DEPTNO") > 0, "1");
		    Assert.IsTrue(sql2.IndexOf("Department.DEPTNO AS DEPTNO_0") > 0, "2");
		    Assert.IsTrue(sql2.IndexOf("dummy_0") < 0, "3");
	    }
    	
	    [Test]
	    public void TestConvertFullColumnName() {
		    IBeanMetaData bmd = new BeanMetaDataImpl(typeof(Employee), new DatabaseMetaDataImpl(dataSource), new MSSQLServer());
		    Assert.AreEqual("EMP.empno", bmd.ConvertFullColumnName("empno"), "1");
		    Assert.AreEqual("Department.dname", bmd.ConvertFullColumnName("dname_0"), "2");
	    }
    	
	    [Test]
	    public void TestHasPropertyTypeByAliasName() {
		    IBeanMetaData bmd = new BeanMetaDataImpl(typeof(Employee), new DatabaseMetaDataImpl(dataSource), new MSSQLServer());
		    Assert.AreEqual(true, bmd.HasPropertyTypeByAliasName("empno"), "1");
		    Assert.AreEqual(true, bmd.HasPropertyTypeByAliasName("dname_0"), "2");
		    Assert.AreEqual(false, bmd.HasPropertyTypeByAliasName("xxx"), "3");
		    Assert.AreEqual(false, bmd.HasPropertyTypeByAliasName("xxx_10"), "4");
		    Assert.AreEqual(false, bmd.HasPropertyTypeByAliasName("xxx_0"), "5");
	    }
    	
	    [Test]
	    public void TestGetPropertyTypeByAliasName() {
		    IBeanMetaData bmd = new BeanMetaDataImpl(typeof(Employee), new DatabaseMetaDataImpl(dataSource), new MSSQLServer());
		    Assert.IsNotNull(bmd.GetPropertyTypeByAliasName("empno"), "1");
		    Assert.IsNotNull(bmd.GetPropertyTypeByAliasName("dname_0"), "2");
	    }
    	
//	    [Test]
//	    public void TestSelfReference() {
//		    IBeanMetaData bmd = new BeanMetaDataImpl(typeof(Employee4), new DatabaseMetaDataImpl(dataSource), new MSSQLServer());
//		    IRelationPropertyType rpt = bmd.GetRelationPropertyType("parent");
//		    Assert.AreEqual(typeof(Employee4), rpt.BeanMetaData.BeanType, "1");
//	    }
    	
//	    [Test]
//	    public void TestNoPersistentPropsEmpty() {
//		    IBeanMetaData bmd = new BeanMetaDataImpl(typeof(Ddd), new DatabaseMetaDataImpl(dataSource), new MSSQLServer());
//		    IPropertyType pt = bmd.GetPropertyType("name");
//		    Assert.AreEqual(true, pt.IsPersistent, "1");
//	    }
	
//	    [Test]
//	    public void TestNoPersistentPropsDefined() {
//		    IBeanMetaData bmd = new BeanMetaDataImpl(typeof(Eee), new DatabaseMetaDataImpl(dataSource), new MSSQLServer());
//		    IPropertyType pt = bmd.GetPropertyType("name");
//		    Assert.AreEqual(false, pt.IsPersistent, "1");
//	    }
    	
	    [Test]
	    public void TestPrimaryKeyForIdentifier() {
		    IBeanMetaData bmd = new BeanMetaDataImpl(typeof(IdentityTable), new DatabaseMetaDataImpl(dataSource), new MSSQLServer());
		    Assert.AreEqual("ID", bmd.GetPrimaryKey(0), "1");
	    }
    	
//	    [Test]
//	    public void TestGetVersionNoPropertyName() {
//		    IBeanMetaData bmd = new BeanMetaDataImpl(typeof(Fff), new DatabaseMetaDataImpl(dataSource), new MSSQLServer());
//		    Assert.AreEqual("version", bmd.VersionNoPropertyName, "1");
//	    }
//    	
//	    [Test]
//	    public void TestGetTimestampPropertyName() {
//		    IBeanMetaData bmd = new BeanMetaDataImpl(typeof(Fff), new DatabaseMetaDataImpl(dataSource), new MSSQLServer());
//		    Assert.AreEqual("updated", bmd.TimestampPropertyName, "1");
//	    }
	
        [Table("MyBean")]
        public class MyBean 
        {
    		
		    private Int32 aaa_;
		    private string bbb_;
		    private Ccc ccc_;
		    private Int32 ddd_;
    		
            [ID("assigned")]
		    public Int32 Aaa {
                get
                {
                    return aaa_;
                }
                set
                {
                    aaa_ = value;
                }
		    }
    		
            [Column("myBbb")]
            public string Bbb
            {
                get
                {
                    return bbb_;
                }
                set
                {
                    bbb_ = value;
                }
            }
    		

            [Relno(0), Relkeys("ddd:id")]
            public Ccc Cccc
            {
                get
                {
                    return ccc_;
                }
                set
                {
                    ccc_ = value;
                }
            }

    		
		    public Int32 Ddd {
                get
                {
                    return ddd_;
                }
                set
                {
                    ddd_ = value;
                }
		    }
    		
	    }
	
	    public class Ccc {
		    private Int32 id_;

            [ID("assigned")]
            public Int32 Id
            {
                get
                {
                    return id_;
                }
                set
                {
                    id_ = value;
                }
            }

	    }
    	
        [NoPersistentProps("")]
        public class Ddd : Ccc 
        {
            private string name_;

            public string Name 
            {
                get
                {
                    return name_;
                }
                set
                {
                    name_ = value;
                }

            }
        }

        [NoPersistentProps("name")]
        public class Eee : Ccc 
        {
            private string name_;

            public string Name 
            {
                get
                {
                    return name_;
                }
                set
                {
                    name_ = value;
                }

            }
        }
	
    	
        [VersionNoProperty("Version")]
        [TimestampProperty("Updated")]
        public class Fff 
        {
		    private int version_;
		    private Int32 id_;
		    private DateTime updated_;

		    public Int32 Id {
                get
                {
                    return id_;
                }
                set
                {
                    id_ = value;
                }
		    }
		    public int Version {
                get
                {
                    return version_;
                }
                set
                {
                    version_ = value;
                }
		    }
		    public DateTime Updated {
                get
                {
                    return updated_;
                }
                set
                {
                    updated_ = value;
                }
		    }
	    }
    
    }
}
