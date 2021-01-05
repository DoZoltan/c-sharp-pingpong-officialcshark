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
        private int PaddleSpeed = 10;
        
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(KeyEvent);
        }

        private void KeyEvent(object sender, KeyEventArgs e)
        {

            Thickness margin = paddle.Margin;
            //Point relativePoint = paddle.TransformToAncestor(grid).Transform(new Point(0, 0));

            if (Keyboard.IsKeyDown(Key.Left))
            {
                MovePaddleLeft(margin);
            }
            else if (Keyboard.IsKeyDown(Key.Right))
            {
                MovePaddleRight(margin);
            }
            else if (Keyboard.IsKeyDown(Key.Escape)) 
            {
                // we also have to stop the ball while message box is active
                ShowMessageBox();
            }
        }

        private void MovePaddleLeft(Thickness margin)
        {
            margin.Left -= PaddleSpeed;
            if (margin.Left != 0)
            {
                paddle.Margin = margin;
            }
        }

        private void MovePaddleRight(Thickness margin)
        {
            margin.Left += PaddleSpeed;
            if (margin.Left + paddle.Width != Application.Current.MainWindow.Width - 30)
            {
                paddle.Margin = margin;
            }
        }

        private void ShowMessageBox() 
        {
            MessageBoxResult result = MessageBox.Show("Do you want to quit?", "Escape menu", MessageBoxButton.YesNo);
            MessageBoxResponse(result);
        }

        private void MessageBoxResponse(MessageBoxResult result) 
        {
            switch (result) 
            {
                case MessageBoxResult.Yes:
                    Application.Current.Shutdown();
                    break;
            }
        }


       
    }
}
