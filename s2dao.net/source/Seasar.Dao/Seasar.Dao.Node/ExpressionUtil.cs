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
using System.Text;
using System.Text.RegularExpressions;

namespace Seasar.Dao.Node
{
    /// <summary>
    /// ExpressionUtil ÇÃäTóvÇÃê‡ñæÇ≈Ç∑ÅB
    /// </summary>
    public class ExpressionUtil
    {
        private IToken current;
        private int start;
        private string text;
        private Regex reOps;
        private Regex reLit;
        private Regex reSym;

        public ExpressionUtil()
        {
            reOps = new Regex(@"^\s*(&&|\|\||<=|>=|==|!=|[=+\-*/^()!<>])", RegexOptions.Compiled);
            reSym = new Regex(@"^\s*(\-?\b*[^=+\-*/^()!<>\s]*)", RegexOptions.Compiled);
            reLit = new Regex(@"^\s*([-+]?[0-9]+(\.[0-9]+)?)", RegexOptions.Compiled);
        }

        public string parseExpression(string expression)
        {
            current = null;
            text = expression;
            StringBuilder sb = new StringBuilder(255);
            while (!EOF())
            {
                IToken token = NextToken();
                sb.Append(token.Value + " ");
            }
            return sb.ToString().TrimEnd(' ');
        }

        protected bool EOF()
        {
            if (current is Eof)
            {
                return true;
            }
            return false;
        }

        protected IToken NextToken()
        {
            Match match;
            match = reLit.Match(text);
            if (match.Length != 0)
            {
                SetNumberLiteralToken(match);
            }
            else
            {
                match = reOps.Match(text);
                if (match.Length != 0)
                {
                    SetOperatorToken(match);
                }
                else
                {
                    match = reSym.Match(text);
                    if (match.Length != 0)
                    {
                        SetSymbolToken(match);
                    }
                    else
                    {
                        current = new Eof();
                    }
                }
            }
            return current;
        }

        private void SetNumberLiteralToken(Match match)
        {
            IToken token;
            start += match.Length;
            text = text.Substring(match.Length);
            token = new NumberLiteral();
            token.Value = match.Groups[1].Value;
            current = token;
        }

        private void SetSymbolToken(Match match)
        {
            IToken token;
            start += match.Length;
            text = text.Substring(match.Length);
            token = new Symbol();
            token.Value = match.Groups[1].Value;
            current = token;
        }

        private void SetOperatorToken(Match match)
        {
            IToken token;
            start += match.Length;
            text = text.Substring(match.Length);
            token = new Operator();
            token.Value = match.Groups[1].Value;
            current = token;
        }
    }

    #region Token
    public interface IToken
    {
        object Value { get; set;}
    }
    public class Eof : IToken
    {
        public Eof()
        {
        }

        public object Value
        {
            get { return null; }
            set { throw new NotImplementedException(); }
        }
    }

    public class Symbol : IToken
    {
        private string _value;
        private string[] _escapes = { "null", "true", "false" };

        public Symbol()
        {
        }

        public object Value
        {
            get { return GetArgValue(); }
            set { _value = (string) value; }
        }

        private string GetArgValue()
        {
            if (_value.StartsWith("'") && _value.EndsWith("'"))
                return _value;

            foreach (string escape in _escapes)
            {
                if (_value.ToLower() == escape)
                    return _value.ToLower();
            }

            return "self.GetArg('" + _value + "')";
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }

    public class NumberLiteral : IToken
    {
        private float _value;

        public NumberLiteral()
        {
        }

        public object Value
        {
            get { return _value; }
            set { _value = (float) Double.Parse(value.ToString()); }
        }
        public override string ToString()
        {
            return _value.ToString();
        }
    }

    public class Operator : IToken
    {
        private string _value;

        public Operator()
        {
        }

        public object Value
        {
            get { return _value; }
            set { _value = (string) value; }
        }
        public override string ToString()
        {
            return _value.ToString();
        }
    }
    #endregion
}
