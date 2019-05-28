using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlg
{
    class Program
    {
        static void Main(string[] args)
        {
            Genetic genetic = new Genetic();

            List<Gen> gen = genetic.FindOpt();

            PrintList(gen.OrderBy(x => x.value).ToList());

            Console.ReadKey();
        }

        private static void PrintList(List<Gen> gen)
        {
            for (int i = 0; i < gen.Count; i++)
            {
                Console.WriteLine("x = {0} F(x) = {1}", gen[i].value, -(gen[i].value * gen[i].value) + 10000);
            }
        }
    }
}
