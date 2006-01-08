using System;
using System.Collections;
using System.Data;
using Seasar.Extension.ADO;
using Seasar.Framework.Util;

namespace Seasar.Dao.Dbms
{
	/// <summary>
	/// Firebird ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class Firebird : Standard
	{
		public override string Suffix
		{
			get { return "_firebird"; }
		}

		public override string GetSequenceNextValString(String sequenceName)
		{
			return "select gen_id( " + sequenceName + ", 1 ) from RDB$DATABASE";
		}

		public override KindOfDbms Dbms
		{
			get { return KindOfDbms.Firebird; }
		}

		public Firebird(IDataSource dataSource, IDbConnection cn)
		{
			base.SetupDatabaseMetaData(GetTableSet(dataSource, cn), dataSource, cn);
		}

		protected IList GetTableSet(IDataSource dataSource, IDbConnection cn)
		{
			IList list = new CaseInsentiveSet();
			string sql = "SELECT RDB$RELATION_NAME FROM RDB$RELATIONS WHERE RDB$SYSTEM_FLAG <> 1";
			using (IDbCommand cmd = dataSource.GetCommand(sql.ToString(), cn))
			{
				DataSourceUtil.SetTransaction(dataSource, cmd);
				using (IDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						list.Add(reader.GetString(0).Trim());
					}
				}
			}
			return list;
		}
	}
}