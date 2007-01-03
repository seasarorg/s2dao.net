#region Copyright
/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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

namespace Seasar.Dao.Attrs
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IDAttribute : Attribute
    {
        private IDType idType = IDType.ASSIGNED;
        private string sequenceName;

        public IDAttribute(string id)
            : this(id, null)
        {
        }

        public IDAttribute(string id, string sequenceName)
        {
            if ("assigned".Equals(id))
            {
                this.idType = IDType.ASSIGNED;
            }
            else if ("identity".Equals(id))
            {
                this.idType = IDType.IDENTITY;
            }
            else if ("sequence".Equals(id))
            {
                this.idType = IDType.SEQUENCE;
            }
            else
            {
                throw new ArgumentException("id");
            }
            this.sequenceName = sequenceName;
        }

        public IDAttribute(IDType idType)
        {
            this.idType = idType;
        }

        public IDAttribute(IDType idType, string sequenceName)
        {
            this.idType = idType;
            this.sequenceName = sequenceName;
        }

        public IDType IDType
        {
            get { return idType; }
        }

        public string SequenceName
        {
            get { return sequenceName; }
            set { sequenceName = value; }
        }
    }
}
