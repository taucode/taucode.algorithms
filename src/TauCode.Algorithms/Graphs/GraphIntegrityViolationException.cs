using System;

namespace TauCode.Algorithms.Graphs
{
    // todo: valid exception pattern.
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