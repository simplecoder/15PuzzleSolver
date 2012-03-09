using System;
using System.Collections.Generic;

namespace _15Puzzle
{
    /// <summary>
    /// The contract all search algorithm implementations must adhere to
    /// </summary>
    public interface ISearchAlgorithm
    {
        int NumNodesGenerated { get; }
        int LengthOfSolutionPath { get; }
        double Pentrance { get; }
        TimeSpan ExecutionTime { get; }
        Func<List<int>, int> Heuristic { get; set; }
        LinkedList<Node> Search(Node initialNode);
    }
}
