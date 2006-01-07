using System;
using System.Text;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Impl
{
	/// <summary>
	/// Department ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
	/// </summary>
    [Table("DEPT")]
    public class Department
	{

        private int deptno;

        private String dname;

        private String loc;
    
        private int versionNo;
    
        private String dummy;

        public Department() 
        {
        }

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
        public String Loc
        {
            set { loc = value; }
            get { return loc; }
        }

        public int VersionNo
        {
            set { versionNo = value; }
            get { return versionNo; }
        }
   
        public Boolean equals(Object other) 
        {
            //if ( !(other instanceof Department) ) return false;
            if ( !(other.GetType() == typeof(Department)) ) return false;
            Department castOther = (Department) other;
            return this.Deptno == castOther.Deptno;
        }

        public int hashCode() 
        {
            return this.Deptno;
        }
    
        public String toString() 
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(deptno).Append(", ");
            buf.Append(dname).Append(", ");
            buf.Append(loc).Append(", ");
            buf.Append(versionNo);
            return buf.ToString();
        }

        public String Dummy
        {
            set { dummy = value; }
            get { return dummy; }
        }


    }
}
