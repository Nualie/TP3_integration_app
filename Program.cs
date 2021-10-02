using System;
using System.Linq;
using System.Collections;
using System.Threading;
using System.Diagnostics;

namespace TP3
{
    class Program
    {
        static void Main(string[] args)
        {
            //EXERCISE 1
            var collections = new MovieCollection().Movies;

            //Count all movies.
            Console.WriteLine(collections.Count());

            //Count all movies with the letter e.
            Console.WriteLine(collections.Count(x=>x.Title.Contains("e")));

            //Count how many time the letter f is in all the titles from this list.
            int fcounter = 0;
            foreach(var item in collections)
            {
                fcounter+=item.Title.Count(x => x=='f');
            }
            Console.WriteLine(fcounter);

            //• Order all movies by who directed them and print the top 3 directors that did the most movies
            var directedMost = collections
                                    .GroupBy(x => x.DirectedBy)
                                    .OrderByDescending(gp => gp.Count())
                                    .Take(3)
                                    .Select(g => g.Key).ToList();
            Console.WriteLine($"{directedMost[0]}\n{directedMost[1]}\n{directedMost[2]}\n");
            collections.OrderBy(x => x.DirectedBy);

            //Display the title of the film with the higher budget.
            var highestBudget = collections.OrderByDescending(x => x.Budget)
                                .Take(1)
                                .Select(g => g).ToList();

            Console.WriteLine($"{highestBudget[0].Title}({highestBudget[0].Budget}$)");

            //Display the title of the movie with the lowest box office.
            var lowestBoxOffice = collections.OrderBy(x => x.BoxOffice)
                                .Where(x => x.BoxOffice>0)
                                .Take(1)
                                .Select(g => g).ToList();

            Console.WriteLine($"{lowestBoxOffice[0].Title}({lowestBoxOffice[0].BoxOffice})");

            //Order the movies by reversed alphabetical order and print the first 11 of the list
            var reverseAlpha = collections.OrderByDescending(x => x.Title)
                                .Take(11)
                                .Select(g => g).ToList();
            foreach (var item in reverseAlpha)
            { 
                Console.WriteLine($"{item.Title}");
            }

            //Count all the movies made before 1980.
            Console.WriteLine(collections.Count(x=>x.ReleaseDate.Year<1980));

            //Display the average running time of movies having a vowel as the first letter
            double averageRunningTime = 0;
            int initialCount = 0;
            foreach (var item in collections.Where(x => x.Title[0] =='A' || x.Title[0] == 'E' || x.Title[0] == 'I' || x.Title[0] == 'O' || x.Title[0] == 'U' || x.Title[0] == 'Y'))
            {
                averageRunningTime += item.RunningTime;
                initialCount++;
            }
            Console.WriteLine($"average of {averageRunningTime / initialCount} hours");

            //Group all films by the number of characters in the title screen and print the count of movies by letter in the film.
            var charactersTitleScreen = collections.GroupBy(x => x.Title.Length)
                                        .OrderBy(x => x.Key)
                                        .Select(g => g).ToList();
            foreach(var item in charactersTitleScreen)
            {
                Console.WriteLine($"{item.Key} char => {item.Count()} movie(s)");
            }

            // Calculate the mean of all Budget / Box Office of every movie ever
            var meanOfAllBudget = collections.GroupBy(x => x.Budget)
                            .Distinct();
            var meanOfAllBoxOffice = collections.GroupBy(x => x.BoxOffice)
                            .Distinct();
            Console.WriteLine($"Mean Budget: {meanOfAllBudget.Average(x => x.Key)}\nMean Box Office: {meanOfAllBoxOffice.Average(x => x.Key)}");

            //Print all movies with the letter H or W in the title, but not the letter I or T
            var MoviesWithHOrW = collections.Where(x => (x.Title.Contains('h') || x.Title.Contains('H') || x.Title.Contains('W') || x.Title.Contains('w')) && !x.Title.Contains('t') && !x.Title.Contains('T') && !x.Title.Contains('i') && !x.Title.Contains('I'))
                                 .Select(g => g).ToList();
            Console.WriteLine("Movies with h or w but not i or t:");
            foreach (var item in MoviesWithHOrW)
            { 
                Console.WriteLine(item.Title); 
            }

            //EXERCISE 2

            createThreads();

        }

        private static Mutex mut = new Mutex();
        static void createThreads()
        {
            
            Thread a = new Thread(startwork);
            Thread b = new Thread(startwork);
            Thread c = new Thread(startwork);
            a.Name = "a";
            b.Name = "b";
            c.Name = "c";
            a.Start();
            b.Start();
            c.Start();
        }

        private static void startwork(object obj)
        {

            var th = Thread.CurrentThread;
            if (th.Name == "a")
            {
                job('_', 10000, 50);
            }
            if (th.Name == "b")
            {
                job('*', 11000, 40);
            }
            if (th.Name == "c")
            {
                job('°', 9000, 20);
            }
            
        }

        public static void job(char character, int time, int beat)
        {
            var sw = Stopwatch.StartNew();
           
            do
            {
                mut.WaitOne();
                Console.Write(character);
                mut.ReleaseMutex();
                Thread.Sleep(beat);
            } while (sw.ElapsedMilliseconds <= time);
            sw.Stop();
        }


    }
}
