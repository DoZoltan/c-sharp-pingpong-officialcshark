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
        private int BallSpeedVertical = 5;
        private int BallSpeedHorizontal = 5;

        DispatcherTimer gameTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(KeyEvent);
            myCanvas.Focus();
            RandomStart();
            gameTimer.Tick += GameTimerEvent;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Stop();
        }

        private void RandomStart()
        {
            Random rnd = new Random();
            int way = rnd.Next(0,2);
            switch (way)
            {
                case 1:
                    BallSpeedHorizontal = -BallSpeedHorizontal;
                    break;
            }
            
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            Canvas.SetLeft(ball, Canvas.GetLeft(ball) - BallSpeedHorizontal);
            Canvas.SetTop(ball, Canvas.GetTop(ball) + BallSpeedVertical);
            if (Canvas.GetLeft(ball) < 1 || Canvas.GetLeft(ball) + (ball.Width + 15) > Application.Current.MainWindow.Width)
            {
               
                BallSpeedHorizontal = -BallSpeedHorizontal;
            }
            else if (Canvas.GetTop(ball) < 1 || Canvas.GetTop(ball) + (ball.Height + 31) > Application.Current.MainWindow.Height)
            {
                BallSpeedVertical = -BallSpeedVertical;
            }
            if (Canvas.GetTop(ball) + (ball.Height + 31) >= Application.Current.MainWindow.Height)
            {
                gameTimer.Stop();
                MessageBox.Show("Congratulations! You reached" + Score + "points.");
                // Zoli restart?
            }
            if (Canvas.GetTop(ball) + (ball.Height) >= Canvas.GetTop(paddle) && Canvas.GetLeft(ball) >= Canvas.GetLeft(paddle) && Canvas.GetLeft(ball) + ball.Width <= Canvas.GetLeft(paddle)+ paddle.Width)
            {
                Score += 1;
                BallSpeedVertical = -BallSpeedVertical;
             
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
                gameTimer.Stop();
                ShowEscapeMessageBox();
            }
            else if (Keyboard.IsKeyDown(Key.Space)) 
            {
                gameTimer.Stop();
                ShowSpaceMessageBox();
            }
        }

        private void MovePaddleLeft(Thickness margin)
        {   
            if(Canvas.GetLeft(paddle) > 10)
            {
                Canvas.SetLeft(paddle, Canvas.GetLeft(paddle) - PaddleSpeed);
            }
        }

        private void MovePaddleRight(Thickness margin)
        {
            if (Canvas.GetLeft(paddle) + (paddle.Width + 20)  < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(paddle, Canvas.GetLeft(paddle) + PaddleSpeed);
            }
        }

        private void ShowEscapeMessageBox() 
        {
            MessageBoxResult result = MessageBox.Show("Do you want to quit?", "Escape menu", MessageBoxButton.YesNo);
            MessageBoxResponse(result);
        }

        private void ShowSpaceMessageBox() 
        {
            MessageBoxResult result = MessageBox.Show("Press SPACE to continue.", "Space menu");
            if (result == MessageBoxResult.OK) 
            {
                gameTimer.Start();
            }
        }

        private void MessageBoxResponse(MessageBoxResult result) 
        {
            switch (result) 
            {
                case MessageBoxResult.Yes:
                    Application.Current.Shutdown();
                    break;
                case MessageBoxResult.No:
                    gameTimer.Start();
                    break;
            }
        }

        private void str_button_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.Start();
        }

        private void rst_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ext_btn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
