using NUnit.Framework;
using TauCode.Algorithms.Graphs;

namespace TauCode.Algorithms.Tests
{
    [TestFixture]
    public class GraphTestBase
    {
        protected Graph<string> Graph { get; set; }

        [SetUp]
        public void SetUpBase()
        {
            this.Graph = new Graph<string>()
            {
                Name = "SutGraph", // system under test
            };
        }

        [TearDown]
        public void TearDownBase()
        {
            this.Graph = null;
        }
    }
}
