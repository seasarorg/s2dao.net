using System;
using System.Collections;
using System.Data;
using Seasar.Extension.ADO;
using Seasar.Framework.Util;

namespace Seasar.Dao.Dbms
{
	/// <summary>
	/// Postgres ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class PostgreSQL : Standard
	{
		public override string Suffix
		{
			get { return "_postgre"; }
		}

		public override string GetSequenceNextValString(String sequenceName)
		{
			return "select nextval('" + sequenceName + "')";
		}

		public override KindOfDbms Dbms
		{
			get { return KindOfDbms.PostgreSQL; }
		}

		public PostgreSQL(IDataSource dataSource, IDbConnection cn)
		{
			base.SetupDatabaseMetaData(GetTableSet(dataSource, cn), dataSource, cn);
		}

		protected IList GetTableSet(IDataSource dataSource, IDbConnection cn)
		{
			IList list = new CaseInsentiveSet();
			string sql = "SELECT tablename FROM pg_tables WHERE schemaname <> 'information_schema' AND schemaname <> 'pg_catalog'";
			using (IDbCommand cmd = dataSource.GetCommand(sql.ToString(), cn))
			{
				DataSourceUtil.SetTransaction(dataSource, cmd);
				using (IDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						list.Add(reader.GetString(0));
					}
				}
			}
			return list;
		}
	}
}