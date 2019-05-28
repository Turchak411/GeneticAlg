using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GeneticAlg
{
    public class Genetic
    {
        private List<Gen> population;
        private List<double> funcValueCollection;
        private List<Gen> newPopulation;

        public Genetic()
        {
            population = new List<Gen>();
            funcValueCollection = new List<double>();
        }

        public List<Gen> FindOpt()
        {
            int objCount = 20;

            int iterCount = 30;
            Console.WriteLine("Итераций: {0}\n", iterCount);

            newPopulation = new List<Gen>();

            for (int i = 0; i < iterCount; i++)
            {
                if (i >= 1)
                {
                    NewPopulationToPopulation();
                }

                // Generate population:
                GeneratePopulation(objCount);

                if (i == 0)
                {
                    PrintList(population.OrderBy(x => x.value).ToList());
                }

                // Calclating Fitness Func:
                CalcFuncValues();

                // Selection:
                DoSelection();

                newPopulation = new List<Gen>();

                // Crossbreeding:
                Crossbreeding(objCount);

                Mutation();

                population = new List<Gen>();
            }

            return newPopulation;
        }

        private static void PrintList(List<Gen> gen)
        {
            for (int i = 0; i < gen.Count; i++)
            {
                Console.WriteLine("x = {0} F(x) = {1}", gen[i].value, -(gen[i].value * gen[i].value) + 10000);
            }

            Console.WriteLine("==========");
        }

        private void NewPopulationToPopulation()
        {
            for (int i = 0; i < newPopulation.Count; i++)
            {
                population.Add(newPopulation[i]);
            }
        }

        private void GeneratePopulation(int objCount)
        {
            for (int i = 0; i < objCount - newPopulation.Count; i++)
            {
                Random rnd = new Random(DateTime.Now.Millisecond);
                Thread.Sleep(6); // To correcting random generate
                population.Add(new Gen(rnd.Next(0, 100)));
            }
        }

        private double Func(double x)
        {
            return -(x * x);
        }

        private void CalcFuncValues()
        {
            for (int i = 0; i < population.Count; i++)
            {
                funcValueCollection.Add(Func(population[i].value));
            }
        }

        private void DoSelection()
        {
            // Генерация величины порога:
            double fAVG = funcValueCollection.Average();

            for (int i = 0; i < population.Count; i++)
            {
                if (fAVG > funcValueCollection[i])
                {
                    population.RemoveAt(i);
                    funcValueCollection.RemoveAt(i);
                }
            }
        }

        private void Crossbreeding(int maxObjCount)
        {
            double crossbreedingChance = 0.65;

            Random rnd = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < population.Count - 1; i+=2)
            {
                if (rnd.NextDouble() < crossbreedingChance)
                {
                    Crossover(population[i], population[i + 1]);
                }
                else
                {
                    newPopulation.Add(population[i]);
                    newPopulation.Add(population[i + 1]);
                }

                Thread.Sleep(50);
            }
        }

        private void Crossover(Gen obj1, Gen obj2)
        {
            string obj1String = DoubleToBin(obj1.value);
            //obj1String += (obj1.value < 0) ? "1" : "0";

            string obj2String = DoubleToBin(obj2.value);
            //obj2String += (obj2.value < 0) ? "1" : "0";

            string newObj1 = "";
            string newObj2 = "";

            // Добавление нулей слева:
            int count = obj1String.Length > obj2String.Length ? obj1String.Length - obj2String.Length
                                                              : obj2String.Length - obj1String.Length;
            int obj1Length = obj1String.Length;
            int obj2Length = obj2String.Length;
            for (int i = 0; i < count; i++)
            {
                if (obj1Length > obj2Length)
                {
                    obj2String = obj2String.Insert(0, "0");
                }
                else
                {
                    obj1String = obj1String.Insert(0, "0");
                }
            }

            // Одноточечное скрещивание
            // Генерация места "разреза":
            Random rnd1 = new Random(DateTime.Now.Millisecond - 123);
            int cutPoint = rnd1.Next(1, obj1String.Length);

            newObj1 = obj1String.Substring(0, cutPoint);
            newObj2 = obj2String.Substring(0, cutPoint);

            newObj1 += obj2String.Substring(cutPoint, obj1String.Length - cutPoint);
            newObj2 += obj1String.Substring(cutPoint, obj2String.Length - cutPoint);

            newPopulation.Add(new Gen(StringToDouble(newObj1)));
            newPopulation.Add(new Gen(StringToDouble(newObj2)));
        }

        private string DoubleToBin(double num)
        {
            return Convert.ToString((Int16)num, 2);
        }

        private double StringToDouble(string value)
        {
            return Convert.ToInt32(value, 2);
        }

        private void Mutation()
        {
            double mutationChance = 0.15;

            Random rnd = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < population.Count - 1; i ++)
            {
                char[] chars = DoubleToBin(population[i].value).ToCharArray();

                for (int k = 0; k < chars.Length; k++)
                {
                    if (rnd.NextDouble() < mutationChance)
                    {
                        chars[k] = chars[k] == '1' ? '0' : '1';
                    }
                }

                population[i].value = StringToDouble(new string(chars));
            }
        }

    }
}
