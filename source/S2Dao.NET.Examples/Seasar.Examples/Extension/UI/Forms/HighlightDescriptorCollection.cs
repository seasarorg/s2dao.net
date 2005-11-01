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
using System.Collections;

namespace Seasar.Extension.UI.Forms
{
    /// <summary>
    /// Summary description for SeperaratorCollection.
    /// </summary>
    public class HighLightDescriptorCollection
    {
        private ArrayList mInnerList = new ArrayList();
        internal HighLightDescriptorCollection()
        {
        }

        public void AddRange(ICollection c)
        {
            mInnerList.AddRange(c);
        }


        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return mInnerList.IsReadOnly;
            }
        }

        public HighlightDescriptor this[int index]
        {
            get
            {
                return (HighlightDescriptor)mInnerList[index];
            }
            set
            {
                mInnerList[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            mInnerList.RemoveAt(index);
        }

        public void Insert(int index, HighlightDescriptor value)
        {
            mInnerList.Insert(index, value);
        }

        public void Remove(HighlightDescriptor value)
        {
            mInnerList.Remove(value);
        }

        public bool Contains(HighlightDescriptor value)
        {
            return mInnerList.Contains(value);
        }

        public void Clear()
        {
            mInnerList.Clear();
        }

        public int IndexOf(HighlightDescriptor value)
        {
            return mInnerList.IndexOf(value);
        }

        public int Add(HighlightDescriptor value)
        {
            return mInnerList.Add(value);
        }

        public bool IsFixedSize
        {
            get
            {
                return mInnerList.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                return mInnerList.IsSynchronized;
            }
        }

        public int Count
        {
            get
            {
                return mInnerList.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            mInnerList.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return mInnerList.SyncRoot;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return mInnerList.GetEnumerator();
        }

        #endregion
    }
}
