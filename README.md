# Sudoku
Les règles du sudoku sont très simples. Un sudoku classique contient neuf lignes et neuf colonnes, donc 81 cases au total.

Le but du jeu est de remplir ces cases avec des chiffres allant de 1 à 9 en veillant toujours à ce qu'un même chiffre ne figure qu'une seule fois par colonne, une seule fois par ligne, et une seule fois par carré de neuf cases.
Une grille initiale de sudoku correctement constituée ne peut aboutir qu'à une et une seule solution. Pour trouver les chiffres manquants, tout est une question de logique et d'observation ( ou de code ;) ).

Pour ce premier TP le travail consiste donc résoudre avec différentes formes d'intelligence artificille un sudoku, la classe est séparée en 7 groupes, soit 6 groupes utilisant 6 méthodes de résolution différentes et un groupe arbitre servant à reçeptionner et organiser le code des autres groupes dans un github commun et réalisant un Benchmark en C# sous visual studio.

Le premier objectif est de créer une application permettant de charger et afficher les Sudokus à résoudre.
Créez une application de Console .Net Core.
Créez une classe Sudoku permettant la prise en charge de l’état d’un Sudoku à résoudre ou en cours de résolution.

Une fois les solveurs correctement traduit en C# ou python et les codes remontés jusqu'à la branche principale le code est affiché via un  benchmark, pour cela, la librairie BenchmarkDotNet est utilisée.
![BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet/blob/master/docs/logo/logo-wide.png)
Ainsi chaque solveur de Sudoku devait être capable de résoudre 3 Sudoku à difficulté variable (sauf pour la résolution par réseau de neurones convolués qui possède une large base de Sudoku pour s'entraîner).
Par exemple avant résolution, un Sodoku (ici le facile) se présentait de la sorte :

![SudokuEasyUnsolved](https://scontent-cdg2-1.xx.fbcdn.net/v/t1.15752-9/83292730_183000729428676_336940759896817664_n.png?_nc_cat=104&_nc_ohc=TvxqNk8W4zcAX-z0MSQ&_nc_ht=scontent-cdg2-1.xx&oh=8d8ad95e7943279c94dc80c4682bf5d2&oe=5EC656F4)

Puis une fois résolu :

![SudokuEasySolved](https://scontent-cdg2-1.xx.fbcdn.net/v/t1.15752-0/p280x280/84449795_484056842283802_2952191976163770368_n.png?_nc_cat=111&_nc_ohc=7JiWdp4X7egAX_j78cD&_nc_ht=scontent-cdg2-1.xx&oh=a30bfd4f77b30b355f8d5a392f18a896&oe=5E93C094)

De plus le temps de résolution de chaque diffénrent solveur est affiché grâce au Benchmark afin de comparer les différents types de résolutions, leur efficacité et leur pertinence en fonction de la difficulté du Sudoku à résoudre.
