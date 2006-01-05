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
using System.Data;
using System.Data.Common;
using System.Text;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Util;

namespace Seasar.Dao.Dbms
{
    /// <summary>
    /// Standard ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
    /// </summary>
    public class Standard : IDbms
    {
        private Hashtable autoSelectFromClauseCache = new Hashtable();
		protected IDatabaseMetaData dbMetadata;

        public Standard()
        {
        }

        public virtual string Suffix
        {
            get { return ""; }
        }

        public virtual KindOfDbms Dbms
        {
            get { return KindOfDbms.None; }
        }

		public IDatabaseMetaData DatabaseMetaData
		{
			get { return dbMetadata; }
		}

        public  string GetAutoSelectSql(IBeanMetaData beanMetaData)
        {
            StringBuilder buf = new StringBuilder(100);
            buf.Append(beanMetaData.AutoSelectList);
            buf.Append(" ");
            string beanName = beanMetaData.BeanType.Name;
            lock(autoSelectFromClauseCache)
            {
                string fromClause = (string) autoSelectFromClauseCache[beanName];
                if(fromClause == null)
                {
                    fromClause = this.CreateAutoSelectFromClause(beanMetaData);
                    autoSelectFromClauseCache[beanName] = fromClause;
                }
                buf.Append(fromClause);
            }
            return buf.ToString();
        }

        protected virtual string CreateAutoSelectFromClause(IBeanMetaData beanMetaData)
        {
            StringBuilder buf = new StringBuilder(100);
            buf.Append("FROM ");
            string myTableName = beanMetaData.TableName;
            buf.Append(myTableName);
            for(int i = 0; i < beanMetaData.RelationPropertyTypeSize; ++i)
            {
                IRelationPropertyType rpt = beanMetaData.GetRelationPropertyType(i);
                IBeanMetaData bmd = rpt.BeanMetaData;
                buf.Append(" LEFT OUTER JOIN ");
                buf.Append(bmd.TableName);
                buf.Append(" ");
                string yourAliasName = rpt.PropertyName;
                buf.Append(yourAliasName);
                buf.Append(" ON ");
                for(int j = 0; j < rpt.KeySize; ++j)
                {
                    buf.Append(myTableName);
                    buf.Append(".");
                    buf.Append(rpt.GetMyKey(j));
                    buf.Append(" = ");
                    buf.Append(yourAliasName);
                    buf.Append(".");
                    buf.Append(rpt.GetYourKey(j));
                    buf.Append(" AND ");
                }
                buf.Length = buf.Length - 5;
            }
            return buf.ToString();
        }

        public virtual string IdentitySelectString
        {
            get { return null; }
        }

        public virtual string GetSequenceNextValString(string sequenceName)
        {
            return null;
        }

		public void SetupDatabaseMetaData(IList tableSet, IDataSource dataSource, IDbConnection cn)
		{
			IDictionary primaryKeys = new Hashtable(CaseInsensitiveHashCodeProvider.Default,
				CaseInsensitiveComparer.Default);
			IDictionary columns = new Hashtable(CaseInsensitiveHashCodeProvider.Default,
				CaseInsensitiveComparer.Default);

			IEnumerator tables = tableSet.GetEnumerator();
			while(tables.MoveNext())
			{
				string tableName = tables.Current as String;
				string sql = "SELECT * FROM " + tableName;
				DbDataAdapter adapter = 
					dataSource.GetDataAdapter(dataSource.GetCommand(sql, cn)) as DbDataAdapter;
				DataTable metadataTable = new DataTable(tableName);
				adapter.FillSchema(metadataTable, SchemaType.Mapped);
				primaryKeys[tableName] = GetPrimaryKeySet(metadataTable.PrimaryKey);
				columns[tableName] = GetColumnSet(metadataTable.Columns);
			}
			DatabaseMetaDataImpl dbMetaData = new DatabaseMetaDataImpl();
			dbMetaData.TableSet = tableSet;
			dbMetaData.PrimaryKeys = primaryKeys;
			dbMetaData.Columns = columns;
			dbMetadata = dbMetaData;
		}

		private IList GetPrimaryKeySet(DataColumn[] primarykeys)
		{
			IList list = new CaseInsentiveSet();
			foreach (DataColumn pkey in primarykeys)
			{
				list.Add(pkey.ColumnName);
			}
			return list;
		}

		private IList GetColumnSet(DataColumnCollection columns)
		{
			IList list = new CaseInsentiveSet();
			foreach (DataColumn column in columns)
			{
				list.Add(column.ColumnName);
			}
			return list;
		}
    }
}
