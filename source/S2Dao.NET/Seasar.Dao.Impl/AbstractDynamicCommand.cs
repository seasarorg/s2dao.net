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
using Seasar.Dao.Context;
using Seasar.Dao.Parser;
using Seasar.Extension.ADO;

namespace Seasar.Dao.Impl
{
    public abstract class AbstractDynamicCommand : AbstractSqlCommand
    {
        private INode rootNode;
        private string[] argNames = new string[0];
        private Type[] argTypes = new Type[0];

        public AbstractDynamicCommand(IDataSource dataSource, ICommandFactory commandFactory)
            : base(dataSource, commandFactory)
        {
        }

        public override string Sql
        {
            set 
            {
                base.Sql = value;
                rootNode = new SqlParserImpl(value).Parse();
            }
            get
            {
                return base.Sql;
            }
        }

        public string[] ArgNames
        {
            get { return argNames; }
            set { argNames = value; }
        }

        public Type[] ArgTypes
        {
            get { return argTypes; }
            set { argTypes = value; }
        }

        protected ICommandContext Apply(object[] args)
        {
            ICommandContext ctx = CreateCommandContext(args);
            rootNode.Accept(ctx);
            return ctx;
        }

        protected ICommandContext CreateCommandContext(object[] args)
        {
            ICommandContext ctx = GetCommandContext();
            if(args != null)
            {
                for(int i = 0; i < args.Length; ++i)
                {
                    Type argType = null;
                    if(args[i] != null)
                    {
                        if(i < argTypes.Length)
                            argType = argTypes[i];
                        else if (args[i] != null)
                            argType = args[i].GetType();
                    }
                    if(i < argNames.Length)
                        ctx.AddArg(argNames[i], args[i], argType);
                    else
                        ctx.AddArg("$" + (i + 1), args[i], argType);
                }
            }
            return ctx;
        }

        private ICommandContext GetCommandContext()
        {
            // Žb’è“I‚É
            return new SqlClientCommandContextImpl();
        }
    }
}
