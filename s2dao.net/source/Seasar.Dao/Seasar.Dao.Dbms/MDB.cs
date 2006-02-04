using System;
using System.Collections;
using System.Data;
using Seasar.Extension.ADO;
using Seasar.Framework.Util;

namespace Seasar.Dao.Dbms
{
	/// <summary>
	/// MDB ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
	public class MDB : Standard
	{
		public override string Suffix
		{
			get { return "_mdb"; }
		}

		public override string GetSequenceNextValString(String sequenceName)
		{
			return "select gen_id( " + sequenceName + ", 1 ) from RDB$DATABASE";
		}

		public override KindOfDbms Dbms
		{
			get { return KindOfDbms.MDB; }
		}

		public MDB(IDataSource dataSource, IDbConnection cn)
		{
			base.SetupDatabaseMetaData(GetTableSet(dataSource, cn), dataSource, cn);
		}

		protected IList GetTableSet(IDataSource dataSource, IDbConnection cn)
		{
			IList list = new CaseInsentiveSet();
			string sql = "select name from msysobjects where flags=0 and type=1";
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