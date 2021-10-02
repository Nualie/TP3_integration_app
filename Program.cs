using System;
using System.Linq;
using System.Collections;

namespace TP3
{
    class Program
    {
        static void Main(string[] args)
        {
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
                                .Take(1)
                                .Select(g => g).ToList();

            Console.WriteLine($"{lowestBoxOffice[0].Title}({lowestBoxOffice[0].BoxOffice})");

            //Order the movies by reversed alphabetical order and print the first 11 of the list
            var reverseAlpha = collections.OrderByDescending(x => x.Title)
                                .Take(11)
                                .Select(g => g).ToList();
            foreach (item in reverseAlpha)
            { 
                Console.WriteLine($"{item.Title}");
            }
        }
    }
}
