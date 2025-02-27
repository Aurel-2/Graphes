using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    const int n = 4000; // Le nombre de points.
    const int dmax = 20; // La distance jusqu'à laquelle on relie deux points.
    static List<int>[] voisin = new List<int>[n]; // Les listes de voisins.
    static int[,] point = new int[n, 2]; // Les coordonnées des points.
    static int[,] arbre = new int[n - 1, 2]; // Les arêtes de l'arbre de Dijkstra.
    static int[] pere = new int[n]; // La relation de filiation de l'arbre de Dijkstra.

    static double DistanceEuclidienne(double x1, double y1, double x2, double y2)
    {
        return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
    }

    static double DistanceManhattan(double x1, double y1, double x2, double y2)
    {
        return Math.Abs(x2 - x1) + Math.Abs(y2 - y1);
    }

    static void AfficheGraphe(int n, int d, int[,] sommet)
    {
        using (StreamWriter output = new StreamWriter("Graphe.ps"))
        {
            output.WriteLine("%!PS-Adobe-3.0");
            output.WriteLine();
            for (int i = 0; i < n; i++)
            {
                output.WriteLine($"{sommet[i, 0]} {sommet[i, 1]} 1 0 360 arc");
                output.WriteLine("0 setgray");
                output.WriteLine("fill");
                output.WriteLine("stroke");
                output.WriteLine();
            }
            output.WriteLine();
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if ((sommet[i, 0] - sommet[j, 0]) * (sommet[i, 0] - sommet[j, 0]) +
                        (sommet[i, 1] - sommet[j, 1]) * (sommet[i, 1] - sommet[j, 1]) <
                        d * d)
                    {
                        output.WriteLine($"{sommet[i, 0]} {sommet[i, 1]} moveto");
                        output.WriteLine($"{sommet[j, 0]} {sommet[j, 1]} lineto");
                        output.WriteLine("stroke");
                        output.WriteLine();
                    }
                }
            }
        }
    }

    static void AfficheArbre(int n, int k, int[,] point, int[,] arbre)
    {
        using (StreamWriter output = new StreamWriter("Arbre.ps"))
        {
            output.WriteLine("%!PS-Adobe-3.0");
            output.WriteLine("%%BoundingBox: 0 0 612 792");
            output.WriteLine();
            for (int i = 0; i < n; i++)
            {
                output.WriteLine($"{point[i, 0]} {point[i, 1]} 1 0 360 arc");
                output.WriteLine("0 setgray");
                output.WriteLine("fill");
                output.WriteLine("stroke");
                output.WriteLine();
            }
            output.WriteLine();
            for (int i = 0; i < k; i++)
            {
                output.WriteLine($"{point[arbre[i, 0], 0]} {point[arbre[i, 0], 1]} moveto");
                output.WriteLine($"{point[arbre[i, 1], 0]} {point[arbre[i, 1], 1]} lineto");
                output.WriteLine("stroke");
                output.WriteLine();
            }
        }
    }

    static void GenereGraphe(int n, int[,] point)
    {
        Random rand = new Random(DateTime.Now.Millisecond);
        for (int i = 0; i < n; i++)
        {
            point[i, 0] = rand.Next(611);
            point[i, 1] = rand.Next(791);
        }
    }

    static void Voisins(int n, int[,] point, int dmax)
    {
        for (int i = 0; i < n; i++)
        {
            voisin[i] = new List<int>();
            for (int j = 0; j < n; j++)
            {
                if (i != j)
                {
                    double distance = DistanceManhattan(point[i, 0], point[i, 1], point[j, 0], point[j, 1]);
                    if (distance <= dmax)
                    {
                        voisin[i].Add(j);
                    }
                }
            }
        }
    }

    static void Dijkstra(int n, int[,] point, List<int>[] voisin, int src)
    {
        double[] dist = new double[n];
        int[] prev = new int[n];
        bool[] visited = new bool[n];

        for (int i = 0; i < n; i++)
        {
            dist[i] = double.PositiveInfinity;
            prev[i] = -1;
            visited[i] = false;
        }

        dist[src] = 0;

        for (int count = 0; count < n - 1; count++)
        {
            int u = -1;
            for (int i = 0; i < n; i++)
            {
                if (!visited[i] && (u == -1 || dist[i] < dist[u]))
                {
                    u = i;
                }
            }

            visited[u] = true;

            foreach (int v in voisin[u])
            {
                if (!visited[v])
                {
                    double weight = DistanceManhattan(point[u, 0], point[u, 1], point[v, 0], point[v, 1]);
                    if (dist[u] + weight < dist[v])
                    {
                        dist[v] = dist[u] + weight;
                        prev[v] = u;
                    }
                }
            }
        }

        for (int i = 0; i < n; i++)
        {
            pere[i] = prev[i];
        }

        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"Distance minimale de {src} a {i} : {dist[i]}, Predecesseur : {pere[i]}");
        }
    }

    static int ConstruireArbre(int n)
    {
        int k = 0;
        for (int i = 1; i < n; i++)
        {
            if (pere[i] != -1)
            {
                arbre[k, 0] = pere[i];
                arbre[k, 1] = i;
                k++;
            }
        }
        return k;
    }

    static void Main(string[] args)
    {
        int[,] points = new int[n, 2];
        GenereGraphe(n, points);
        Voisins(n, points, dmax);
        for (int i = 0; i < n; i++)
        {
            Console.Write($"Voisins de {i}: ");
            foreach (int j in voisin[i])
            {
                Console.Write($"{j} ");
            }
            Console.WriteLine();
        }

        AfficheGraphe(n, dmax, points);
        Console.WriteLine("Graphe dans le fichier Graphe.ps");
        int source = 0;
        Dijkstra(n, points, voisin, source);
        int k = ConstruireArbre(n);
        AfficheArbre(n, k, points, arbre);
        Console.WriteLine("Arbre dans le fichier Arbre.ps");
    }
}