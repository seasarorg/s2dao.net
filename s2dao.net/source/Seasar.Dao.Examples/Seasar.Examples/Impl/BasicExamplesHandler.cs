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
    /// �f�������s����N���X�̎����B
    /// �����\�ȃf���R�[�h�́A����������Main���\�b�h�������A���s���ʂ��e�L�X�g�R���\�[���o�͂���B
    /// </summary>
    public class BasicExamplesHandler : IExamplesHandler
    {    
        private string title = "UnKnown";
        private int codeCodepage = 932;
        private int diconCodepage = 65001;
        
        // ���s�����f���R�[�h
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
        
        #region IExamplesHandler �����o

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
                    // ��{�I�ɂ̓r���h���ɃR�[�h��dicon�t�@�C���̗������R�s�[���Ă���̂Ńt�@�C�������݂��锤�B
                    sr = new StreamReader(path, Encoding.GetEncoding(codepage));
                }
                else 
                {
                    sr = ResourceUtil.GetResourceAsStreamReader(pathWithoutExt, extension);
                }
                appender.WriteLine(sr.ReadToEnd()); // �T���v���R�[�h�Ȃ̂ŁA����ȕ�����ł͖������c�B
            }
        }

        #endregion

    }
}
