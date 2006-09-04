#region Copyright
/*
 * Copyright 2005-2006 the Seasar Foundation and the Others.
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
using System.Collections;
using Seasar.Framework.Util;

namespace Seasar.Dao.Node
{
    public abstract class AbstractNode : INode
    {
        private IList children = new ArrayList();

        public AbstractNode()
        {
        }

        #region INode �����o

        public int ChildSize
        {
            get
            {
                return children.Count;
            }
        }

        public INode GetChild(int index)
        {
            return (INode) children[index];
        }

        public void AddChild(INode node)
        {
            children.Add(node);
        }

        public bool ContainsChild(Type childType)
        {
            foreach(INode child in children)
            {
                if(child.GetType().Equals(childType)) return true;
            }
            return false;
        }

        public abstract void Accept(ICommandContext ctx);

        #endregion

        protected object InvokeExpression(string expression, ICommandContext ctx)
        {
            Hashtable ht = new Hashtable();
            ht["self"] = ctx;
            ht["out"] = Console.Out;
            ht["err"] = Console.Error;
            return JScriptUtil.Evaluate(expression, ht, null);
        }
    }
}
