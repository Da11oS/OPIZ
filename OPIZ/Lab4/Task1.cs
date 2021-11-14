using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPIZ.Lab4
{
    class Lb4
    {
        public readonly double y0;
        public readonly double h;
        public readonly (double begin, double end) _section;
        public Func<double, double,double> f;
        public readonly Func<double, double, double> dety;
        public Lb4(double y0, double h, (double begin, double end)  section, Func<double, double, double> f)
        {
            this.y0 = y0;
            this.h = h;
            this._section = section;
            dety = (x, y) => y + f(x, y) * h;
            this.f = f;
        }

        public string Task1()
        {
            var result = GetStepValues();
            return string.Join("\n", result.Select(x => $"{x.i :00}  {x.x:00.000}   {x.y:00.000} {f(x.x, x.y):00.000}"));
        }

        private IEnumerable<(int i, double x, double y)> GetStepValues()
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
}
