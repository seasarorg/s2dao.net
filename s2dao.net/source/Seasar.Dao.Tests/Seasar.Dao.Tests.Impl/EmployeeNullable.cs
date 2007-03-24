#region Copyright
/*
 * Copyright 2005-2007 the Seasar Foundation and the Others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */
#endregion

using System;
using System.Text;
using System.Data.SqlTypes;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Impl
{
    // [TimestampProperty("NullableNextRestDate")]
    [Table("EMP_NULLABLE")]
    public class EmployeeNullable
    {
        private long empno;

        private string ename;

        private string job;

        private SqlInt16 mgr;

        private DateTime hiredate = DateTime.Now;

        private SqlSingle sal;

        private SqlSingle comm;

        private int deptno;

        private DateTime tstamp = DateTime.Now;

        private Nullables.NullableDateTime nullableNextRestDate;

        public EmployeeNullable()
        {
        }

        public long Empno
        {
            set { empno = value; }
            get { return empno; }
        }

        public string Ename
        {
            set { ename = value; }
            get { return ename; }
        }

        [Column("Job")]
        public string JobName
        {
            set { job = value; }
            get { return job; }
        }

        public SqlInt16 Mgr
        {
            set { mgr = value; }
            get { return mgr; }
        }

        public DateTime HireDate
        {
            set { hiredate = value; }
            get { return hiredate; }
        }

        public SqlSingle Sal
        {
            set { sal = value; }
            get { return sal; }
        }

        public SqlSingle Comm
        {
            set { comm = value; }
            get { return comm; }
        }

        public int Deptno
        {
            set { deptno = value; }
            get { return deptno; }
        }

        public DateTime TStamp
        {
            set { tstamp = value; }
            get { return tstamp; }
        }

        public Nullables.NullableDateTime NullableNextRestDate
        {
            set { nullableNextRestDate = value; }
            get { return nullableNextRestDate; }
        }

        public bool equals(object other)
        {
            if (!(other.GetType() == typeof(EmployeeNullable))) return false;
            EmployeeNullable castOther = (EmployeeNullable) other;
            return this.Empno == castOther.Empno;
        }

        public int hashCode()
        {
            return (int) this.Empno;
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder(50);
            buf.Append(empno).Append(", ");
            buf.Append(ename).Append(", ");
            buf.Append(job).Append(", ");
            buf.Append(mgr).Append(", ");
            buf.Append(hiredate).Append(", ");
            buf.Append(sal).Append(", ");
            buf.Append(comm).Append(", ");
            buf.Append(deptno).Append(", ");
            buf.Append(tstamp).Append(", ");
            buf.Append(nullableNextRestDate);
            return buf.ToString();
        }
    }
}
