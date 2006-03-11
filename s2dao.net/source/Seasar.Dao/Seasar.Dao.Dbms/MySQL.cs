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
	}
}
