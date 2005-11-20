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
using Seasar.Framework.Container;
using Seasar.Framework.Container.Factory;

namespace Seasar.Dao.Examples.PersistentPropsAttr
{
    /// <summary>
    /// PersistentProps属性のサンプルを実行します。
    /// </summary>
    public class DB2PersistentPropsAttrClient
    {
        private const string PATH = 
            "Seasar.Dao.Examples/PersistentPropsAttr/DB2PersistentPropsAttr.dicon";

        public void Main()
        {
            IS2Container container = S2ContainerFactory.Create(PATH);
            IPersistentPropsAttrLogic logic = (IPersistentPropsAttrLogic) 
                container.GetComponent(typeof(IPersistentPropsAttrLogic));

            try
            {
                logic.TestPersistentPropsAttr();
            }
            catch(ForCleanupException){}
        }
    }
}
