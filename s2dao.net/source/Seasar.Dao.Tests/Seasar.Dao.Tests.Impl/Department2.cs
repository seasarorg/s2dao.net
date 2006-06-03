using System;
using System.Text;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Impl
{
	/// <summary>
	/// Department ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
    [Table("DEPT2")]
    public class Department2
	{

        private int deptno;

        private String dname;

        private Boolean active;
    
        public int Deptno
        {
            set { deptno = value; }
            get { return deptno; }
        }

        public String Dname
        {
            set { dname = value; }
            get { return dname; }
        }
        public Boolean IsActive
        {
            set { active = value; }
            get { return active; }
        }

   
   
        public override string ToString() 
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(deptno).Append(", ");
            buf.Append(dname).Append(", ");
            return buf.ToString();
        }


    }
}
