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
	}
}