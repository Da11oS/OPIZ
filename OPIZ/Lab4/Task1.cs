using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPIZ.Lab4
{
    public class Eyler
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

        public virtual IEnumerable<(int i, double x, double y)> GetStepValues()
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

        public override IEnumerable<(int i, double x, double y)> GetStepValues()
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
    public class Adams : Eyler
    {
        protected Eyler _eylerMethod;
        protected List<(int i, double x, double y)> _results;
        public Adams(double y0, double h, (double begin, double end) section, Func<double, double, double> f) : base(y0, h, section, f)
        {
            _results = new ();

            _eylerMethod = new Eyler(y0, h, (_section.begin, section.begin + h * 3), f);
        }
        public virtual string Task1()
        {
            SetResults();
            return string.Join("\n", _results.Select(x => $"{x.i:00}  {x.x:00.000}   {x.y:00.000} {f(x.x, x.y):00.000}"));
        }
        public virtual void SetResults()
        {
            //_results.Add((0, _section.begin, y0));
            _results.AddRange(_eylerMethod.GetStepValues());
            var x = _results.Last().x + h;
            var y = _results.Last().y;
            int i = _results.Last().i;
            while (x < _section.end)
            {
                y = _results[i].y + h * f(x, y) + 
                    (h * h / 2) * GetDet1(i) + 
                    (5 * Math.Pow(h, 3) / 12) * GetDet2(i) + 
                    (3 * Math.Pow(h, 4) / 8) * GetDet3(i);
                _results.Add((++i, x, y));
                x += h;
            } 
        }
        private double GetDet1(int i) => f(_results[i].x, _results[i].y) - f(_results[i - 1].x, _results[i - 1].y);
        private double GetDet2(int i) => f(_results[i].x, _results[i].y) - 2 * f(_results[i - 1].x, _results[i - 1].y) + f(_results[i - 2].x, _results[i -2].y);
        private double GetDet3(int i) => 
            f(_results[i].x, _results[i].y) - 3 * f(_results[i - 1].x, _results[i - 1].y) + 
            3 * f(_results[i - 2].x, _results[i - 2].y) - f(_results[i - 3].x, _results[i - 3].y);
    }
    public class Miln : Adams
    {
        public Miln(double y0, double h, (double begin, double end) section, Func<double, double, double> f) : base(y0, h, section, f)
        {
        }
        public override string Task1()
        {
            SetResults();
            return string.Join("\n", _results.Select(x => $"{x.i:00}  {x.x:00.000}   {x.y:00.000} {f(x.x, x.y):00.000}"));
        }

        public override void SetResults()
        {
            _results.AddRange(_eylerMethod.GetStepValues());
            List<double> dY = _results.Select(e => f(e.x, e.y)).ToList();
            var x = _results.Last().x+h;
            var y = _results.Last().y;
            double dy1 = 0;
            double dy2 = 0;
            int i = 3;

            do
            {
                y = _results[i - 3].y + (4 / 3) * h * (2 * dY[i] - dY[i - 1] + 2 * dY[i - 2]);
                dy1 = f(x, y);
                while (true)
                {
                    y = _results[i - 1].y + (h / 3) * (dy1 + 4 * dY[i] + dY[i - 1]);
                    dy2 = f(x, y);
                    if (Math.Abs(dy2 - dy1) < 1e-3) break;
                    dy1 = dy2;
                }
                dY.Add(dy2);
                y = _results[i - 1].y + (h / 3) * (dY[i + 1] + 4 * dY[i] + dY[i - 1]);
                _results.Add((++i, x, y));
                x += h;
            } while  (x <= _section.end);
        }
    }
}
