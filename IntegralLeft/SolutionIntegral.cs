using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegralLeft
{
    class SolutionIntegral
    {
        //Число шагов для каждого интеграла
        private int Nx = 0;//1
        private int Ny = 0;//2
        private int Nz = 0;//3

        //Пределы интегрирования
        private double start_int1 = 0.00;
        private double end_int1 = 0.00;
        private double start_int2 = 0.00;
        private double end_int2 = 0.00;
        private double start_int3 = 0.00;
        private double end_int3 = 0.00;

        private string text_func;

        public SolutionIntegral()
        {

        }

        public SolutionIntegral(string text_func, int Ni, int Nj, int Nk, double s_i1, double e_i1, double s_i2, double e_i2, double s_i3, double e_i3)
        {
            this.text_func = text_func;
            this.Nx = Ni;
            this.Ny = Nj;
            this.Nz = Nk;
            start_int1 = s_i1;
            end_int1 = e_i1;
            start_int2 = s_i2;
            end_int2 = e_i2;
            start_int3 = s_i3;
            end_int3 = e_i3;
        }

        public double Solution()
        {
            var calc = new Sprache.Calc.XtensibleCalculator();

            //var expr = calc.ParseExpression("sin(y/x*z)", x => 1, y => 2, z => 3);
            //var func = expr.Compile();

            double result = 0.00;

            double hx = (end_int1 - start_int1) / Nx;
            double hy = (end_int2 - start_int2) / Ny;
            double hz = (end_int3 - start_int3) / Nz;

            double temp_hx = start_int1;
            for (int i = 0; i < Nx; i++)
            {
                double temp_hy = start_int2;
                for (int j = 0; j < Ny; j++)
                {
                    double temp_hz = start_int3;
                    for (int k = 0; k < Nz; k++)
                    {
                        var expr = calc.ParseExpression(text_func, x => temp_hx, y => temp_hy, z => temp_hz);
                        var func = expr.Compile();
                        result += func();
                        temp_hz += hz;
                    }
                    temp_hy += hy;
                }
                temp_hx += hx;
            }

            return result;
        }
    }
}
