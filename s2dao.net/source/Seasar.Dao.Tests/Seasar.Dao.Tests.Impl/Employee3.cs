#region Copyright
/*
 * Copyright 2005 the Seasar Foundation and the Others.
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

using System.Data.SqlTypes;
using System.Text;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Impl
{
    [Table("EMP")]
    public class Employee3
    {
        private SqlInt64 empno;

        private string ename;

        private string job;

        private SqlInt16 mgr;

        private SqlDateTime hiredate;

        private SqlSingle sal;

        private SqlSingle comm;

        private SqlInt32 deptno;

        private Department department;

        public Employee3()
        {
        }

        public Employee3(SqlInt64 empno)
        {
            this.empno = empno;
        }

        public SqlInt64 Empno
        {
            set { empno = value; }
            get { return empno; }
        }

        public string Ename
        {
            set { ename = value; }
            get { return ename; }
        }

        public string Job
        {
            set { job = value; }
            get { return job; }
        }

        public SqlInt16 Mgr
        {
            set { mgr = value; }
            get { return mgr; }
        }

        public SqlDateTime Hiredate
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

        public SqlInt32 Deptno
        {
            set { deptno = value; }
            get { return deptno; }
        }

        public Department Department
        {
            set { department = value; }
            get { return department; }
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(empno).Append(", ");
            buf.Append(ename).Append(", ");
            buf.Append(job).Append(", ");
            buf.Append(mgr).Append(", ");
            buf.Append(hiredate).Append(", ");
            buf.Append(sal).Append(", ");
            buf.Append(comm).Append(", ");
            buf.Append(deptno).Append(", {");
            buf.Append(department).Append("}");
            return buf.ToString();
        }
    }
}
