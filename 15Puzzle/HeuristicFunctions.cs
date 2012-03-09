using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _15Puzzle
{
    public class HeuristicFunctions
    {


        /// <param name="state">A list representation of the state of the board</param>
        /// <returns>H value equal to how many tiles were out of their place</returns>
        /// <example>
        /// The following state would return 13
        /// 1   2   []  3
        /// 6   7   11  4
        /// 5   9   12  8
        /// 13  10  14  15
        /// </example>
        public static int number_of_tiles_out_of_place (List<int> state)
        {
            var count = 0;
            var expectedValue = 1;
            
            foreach(var value in state)
            {
                if (value != expectedValue)
                    count++;
                
                expectedValue = expectedValue == state.Count -1 ?  Node.SPACE : expectedValue + 1;
            }
            
            return count;
        }

        /// <param name="state">A list representation of the state of the board</param>
        /// <returns>H value equal to sum distance each tile is away from its position taken by |x1-x2| + |y1-y2|</returns>
        /// <example>
        /// The following state would return 4
        /// 1   2   3   4
        /// 5   6   7   []
        /// 9   10  11  8
        /// 13  14  15  12
        /// </example>
        public static int sum_distance_of_tiles_away_from_their_place(List<int> state)
        {
            var sum = 0;
            var length = (int) Math.Sqrt(state.Count);
            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < length; j++)
                {
                    var current = state[i*length + j];
                    current = (current == Node.SPACE) ? 15 : current - 1;
                    int k = current/length;
                    int l = current%length;
                    sum += (Math.Abs(i - k) + Math.Abs(j - l));
                }
            }
            return sum;
        }
    }
}
