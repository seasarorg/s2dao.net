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

namespace Seasar.Dao.Node
{
    public class IfNode : ContainerNode
    {
        private string expression;
        private ElseNode elseNode;
        private ExpressionUtil expressionUtil;
        
        public IfNode(string expression)
        {
			expressionUtil = new ExpressionUtil();
            this.expression = expressionUtil.parseExpression(expression);
            if (this.expression == null)
                throw new ApplicationException("IllegalBoolExpression=[" + this.expression + "]");
        }

        public string Expression
        {
            get { return this.expression; }
        }

        public ElseNode ElseNode
        {
            get { return elseNode; }
            set { elseNode = value; }
        }

        public override void Accept(ICommandContext ctx)
        {
            object result = InvokeExpression(this.expression, ctx);
            if (result != null)
            {
				if(Convert.ToBoolean(result))
				{
					base.Accept(ctx);
					ctx.IsEnabled = true;
				}
                else if (elseNode != null)
                {
                    elseNode.Accept(ctx);
                    ctx.IsEnabled = true;
                }
            }
            else
            {
                throw new ApplicationException("IllegalBoolExpression=[" + this.expression + "]");
            }
        }
    }
}
