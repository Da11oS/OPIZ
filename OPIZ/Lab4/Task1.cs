using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPIZ.Lab4
{
    class Eyler
    {
        public readonly double y0;
        public readonly double h;
        public readonly (double begin, double end) _section;
        public Func<double, double,double> f;
        public readonly Func<double, double, double> dety;

        public Eyler(double y0, double h, (double begin, double end)  section, Func<double, double, double> f)
        {
            this.y0 = y0;
            this.h = h;
            this._section = section;
            dety = (x, y) => y + f(x, y) * h;
            this.f = f;
        }

        public string Solve()
        {
            var result = GetStepValues();
            return string.Join("\n", result.Select(x => $"{x.i :00}  {x.x:00.000}   {x.y:00.000} {f(x.x, x.y):00.000}"));
        }

        protected virtual IEnumerable<(int i, double x, double y)> GetStepValues()
        {
            var x = _section.begin;
            var y = y0;
            int i = 0;
            do
            {
                yield return (i++, x,y);
                y = dety(x, y);
                x += h;
            } while (x <= _section.end);
        }
    }
    class Runge : Eyler
    {
        public Runge(double y0, double h, (double begin, double end) section, Func<double, double, double> f):base(y0, h,section,f)
        {
        }
        public string Task1()
        {
            var result = GetStepValues();
            return string.Join("\n", result.Select(x => $"{x.i:00}  {x.x:00.000}   {x.y:00.000} {f(x.x, x.y):00.000}"));
        }

        protected override IEnumerable<(int i, double x, double y)> GetStepValues()
        {
            var x = _section.begin;
            var y = y0;
            int i = 0;
            do
            {
                yield return (i++, x, y);
                y +=  (k1(x,y) + 2 * k2(x, y) + 2 * k3(x, y) + k4(x, y)) * h/6;
                x += h;
            } while (x <= _section.end);
        }
        private double k1(double x, double y) => f(x, y);
        private double k2(double x, double y) => f(x + h/2, y + k1(x,y) * h/2);
        private double k3(double x, double y) => f(x + h / 2, y + k2(x, y) * h / 2);
        private double k4(double x, double y) => f(x + h , y + k3(x, y) * h);
    }
    class Adams : Eyler
    {
        public Adams(double y0, double h, (double begin, double end) section, Func<double, double, double> f) : base(y0, h, section, f)
        {
        }
        public string Task1()
        {
            var result = GetStepValues();
            return string.Join("\n", result.Select(x => $"{x.i:00}  {x.x:00.000}   {x.y:00.000} {f(x.x, x.y):00.000}"));
        }

        protected override IEnumerable<(int i, double x, double y)> GetStepValues()
        {
            var x = _section.begin;
            var y = y0;
            int i = 0;
            do
            {
                yield return (i++, x, y);
                y += (k1(x, y) + 2 * k2(x, y) + 2 * k3(x, y) + k4(x, y)) * h / 6;
                x += h;
            } while (x <= _section.end);
        }
        private double k1(double x, double y) => f(x, y);
        private double k2(double x, double y) => f(x + h / 2, y + k1(x, y) * h / 2);
        private double k3(double x, double y) => f(x + h / 2, y + k2(x, y) * h / 2);
        private double k4(double x, double y) => f(x + h, y + k3(x, y) * h);
    }
}
