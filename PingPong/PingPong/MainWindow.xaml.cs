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
        private int GemSpeed = 10;
        private int PaddleSpeed = 10;
        private int BallSpeedVertical = 5;
        private int BallSpeedHorizontal = 5;
        private Random rnd = new Random();

        DispatcherTimer GameTimer = new DispatcherTimer();
        DispatcherTimer LevelUp = new DispatcherTimer();
        DispatcherTimer GemStarts = new DispatcherTimer();
        DispatcherTimer FallingGem = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(KeyEvent);
            myCanvas.Focus();
            RandomStart();
            StartTimers();

        }

        private void StopTimers()
        {
            GameTimer.Stop();
            GemStarts.Stop();
            FallingGem.Stop();
            LevelUp.Stop();

        }
        private void StartTimers()
        {
            GemStarts.Tick += StartGemEvent;
            GemStarts.Interval = TimeSpan.FromSeconds(40);
            GemStarts.Start();

            GameTimer.Tick += GameTimerEvent;
            GameTimer.Interval = TimeSpan.FromMilliseconds(40);
            GameTimer.Start();

            LevelUp.Tick += LevelUpEvent;
            LevelUp.Interval = TimeSpan.FromSeconds(10);
            LevelUp.Start();

            FallingGem.Tick += FallingGemEvent;
            FallingGem.Interval = TimeSpan.FromMilliseconds(40);
        }

        private void LevelUpEvent(object sender, EventArgs e)
        {
            if(paddle.Width > 20) paddle.Width -= 10;
        }

        private void SetGemToStartPosition()
        {
            Canvas.SetTop(gem, -20);
        }

        private void FallingGemEvent(object sender, EventArgs e)
        {
            Canvas.SetTop(gem, Canvas.GetTop(gem) + GemSpeed);
            if (CheckItemMeetWithPaddle(gem) || Canvas.GetTop(gem) + (gem.Height) > Application.Current.MainWindow.Height)
            {
                FallingGem.Stop();
                SetGemToStartPosition();
            }
        }

        private void StartGemEvent(object sender, EventArgs e)
        {

            SetGemToStartPosition();
            RandomPosition(gem);
            FallingGem.Start();

        }

        private void RandomPosition(Rectangle item)
        {
            int horizontalPosition = rnd.Next(10, ((int)Application.Current.MainWindow.Width - (int)item.Width * 2));
            Canvas.SetLeft(item, horizontalPosition);
        }

        private void RandomStart()
        {
            RandomPosition(ball); 
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
                StopTimers();
                MessageBox.Show("Congratulations! You reached" + Score + "points.");
                // Zoli restart?
            }
            //if (Canvas.GetTop(ball) + (ball.Height) >= Canvas.GetTop(paddle) && Canvas.GetLeft(ball) >= Canvas.GetLeft(paddle) && Canvas.GetLeft(ball) + ball.Width <= Canvas.GetLeft(paddle)+ paddle.Width)
            if(CheckItemMeetWithPaddle(ball))
            {
                Score += 1;
                BallSpeedVertical = -BallSpeedVertical;
             
            }
        }
        private bool CheckItemMeetWithPaddle(Rectangle item)
        {
            return (Canvas.GetTop(item) + (item.Height) <= Canvas.GetTop(paddle) &&
                Canvas.GetTop(item) + (item.Height) >= Canvas.GetTop(paddle) - item.Height &&
                Canvas.GetLeft(item) >= Canvas.GetLeft(paddle) - item.Width + 1 &&
                Canvas.GetLeft(item) - 1 <= Canvas.GetLeft(paddle) + paddle.Width
                );
        }



        private void KeyEvent(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Left))
            {
                MovePaddleLeft();
            }
            else if (Keyboard.IsKeyDown(Key.Right))
            {
                MovePaddleRight();
            }
            else if (Keyboard.IsKeyDown(Key.Escape))
            {
                // we also have to stop the ball while message box is active
                GameTimer.Stop();
                ShowEscapeMessageBox();
            }
            else if (Keyboard.IsKeyDown(Key.Space)) 
            {
                GameTimer.Stop();
                ShowSpaceMessageBox();
            }
        }

        private void MovePaddleLeft()
        {   
            if(Canvas.GetLeft(paddle) > 10)
            {
                Canvas.SetLeft(paddle, Canvas.GetLeft(paddle) - PaddleSpeed);
            }
        }

        private void MovePaddleRight()
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
                GameTimer.Start();
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
                    GameTimer.Start();
                    break;
            }
        }

        private void str_button_Click(object sender, RoutedEventArgs e)
        {
            GameTimer.Start();
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
