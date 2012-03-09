using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace _15Puzzle.Tests
{
    /// <summary>
    /// Helper methods used in the unit tests
    /// </summary>
    public class HelperMethods
    {
        /// <summary>
        /// Verifies that the solution path, from initial node to goal node is valid
        /// </summary>
        /// <param name="solutionPath">The sequence of nodes from initial node to goal node</param>
        /// <returns>True if solution path is valid</returns>
        public static bool SolutionPathLeadsToGoal(LinkedList<Node> solutionPath)
        {
            if (solutionPath == null) throw new ArgumentNullException("solutionPath");

            var prevIdxOfSpace = 0;
            var length = (int) Math.Sqrt(solutionPath.First.Value.State.Count);

            // Check that next move is legal
            foreach (var node in solutionPath) 
            {
                var currIdxOfSpace = node.State.TakeWhile(value => value != Node.SPACE).Count();

                if (prevIdxOfSpace != 0) 
                    if (!BestFirst.IsMovePossible(prevIdxOfSpace, currIdxOfSpace,length))
                        return false;

                prevIdxOfSpace = currIdxOfSpace;
            }

            return solutionPath.Last.Value.FValue == 0; // Check whether goal state is reached
            
        }

        /// <summary>
        /// Outputs the results of a sample run
        /// </summary>
        public static  void OutputResults (LinkedList<Node> solutionPath, ISearchAlgorithm searchAlgorithm, List<int> inputState)
        {
            var stackTrace = new StackTrace();
            Console.WriteLine("Test name: {0}", stackTrace.GetFrame(1).GetMethod().Name);
            Console.Write("Input state: ");
            inputState.ForEach(e => Console.Write(e + " "));
            Console.WriteLine();
            Console.WriteLine("Number of nodes generated: {0}", searchAlgorithm.NumNodesGenerated);
            Console.WriteLine("Length of solution path: {0}", searchAlgorithm.LengthOfSolutionPath);
            Console.WriteLine("Execution time: {0:0} ms", searchAlgorithm.ExecutionTime.TotalMilliseconds);
            Console.WriteLine("Penetrance: {0:0.0000}", searchAlgorithm.Pentrance);
            PrintSolutionPath1(solutionPath);
            PrintSolutionPath2(solutionPath);

        }

        /// <summary>
        /// Prints the solution path as a sequence of operators (Up, Down, Left, Right)
        /// </summary>
        /// <param name="solutionPath"></param>
        public static void PrintSolutionPath1 (LinkedList<Node> solutionPath)
        {
            int prevIdxOfSpace = 0;
            var length = (int)Math.Sqrt(solutionPath.First.Value.State.Count);

            Console.Write("Solution Path: ");
            foreach (var node in solutionPath)
            {
                var currIdxOfSpace = node.State.TakeWhile(value => value != Node.SPACE).Count();

                if (prevIdxOfSpace != 0) 
                    if (currIdxOfSpace == prevIdxOfSpace - 1)
                        Console.Write("Left ");
                    else if (currIdxOfSpace == prevIdxOfSpace + 1)
                        Console.Write("Right ");
                    else if (currIdxOfSpace == prevIdxOfSpace - length)
                        Console.Write("Top ");
                    else if (currIdxOfSpace == prevIdxOfSpace + length)
                        Console.Write("Down ");

                prevIdxOfSpace = currIdxOfSpace;

            }
            Console.WriteLine("\n");
        }

        /// <summary>
        /// Prints the solution path as board configurations
        /// </summary>
        public static void PrintSolutionPath2 (LinkedList<Node> solutionPath)
        {
            var sideLength = (int)Math.Sqrt(solutionPath.First.Value.State.Count);
            Console.WriteLine("Board configurations: \n");
            
            foreach (var node in solutionPath)
            {
                var counter = 1;
                foreach (var value in node.State)
                {
                    Console.Write("{0,5}", value);
                    
                    if (counter == sideLength)
                    {
                        Console.WriteLine();
                        counter = 0;
                    }
                    counter++;
                }

                Console.WriteLine("");
            }
        }
    }
}