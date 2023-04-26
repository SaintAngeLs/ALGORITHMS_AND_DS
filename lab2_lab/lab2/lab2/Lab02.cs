using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;



namespace Lab02
{
    public class PatternMatching : MarshalByRefObject
    {
        //int MAX_VAL = -20;
        /// <summary>
        /// Etap 1 - wyznaczenie trasy, zgodnie z którą robot przemieści się z pozycji poczatkowej (0,0) na pozycję docelową (-n-1, m-1)
        /// </summary>
        /// <param name="n">wysokość prostokąta</param>
        /// <param name="m">szerokość prostokąta</param>
        /// <param name="obstacles">tablica ze współrzędnymi przeszkód</param>
        /// <returns>krotka (bool result, string path) - result ma wartość true jeżeli trasa istnieje, false wpp., path to wynikowa trasa</returns>
        public (bool result, string path) Lab02Stage1(int n, int m, (int, int)[] obstacles)
        {
            // Create a 2D boolean array to represent the grid
            bool[,] grid = new bool[n, m];

            // Mark obstacles as true in the grid
            foreach ((int x, int y) in obstacles)
            {
                grid[x, y] = true;
            }

            // Create a 2D boolean array to store the dynamic programming table
            bool[,] T = new bool[n, m];

            // Fill in the first row and column of the T table
            T[0, 0] = !grid[0, 0];
            for (int i = 1; i < n; i++)
            {
                T[i, 0] = T[i - 1, 0] && !grid[i, 0];
            }
            for (int j = 1; j < m; j++)
            {
                T[0, j] = T[0, j - 1] && !grid[0, j];
            }

            // Fill in the rest of the T table
            for (int i = 1; i < n; i++)
            {
                for (int j = 1; j < m; j++)
                {
                    if (grid[i, j])
                    {
                        T[i, j] = false;
                    }
                    else
                    {
                        T[i, j] = T[i - 1, j] || T[i, j - 1];
                    }
                }
            }

            // If there is no path, return false
            if (!T[n - 1, m - 1])
            {
                return (false, "");
            }

            // Otherwise, construct the path
            StringBuilder pathBuilder = new StringBuilder();
            int row = n - 1;
            int col = m - 1;
            while (row != 0 || col != 0)
            {
                if (row == 0)
                {
                    pathBuilder.Append('R');
                    col--;
                }
                else if (col == 0)
                {
                    pathBuilder.Append('D');
                    row--;
                }
                else if (T[row - 1, col])
                {
                    pathBuilder.Append('D');
                    row--;
                }
                else
                {
                    pathBuilder.Append('R');
                    col--;
                }
            }
            char[] pathChars = pathBuilder.ToString().ToCharArray();
            Array.Reverse(pathChars);
            string path = new string(pathChars);

            return (true, path);
        }

        /// <summary>
        /// Etap 2 - wyznaczenie trasy realizującej zadany wzorzec, zgodnie z którą robot przemieści się z pozycji poczatkowej (0,0) na pozycję docelową (-n-1, m-1)
        /// </summary>
        /// <param name="n">wysokość prostokąta</param>
        /// <param name="m">szerokość prostokąta</param>
        /// <param name="pattern">zadany wzorzec</param>
        /// <param name="obstacles">tablica ze współrzędnymi przeszkód</param>
        /// <returns>krotka (bool result, string path) - result ma wartość true jeżeli trasa istnieje, false wpp., path to wynikowa trasa</returns>

        public (bool result, string path) Lab02Stage2(int n, int m, string pattern, (int, int)[] obstacles)
        {

            // Initialize the 3D T table

            bool[,,] T = new bool[n, m, pattern.Length + 1];

            // Initialize the set of obstacles
            var obstacleHashSet = new HashSet<(int, int)>(obstacles);

            // Setting the base cases
            T[0, 0, 0] = true;
            for (int i = 1; i < n; i++)
            {
                T[i, 0, 0] = false;
            }
            for (int j = 1; j < m; j++)
            {
                T[0, j, 0] = false;
            }

            // Set the T table values for each character in the pattern
            for (int k = 1; k <= pattern.Length; k++)
            {

                char c = pattern[k - 1];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        // Check if the current cell is an obstacle
                        bool isObstacle = obstacleHashSet.Contains((i, j));
                        
                       // If the current cell is an obstacle, set T[i, j, k] to false
                        if (isObstacle)
                        {
                            T[i, j, k] = false;
                        }
                        else
                        {
                            bool result = false;

                            // Check if the current character in the pattern is 'D'
                            if (c == 'D')
                            {
                                if (i > 0)
                                {
                                    result = T[i - 1, j, k - 1];
                                }
                            }
                            // Check if the current character in the pattern is 'R'
                            else if (c == 'R')
                            {
                                if (j > 0)
                                {
                                    result = T[i, j - 1, k - 1];
                                }
                            }
                            // Check if the current character in the pattern is '*'
                            else if (c == '*')
                            {
                                if (i > 0)
                                {
                                    result = T[i - 1, j, k] || result;
                                }
                                if (j > 0)
                                {
                                    result = T[i, j - 1, k] || result;
                                }
                                result = T[i, j, k - 1] || result;
                            }
                            // Check if the current character in the pattern is '?'
                            else if (c == '?')
                            {
                                if (i > 0)
                                {
                                    result = T[i - 1, j, k - 1] || result;
                                }
                                if (j > 0)
                                {
                                    result = T[i, j - 1, k - 1] || result;
                                }
                            }

                            // Set the value of T[i, j, k] based on the current character in the pattern
                            T[i, j, k] = result;
                        }
                    }
                }
            }

            // Return the result and path
            bool canReachTarget = T[n - 1, m - 1, pattern.Length];

            char[] pathChars = new char[m + n - 2];
            int idx = m + n - 3;
            if (canReachTarget)
            {
                int i = n - 1;
                int j = m - 1;
                int k = pattern.Length;

                while (i >= 0 && j >= 0 && k > 0)
                {
                    char c = pattern[k - 1];
                    bool isObstacle = obstacleHashSet.Contains((i, j));
                    //bool isObstacle = false;
                    //foreach ((int, int) obstacle in obstacles)
                    //{
                    //    if (obstacle.Item1 == i && obstacle.Item2 == j)
                    //    {
                    //        isObstacle = true;
                    //        break;
                    //    }
                    //}

                    if (c == 'R')
                    {
                        if (j > 0 && T[i, j - 1, k - 1] && !isObstacle)
                        {
                            pathChars[idx--] = 'R';
                            j--;
                            k--;
                        }
                    }
                    else if (c == 'D')
                    {
                        if (i > 0 && T[i - 1, j, k - 1] && !isObstacle)
                        {
                            pathChars[idx--] = 'D';
                            i--;
                            k--;
                        }
                    }
                    else if (c == '*')
                    {
                        bool found = false;
                        if (i > 0 && T[i - 1, j, k] && !isObstacle)
                        {
                            pathChars[idx--] = 'D';
                            i--;
                            found = true;
                        }
                        if (j > 0 && T[i, j - 1, k] && !isObstacle)
                        {
                            pathChars[idx--] = 'R';
                            j--;
                            found = true;
                        }
                        if (!found)
                        {
                            k--;
                        }
                    }
                    else if (c == '?')
                    {
                        bool found = false;
                        if (i > 0 && T[i - 1, j, k - 1] && !isObstacle)
                        {
                            pathChars[idx--] = 'D';
                            i--;
                            found = true;
                        }
                        if (!found && j > 0 && T[i, j - 1, k - 1] && !isObstacle)
                        {
                            pathChars[idx--] = 'R';
                            j--;
                            found = true;
                        }
                        k--;
                    }
                }

                // Reverse the char array to get the correct order of characters
                // Array.Reverse(pathChars, 0, idx);
                // No reversing decreasing the time;
                string path = new string(pathChars);
                return (true, path);
            }
            else
            {
                return (false, "");
            }
        }
        /// <summary>
        /// Function to figure out if the place in the table defidned by the coordenate i and j is and obstacle.
        /// </summary>
        /// <param name="obstacles"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns>Bool</returns>
        private static bool IsObstacle((int, int)[] obstacles, int i, int j)
        {
            HashSet<(int, int)> obstacleHashSet = new HashSet<(int, int)>(obstacles);
            return obstacleHashSet.Contains((i, j));
        }
    }
}