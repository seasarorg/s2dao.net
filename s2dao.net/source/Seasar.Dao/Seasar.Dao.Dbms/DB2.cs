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
            base.SetupDatabaseMetaData(GetTableSet(dataSource, cn), dataSource, cn);
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
                    string uid = GetUserID(cn);
                    while(reader.Read())
                    {
                        list.Add(reader["tabschema"] + "." + reader["tabname"]);
                        string schema = ((string)reader["tabschema"]).Trim();
                        if(uid != null && string.Compare(schema, uid, true) == 0)
                        {
                            list.Add(reader["tabname"]);
                        }
                    }
                }
            }
			
            return list;
        }
	}
}
