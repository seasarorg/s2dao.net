using System;

namespace Seasar.Dao.Dbms
{
	/// <summary>
	/// Postgres �̊T�v�̐����ł��B
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