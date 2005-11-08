using System;
using System.Data;
using Seasar.Extension.ADO;

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
			
		}

		public DB2()
		{
		}
	}
}
