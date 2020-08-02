using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Algorithms.Graphs
{
    public class GraphSlicingAlgorithm<T>
    {
        //private readonly IGraph2<T> _graph;
        //private IGraph2<T> _clonedGraph;

        private readonly IGraph<T> _graph;
        private List<IGraph<T>> _result;

        public GraphSlicingAlgorithm(IGraph<T> graph)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
        }
        
        public IGraph<T>[] Slice()
        {
            //_clonedGraph = _graph.CloneGraph();
            _result = new List<IGraph<T>>();

            while (true)
            {
                var nodes = this.GetTopLevelNodes();
                if (nodes.Count == 0)
                {
                    if (_graph.Nodes.Any())
                    {
                        _result.Add(_graph);
                    }

                    break;
                }

                var slice = new Graph<T>();
                slice.CaptureNodesFrom(_graph, nodes);
                _result.Add(slice);
            }

            return _result.ToArray();
        }

        private IReadOnlyList<INode<T>> GetTopLevelNodes()
        {
            var result = new List<INode<T>>();

            var nodes = _graph.Nodes;
            foreach (var node in nodes)
            {
                var outgoingEdges = node.GetOutgoingEdgesLyingInGraph(_graph);

                var isTopLevel = true;

                foreach (var outgoingEdge in outgoingEdges)
                {
                    if (outgoingEdge.To == node)
                    {
                        // node referencing self, don't count - it still might be "top-level"
                        continue;
                    }

                    // node referencing another node, i.e. is not "top-level"
                    isTopLevel = false;
                    break;
                }

                if (isTopLevel)
                {
                    result.Add(node);
                }
            }

            return result;
        }
    }
}
