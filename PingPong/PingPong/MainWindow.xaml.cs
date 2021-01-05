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

namespace PingPong
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(key_test);
        }

        private void key_test(object sender, KeyEventArgs e)
        {

            Thickness margin = paddle.Margin;
            //Point relativePoint = paddle.TransformToAncestor(grid).Transform(new Point(0, 0));


            if (Keyboard.IsKeyDown(Key.Left))
            {
                margin.Left -= 10;
                if (margin.Left != 0)
                {
                    paddle.Margin = margin;
                }
            }
            else if (Keyboard.IsKeyDown(Key.Right))
            {
                margin.Left += 10;
                if (margin.Left + paddle.Width != Application.Current.MainWindow.Width - 30)
                {
                    paddle.Margin = margin;
                }
            }
        }
    }
}
