using System;

namespace TauCode.Algorithms.Graphs
{
    internal class Edge2<T> : IEdge<T>
    {
        #region Fields

        private readonly Node2<T> _from;
        private readonly Node2<T> _to;
        private bool _isAlive;

        #endregion

        #region Constructor

        internal Edge2(Node2<T> from, Node2<T> to)
        {
            // arg checks omitted since the type is internal.

            _from = from;
            _to = to;
            _isAlive = true;
        }

        #endregion

        public INode2<T> From
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

        public INode2<T> To
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
