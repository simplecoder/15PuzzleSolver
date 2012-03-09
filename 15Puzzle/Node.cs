using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _15Puzzle
{
    /// <summary>
    /// Represents a node in the tree that is to be traversed
    /// </summary>
    public class Node
    {
        public const int SPACE = 0;
        public Node BackPtr;
        public int FValue;  
        
        /// <summary>
        /// 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 []
        /// represents the following puzzle -
        /// 1   2   3   4
        /// 5   6   7   8
        /// 9   10  11  12
        /// 13  14  15  []
        /// </summary>
        public List<int> State;   

    }
}
