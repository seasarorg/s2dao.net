using System;
using Seasar.Dao.Attrs;
using Nullables;

namespace Seasar.Dao.Examples
{
    /// <summary>
    /// Department
    /// </summary>
    [Table("DEPT2")]
    public class Department2
    {
        private NullableInt32 deptno;
        private string dname;
        private NullableInt16 active;

        public Department2()
        {
        }

        public NullableInt32 Deptno
        {
            set { deptno = value; }
            get { return deptno; }
        }

        public string Dname
        {
            set { dname = value; }
            get { return dname; }
        }

        public NullableInt16 Active
        {
            set { active = value; }
            get { return active; }
        }

        public override string ToString()
        {
            return "deptno=" + (!deptno.HasValue ? "null" : deptno.Value.ToString()) 
                + ", dname=" + (dname == null ? "null" : dname)
                + ", active=" + (!active.HasValue ? "null" : active.Value.ToString());
        }

    }
}
