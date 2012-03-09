using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _15Puzzle.Tests
{
    /// <summary>
    /// Runs the three sample data using the Best First Search implementation and two different heuristic functions
    /// </summary>
    [TestClass]
    public class SampleTestRuns
    {

        /// <summary>
        /// Tests the following input sample, using best first search and number of tiles out of place heuristic        
        /// 1   2   []  3
        /// 6   7   11  4
        /// 5   9   12  8
        /// 13  10  14  15
        /// </summary>
        [TestMethod]
        public void BestFirst_Sample1NumTilesOutOfOrder_GeneratesSolution()
        { 
            ISearchAlgorithm searchAlgorithm = new BestFirst();
            var state = new List<int>(new int[] {1, 2, Node.SPACE, 3, 6, 7, 11, 4, 5, 9, 12, 8, 13, 10, 14, 15});
            var initialNode = new Node {State = state};
            searchAlgorithm.Heuristic = HeuristicFunctions.number_of_tiles_out_of_place;
            var solutionPath = searchAlgorithm.Search(initialNode);
            Assert.AreEqual(13, solutionPath.Count);
            Assert.IsTrue(HelperMethods.SolutionPathLeadsToGoal (solutionPath));
            HelperMethods.OutputResults(solutionPath, searchAlgorithm, state);
        }

        /// <summary>
        /// Tests the following input sample, using best first search and number of tiles out of place heuristic
        /// 1   2   3   4
        /// 12  13  14  5
        /// 11  []  15  6
        /// 10  9   8   7
        /// </summary>
        [TestMethod]
        public void BestFirst_Sample2NumTilesOutOfOrder_GeneratesSolution()
        {
            ISearchAlgorithm searchAlgorithm = new BestFirst();
            var state = new List<int>(new int[] { 1,2,3,4,12,13,14,5,11,Node.SPACE,15,6,10,9,8,7 });
            var initialNode = new Node { State = state };
            searchAlgorithm.Heuristic = HeuristicFunctions.number_of_tiles_out_of_place;
            var solutionPath = searchAlgorithm.Search(initialNode);
            Assert.IsTrue(HelperMethods.SolutionPathLeadsToGoal(solutionPath));
            HelperMethods.OutputResults(solutionPath, searchAlgorithm, state);
        }

        /// <summary>
        /// Tests the following input sample, using best first search and number of tiles out of place heuristic
        /// []  15  14  13
        /// 12  11  10  9
        /// 8   7   6   5
        /// 4   3   2   1
        /// </summary>
        [TestMethod]
        public void BestFirst_Sample3NumTilesOutOfOrder_GeneratesSolution()
        {
            ISearchAlgorithm searchAlgorithm = new BestFirst();
            var state = new List<int>(new int[] { Node.SPACE,15,14,13,12,11,10,9,8,7,6,5,4,3,2,1 });
            var initialNode = new Node { State = state };
            searchAlgorithm.Heuristic = HeuristicFunctions.number_of_tiles_out_of_place;
            var solutionPath = searchAlgorithm.Search(initialNode);
            Assert.IsTrue(HelperMethods.SolutionPathLeadsToGoal(solutionPath));
            HelperMethods.OutputResults(solutionPath, searchAlgorithm, state);
        }

        /// <summary>
        /// Tests the following input sample, using best first search and sum distance away heuristic        
        /// 1   2   []  3
        /// 6   7   11  4
        /// 5   9   12  8
        /// 13  10  14  15
        [TestMethod]
        public void BestFirst_Sample1SumDistanceAway_GeneratesSolution()
        {
            ISearchAlgorithm searchAlgorithm = new BestFirst();
            var state = new List<int>(new int[] { 1, 2, Node.SPACE, 3, 6, 7, 11, 4, 5, 9, 12, 8, 13, 10, 14, 15 });
            var initialNode = new Node { State = state };
            searchAlgorithm.Heuristic = HeuristicFunctions.sum_distance_of_tiles_away_from_their_place;
            var solutionPath = searchAlgorithm.Search(initialNode);
            Assert.IsTrue(HelperMethods.SolutionPathLeadsToGoal(solutionPath));
            HelperMethods.OutputResults(solutionPath, searchAlgorithm, state);
        }

        /// <summary>
        /// Tests the following input sample, using best first search and sum distance away heuristic        
        /// 1   2   3   4
        /// 12  13  14  5
        /// 11  []  15  6
        /// 10  9   8   7
        /// </summary>
        [TestMethod]
        public void BestFirst_Sample2SumDistanceAway_GeneratesSolution()
        {
            ISearchAlgorithm searchAlgorithm = new BestFirst();
            var state = new List<int>(new int[] { 1, 2, 3, 4, 12, 13, 14, 5, 11, Node.SPACE, 15, 6, 10, 9, 8, 7 });
            var initialNode = new Node { State = state };
            searchAlgorithm.Heuristic = HeuristicFunctions.sum_distance_of_tiles_away_from_their_place;
            var solutionPath = searchAlgorithm.Search(initialNode);
            Assert.IsTrue(HelperMethods.SolutionPathLeadsToGoal(solutionPath));
            HelperMethods.OutputResults(solutionPath, searchAlgorithm, state);
        }

        /// <summary>
        /// Tests the following input sample, using best first search and sum distance away heuristic 
        /// []  15  14  13
        /// 12  11  10  9
        /// 8   7   6   5
        /// 4   3   2   1
        /// </summary>
        [TestMethod]
        public void BestFirst_Sample3SumDistanceAway_GeneratesSolution()
        {
            ISearchAlgorithm searchAlgorithm = new BestFirst();
            var state = new List<int>(new int[] { Node.SPACE, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 });
            var initialNode = new Node { State = state };
            searchAlgorithm.Heuristic = HeuristicFunctions.sum_distance_of_tiles_away_from_their_place;
            var solutionPath = searchAlgorithm.Search(initialNode);
            Assert.IsTrue(HelperMethods.SolutionPathLeadsToGoal(solutionPath));
            HelperMethods.OutputResults(solutionPath, searchAlgorithm, state);
        }
    }
}
