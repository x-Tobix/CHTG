namespace CHTG.Utilities
{
    public static class IntArrayExtensions
    {
        public static void AddMod(this int[] graph, int mod)
        {
            graph[^1] += 1;
            for (int i = graph.Length - 1; i > 0; i--)
            {
                if (graph[i] == mod + 1)
                {
                    graph[i] = 1;
                    graph[i - 1]++;
                }
            }
        }
    }
}
