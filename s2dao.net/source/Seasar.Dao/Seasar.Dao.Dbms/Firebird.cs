using System;
using System.Collections;
using System.Data;
using Seasar.Extension.ADO;
using Seasar.Framework.Util;

namespace Seasar.Dao.Dbms
{
	/// <summary>
	/// Firebird ‚ÌŠT—v‚Ìà–¾‚Å‚·B
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

	}
}