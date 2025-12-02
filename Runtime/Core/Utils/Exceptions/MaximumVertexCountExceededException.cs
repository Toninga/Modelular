using System;

namespace Modelular.Runtime
{

    public class MaximumVertexCountExceededException : Exception
    {
        public MaximumVertexCountExceededException()
        {
        }

        public MaximumVertexCountExceededException(string message)
            : base(message)
        {
        }

        public MaximumVertexCountExceededException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}