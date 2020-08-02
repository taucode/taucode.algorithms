using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Algorithms.Graphs2
{
    public class GraphSlicingAlgorithm2<T>
    {
        //private readonly IGraph2<T> _graph;
        //private IGraph2<T> _clonedGraph;

        private readonly IGraph2<T> _graph;
        private List<IGraph2<T>> _result;

        public GraphSlicingAlgorithm2(IGraph2<T> graph)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
        }
        
        public IGraph2<T>[] Slice()
        {
            //_clonedGraph = _graph.CloneGraph();
            _result = new List<IGraph2<T>>();

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

                var slice = new Graph2<T>();
                slice.CaptureNodesFrom(_graph, nodes);
                _result.Add(slice);
            }

            return _result.ToArray();
        }

        private IReadOnlyList<INode2<T>> GetTopLevelNodes()
        {
            var result = new List<INode2<T>>();

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
