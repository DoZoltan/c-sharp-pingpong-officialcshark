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
using System.Windows.Threading;

namespace PingPong
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int Score = 0;
        private int PaddleSpeed = 10;
        private int BallSpeed = 5;
        DispatcherTimer gameTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(KeyEvent);
            myCanvas.Focus();

            gameTimer.Tick += GameTimerEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            Canvas.SetLeft(ball, Canvas.GetLeft(ball) - BallSpeed);
            if (Canvas.GetLeft(ball) < 1 || Canvas.GetLeft(ball) + (ball.Width + 15) > Application.Current.MainWindow.Width)
            {
                BallSpeed = -BallSpeed;
            }
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
            if(Canvas.GetLeft(paddle) > 10)
            {
                Canvas.SetLeft(paddle, Canvas.GetLeft(paddle) - PaddleSpeed);
            }
            /*
            margin.Left -= PaddleSpeed;
            if (margin.Left != 0)
            {
                paddle.Margin = margin;
            }*/
        }

        private void MovePaddleRight(Thickness margin)
        {
            if (Canvas.GetLeft(paddle) + (paddle.Width + 20)  < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(paddle, Canvas.GetLeft(paddle) + PaddleSpeed);
            }
            /*
            margin.Left += PaddleSpeed;
            if (margin.Left + paddle.Width != Application.Current.MainWindow.Width - 30)
            {
                paddle.Margin = margin;
            }*/
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
