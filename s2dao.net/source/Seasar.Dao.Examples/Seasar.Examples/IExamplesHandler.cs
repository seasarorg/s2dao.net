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

using Seasar.Extension.UI;

namespace Seasar.Examples
{
    /// <summary>
    /// Examples Explorer�ŕ\������ׂ̃f���A�v��
    /// </summary>
    public interface IExamplesHandler
    {
        /// <summary>
        /// �f�������s���܂��B
        /// </summary>
        void Main(ExamplesContext context);
        
        /// <summary>
        /// ���Y�f���A�v���̐ݒ�t�@�C�����o�͂��܂��B
        /// </summary>
        void AppendDicon(TextAppender appender);

        /// <summary>
        /// ���Y�f���A�v���̃\�[�X�R�[�h���o�͂��܂��B
        /// </summary>
        void AppendCode(TextAppender appender);

        String Title
        {
            get;
        }
        
    }
}
