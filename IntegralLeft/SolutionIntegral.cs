namespace IntegralLeft
{
    class SolutionIntegral
    {
        //Число шагов для каждого интеграла
        private double Nx = 0;//1
        private double Ny = 0;//2
        private double Nz = 0;//3

        //Пределы интегрирования
        //1
        private double start_int1 = 0.00;
        private double end_int1 = 0.00;
        //2
        private double start_int2 = 0.00;
        private double end_int2 = 0.00;
        //3
        private double start_int3 = 0.00;
        private double end_int3 = 0.00;

        //Строковое представление подинтегральной функции
        private string text_func;

        public SolutionIntegral()
        {

        }

        public SolutionIntegral(string text_func, double Ni, double Nj, double Nk, double s_i1, double e_i1, double s_i2, double e_i2, double s_i3, double e_i3)
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

            double result = 0.00;

            double hx = (end_int1 - start_int1) / Nx;
            double hy = (end_int2 - start_int2) / Ny;
            double hz = (end_int3 - start_int3) / Nz;

            double temp_hx = start_int1;
            while (temp_hx <= end_int1 - hx)
            {
                double temp_hy = start_int2;
                while (temp_hy <= end_int2 - hy)
                {
                    double temp_hz = start_int3;
                    while (temp_hz <= end_int3 - hz)
                    {
                        var expr = calc.ParseExpression(text_func, x => temp_hx, y => temp_hy, z => temp_hz);
                        var func = expr.Compile();
                        result += func();
                        temp_hz += hz;
                    }
                    result *= hz;
                    temp_hy += hy;
                }
                result *= hy;
                temp_hx += hx;
            }
            result *= hx;

            return result;
        }
    }
}
