using System;
using System.Runtime.Serialization;

namespace Seasar.Dao.Pager
{
    [Serializable]
    public class PagingParameterDefinitionException : Exception
    {
        public PagingParameterDefinitionException()
        {
        }

        public PagingParameterDefinitionException(string message) : base(message)
        {
        }

        public PagingParameterDefinitionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected PagingParameterDefinitionException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}