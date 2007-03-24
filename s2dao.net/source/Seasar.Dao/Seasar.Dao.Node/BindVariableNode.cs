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
using System.Reflection;
using Seasar.Framework.Log;

namespace Seasar.Dao.Node
{
    public class BindVariableNode : AbstractNode
    {
        private static readonly Logger logger = Logger.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private string expression;

        public BindVariableNode(string expression)
        {
            this.expression = expression;
        }

        public string Expression
        {
            get { return expression; }
        }

        public override void Accept(ICommandContext ctx)
        {
            object value = ctx.GetArg(expression);
            Type type = null;
            if (value != null)
            {
                type = value.GetType();
            }
            else
            {
                logger.Log("WDAO0001", new object[] { expression });
            }
            ctx.AddSql(value, type, expression.Replace('.', '_'));
        }
    }
}
