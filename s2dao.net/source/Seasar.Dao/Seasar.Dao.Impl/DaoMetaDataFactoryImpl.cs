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
using System.Collections;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;

namespace Seasar.Dao.Impl
{
    public class DaoMetaDataFactoryImpl : IDaoMetaDataFactory
    {
        private Hashtable daoMetaDataCache = new Hashtable();
        private IDataSource dataSource;
        private ICommandFactory commandFactory;
        private IDataReaderFactory dataReaderFactory;
        private IDatabaseMetaData dbMetaData;

        public DaoMetaDataFactoryImpl(IDataSource dataSource,
            ICommandFactory commandFactory, IDataReaderFactory dataReaderFactory)
        {
            this.dataSource = dataSource;
            this.commandFactory = commandFactory;
            this.dataReaderFactory = dataReaderFactory;
            this.dbMetaData = new DatabaseMetaDataImpl(dataSource);
        }

        #region IDaoMetaDataFactory ÉÅÉìÉo

        public IDaoMetaData GetDaoMetaData(Type daoType)
        {
            lock(this)
            {
                string key = daoType.FullName;
                IDaoMetaData dmd = (IDaoMetaData) daoMetaDataCache[key];
                if(dmd != null) return dmd;
                dmd = new DaoMetaDataImpl(daoType, dataSource, commandFactory,
                    dataReaderFactory, dbMetaData);
                daoMetaDataCache[key] = dmd;
                return dmd;
            }
        }

        #endregion
    }
}
