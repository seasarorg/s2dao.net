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
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Seasar.Extension.UI
{
    /// <summary>
    /// �e�L�X�g�{�b�N�X�ɁA������X�g���[���𗬂����ވׂ̃N���X�ł��B
    /// </summary>
    public class TextAppender : StringWriter
    {
        private delegate void WriteEventHandler(String s);

        private TextBoxBase textBox;
        private WriteEventHandler WriteEvent;

        public TextAppender(TextBoxBase textBox)
        {
            this.textBox = textBox;
            this.textBox.HandleCreated += new EventHandler(OnHandleCreated);
            this.textBox.HandleDestroyed += new EventHandler(OnHandleDestroyed);

            this.WriteEvent = new WriteEventHandler(BufferText);
        }

        private void OnHandleCreated(object sender, EventArgs e)
        {
            this.textBox.AppendText(base.ToString()); // ���Ƀo�b�t�@�����O����Ă��镶������������ށB
            this.WriteEvent = new WriteEventHandler(AppendText);
        }

        private void OnHandleDestroyed(object sender, EventArgs e)
        {
            this.WriteEvent = new WriteEventHandler(DoNothing);
        }

        public override void Write(String s)
        {
            this.WriteEvent(s);
        }

        private void BufferText(string s)
        {
            base.Write(s);
        }

        private void AppendText(string s)
        {
            this.textBox.AppendText(s);
        }

        private void DoNothing(string s)
        {
        }
        public override void WriteLine(string s)
        {
            Write(s + base.NewLine);
        }

        public override void Write(char c)
        {
            Write(c.ToString());
        }
    }
}
