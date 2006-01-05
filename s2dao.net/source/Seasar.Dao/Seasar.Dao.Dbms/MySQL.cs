using System.Data;
using System.Collections;
using Seasar.Extension.ADO;
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
			base.SetupDatabaseMetaData(GetTableSet(dataSource, cn), dataSource, cn);
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
	}
}
