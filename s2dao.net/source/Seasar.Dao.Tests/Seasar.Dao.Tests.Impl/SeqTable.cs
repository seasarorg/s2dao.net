using System;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Impl
{
	/// <summary>
	/// SeqTable ‚ÌŠT—v‚Ìà–¾‚Å‚·B
	/// </summary>
	public class SeqTable
	{
        private int id;
	
        private String name;

        [ID("sequence, sequenceName=myseq")]
        public int ID
        {
            set { id = value; }
            get { return id; }
        }

        public string Name
        {
            set { name = value; }
            get { return name; }
        }
    }
}
