using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace OPIZ.Lab3
{
    public class Integral
    {
        public readonly (double a, double b) Section;
        public readonly int N;
        public readonly double Accurance;
        public readonly Func<double, double> f;
        public double h { protected set; get; }
        public double Result { protected set; get; }
        public IEnumerable<(int i, double x, double y)> Ys { protected set; get; }
        public Integral((double, double) section, Func<double, double> f, int n = 20, double accurance = 0.0001)
        {
            Section = section;
            Accurance = accurance;
            N = n;
            this.f = f;
        }
        public double SolveSimpson()
        {
            h = (Section.b - Section.a) / (N);
            Ys = GetYArray(Section.b + h);
            var sumNorEven = Ys.Skip(1).SkipLast(1).Where(x => x.i % 2 != 0).Select(x => x.y).Sum();
            var sumEven = Ys.Skip(2).SkipLast(2).Where(x => x.i % 2 == 0).Select(x => x.y).Sum();
            Result = (h / 3) * (Ys.Select(x => x.y).First() + 4 * sumNorEven + 2 * sumEven + Ys.Select(x => x.y).Last());
            return Result;
        }
        public double SolveRect()
        {
            h = (Section.b - Section.a) / (N);
            Ys = GetYArray(Section.b);
            Result = h * (Ys.SkipLast(1).Select(x => x.y).Sum());
            return Result;
        }
        public double SolveTrapez()
        {
            h = (Section.b - Section.a) / (N);
            Ys = GetYArray(Section.b);
            Result = ((Ys.Select(x => x.y).First() + Ys.Select(x => x.y).Last()) / 2 + Ys.SkipLast(1).Skip(1).Select(x => x.y).Sum()) * h;
            return Result;
        }
        public IEnumerable<(int i, double x, double y)> GetYArray(double endValue)
        {
            int i = 0;
            double x = Section.a;
            do
            {
                var y = f(x);
                yield return (i++, x, y);
                x += h;
            } while (x <= endValue);
            yield break;
        }
    }
    public class Gaus : Integral
    {
        public new IEnumerable<(int i, double a, double b, double y)> Ys { protected set; get; }
        public readonly Func<double, double, double> func;
        public Gaus((double, double) section, Func<double, double> f, int n = 20, double accurance = 0.0001) : base(section, f, n, accurance)
        {
            func =
            (a, b) => (f(((a + b) / 2) - ((b - a) / (2 * Sqrt(3)))) +
            f(((a + b) / 2) + ((b - a) / (2 * Sqrt(3))))) * ((b - a) / 2);
        }
        public double SolveGaus()
        {
            (double a, double b) = Section;
            h = (Section.b - Section.a) / (N);
            Ys = GetWt();
            return (Ys.Select(x => x.y).Sum());
        }
        public IEnumerable<(int i, double a, double b, double y)> GetWt()
        {
            var x = Section.a;
            int i = 0;
            do
            {
                var y = func(x, x + h);
                yield return (i++, x, x+h, y);
                x += h;
            } while (x <= Section.b - h);
        }
    }
}
