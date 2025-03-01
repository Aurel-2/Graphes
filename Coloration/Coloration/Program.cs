using System;

class Program
{
     const int n = 15; // Nombre de sommets
     const int p = 40; // Pourcentage
     static readonly int[,] matrice_adjacence = new int[n, n]; // Matrice d'adjacence
     static readonly int[] couleur1 = new int[n]; // Tableau des couleurs
     static bool trouvee = false; // Indique si une coloration a été trouvée

     // Vérifie si la couleur c convient pour le sommet x
     static bool Convient(int x, int c)
     {
          for (int i = 0; i < x; i++)
          {
               if (matrice_adjacence[x, i] != 0 && couleur1[i] == c)
               {
                    return false;
               }
          }
          return true;
     }

     // Fonction récursive pour la coloration
     static void ColorRR(int x, int k)
     {
          if (x == n)
          {
               Console.WriteLine($"Coloration en {k} couleurs trouvée :");
               for (int i = 0; i < n; i++)
               {
                    Console.WriteLine($"Sommet {i} : Couleur {couleur1[i]}");
               }
               trouvee = true;
          }
          else
          {
               for (int c = 1; c <= k; c++)
               {
                    if (Convient(x, c))
                    {
                         couleur1[x] = c;
                         ColorRR(x + 1, k);
                         if (trouvee)
                         {
                              return;
                         }
                    }
               }
          }
     }

     // Fonction pour trouver une coloration exacte avec k couleurs
     static void CouleurExacte(int k)
     {
          for (int i = 0; i < n; i++)
          {
               couleur1[i] = 0;
          }
          trouvee = false;
          ColorRR(0, k);
          if (!trouvee)
          {
               Console.WriteLine($"Pas de coloration en {k} couleurs.");
          }
     }
     
     static int NbChromatique()
     {
          int k = 0;
          while (true)
          {
               trouvee = false;
               CouleurExacte(k);
               if (trouvee)
               {
                    return k;
               }
               k++;
          }
     }



     // Génère un graphe aléatoire avec n sommets et une probabilité p d'arête
     static void Generegraphe(int n, int p)
     {
          Random rand = new Random(DateTime.Now.Millisecond);

          for (int i = 0; i < n; i++)
          {
               for (int j = i + 1; j < n; j++)
               {
                    if (rand.Next(100) < p)
                    {
                         matrice_adjacence[i, j] = 1;
                         matrice_adjacence[j, i] = 1;
                    }
               }
          }
     }

     static void Affichergraphe(int n)
     {
          Console.WriteLine("Graphe généré :");
          for (int i = 0; i < n; i++)
          {
               Console.WriteLine($"Sommet {i} : ");
               Console.Write("Voisins : ");

               for (int j = 0; j < n; j++)
               {
                    if (matrice_adjacence[i, j] == 1)
                    {
                         Console.Write($"{j} ");
                    }
               }
               Console.WriteLine();
               Console.WriteLine();

          }
     }


     // Programme principal
     static void Main(string[] args)
     {
          Generegraphe(n, p);
          Affichergraphe(n);

          int chromatique = NbChromatique();
          Console.WriteLine($"Le nombre chromatique du graphe est : {chromatique}");
     }
}