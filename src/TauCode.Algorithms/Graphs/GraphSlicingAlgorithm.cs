using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Algorithms.Graphs
{
    public class GraphSlicingAlgorithm<T>
    {
        private readonly Graph<T> _graph;
        private Graph<T> _clonedGraph;
        private List<Graph<T>> _result;

        public GraphSlicingAlgorithm(Graph<T> graph)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
        }

        public Graph<T>[] Slice()
        {
            _clonedGraph = _graph.Clone();
            _result = new List<Graph<T>>();

            while (true)
            {
                var nodes = this.GetTopLevelNodes();
                if (nodes.Count == 0)
                {
                    if (_clonedGraph.Nodes.Any())
                    {
                        _result.Add(_clonedGraph);
                    }

                    break;
                }

                var slice = new Graph<T>();
                slice.CaptureNodes(nodes);
                _result.Add(slice);
            }

            return _result.ToArray();
        }

        private IReadOnlyList<Node<T>> GetTopLevelNodes()
        {
            var result = new List<Node<T>>();


            return _clonedGraph.Nodes
                .Where(x => x.OutgoingEdges.Count == 0)
                .ToList();
        }
    }
}
