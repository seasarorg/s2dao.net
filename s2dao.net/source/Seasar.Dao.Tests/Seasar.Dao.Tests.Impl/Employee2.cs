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

using System.Text;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Impl
{
    [Table("EMP2")]
    public class Employee2
    {
        private long empno;

        private string ename;

        private int deptnum;

        private Department2 department2;

        public Employee2()
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

        public int Deptnum
        {
            set { deptnum = value; }
            get { return deptnum; }
        }

        [Relno(0), Relkeys("DEPTNUM:DEPTNO")]
        public Department2 Department2
        {
            set { department2 = value; }
            get { return department2; }
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(empno).Append(", ");
            buf.Append(ename).Append(", ");
            buf.Append(deptnum).Append(", {");
            buf.Append(department2).Append("}");
            return buf.ToString();
        }
    }
}
