using System;
using SolveurGenetic;
using SolveurORTools;
using SolveurLiensDansants;
using Noyau;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO ;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {

            //Menu numéro 1 de choix de la grille à jouer

            Console.WriteLine("\n\n\n TP1 - Résolution de Sudoku par différentes méthodes\n");
            Console.WriteLine("1. Grille Initiale");
            Console.WriteLine("2. Grille Easy");
            Console.WriteLine("3. Grille Hardest");
            Console.WriteLine("4. Grille Top 95");
            Console.WriteLine("5. Quitter la résolution de Sudoku");

            Console.WriteLine("\n=> Entrez votre choix: ");
            int choix;

            //Test de conformité du choix
            try

            {

                choix = int.Parse(Console.ReadLine());

            }

            catch (Exception e)

            {

                choix = -1;

                Console.WriteLine("\n\nSaisie invalide\n\n");

            }

            //Initialisation du detecteur de grille
            int numSudo;


            switch (choix)
            {
                case 1:
                    Console.WriteLine("Grille Initiale");
                    //Le detecteur prend un chiffre de 1 à 4 en fonction de la grille choisie
                    numSudo = 1;
                    //L'initialisation du Sudoku se lance en prenant en argument ce detecteur
                    Sudoku(numSudo);
                    break;
                case 2:
                    Console.WriteLine("Grille Easy");
                    numSudo = 2;
                    Sudoku(numSudo);
                    break;
                case 3:
                    Console.WriteLine("Grille Hardest");
                    numSudo = 3;
                    Sudoku(numSudo);
                    break;
                case 4:
                    Console.WriteLine("Grille Top 95");
                    numSudo = 4;
                    Sudoku(numSudo);
                    break;
                case 5:
                    Console.WriteLine("Vous avez choisi de quitter le programme.");
                    Console.ReadLine();


                    break;
            }

            //Initialisation du Sudoku
            void Sudoku(int n)
            {

                //Initialisation des variables
                String text;
                int i, j;
                i = 0;
                j = 0;
                int k = 0;

                //Création de l'objet Sudoku en fonction du detecteur
                if (n == 1)
                {
                    //Création d'une liste de 0
                    int[] grid = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0,

                                             0, 0, 0, 0, 0, 0, 0, 0, 0,

                                             0, 0, 0, 0, 0, 0, 0, 0, 0,

                                             0, 0, 0, 0, 0, 0, 0, 0, 0,

                                             0, 0, 0, 0, 0, 0, 0, 0, 0,

                                             0, 0, 0, 0, 0, 0, 0, 0, 0,

                                             0, 0, 0, 0, 0, 0, 0, 0, 0,

                                             0, 0, 0, 0, 0, 0, 0, 0, 0,

                                             0, 0, 0, 0, 0, 0, 0, 0, 0 };

                    Noyau.Sudoku recup = new Noyau.Sudoku(grid);
                    Console.WriteLine(recup.ToString());
                    //Envoi de l'objet dans le solveur
                    Sudoku_Solver(1, recup);

                }

                if (n == 2){
                    Noyau.Sudoku recup = new Sudoku();
                    text = getLine("Sudoku_Easy50.txt", -1);
                    char[] b = new char[text.Length];
                    using (StringReader sr = new StringReader(text))
                    {
                        while (k != 81)
                        {
                            if (j == 9)
                            {
                                i++;
                                j = 0;
                            }
                            sr.Read(b, 0, 1);
                            recup.SetCell(i, j, Int32.Parse(b));

                            j++;
                            k++;

                        }
                    }
                    Console.WriteLine(recup.ToString());
                    Sudoku_Solver(2, recup);
                }

                if (n == 3)
                {
                    Noyau.Sudoku recup = new Sudoku();
                    text = getLine("Sudoku_hardest.txt", -1);
                    char[] b = new char[text.Length];
                    using (StringReader sr = new StringReader(text))
                    {
                        while (k != 81)
                        {
                            if (j == 9)
                            {
                                i++;
                                j = 0;
                            }
                            sr.Read(b, 0, 1);
                            recup.SetCell(i, j, Int32.Parse(b));

                            j++;
                            k++;

                        }
                    }
                    Sudoku_Solver(3, recup);
                }

                if (n == 4)
                {
                    Noyau.Sudoku recup = new Sudoku();
                    text = getLine("Sudoku_top95.txt", -1);
                    char[] b = new char[text.Length];
                    using (StringReader sr = new StringReader(text))
                    {
                        while (k != 81)
                        {
                            if (j == 9)
                            {
                                i++;
                                j = 0;
                            }
                            sr.Read(b, 0, 1);
                            recup.SetCell(i, j, Int32.Parse(b));

                            j++;
                            k++;

                        }
                    }
                    Sudoku_Solver(4, recup);
                }
                

                

            }


            void Sudoku_Solver(int n, Sudoku s)
            {
                //Menu numéro 2 du choix de la méthode de resolution
                Console.WriteLine("\nChoisissez parmis les différentes méthodes à disposition: \n\n");
                Console.WriteLine(" 1. Algorithme génétique");
                Console.WriteLine(" 2. SMT OR-Tools");
                Console.WriteLine(" 3. CSP ");
                Console.WriteLine(" 4. SMT Z3 ");
                Console.WriteLine(" 5. Liens dansants  ");
                Console.WriteLine(" 6. Norvig ");
                Console.WriteLine(" 7. Réseaux de neurones ");
                Console.WriteLine("\n\n\n");

                //Declaration du chronometre
                Stopwatch stopwatch = new Stopwatch();
                //Declaration de la fitness
                var fitness = new SudokuFitness(s);

                int solution;

                try

                {

                    solution = int.Parse(Console.ReadLine());

                }

                catch (Exception e)

                {

                    solution = -1;

                    Console.WriteLine("\n\n                Saisie invalide\n\n");

                }

                Console.WriteLine("******************************************************");
                Console.WriteLine("\n");

                Console.WriteLine("   Grille choisie :");
                Console.WriteLine("\n");
                //Print de la grille choisie
                Console.WriteLine(s.ToString());

                Console.WriteLine("\n");
                Console.WriteLine("******************************************************");


                switch (solution)
                {
                    case 1:
                        Console.WriteLine("Résolution du Sudoku par Algorithme génétique");

                        SolverGeneticSharp sgs = new SolverGeneticSharp();

                        //Demarrage du chronometre
                        stopwatch.Start();

                        s = sgs.ResoudreSudoku(s);

                        //Arrete du chronometre
                        stopwatch.Stop();

                        Console.WriteLine(s.ToString());
                        //Evaluation de la fitness : si fitness = 0 alors le Sudoku est résolu parfaitement
                        Console.WriteLine("Fitness : ");
                        Console.WriteLine(fitness.Evaluate(s));

                        //Temps d'execution
                        Console.WriteLine("Durée d'exécution: {0} secondes", stopwatch.Elapsed.TotalSeconds);
                        stopwatch.Reset();

                        Console.WriteLine("\n");
                        Console.WriteLine("******************************************************");
                        Console.WriteLine("\n");

                        Console.ReadLine();
                        break;

                    case 2:
                        Console.WriteLine("\n");

                        Console.WriteLine("Résolution du Sudoku par SMT ORTools");

                        Solveur_tools ots = new Solveur_tools();

                        stopwatch.Start();

                        s = ots.ResoudreSudoku(s);

                        stopwatch.Stop();

                        Console.WriteLine(s.ToString());
                        Console.WriteLine("Fitness : ");
                        Console.WriteLine(fitness.Evaluate(s));

                        Console.WriteLine("Durée d'exécution: {0} secondes", stopwatch.Elapsed.TotalSeconds);
                        stopwatch.Reset();

                        Console.WriteLine("\n");
                        Console.WriteLine("******************************************************");
                        Console.WriteLine("\n");

                        Console.ReadLine();
                        break;

                    case 3:
                        Console.WriteLine("Résolution du Sudoku par CSP ");

                        //CSP csp = new CSP();

                        stopwatch.Start();

                        //s = csp.ResoudreSudoku(s);

                        stopwatch.Stop();

                        Console.WriteLine(s.ToString());
                        Console.WriteLine("Fitness : ");
                        Console.WriteLine(fitness.Evaluate(s));

                        Console.WriteLine("Durée d'exécution: {0} secondes", stopwatch.Elapsed.TotalSeconds);
                        stopwatch.Reset();

                        Console.WriteLine("\n");
                        Console.WriteLine("******************************************************");
                        Console.WriteLine("\n");

                        Console.ReadLine();
                        break;

                    case 4:
                        Console.WriteLine("Résolution du Sudoku par SMT Z3 ");

                        //SMT smt = new SMT();

                        stopwatch.Start();

                        //s = smt.ResoudreSudoku(s);

                        stopwatch.Stop();

                        Console.WriteLine(s.ToString());
                        Console.WriteLine("Fitness : ");
                        Console.WriteLine(fitness.Evaluate(s));

                        Console.WriteLine("Durée d'exécution: {0} secondes", stopwatch.Elapsed.TotalSeconds);
                        stopwatch.Reset();

                        Console.WriteLine("\n");
                        Console.WriteLine("******************************************************");
                        Console.WriteLine("\n");

                        Console.ReadLine();
                        break;

                    case 5:
                        Console.WriteLine("Résolution du Sudoku par LiensDansants  ");

                        SolveurLD dancing = new SolveurLD();

                        stopwatch.Start();

                        s = dancing.ResoudreSudoku(s);

                        stopwatch.Stop();

                        Console.WriteLine(s.ToString());
                        Console.WriteLine("Fitness : ");
                        Console.WriteLine(fitness.Evaluate(s));

                        Console.WriteLine("Durée d'exécution: {0} secondes", stopwatch.Elapsed.TotalSeconds);
                        stopwatch.Reset();

                        Console.WriteLine("\n");
                        Console.WriteLine("******************************************************");
                        Console.WriteLine("\n");

                        Console.ReadLine();
                        break;


                    case 6:
                        Console.WriteLine("Résolution du Sudoku par Norvig ");

                        //Norvig norvig = new Norvig();

                        stopwatch.Start();

                        //s = norvig.ResoudreSudoku(s);

                        stopwatch.Stop();

                        Console.WriteLine(s.ToString());
                        Console.WriteLine("Fitness : ");
                        Console.WriteLine(fitness.Evaluate(s));

                        Console.WriteLine("Durée d'exécution: {0} secondes", stopwatch.Elapsed.TotalSeconds);
                        stopwatch.Reset();

                        Console.WriteLine("\n");
                        Console.WriteLine("******************************************************");
                        Console.WriteLine("\n");

                        Console.ReadLine();
                        break;


                    case 7:
                        Console.WriteLine(" Résolution du Sudoku par réseau de neurones convolués ");

                        //Neurones neur = new Neurones();

                        stopwatch.Start();

                        //s = neur.ResoudreSudoku(s);

                        stopwatch.Stop();

                        Console.WriteLine(s.ToString());
                        Console.WriteLine("Fitness : ");
                        Console.WriteLine(fitness.Evaluate(s));

                        Console.WriteLine("Durée d'exécution: {0} secondes", stopwatch.Elapsed.TotalSeconds);
                        stopwatch.Reset();

                        Console.WriteLine("\n");
                        Console.WriteLine("******************************************************");
                        Console.WriteLine("\n");

                        Console.ReadLine();
                        break;
                }

            }

            

            string getLine(string fileName, int index) //Récupère un String Sudoku d'un fichier
            {
                String[] lines = getFile(fileName);

                if (index < 0 || index >= lines.Length)
                {
                    Random rnd = new Random();
                    index = rnd.Next(lines.Length);
                }
                return lines[index];
            }


            string[] getFile(string fileName)  //Récupère tout les Sudokus d'un fichier et les stocks dans une liste
            {
                DirectoryInfo myDirectory = new DirectoryInfo(Environment.CurrentDirectory);
                String path = Path.Combine(myDirectory.Parent.Parent.Parent.FullName, fileName);
                String[] lines = File.ReadAllLines(path);
                return lines;
            }

        }
    }
}