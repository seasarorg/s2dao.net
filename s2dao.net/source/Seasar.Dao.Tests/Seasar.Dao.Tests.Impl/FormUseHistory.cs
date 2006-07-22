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

namespace Seasar.Dao.Tests.Impl
{
    [Table("CWEB_FORM_HIST")]
    public class FormUseHistory
    {
        /** WEBユーザコード */
        private string webUserCode;
        
        /** WEB画面ID */
        private string webFormId;
        
        /** 参照タイムスタンプ */
//      private java.sql.Timestamp referenceTimestamp;
        
        /** 参照ホストIP */
        private string referenceHostIp;
        
        [Column("W_USER_CD")]
        public string WebUserCode
        {
            set { this.webUserCode = value; }
            get { return this.webUserCode; }
        }

        [Column("W_FORM_ID")]
        public string WebFormId
        {
            set { this.webFormId = value; }
            get { return this.webFormId; }
        }

//        [Column("REF_TIMESTAMP")]
//        public timestamp ReferenceTimestamp
//        {
//            set { this.referenceTimestamp = value; }
//            get { return this.referenceTimestamp; }
//        }


        [Column("REF_HOST_IP")]
        public string ReferenceHostIp
        {
            set { this.referenceHostIp = value; }
            get { return this.referenceHostIp; }
        }

        public override string ToString() {
            StringBuilder buffer = new StringBuilder();
            buffer.Append("webUserCode[").Append(this.webUserCode).Append("]");
            buffer.Append("webFormId[").Append(this.webFormId).Append("]");
//          buffer.Append("referenceTimestamp[").Append(this.referenceTimestamp).Append("]");
            buffer.Append("referenceHostIp[").Append(this.referenceHostIp).Append("]");
            return buffer.ToString();
        }
    }
}
