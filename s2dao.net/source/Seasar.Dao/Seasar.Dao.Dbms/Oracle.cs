using System;
using System.Data;
using System.Collections;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Util;

namespace Seasar.Dao.Dbms
{
	/// <summary>
	/// Oracle
	/// </summary>
	public class Oracle : Standard
	{
        public override string Suffix
        {
            get
            {
                return "_oracle";
            }
        }

        public override string GetSequenceNextValString(string sequenceName)
        {
            return "select " + sequenceName + ".nextval from dual";
        }

        public override KindOfDbms Dbms
        {
            get
            {
                return KindOfDbms.Oracle;
            }
        }


        protected override string CreateAutoSelectFromClause(IBeanMetaData beanMetaData)
        {
            return null;
        }


		public Oracle(IDataSource dataSource, IDbConnection cn)
		{
		}
	}
}
