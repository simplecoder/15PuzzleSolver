using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _15Puzzle.Tests
{
    /// <summary>
    /// Tests methods and functionality of the best first search implementation
    /// </summary>
    [TestClass]
    public class SearchHelperMethodsTest
    {
        [TestMethod]
        public void Heuristics_NumberOfTilesOutOfPlace()
        {
            var state = new List<int>(new int[] { 1, 2, Node.SPACE, 3, 6, 7, 11, 4, 5, 9, 12, 8, 13, 10, 14, 15 });
            Assert.AreEqual(13, HeuristicFunctions.number_of_tiles_out_of_place(state));
        }

        [TestMethod]
        public void Heuristics_SumDistanceOfTilesAwayFromTheirPlace()
        {
            var state = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, Node.SPACE, 9, 10, 11, 8, 13, 14, 15, 12});
            Assert.AreEqual(4, HeuristicFunctions.sum_distance_of_tiles_away_from_their_place(state));
        }

        [TestMethod]
        public void SearchHelperMethods_IsDuplicateWithADuplicate_ReturnsTrue()
        {
            var t = new BestFirst_Accessor();
            var state1 = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, Node.SPACE });
            var node = new Node() { State = state1 };
            t.OpenSet.AddFirst(node);
            Assert.IsTrue(t.IsDuplicate(node));
        }

        [TestMethod]
        public void SearchHelperMethods_IsDuplicateWithANonDuplicate_ReturnsFalse()
        {
            var t = new BestFirst_Accessor();
            var state1 = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, Node.SPACE });
            var node = new Node() { State = state1 };
            t.OpenSet.AddFirst(node);
            var state2 = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, Node.SPACE, 15 });
            node = new Node() { State = state2 };
            Assert.IsFalse(t.IsDuplicate(node));
        }

        [TestMethod]
        public void SearchHelperMethods_AddsToOpenSetInOrder()
        {
            var t = new BestFirst_Accessor();
            var state1 = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, Node.SPACE, 15 });
            var node1 = new Node()
            {
                State = state1,
                FValue = HeuristicFunctions.number_of_tiles_out_of_place(state1)
            };
            var state2 = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, Node.SPACE, 14, 15 });
            var node2 = new Node()
            {
                State = state2,
                FValue = HeuristicFunctions.number_of_tiles_out_of_place(state2)
            };
            var state3 = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 13, 12, Node.SPACE, 14, 15 });
            var node3 = new Node()
            {
                State = state3,
                FValue = HeuristicFunctions.number_of_tiles_out_of_place(state3)
            };
            var param = new LinkedList<Node>();
            param.AddFirst(node1);
            param.AddFirst(node2);
            param.AddFirst(node3);
            t.AddToOpenSet(param);
            Assert.AreEqual(node1.FValue, t.OpenSet.First.Value.FValue);
            Assert.AreEqual(node3.FValue, t.OpenSet.Last.Value.FValue);
        }

        [TestMethod]
        public void SearchHelperMethods_GenerateSuccessorsCorrectly()
        {
            var t = new BestFirst_Accessor();
            t.Heuristic = HeuristicFunctions.number_of_tiles_out_of_place;
            var state1 = new List<int>(new int[] { 1, 2, Node.SPACE, 3, 6, 7, 11, 4, 5, 9, 12, 8, 13, 10, 14, 15 });
            var node1 = new Node()
            {
                State = state1,
                FValue = HeuristicFunctions.number_of_tiles_out_of_place(state1)
            };
            var successors = t.GenerateSuccessors(node1);
            var length = (int)Math.Sqrt(successors.First.Value.State.Count);
            Assert.AreEqual(3, successors.Count);
            Assert.IsTrue(BestFirst.IsMovePossible(2, successors.First.Value.State.TakeWhile(value => value != Node.SPACE).Count(), length));
            Assert.IsTrue(BestFirst.IsMovePossible(2, successors.Last.Value.State.TakeWhile(value => value != Node.SPACE).Count(), length));
        }

        [TestMethod]
        public void SearchHelperMethods_FindGoalWhenGoalExists_ReturnsGoal()
        {
            var t = new BestFirst_Accessor();
            var state1 = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, Node.SPACE });
            var node1 = new Node() { State = state1 };
            var nodes = new LinkedList<Node>();
            nodes.AddFirst(node1);
            var result = t.FindGoal(nodes);
            Assert.IsTrue(node1.State.SequenceEqual(result.State));
        }

        [TestMethod]
        public void SearchHelperMethods_FindGoalWhenGoalNotExists_ReturnsNull()
        {
            var t = new BestFirst_Accessor();
            var state1 = new List<int>(new int[] { 1, 2, Node.SPACE, 3, 6, 7, 11, 4, 5, 9, 12, 8, 13, 10, 14, 15 });
            var node1 = new Node()
            {
                State = state1,
                FValue = HeuristicFunctions.number_of_tiles_out_of_place(state1)
            };
            var nodes = new LinkedList<Node>();
            nodes.AddFirst(node1);
            Assert.IsNull(t.FindGoal(nodes));
        }
    }
}
