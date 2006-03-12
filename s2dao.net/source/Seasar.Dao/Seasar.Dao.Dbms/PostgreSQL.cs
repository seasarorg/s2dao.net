using System;

namespace Seasar.Dao.Dbms
{
	/// <summary>
	/// Postgres ‚ÌŠT—v‚Ìà–¾‚Å‚·B
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