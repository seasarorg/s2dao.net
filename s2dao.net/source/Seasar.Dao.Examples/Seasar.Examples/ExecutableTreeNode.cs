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
using System.Windows.Forms;

namespace Seasar.Examples
{
    /// <summary>
    /// ExecutableTreeNode �̊T�v�̐����ł��B
    /// </summary>
    public class ExecutableTreeNode : TreeNode
    {
        private IExamplesHandler examplesHandler;
        public ExecutableTreeNode(IExamplesHandler handler) : base(handler.Title)
        {
            this.examplesHandler = handler;
        }
        
        public IExamplesHandler ExamplesHandler 
        {
            get 
            {
                return this.examplesHandler;
            }
        }
    }
}
