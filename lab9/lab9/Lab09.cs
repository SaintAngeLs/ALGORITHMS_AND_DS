
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ASD.Graphs;

/// <summary>
/// Klasa rozszerzająca klasę Graph o rozwiązania problemów największej kliki i izomorfizmu grafów metodą pełnego przeglądu (backtracking)
/// </summary>
public static class Lab10GraphExtender
{
    /// <summary>
    /// Wyznacza największą klikę w grafie i jej rozmiar metodą pełnego przeglądu (backtracking)
    /// </summary>
    /// <param name="g">Badany graf</param>
    /// <param name="clique">Wierzchołki znalezionej największej kliki - parametr wyjściowy</param>
    /// <returns>Rozmiar największej kliki</returns>
    /// <remarks>
    /// Nie wolno modyfikować badanego grafu.
    /// </remarks>
    /// 
    public static int MaxClique(this Graph g, out int[] clique)
    {
        int[] currentClique = new int[g.VertexCount];

        int[] maxClique = new int[g.VertexCount];

        int maxSize = 0;

        MaxCliqueRec(g, 0, currentClique, 0, ref maxClique, ref maxSize);

        clique = new int[maxSize];

        Array.Copy(maxClique, clique, maxSize);

        return maxSize;
    }

    private static void MaxCliqueRec(Graph g, int vertex, int[] currentClique, int currentSize, ref int[] maxClique, ref int maxSize)
    {
        if (vertex == g.VertexCount)
        {
            if (currentSize > maxSize)
            {

                maxSize = currentSize;

                maxClique = (int[])currentClique.Clone();

            }
            return;
        }

        bool canAdd = true;

        foreach (int v in currentClique.Take(currentSize))
        {
            if (!g.HasEdge(vertex, v))
            {

                canAdd = false;

                break;

            }
        }

        if (canAdd)
        {

            currentClique[currentSize] = vertex;

            MaxCliqueRec(g, vertex + 1, currentClique, currentSize + 1, ref maxClique, ref maxSize);

        }

        MaxCliqueRec(g, vertex + 1, currentClique, currentSize, ref maxClique, ref maxSize);
    }

    /// <summary>
    /// Bada izomorfizm grafów metodą pełnego przeglądu (backtracking)
    /// </summary>
    /// <param name="g">Pierwszy badany graf</param>
    /// <param name="h">Drugi badany graf</param>
    /// <param name="map">Mapowanie wierzchołków grafu h na wierzchołki grafu g (jeśli grafy nie są izomorficzne to null) - parametr wyjściowy</param>
    /// <returns>Informacja, czy grafy g i h są izomorficzne</returns>
    /// <remarks>
    /// 1) Uwzględniamy wagi krawędzi
    /// 3) Nie wolno modyfikować badanych grafów.
    /// </remarks>
    /// 
    public static bool IsomorphismTest(this Graph<int> g, Graph<int> h, out int[] map)
    {
        map = null;

        // przypadek gdy g ma mniej wierzchołków niżgraf h: nie mogą byś te grafy izomorficzne

        if (g.VertexCount < h.VertexCount)

            return false;

        // wierzchołki zuzyte

        var used = new bool[g.VertexCount];

        int vc = 0;

        map = new int[g.VertexCount];


        // Jeśli znaleziono mapowanie, zwróć true, w przeciwnym razie zwróć false i ustaw mapowanie na null
        if (FindMapping(g, h, used, vc, ref map))

            return true;

        map = null;

        return false;
    }
    /// <summary>
    /// Rekurencyjna metoda, która próbuje znaleźć mapowanie wierzchołków grafów g i h
    /// </summary>
    /// <param name="g">Pierwszy badany graf</param>
    /// <param name="h">Drugi badany graf</param>
    /// <param name="used">Tablica oznaczająca, które wierzchołki grafu g zostały już użyte</param>
    /// <param name="vc">Liczba aktualnie mapowanych wierzchołków</param>
    /// <param name="iso">Referencja do tablicy mapowania</param>
    /// <returns>Informacja, czy znaleziono poprawne mapowanie wierzchołków grafów g i h</returns>
    /// 
    public static bool FindMapping(Graph<int> g, Graph<int> h, bool[] used, int vc, ref int[] iso)
    {
        // Jeśli liczba mapowanych wierzchołków równa się liczbie wierzchołków w grafie g, zwróć true


        if (vc == g.VertexCount)
            return true;

        // Przeszukaj wszystkie wierzchołki grafu g

        for (int i = 0; i < g.VertexCount; i++)
        {

            int j = 0;

            // Jeśli wierzchołek nie został użyty i liczba sąsiadów w grafie g i h jest równa, sprawdź mapowaniedla tego wierzchołka

            if (!used[i] && g.OutNeighbors(i).Count() == h.OutNeighbors(vc).Count())
            {
                // Sprawdź, czy krawędzie między zmapowanymi wierzchołkami są izomorficzne

                for (j = 0; j < vc; j++)
                {
                    // Jeśli krawędzie nie są izomorficzne, przerwij pętlę

                    if (g.HasEdge(iso[j], i) != h.HasEdge(j, vc) || g.HasEdge(iso[j], i) && g.GetEdgeWeight(iso[j], i) != h.GetEdgeWeight(j, vc))
                        break;
                }

                // Jeśli krawędzie izomorficzne nie są, kontynuuj przeszukiwanie wierzchołków

                if (j < vc)
                    continue;

                // Jeśli krawędzie są izomorficzne, zaktualizuj mapowanie i tablicę użytych wierzchołków

                iso[vc] = i;

                used[i] = true;

                // Wywołaj rekurencyjnie metodę FindMapping dla kolejnego wierzchołka

                if (FindMapping(g, h, used, vc + 1, ref iso))
                    return true;

                // Jeśli rekurencyjne wywołanie nie znalazło mapowania, cofnij zmiany w mapowaniu i tablicy użytych wierzchołków

                used[i] = false; 
            }   

        }

        // Jeśli nie znaleziono mapowania dla żadnego wierzchołka, zwróć false

        return false;
        
    }

}

