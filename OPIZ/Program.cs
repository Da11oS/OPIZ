using OPIZ.Lab3;
using OPIZ.Lab4;
using System;
using System.Linq;

namespace OPIZ
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var simpson = new Integral((1.6, 3.2), (x) => (x / (2 * Math.Log((x * x) / 2))), 10);

            Console.WriteLine($"Формула Симпсона:  {simpson.SolveSimpson()}");
            Console.WriteLine("i\tx\ty");
            Console.WriteLine(string.Join("\n",     string.Join("\n", simpson.Ys.Select(x => $"{x.i:00}  {x.x:00.000}   {x.y:00.000}"))));

            var square = new Integral((0, 1), (x) => (2 * x + Math.Exp(-(x * x))), 10);
            Console.WriteLine($"Формула прямоугольния: \n {square.SolveRect()}");
            Console.WriteLine("i\tx\ty\tf");
            Console.WriteLine(string.Join("\n", string.Join("\n", square.Ys.Select(x => $"{x.i:00}  {x.x:00.000}   {x.y:00.000} "))));

            Console.WriteLine($"Формула тропеции: \n =  {square.SolveTrapez()}");
            Console.WriteLine("i\tx\ty\tf");
            Console.WriteLine(string.Join("\n", string.Join("\n", square.Ys.Select(x => $"{x.i:00}  {x.x:00.000}   {x.y:00.000}"))));
            var gaus = new Gaus((0, 1), (x) => (2 * x + Math.Exp(-(x * x))), 10);
            Console.WriteLine($"SolveGaus =  {gaus.SolveGaus()}");
            Console.WriteLine("i\t a \t b \t y\tf");
            Console.WriteLine(string.Join("\n", string.Join("\n", gaus.Ys.Select(x => $"{x.i:00}  {x.a:00.000} {x.b :00.000}  {x.y:00.000} "))));
            Lb4();
            Lb5();
            Console.ReadKey();
        }
        public static void Lb4 ()
        {
            Console.WriteLine("Эйлер");
            Console.WriteLine("i\tx\ty\tf");
            Console.WriteLine(new Eyler(1, 0.1, (0, 1), (x, y) => (y + (Math.Cos(x) / 3))).Solve());
        }
        public static void Lb5()
        {
            Console.WriteLine("Рунге-Кутт");
            Console.WriteLine("i\tx\ty\tf");
            Console.WriteLine(new Eyler(1, 0.1, (0, 1), (x, y) => (y + Math.Cos(x) / 3)).Solve());
        }
    }
}
