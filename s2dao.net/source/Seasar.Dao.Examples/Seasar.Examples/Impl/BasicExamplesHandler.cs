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
        private string _title = "UnKnown";
        private int _codeCodepage = 932;
        private int _diconCodepage = 65001;
        
        // ���s�����f���R�[�h
        private object _examples = null;

        private readonly ArrayList _dicons = new ArrayList();

        private readonly ArrayList _codes = new ArrayList();

        public object Examples
        {
            get { return _examples;  }
            set { _examples = value; }
        }

        public int CodeCodepage
        {
            set { _codeCodepage = value; }
            get { return _codeCodepage; }
        }

        public int DiconCodepage
        {
            set { _diconCodepage = value; }
            get { return _diconCodepage; }
        }

        public void AddDicon(string path) 
        {
            _dicons.Add(path);
        }

        public void AddCode(string path)
        {
            _codes.Add(path);
        }
        
        #region IExamplesHandler �����o

        public void Main(ExamplesContext context) 
        {
            if(_examples != null)
            {
                Type t = _examples.GetType();
                MethodInfo method = t.GetMethod("Main");
                MethodUtil.Invoke(method, _examples, null);
            }
        }

        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }
        
        public void AppendDicon(TextAppender appender) 
        {
            AppendText(_dicons, appender, _diconCodepage);
        }

        public void AppendCode(TextAppender appender) 
        {
            AppendText(_codes, appender, _codeCodepage);
        }

        private void AppendText(ArrayList pathNames, TextAppender appender, int codepage)
        {
            foreach(string path in pathNames)
            {
                string pathWithoutExt = Path.Combine(
                    Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
                string extension = ResourceUtil.GetExtension(path);
                StreamReader sr;
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
