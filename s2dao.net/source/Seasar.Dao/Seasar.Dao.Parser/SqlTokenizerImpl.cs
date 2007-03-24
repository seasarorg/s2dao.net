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

namespace Seasar.Dao.Parser
{
    public class SqlTokenizerImpl : ISqlTokenizer
    {
        private string sql;
        private int position = 0;
        private string token;
        private TokenType tokenType = TokenType.SQL;
        private TokenType nextTokenType = TokenType.SQL;
        private int bindVariableNum = 0;

        public SqlTokenizerImpl(string sql)
        {
            this.sql = sql;
        }

        #region ISqlTokenizer ƒƒ“ƒo

        public string Token
        {
            get
            {
                return token;
            }
        }

        public string Before
        {
            get
            {
                return sql.Substring(0, position);
            }
        }

        public string After
        {
            get
            {
                return sql.Substring(position);
            }
        }

        public int Position
        {
            get
            {
                return position;
            }
        }

        public TokenType TokenType
        {
            get
            {
                return tokenType;
            }
        }

        public TokenType NextTokenType
        {
            get { return NextTokenType; }
        }

        public TokenType Next()
        {
            if (position >= sql.Length)
            {
                token = null;
                tokenType = TokenType.EOF;
                nextTokenType = TokenType.EOF;
                return tokenType;
            }
            switch (nextTokenType)
            {
                case TokenType.SQL:
                    ParseSql();
                    break;
                case TokenType.COMMENT:
                    ParseComment();
                    break;
                case TokenType.ELSE:
                    ParseElse();
                    break;
                case TokenType.BIND_VARIABLE:
                    ParseBindVariable();
                    break;
                default:
                    ParseEof();
                    break;
            }
            return tokenType;
        }

        public string SkipToken()
        {
            int index = sql.Length;
            char quote = position < sql.Length ? sql.ToCharArray()[position] : '\0';
            bool quoting = quote == '\'' || quote == '(';
            if (quote == '(') quote = ')';

            for (int i = quoting ? position + 1 : position; i < sql.Length; ++i)
            {
                char c = sql.ToCharArray()[i];
                if ((Char.IsWhiteSpace(c) || c == ',' || c == ')' || c == '(')
                    && !quoting)
                {
                    index = i;
                    break;
                }
                else if (c == '/' && i + 1 < sql.Length
                    && sql.ToCharArray()[i + 1] == '*')
                {
                    index = i;
                    break;
                }
                else if (c == '-' && i + 1 < sql.Length
                    && sql.ToCharArray()[i + 1] == '-')
                {
                    index = i;
                    break;
                }
                else if (quoting && quote == '\'' && c == '\''
                    && (i + 1 >= sql.Length || sql.ToCharArray()[i + 1] != '\''))
                {
                    index = i + 1;
                    break;
                }
                else if (quoting && c == quote)
                {
                    index = i + 1;
                    break;
                }
            }
            token = sql.Substring(position, (index - position));
            tokenType = TokenType.SQL;
            nextTokenType = TokenType.SQL;
            position = index;
            return token;
        }

        public string SkipWhitespace()
        {
            int index = SkipWhitespace(position);
            token = sql.Substring(position, (index - position));
            position = index;
            return token;
        }

        #endregion

        protected void ParseSql()
        {
            int commentStartPos = sql.IndexOf("/*", position);
            int lineCommentStartPos = sql.IndexOf("--", position);
            int bindVariableStartPos = sql.IndexOf("?", position);
            int elseCommentStartPos = -1;
            int elseCommentLength = -1;

            if (bindVariableStartPos < 0)
            {
                bindVariableStartPos = sql.IndexOf("?", position);
            }
            if (lineCommentStartPos >= 0)
            {
                int skipPos = SkipWhitespace(lineCommentStartPos + 2);
                if (skipPos + 4 < sql.Length
                    && "ELSE" == sql.Substring(skipPos, ((skipPos + 4) - skipPos)))
                {
                    elseCommentStartPos = lineCommentStartPos;
                    elseCommentLength = skipPos + 4 - lineCommentStartPos;
                }
            }
            int nextStartPos = GetNextStartPos(commentStartPos,
                elseCommentStartPos, bindVariableStartPos);
            if (nextStartPos < 0)
            {
                token = sql.Substring(position);
                nextTokenType = TokenType.EOF;
                position = sql.Length;
                tokenType = TokenType.SQL;
            }
            else
            {
                token = sql.Substring(position, nextStartPos - position);
                tokenType = TokenType.SQL;
                bool needNext = nextStartPos == position;
                if (nextStartPos == commentStartPos)
                {
                    nextTokenType = TokenType.COMMENT;
                    position = commentStartPos + 2;
                }
                else if (nextStartPos == elseCommentStartPos)
                {
                    nextTokenType = TokenType.ELSE;
                    position = elseCommentStartPos + elseCommentLength;
                }
                else if (nextStartPos == bindVariableStartPos)
                {
                    nextTokenType = TokenType.BIND_VARIABLE;
                    position = bindVariableStartPos;
                }
                if (needNext) Next();
            }
        }

        protected int GetNextStartPos(int commentStartPos, int elseCommentStartPos,
            int bindVariableStartPos)
        {
            int nextStartPos = -1;
            if (commentStartPos >= 0)
                nextStartPos = commentStartPos;

            if (elseCommentStartPos >= 0
                && (nextStartPos < 0 || elseCommentStartPos < nextStartPos))
                nextStartPos = elseCommentStartPos;

            if (bindVariableStartPos >= 0
                && (nextStartPos < 0 || bindVariableStartPos < nextStartPos))
                nextStartPos = bindVariableStartPos;

            return nextStartPos;
        }

        protected string NextBindVariableName
        {
            get { return "$" + ++bindVariableNum; }
        }

        protected void ParseComment()
        {
            int commentEndPos = sql.IndexOf("*/", position);
            if (commentEndPos < 0)
                throw new TokenNotClosedRuntimeException("*/",
                    sql.Substring(position));

            token = sql.Substring(position, (commentEndPos - position));
            nextTokenType = TokenType.SQL;
            position = commentEndPos + 2;
            tokenType = TokenType.COMMENT;
        }

        protected void ParseBindVariable()
        {
            token = NextBindVariableName;
            nextTokenType = TokenType.SQL;
            position++;
            tokenType = TokenType.BIND_VARIABLE;
        }

        protected void ParseElse()
        {
            token = null;
            nextTokenType = TokenType.SQL;
            tokenType = TokenType.ELSE;
        }

        protected void ParseEof()
        {
            token = null;
            tokenType = TokenType.EOF;
            nextTokenType = TokenType.EOF;
        }

        private int SkipWhitespace(int position)
        {
            int index = sql.Length;
            for (int i = position; i < sql.Length; ++i)
            {
                char c = sql.ToCharArray()[i];
                if (!Char.IsWhiteSpace(c))
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
    }
}
