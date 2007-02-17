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
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;

using Seasar.Framework.Util;

using Seasar.Examples;
using Seasar.Extension.UI;


namespace Seasar.Examples.Impl
{
    /// <summary>
    /// デモを実行するクラスの実装。
    /// 処理可能なデモコードは、引数無しのMainメソッドを持ち、実行結果をテキストコンソール出力する。
    /// </summary>
    public class BasicExamplesHandler : IExamplesHandler
    {    
        private string title = "UnKnown";
        private int codeCodepage = 932;
        private int diconCodepage = 65001;
        
        // 実行されるデモコード
        private object examples = null;

        private ArrayList dicons = new ArrayList();

        private ArrayList codes = new ArrayList();

        public BasicExamplesHandler() {}

        public object Examples
        {
            get { return this.examples;  }
            set { this.examples = value; }
        }

        public int CodeCodepage
        {
            set { codeCodepage = value; }
            get { return codeCodepage; }
        }

        public int DiconCodepage
        {
            set { diconCodepage = value; }
            get { return diconCodepage; }
        }

        public void AddDicon(string path) 
        {
            this.dicons.Add(path);
        }

        public void AddCode(string path)
        {
            this.codes.Add(path);
        }
        
        #region IExamplesHandler メンバ

        public void Main(ExamplesContext context) 
        {
            if(this.examples != null)
            {
                Type t = this.examples.GetType();
                MethodInfo method = t.GetMethod("Main");
                MethodUtil.Invoke(method, this.examples, null);
            }
        }

        public String Title
        {
            get { return this.title; }
            set { this.title = value; }
        }
        
        public void AppendDicon(TextAppender appender) 
        {
            AppendText(this.dicons, appender, diconCodepage);
        }

        public void AppendCode(TextAppender appender) 
        {
            AppendText(this.codes, appender, codeCodepage);
        }

        private void AppendText(ArrayList pathNames, TextAppender appender, int codepage)
        {
            foreach(string path in pathNames)
            {
                string pathWithoutExt = Path.Combine(
                    Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
                string dotPath = Path.Combine(path.Substring(0, 
                    path.LastIndexOf("/")).Replace("/", "."), Path.GetFileName(path));
                string extension = ResourceUtil.GetExtension(path);
                StreamReader sr = null;
                if(File.Exists(path))
                {
                    // 基本的にはビルド時にコードとdiconファイルの両方をコピーしているのでファイルが存在する筈。
                    sr = new StreamReader(path, Encoding.GetEncoding(codepage));
                }
                else 
                {
                    sr = ResourceUtil.GetResourceAsStreamReader(pathWithoutExt, extension);
                }
                appender.WriteLine(sr.ReadToEnd()); // サンプルコードなので、長大な文字列では無い筈…。
            }
        }

        #endregion

    }
}
