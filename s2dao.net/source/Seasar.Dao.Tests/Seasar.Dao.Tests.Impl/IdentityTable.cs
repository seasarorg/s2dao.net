using System;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Impl
{
	/// <summary>
	/// IdentityTable の概要の説明です。
	/// </summary>
    public class IdentityTable
	{
        private int myid;
	
        private String idName;

        [ID("identity")]
        [Column("ID")]
        public int Myid
        {
            set { myid = value; }
            get { return myid; }
        }

        public String Name
        {
            set { idName = value; }
            get { return idName; }
        }
	}
}
