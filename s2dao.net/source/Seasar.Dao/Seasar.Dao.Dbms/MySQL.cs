using System;
using System.Data;
using System.Collections;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Util;

namespace Seasar.Dao.Dbms
{
	public class MySQL : Standard
	{
        public override string Suffix
        {
            get { return "_mysql"; }
        }

        public override string IdentitySelectString
        {
            get { return "SELECT LAST_INSERT_ID()"; }
        }

        public override KindOfDbms Dbms
        {
            get
            {
                return KindOfDbms.MySQL;
            }
        }

        public MySQL(IDataSource dataSource, IDbConnection cn)
        {
            IList tableSet = GetTableSet(dataSource, cn);

            IDictionary primaryKeys = new Hashtable(CaseInsensitiveHashCodeProvider.Default,
                CaseInsensitiveComparer.Default);
            IDictionary columns = new Hashtable(CaseInsensitiveHashCodeProvider.Default,
                CaseInsensitiveComparer.Default);

            IEnumerator tables = tableSet.GetEnumerator();
            while(tables.MoveNext())
            {
                string tableName = tables.Current as String;
                primaryKeys[tableName] = GetPrimaryKeySet(
                    dataSource, cn, tableName);
                columns[tableName] = GetColumnSet(dataSource, cn, tableName);
            }
            DatabaseMetaDataImpl dbMetaData = new DatabaseMetaDataImpl();
            dbMetaData.TableSet = tableSet;
            dbMetaData.PrimaryKeys = primaryKeys;
            dbMetaData.Columns = columns;
            this.dbMetadata = dbMetaData;
        }

        protected IList GetTableSet(IDataSource dataSource, IDbConnection cn)
        {
            IList list = new CaseInsentiveSet();
            string sql = "show tables";
            using(IDbCommand cmd = dataSource.GetCommand(sql, cn))
            {
                DataSourceUtil.SetTransaction(dataSource, cmd);
                using(IDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        list.Add(reader.GetString(0));
                    }
                }
            }
            return list;
        }

        protected IList GetPrimaryKeySet(IDataSource dataSource, IDbConnection cn, string tableName)
        {
            IList list = new CaseInsentiveSet();
            string sql = "show index from " + tableName;
            using(IDbCommand cmd = dataSource.GetCommand(sql, cn))
            {
                DataSourceUtil.SetTransaction(dataSource ,cmd);
                using(IDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        if(!"PRIMARY".Equals((string) reader["Key_name"])) continue;
                        list.Add(reader["Column_name"]);
                    }
                }
            }
            return list;
        }

        protected IList GetColumnSet(IDataSource dataSource, IDbConnection cn, string tableName)
        {
            IList list = new CaseInsentiveSet();
            string sql = "show columns from " + tableName;
            using(IDbCommand cmd = dataSource.GetCommand(sql, cn))
            {
                DataSourceUtil.SetTransaction(dataSource ,cmd);
                using(IDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        list.Add(reader["Field"]);
                    }
                }
            }
            return list;
        }
	}
}
