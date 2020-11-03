using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IntegralLeft
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*var calc = new Sprache.Calc.XtensibleCalculator();

            var expr = calc.ParseExpression("sin(y/x*z)", x => 1, y => 2, z => 3);
            var func = expr.Compile();*/

            SolutionIntegral solution = new SolutionIntegral("Sin(y/x*z)", 1, 1, 1, 1, 2, 1, 3, 1, 4);

            Console.WriteLine("Result = {0}", solution.Solution());
        }
    }
}
