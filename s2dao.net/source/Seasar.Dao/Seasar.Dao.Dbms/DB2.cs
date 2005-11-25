using System;
using System.Data;
using System.Collections;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Util;

namespace Seasar.Dao.Dbms
{
	public class DB2 : Standard
	{
		public override string Suffix
		{
			get { return "_db2"; }
		}

		public override string IdentitySelectString
		{
			get { return "values IDENTITY_VAL_LOCAL()"; }
		}

		public override string GetSequenceNextValString(String sequenceName)
		{
			return "values nextval for " + sequenceName;
		}

		public override KindOfDbms Dbms
		{
			get
			{
				return KindOfDbms.DB2;
			}
		}

        public DB2(IDataSource dataSource, IDbConnection cn)
        {
            IList tableSet = GetTableSet(dataSource, cn);

            IDictionary primaryKeys = new Hashtable(CaseInsensitiveHashCodeProvider.Default,
                CaseInsensitiveComparer.Default);
            IDictionary columns = new Hashtable(CaseInsensitiveHashCodeProvider.Default,
                CaseInsensitiveComparer.Default);
            IEnumerator tables = tableSet.GetEnumerator();
            string uid = GetUserID(cn);

            while(tables.MoveNext())
            {
                string tableName = tables.Current as String;
                string[] schemaAndName = tableName.Split('.');
                primaryKeys[tableName] = GetPrimaryKeySet(
                    dataSource, cn, tableName);
                columns[tableName] = GetColumnSet(dataSource, cn, tableName);
                if(uid != null && string.Compare(schemaAndName[0].Trim(), uid, true) == 0)
                {
                    primaryKeys[schemaAndName[1]] = primaryKeys[tableName];
                    columns[schemaAndName[1]] = columns[tableName];
                }
            }
            DatabaseMetaDataImpl dbMetaData = new DatabaseMetaDataImpl();
            dbMetaData.TableSet = tableSet;
            dbMetaData.PrimaryKeys = primaryKeys;
            dbMetaData.Columns = columns;
            this.dbMetadata = dbMetaData;
        }

        protected string GetUserID(IDbConnection cn)
        {
            string ret = GetParamValue(cn.ConnectionString.ToLower(), "UID".ToLower());
            if(ret != null) return ret;

            ret = GetParamValue(cn.ConnectionString.ToLower(), "User ID".ToLower());
			if(ret == null) ret = Environment.UserName;

            return ret.Trim();
        }

        private string GetParamValue(string str, string name)
        {
            string ret = null;
            int index = str.IndexOf(name);
            if(index > -1)
            {
                int endIndex = str.IndexOf(";", index);
                if(endIndex == -1) endIndex = str.Length - 1;
                int startIndex = str.IndexOf("=", index) + 1;
                ret = str.Substring(startIndex, endIndex - startIndex).Trim();
            }
            return ret;
        }

        protected IList GetTableSet(IDataSource dataSource, IDbConnection cn)
        {
            IList list = new CaseInsentiveSet();
            string sql = @"select tabschema, tabname from syscat.tables 
                where substr(tabschema,1,3) <> 'SYS'";
            using(IDbCommand cmd = dataSource.GetCommand(sql, cn))
            {
                DataSourceUtil.SetTransaction(dataSource, cmd);
                using(IDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        list.Add(reader["tabschema"] + "." + reader["tabname"]);
                    }
                }
            }
			
            return list;
        }

        protected IList GetPrimaryKeySet(IDataSource dataSource, IDbConnection cn, string tableName)
        {

            IList list = new CaseInsentiveSet();
            string[] schemaAndName = tableName.Split('.');
            string sql = @"select colname from syscat.keycoluse
                where tabschema=? and tabname=? order by colseq asc";
            using(IDbCommand cmd = dataSource.GetCommand(sql, cn))
            {
                cmd.Parameters.Add(dataSource.GetParameter("tabschema", schemaAndName[0]));
                cmd.Parameters.Add(dataSource.GetParameter("tabname", schemaAndName[1]));
                DataSourceUtil.SetTransaction(dataSource, cmd);
                using(IDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        list.Add(reader["colname"]);
                    }
                }
            }
            return list;
        }

        protected IList GetColumnSet(IDataSource dataSource, IDbConnection cn, string tableName)
        {
            IList list = new CaseInsentiveSet();
            string[] schemaAndName = tableName.Split('.');
            string sql = @"select colname from syscat.columns
                where tabschema=? and tabname=? order by colno asc";
            using(IDbCommand cmd = dataSource.GetCommand(sql, cn))
            {
                cmd.Parameters.Add(dataSource.GetParameter("tabschema", schemaAndName[0]));
                cmd.Parameters.Add(dataSource.GetParameter("tabname", schemaAndName[1]));
                DataSourceUtil.SetTransaction(dataSource, cmd);
                using(IDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        list.Add(reader["colname"]);
                    }
                }
            }
            return list;
        }
	}
}
