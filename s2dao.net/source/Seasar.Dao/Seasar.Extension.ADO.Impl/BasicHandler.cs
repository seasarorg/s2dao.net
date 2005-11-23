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
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using Seasar.Extension.ADO;
using Seasar.Extension.ADO.Types;
using Seasar.Framework.Exceptions;
using Seasar.Framework.Util;

namespace Seasar.Extension.ADO.Impl
{
    /// <summary>
    /// BasicHandler ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
    /// </summary>
    public class BasicHandler
    {
        private IDataSource dataSource;
        private string sql;
        private ICommandFactory commandFactory = BasicCommandFactory.INSTANCE;
        
        public BasicHandler()
        {
        }

        public BasicHandler(IDataSource ds, string sql)
            : this(ds, sql, BasicCommandFactory.INSTANCE)
        {
        }

        public BasicHandler(IDataSource ds, string sql, ICommandFactory commandFactory)
        {
            this.DataSource = ds;
            this.Sql = sql;
            this.CommandFactory = commandFactory;
        }

        public IDataSource DataSource
        {
            get { return this.dataSource; }
            set { this.dataSource = value; }
        }

        public string Sql
        {
            get { return this.sql; }
            set { this.sql = value; }
        }

        public ICommandFactory CommandFactory
        {
            get { return commandFactory; }
            set { commandFactory = value; }
        }

        protected IDbConnection Connection
        {
            get
            {
                if(this.dataSource == null) throw new EmptyRuntimeException("dataSource");
                return DataSourceUtil.GetConnection(this.dataSource);
            }
        }

        protected virtual IDbCommand Command(IDbConnection connection)
        {
            if(this.sql == null) throw new EmptyRuntimeException("sql");
            if(!UseAtmark(connection)) this.sql = GetCommandText(this.sql);
            if(IsMySql(connection)) this.sql = GetMySqlCommandText(this.sql);
            return this.dataSource.GetCommand(sql, connection);
        }

        protected void BindArgs(IDbCommand command, object[] args, Type[] argTypes,
            string[] argNames)
        {
            if(args == null) return;
            for(int i = 0; i < args.Length; ++i)
            {
                IValueType valueType = ValueTypes.GetValueType(argTypes[i]);
                try
                {
                    valueType.BindValue(command, argNames[i], args[i]);
                }
                catch(Exception ex)
                {
                    throw new SQLRuntimeException(ex);
                }
            }
        }

        protected string GetCompleteSql(object[] args)
        {
            if(args == null || args.Length == 0) return this.sql;
            return GetCompleteSql(sql, args);
        }

        private bool UseAtmark(IDbConnection cn)
        {
            string cnTypeName = cn.GetType().Name;
            if("SqlConnection".Equals(cnTypeName)) return true;
            if("DB2Connection".Equals(cnTypeName)) return true;
            if("MySqlConnection".Equals(cnTypeName)) return true;
            return false;
        }

        private bool IsMySql(IDbConnection cn)
        {
            if("MySqlConnection".Equals(cn.GetType().Name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GetCompleteSql(string sql, object[] args)
        {
            Regex regex = new Regex(@"(@\w+)");
            MatchCollection matches = regex.Matches(sql);
            return ReplaceSql(sql, args, matches);
        }

        private string ReplaceSql(string sql, object[] args, MatchCollection matches)
        {
            for(int i = 0; i < matches.Count; ++i)
            {
                string capture = matches[i].Captures[0].Value;
                sql = sql.Replace(capture, GetBindVariableText(args[i]));
            }
            return sql;
        }

        private string GetCommandText(string sql)
        {
            Regex regex = new Regex(@"(@\w+)");
            MatchCollection matches = regex.Matches(sql);
            return ReplaceSql(sql, "?", matches);
        }

        private string GetMySqlCommandText(string sql)
        {
            string text = sql;
            Regex regex = new Regex(@"(@\w+)");
            MatchCollection matches = regex.Matches(sql);
            for(int i = 0; i < matches.Count; ++i)
            {
                if(!matches[i].Success) continue;
                text = text.Replace(matches[i].Value, "?" + matches[i].Value.Substring(1));
            }
            return text;
        }

        private string ReplaceSql(string sql, string newValue, MatchCollection matches)
        {
            for(int i = 0; i < matches.Count; ++i)
            {
                string capture = matches[i].Captures[0].Value;
                sql = sql.Replace(capture, newValue);
            }
            return sql;
        }

        protected string GetBindVariableText(object bindVariable)
        {
            if(bindVariable is string)
            {
                return "'" + bindVariable + "'";
            }
			else if(bindVariable == null)
			{
				return "null";
			}
            else if(bindVariable.GetType().IsPrimitive)
            {
                return bindVariable.ToString();
            }
            else if(bindVariable is decimal)
            {
                return bindVariable.ToString();
            }
            else if(bindVariable is DateTime)
            {
                if((DateTime) bindVariable == ((DateTime) bindVariable).Date)
                {
                    return "'" + ((DateTime) bindVariable).ToString("yyyy-MM-dd") + "'";
                }
                else
                {
                    return "'" + ((DateTime) bindVariable).ToString("yyyy-MM-dd HH.mm.ss") + "'";
                }
            }
            else if(bindVariable is bool)
            {
                return bindVariable.ToString();
            }
            else
            {
                return "'" + bindVariable.ToString() + "'";
            }
        }
    }
}
