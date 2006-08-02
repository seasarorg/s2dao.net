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
    [Table("DEPT")]
    [Serializable()]
    public class Department
    {
        private int deptno;
        private string dname;
        private string loc;
        private int versionNo;

        public Department()
        {
        }

        public int Deptno
        {
            set { deptno = value; }
            get { return deptno; }
        }

        public string Dname
        {
            set { dname = value; }
            get { return dname; }
        }

        public string Loc
        {
            set { loc = value; }
            get { return loc; }
        }
	    
        public int VersionNo
        {
            set { versionNo = value; }
            get { return versionNo; }
        }
	    
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(deptno).Append(", ");
            buf.Append(dname).Append(", ");
            buf.Append(loc).Append(", ");
            buf.Append(versionNo);
            return buf.ToString();
        }
    }
}