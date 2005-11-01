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

using System;
using System.Text;
using Seasar.Dao.Attrs;

namespace Seasar.Dao.Tests.Dbms
{
    [Table("EMP2")]
    public class Employee
    {
        private int empno;
        private string ename;
        private short deptnum;

        public Employee()
        {
        }

        public int Empno
        {
            set { empno = value; }
            get { return empno; }
        }

        public string Ename
        {
            set { ename = value; }
            get { return ename; }
        }

        public short Deptnum
        {
            set { deptnum = value; }
            get { return deptnum; }
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder(50);
            buf.Append("Empno=");
            buf.Append(Empno);
            buf.Append(", Ename=");
            buf.Append(Ename);
            buf.Append(", Deptnum=");
            buf.Append(Deptnum);
            return buf.ToString();
        }

    }
}
