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

namespace Seasar.Dao.Impl
{
    public sealed class RelationKey
    {
        private object[] values;
        private int hashCode;

        public RelationKey(object[] values)
        {
            this.values = values;
            foreach (object value in values)
                hashCode += value.GetHashCode();
        }

        public object[] Values
        {
            get { return values; }
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RelationKey)) return false;
            object[] otherValues = ((RelationKey) obj).values;
            if (values.Length != otherValues.Length) return false;
            for (int i = 0; i < values.Length; ++i)
            {
                if (!values[i].Equals(otherValues[i])) return false;
            }
            return true;
        }

    }
}
