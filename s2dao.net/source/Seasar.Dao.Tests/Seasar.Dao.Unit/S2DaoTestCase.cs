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
using Seasar.Dao.Dbms;
using Seasar.Dao.Impl;
using Seasar.Extension.ADO.Impl;
using Seasar.Extension.Unit;

namespace Seasar.Dao.Unit
{
	public class S2DaoTestCase : S2TestCase
	{
		public S2DaoTestCase()
		{
		}

        protected IDbms Dbms 
        {
            get 
            {
                return DbmsManager.GetDbms(DataSource);
            }
        }

        protected BeanMetaDataImpl CreateBeanMetaData(Type beanType) 
        {
            BeanMetaDataImpl beanMetaData = new BeanMetaDataImpl(
                beanType,
                new DatabaseMetaDataImpl(DataSource),
                Dbms
                );
            return beanMetaData;
        }

        protected DaoMetaDataImpl CreateDaoMetaData(Type daoType) 
        {
            DaoMetaDataImpl dmd = new DaoMetaDataImpl();
            dmd.DaoType = daoType;
            dmd.DataSource = DataSource;
            dmd.CommandFactory = BasicCommandFactory.INSTANCE;
            dmd.DataReaderFactory = BasicDataReaderFactory.INSTANCE;
            dmd.DatabaseMetaData = new DatabaseMetaDataImpl(DataSource);
            dmd.Initialize();
            return dmd;
        }
    }
}