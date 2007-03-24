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
    [Table("CWEB_FORM_HIST")]
    public class FormUseHistory
    {
        /** WEB���[�U�R�[�h */
        private string _webUserCode;

        /** WEB���ID */
        private string _webFormId;

        /** �Q�ƃ^�C���X�^���v */
        //      private java.sql.Timestamp referenceTimestamp;

        /** �Q�ƃz�X�gIP */
        private string _referenceHostIp;

        [Column("W_USER_CD")]
        public string WebUserCode
        {
            set { _webUserCode = value; }
            get { return _webUserCode; }
        }

        [Column("W_FORM_ID")]
        public string WebFormId
        {
            set { _webFormId = value; }
            get { return _webFormId; }
        }

        //        [Column("REF_TIMESTAMP")]
        //        public timestamp ReferenceTimestamp
        //        {
        //            set { referenceTimestamp = value; }
        //            get { return referenceTimestamp; }
        //        }


        [Column("REF_HOST_IP")]
        public string ReferenceHostIp
        {
            set { _referenceHostIp = value; }
            get { return _referenceHostIp; }
        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append("webUserCode[").Append(_webUserCode).Append("]");
            buffer.Append("webFormId[").Append(_webFormId).Append("]");
            //          buffer.Append("referenceTimestamp[").Append(referenceTimestamp).Append("]");
            buffer.Append("referenceHostIp[").Append(_referenceHostIp).Append("]");
            return buffer.ToString();
        }
    }
}
