using System;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Impl
{
	/// <summary>
	/// IdentityTable �̊T�v�̐����ł��B
	/// </summary>
    public class IdentityTable
	{
        private int myid;
	
        private String idName;

        [ID("identity")]
        [Column("id")]
        public int Myid
        {
            set { myid = value; }
            get { return myid; }
        }

        public String IdName
        {
            set { idName = value; }
            get { return idName; }
        }
	}
}
