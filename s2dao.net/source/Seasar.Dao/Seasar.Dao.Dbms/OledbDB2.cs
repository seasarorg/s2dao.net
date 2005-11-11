using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Impl;
using Seasar.Framework.Util;

namespace Seasar.Dao.Dbms
{
	public class OledbDB2 : DB2
	{
		public OledbDB2(IDataSource dataSource, IDbConnection cn)
		{
			IList tableSet = GetTableSet(dataSource, cn);

			IDictionary primaryKeys = new Hashtable(CaseInsensitiveHashCodeProvider.Default,
				CaseInsensitiveComparer.Default);
			IDictionary columns = new Hashtable(CaseInsensitiveHashCodeProvider.Default,
				CaseInsensitiveComparer.Default);
			IEnumerator tables = tableSet.GetEnumerator();
			string uid = GetUserID(cn);

			while(tables.MoveNext())
			{
				string tableName = tables.Current as String;
				string[] schemaAndName = tableName.Split('.');
				primaryKeys[tableName] = GetPrimaryKeySet(
					dataSource, cn, (string) tables.Current);
				columns[tableName] = GetColumnSet(dataSource, cn, (string) tables.Current);
				if(uid != null && string.Compare(schemaAndName[0], uid, true) == 0)
				{
					primaryKeys[schemaAndName[1].Substring(1)] = primaryKeys[tableName];
					columns[schemaAndName[1].Substring(1)] = columns[tableName];
				}
			}
			DatabaseMetaDataImpl dbMetaData = new DatabaseMetaDataImpl();
			dbMetaData.TableSet = tableSet;
			dbMetaData.PrimaryKeys = primaryKeys;
			dbMetaData.Columns = columns;
			this.dbMetadata = dbMetaData;
		}

		protected string GetUserID(IDbConnection cn)
		{
			string ret = GetParamValue(cn.ConnectionString.ToLower(), "UID".ToLower());
			if(ret != null) return ret;

			ret = GetParamValue(cn.ConnectionString.ToLower(), "User ID".ToLower());
			return ret;
		}

		private string GetParamValue(string str, string name)
		{
			string ret = null;
			int index = str.IndexOf(name);
			if(index > -1)
			{
				int endIndex = str.IndexOf(";", index);
				if(endIndex == -1) endIndex = str.Length - 1;
				int startIndex = str.IndexOf("=", index);
				ret = str.Substring(startIndex, endIndex - startIndex).Trim();
			}
			return ret;
		}

		protected IList GetTableSet(IDataSource dataSource, IDbConnection cn)
		{
			OleDbConnection oledbCn = cn as OleDbConnection;
			IList list = new CaseInsentiveSet();
			DataTable dt = oledbCn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
				new object[] {null, null, null, "TABLE"});
			
			foreach(DataRow row in dt.Rows)
			{
				list.Add(row["TABLE_SCHEMA"] + "." + row["TABLE_NAME"]);
			}
			return list;
		}

		protected IList GetPrimaryKeySet(IDataSource dataSource, IDbConnection cn, string tableName)
		{
			OleDbConnection oledbCn = cn as OleDbConnection;
			IList list = new CaseInsentiveSet();
			string[] schemaAndName = tableName.Split('.');
			DataTable dt = oledbCn.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys,
				new object[] { null, schemaAndName[0], schemaAndName[1].Substring(1)});
			
			foreach(DataRow row in dt.Rows)
			{
				list.Add(row["COLUMN_NAME"]);
			}
			return list;
		}

		protected IList GetColumnSet(IDataSource dataSource, IDbConnection cn, string tableName)
		{
			OleDbConnection oledbCn = cn as OleDbConnection;
			IList list = new CaseInsentiveSet();
			string[] schemaAndName = tableName.Split('.');
			DataTable dt = oledbCn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns,
				new object[] { null, schemaAndName[0], schemaAndName[1].Substring(1), null});
			
			foreach(DataRow row in dt.Rows)
			{
				list.Add(row["COLUMN_NAME"]);
			}
			return list;
		}
	}
}
