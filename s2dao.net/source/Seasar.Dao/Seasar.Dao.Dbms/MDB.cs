using System;

namespace Seasar.Dao.Dbms
{
	/// <summary>
	/// MDB ‚ÌŠT—v‚Ìà–¾‚Å‚·B
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
	}
}