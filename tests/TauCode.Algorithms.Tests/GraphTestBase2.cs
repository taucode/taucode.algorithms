using NUnit.Framework;
using TauCode.Algorithms.Graphs;

namespace TauCode.Algorithms.Tests
{
    [TestFixture]
    public class GraphTestBase2
    {
        protected IGraph<string> Graph { get; set; }

        [SetUp]
        public void SetUpBase()
        {
            this.Graph = new Graph2<string>()
            {
            };
        }

        [TearDown]
        public void TearDownBase()
        {
            this.Graph = null;
        }
    }
}
