using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace UP_2024.db
{
    public partial class Product
    {
        

        public TextBlock Passed
        {
            get
            {
                foreach (var test in Test)
                {
                    if (test.isPassed == false)
                        return new TextBlock() { Foreground = Brushes.Red, Text = "Не пройден" };
                }
                return new TextBlock() { Foreground = Brushes.LightGreen, Text = "Пройден" };
            }
        }
    }
}
