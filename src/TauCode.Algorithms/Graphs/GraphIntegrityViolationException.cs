using System;

namespace TauCode.Algorithms.Graphs
{
    public class GraphIntegrityViolationException : Exception
    {
        public GraphIntegrityViolationException()
            : this("Graph integrity violation")
        {
        }

        public GraphIntegrityViolationException(string message)
            : base(message)
        {
        }
    }
}