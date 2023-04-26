 /// <summary>
        /// Etap 1 - wyznaczanie numerów grup, które jest w stanie odwiedzić Karol, zapisując się na początku do podanej grupy
        /// </summary>
        /// <param name="graph">Ważony graf skierowany przedstawiający zasady dołączania do grup</param>
        /// <param name="start">Numer grupy, do której początkowo zapisuje się Karol</param>
        /// <returns>Tablica numerów grup, które może odwiedzić Karol, uporządkowana rosnąco</returns>
        
 public int[] Lab04Stage1(DiGraph<int> graph, int start)
        {
            //// TODO
            
            // Create a set to store the groups that can be reached by Karol
            HashSet<int> reachableGroups = new HashSet<int>();
            // Add the starting group to the set
            reachableGroups.Add(start);
            // Create a queue to perform a breadth-first search of the graph
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(start);
            // Perform the breadth-first search
            while (queue.Count > 0)
            {
                int currentGroup = queue.Dequeue();
                // Iterate over the outgoing edges of the current group
                foreach (var edge in graph.OutEdges(currentGroup))
                {
                    int targetGroup = edge.To;
                    int weight = edge.Weight;
                    if (weight == -1 && !reachableGroups.Contains(targetGroup))
                    {
                        // Apply rule 1: join the target group if it is allowed
                        reachableGroups.Add(targetGroup);
                        queue.Enqueue(targetGroup);
                    }
                    else if (weight >= 0 && reachableGroups.Contains(weight) && !reachableGroups.Contains(targetGroup))
                    {
                        // Apply rule 2: join the target group if it is allowed
                        reachableGroups.Add(targetGroup);
                        queue.Enqueue(targetGroup);
                    }
                    Console.WriteLine();
                    Console.Write($"{edge.From} --> {edge.To}");
                    Console.WriteLine();
                }
            }
            // Convert the set to an array and sort it
            int[] result = reachableGroups.ToArray();
            Array.Sort(result);
            return result;


        }