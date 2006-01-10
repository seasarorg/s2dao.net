using System;
using System.Data.SqlTypes;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Examples
{
	/// <summary>
	/// Department
	/// </summary>
	[Table("DEPT2")]
    public class Department1
	{
        private SqlInt32 deptno;
        private SqlString dname;
        private SqlInt16 active;

		public Department1()
		{
		}

        public SqlInt32 Deptno
        {
            set { deptno = value; }
            get { return deptno; }
        }

        public SqlString Dname
        {
            set { dname = value; }
            get { return dname; }
        }

        public SqlInt16 Active
        {
            set { active = value; }
            get { return active; }
        }

        public override string ToString()
        {
            return "deptno=" + (deptno.IsNull ? "null" : deptno.Value.ToString()) 
                + ", dname=" + (dname.IsNull ? "null" : dname.Value)
                + ", active=" + (active.IsNull ? "null" : active.Value.ToString());
        }

	}
}
