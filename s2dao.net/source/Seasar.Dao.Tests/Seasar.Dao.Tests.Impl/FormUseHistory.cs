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
//	    private java.sql.Timestamp referenceTimestamp;
    	
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


	    /**
	    * 文字列化
	    * @return 文字列
	    */
	    public override string ToString() {
		    StringBuilder buffer = new StringBuilder();
		    buffer.Append("webUserCode[").Append(this.webUserCode).Append("]");
		    buffer.Append("webFormId[").Append(this.webFormId).Append("]");
//		    buffer.Append("referenceTimestamp[").Append(this.referenceTimestamp).Append("]");
		    buffer.Append("referenceHostIp[").Append(this.referenceHostIp).Append("]");
		    return buffer.ToString();
	    }
	}
}
