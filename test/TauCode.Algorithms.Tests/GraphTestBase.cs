using NUnit.Framework;
using TauCode.Algorithms.Graphs;

namespace TauCode.Algorithms.Tests
{
    [TestFixture]
    public class GraphTestBase
    {
        protected IGraph<string> Graph { get; set; }

        [SetUp]
        public void SetUpBase()
        {
            this.Graph = new Graph<string>()
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
