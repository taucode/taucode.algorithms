using System;

namespace TauCode.Algorithms.Graphs
{
    internal class Edge<T> : IEdge<T>
    {
        #region Fields

        private readonly Node<T> _from;
        private readonly Node<T> _to;
        private bool _isAlive;

        #endregion

        #region Constructor

        internal Edge(Node<T> from, Node<T> to)
        {
            // arg checks omitted since the type is internal.

            _from = from;
            _to = to;
            _isAlive = true;
        }

        #endregion

        public INode<T> From
        {
            get
            {
                if (!_isAlive)
                {
                    throw new NotImplementedException(); // todo: 'CheckIsAlive'
                }

                return _from;
            }
        }

        public INode<T> To
        {
            get
            {
                if (!_isAlive)
                {
                    throw new NotImplementedException(); // todo
                }

                return _to;
            }
        }

        public void Disappear()
        {
            if (!_isAlive)
            {
                throw new NotImplementedException(); // todo
            }

            _isAlive = false;
        }
    }
}
