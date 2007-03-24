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

namespace Seasar.Dao.Node
{
    public class PrefixSqlNode : AbstractNode
    {
        private string prefix;
        private string sql;

        public PrefixSqlNode(string prefix, string sql)
        {
            this.prefix = prefix;
            this.sql = sql;
        }

        public string Prefix
        {
            get { return prefix; }
        }

        public string Sql
        {
            get { return sql; }
        }

        public override void Accept(ICommandContext ctx)
        {
            if (ctx.IsEnabled) ctx.AddSql(prefix);
            ctx.AddSql(sql);
        }

    }
}
