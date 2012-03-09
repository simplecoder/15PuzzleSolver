using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace _15Puzzle
{
    /// <summary>
    /// Implementation of the Best First search algorithm to solve the 15 puzzle
    /// </summary>
    public class BestFirst : ISearchAlgorithm
    {
        /// <summary>
        /// Holds the frontier nodes, ordered by F value ascending
        /// </summary>
        private LinkedList<Node> OpenSet { get; set; }

        /// <summary>
        /// Holds the nodes that were already expended
        /// </summary>
        private LinkedList<Node> ClosedSet { get; set; }

        /// <summary>
        /// Fields to hold statistical data
        /// </summary>
        private int _numNodesGenerated;
        private int _lengthOfSolutionPath;
        private TimeSpan _executionTime;
        private int _branchingFactor;
        public int NumNodesGenerated{ get { return _numNodesGenerated; }}
        public int LengthOfSolutionPath {get { return _lengthOfSolutionPath; }}
        public TimeSpan ExecutionTime { get { return _executionTime; } }
        public int BranchingFactor { get { return _branchingFactor; } }

        public double Pentrance
        {
            get {return NumNodesGenerated == 0 ? 0 : (double) LengthOfSolutionPath / NumNodesGenerated;}
        }
        
        /// <summary>
        /// The heuristic function that will be used by the search algorithm
        /// </summary>
        public Func<List<int>, int> Heuristic { get; set; }

        public BestFirst()
        {
            OpenSet = new LinkedList<Node>();
            ClosedSet = new LinkedList<Node>();
        }

        /// <param name="parent"> The parent node of successor</param>
        /// <param name="prev"> The position of space in the parent </param>
        /// <param name="next"> The position the space is to be moved to</param>
        /// <returns>The successor node resulting from moving the space in the parent state from prev to next </returns>
        private Node CreateSuccessorNode (Node parent, int prev, int next)
        {
            var child = new Node();
            child.State = new List<int>(parent.State);               
            child.State[prev] = parent.State[next];
            child.State[next] = parent.State[prev];
            child.FValue = Heuristic(child.State);
            child.BackPtr = parent;
            return child;
        }

        
        /// <param name="prev">The starting position of the space</param>
        /// <param name="next">The ending position of the space</param>
        /// <param name="sideLen">The side length of the board</param>
        /// <returns>True if the move from prev to next is valid</returns>
        public static bool IsMovePossible(int prev, int next, int sideLen)
        {
            var leftEdgeNodes = new int[] {0, 4, 8, 12};
            var rightEdgeNodes = new int[] { 3, 7, 11, 15 };
            if (next < 0 || next >= sideLen * sideLen) // out of bounds
                return false;
            if (leftEdgeNodes.Any(p => p == prev) && (prev - 1 == next)) // not a legal left move
                return false;
            if (rightEdgeNodes.Any(p => p == prev) && (prev + 1 == next)) // not a legal right move
                return false;
            return true;
        }

        /// <param name="parent">The parent node whose successors will be calculated</param>
        /// <returns>The successor nodes for the given parent</returns>
        private LinkedList<Node> GenerateSuccessors (Node parent)
        {
            var successors = new LinkedList<Node>();
            var sideLength = (int)Math.Sqrt(parent.State.Count);
            var i = parent.State.TakeWhile(value => value != Node.SPACE).Count();

            if (IsMovePossible(i, i - 1, sideLength)) // move space left
                successors.AddFirst(CreateSuccessorNode(parent, i, i - 1));
            if (IsMovePossible(i, i + 1, sideLength)) // move space right
                successors.AddFirst(CreateSuccessorNode(parent, i, i + 1));
            if (IsMovePossible(i, i - sideLength, sideLength)) // move space up
                successors.AddFirst(CreateSuccessorNode(parent, i, i - sideLength));
            if (IsMovePossible(i, i + sideLength, sideLength)) // move space down
                successors.AddFirst(CreateSuccessorNode(parent, i, i + sideLength));

            return successors;
        }

        /// <summary>
        /// Adds the given nodes to the Open Set while preserving the invariant of the Open Set (being ordered by F value)
        /// </summary>
        /// <param name="nodes">The nodes to be added to the OpenSet</param>
        private void AddToOpenSet (IEnumerable<Node> nodes)
        {
            if (nodes == null) throw new ArgumentNullException("nodes");

            foreach (var candidateNode in nodes.Where(node => !IsDuplicate(node)).OrderByDescending(node => node.FValue))
            {
                var curr = OpenSet.First;
                var added = false;
                while (curr != null && !added)
                {
                    if (curr.Value.FValue > candidateNode.FValue)
                    {
                        OpenSet.AddBefore(curr, candidateNode);
                        added = true;
                    }
                    curr = curr.Next;
                }

                if (!added)
                    OpenSet.AddLast(candidateNode);
            }
          
        }

        /// <param name="nodes">The nodes to be searched for the goal state</param>
        /// <returns>The node if the goal is found within the nodes, null otherwise</returns>
        private Node FindGoal(IEnumerable<Node> nodes )
        {
            return nodes.FirstOrDefault(node => node.FValue == 0);
        }

        /// <param name="candidateNode">The node to be checked for duplication</param>
        /// <returns>True if the candidate node already exists in either the open or closed sets</returns>
        private bool IsDuplicate (Node candidateNode)
        {
            return OpenSet.Concat(ClosedSet)
                        .Where(node => node.FValue == candidateNode.FValue)
                        .Any(node => node.State.SequenceEqual(candidateNode.State));
        }

        /// <param name="goal">The goal node that will be used to trace back up to the initial node</param>
        /// <returns>The list of nodes from goal node to initial node</returns>
        private LinkedList<Node> GetSolutionPath (Node goal)
        {
            if (goal == null) return null;

            var solution = new LinkedList<Node>();
            var current = goal;
            while (current != null)
            {
                _lengthOfSolutionPath++;
                solution.AddFirst(current);
                current = current.BackPtr;
            }
            return solution;
        }

        /// <summary>
        /// Best first search implementation
        /// </summary>
        /// <param name="initialNode">The initial node, representing the puzzle to be solved</param>
        /// <returns>The solution path from inital node to goal node</returns>
        public LinkedList<Node> Search(Node initialNode)
        {
            // Check for valid params
            if (initialNode == null) throw new ArgumentNullException("initialNode");
            if (Heuristic == null)
                throw new NullReferenceException("Heuristic function not specified, this search requires a heuristic");

            var watch = new Stopwatch();
            watch.Start();
            Node goalNode = null;
            var depth = 0;

            // Calculate h(start)
            initialNode.FValue = Heuristic(initialNode.State);
            
            // Put IS on open
            OpenSet.AddFirst(initialNode);
                
            // Loop until |open| = 0 or goalState found
            while (OpenSet.Count > 0 && goalNode == null)
            {
                var currentNode = OpenSet.First; // First is the lowest F value, because OpenSet is sorted by F Value
                OpenSet.Remove(currentNode);
                ClosedSet.AddFirst(currentNode);
                var successors = GenerateSuccessors(currentNode.Value);
                _numNodesGenerated += successors.Count;
                AddToOpenSet(successors);
                goalNode = FindGoal(successors);
                depth++;
            }
            _branchingFactor = _numNodesGenerated/depth;
            watch.Stop();
            _executionTime = watch.Elapsed;
            
            // If goal found then return the solution path, else return null
            return goalNode == null ? null : GetSolutionPath(goalNode);
        }

        

    }
}
