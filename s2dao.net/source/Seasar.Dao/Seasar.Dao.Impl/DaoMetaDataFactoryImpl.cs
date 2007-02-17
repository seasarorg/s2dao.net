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
using System.Text;
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
        private IAnnotationReaderFactory readerFactory;
        private IDatabaseMetaData dbMetaData;
        protected string sqlFileEncoding = Encoding.Default.WebName;
        protected string[] insertPrefixes;
        protected string[] updatePrefixes;
        protected string[] deletePrefixes;

        public DaoMetaDataFactoryImpl(IDataSource dataSource,
            ICommandFactory commandFactory, IAnnotationReaderFactory readerFactory, 
            IDataReaderFactory dataReaderFactory)
        {
            this.dataSource = dataSource;
            this.commandFactory = commandFactory;
            this.readerFactory = readerFactory;
            this.dataReaderFactory = dataReaderFactory;
            this.dbMetaData = new DatabaseMetaDataImpl(dataSource);
        }

        public string[] InsertPrefixes 
        {
            set { insertPrefixes = value; }
        }

        public string[] UpdatePrefixes 
        {
            set { updatePrefixes = value; }
        }

        public string[] DeletePrefixes 
        {
            set { deletePrefixes = value; }
        }

        public string SqlFileEncoding 
        {
            set { sqlFileEncoding = value; }
        }

        #region IDaoMetaDataFactory ÉÅÉìÉo

        public IDaoMetaData GetDaoMetaData(Type daoType)
        {
            lock(this)
            {
                string key = daoType.FullName;
                IDaoMetaData dmd = (IDaoMetaData) daoMetaDataCache[key];
                if(dmd != null) 
                {
                    return dmd;
                }
                dmd = CreateDaoMetaData(daoType);
                daoMetaDataCache[key] = dmd;
                return dmd;
            }
        }

        #endregion

        protected virtual IDaoMetaData CreateDaoMetaData(Type daoType) 
        {
            DaoMetaDataImpl dmd = new DaoMetaDataImpl();
            dmd.DaoType = daoType;
            dmd.DataSource = dataSource;
            dmd.CommandFactory = commandFactory;
            dmd.DataReaderFactory = dataReaderFactory;
            dmd.AnnotationReaderFactory = readerFactory;
            dmd.DatabaseMetaData = dbMetaData;
            if (sqlFileEncoding != null) 
            {
                dmd.SqlFileEncoding = sqlFileEncoding;
            }
            if (insertPrefixes != null) 
            {
                dmd.InsertPrefixes = insertPrefixes;
            }
            if (updatePrefixes != null) 
            {
                dmd.UpdatePrefixes = updatePrefixes;
            }
            if (deletePrefixes != null) 
            {
                dmd.DeletePrefixes = deletePrefixes;
            }
            dmd.Initialize();
            return dmd;
        }
    }
}
