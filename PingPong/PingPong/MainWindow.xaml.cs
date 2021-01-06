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
        private double OfficialPaddleWidth;
        private int Score = 0;
        private int OfficialPaddleSpeed = 0;
        private int PaddleSpeed = 20;
        private int ActualLevel = 1;
        private int GemSpeed = 1;
        private int OfficialBallSpeedVertical = 0;
        private int BallSpeedVertical = 1;
        private int OfficialBallSpeedHorizontal = 0;
        private int BallSpeedHorizontal = 1;
        private int RepetitionCounter = 0;
        private Random rnd = new Random();

        DispatcherTimer GameTimer = new DispatcherTimer();
        DispatcherTimer LevelUp = new DispatcherTimer();
        DispatcherTimer GemStarts = new DispatcherTimer();
        DispatcherTimer FallingGem = new DispatcherTimer();
        DispatcherTimer GemIsActive = new DispatcherTimer();
        DispatcherTimer AcceleratedBall = new DispatcherTimer();
        DispatcherTimer ProgressBar = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(KeyEvent);
            myCanvas.Focus();
            RandomStart();
            //StartTimers();
            LoadTimers();
            str_button.IsEnabled = true;

        }

        private void RandomPropertyActivator()
        {
            OfficialPaddleWidth = paddle.Width;
            OfficialBallSpeedHorizontal = BallSpeedHorizontal;
            OfficialBallSpeedVertical = BallSpeedVertical;
            OfficialPaddleSpeed = PaddleSpeed;
            int randomProperty = rnd.Next(0, 4);
            switch (randomProperty)
            {
                case 0:
                    paddle.Width /= 2;
                    break;
                case 1:
                    paddle.Width *= 2;
                    break;
                case 2:
                    PaddleSpeed += 5;
                    break;
                case 3:
                    BallSpeedHorizontal += 1;
                    BallSpeedVertical += 1; 
                    break;

            }
        }

        private void OriginalValueAdjuster()
        {
            BallSpeedHorizontal = OfficialBallSpeedHorizontal;
            BallSpeedVertical = OfficialBallSpeedVertical;
            PaddleSpeed = OfficialPaddleSpeed;
            paddle.Width = OfficialPaddleWidth;
        }

        private void GemActivatePropertyChecker(object sender, EventArgs e)
        {
            if (GemIsActive.IsEnabled && RepetitionCounter == 1)
            {
                GemIsActive.Stop();
                OriginalValueAdjuster();
                RepetitionCounter = 0;
                
            }
            else
            {
                RepetitionCounter = 1;
            }
        }

        private void LoadTimers() 
        {
            GemStarts.Tick += StartGemEvent;
            GemStarts.Interval = TimeSpan.FromSeconds(18);

            GameTimer.Tick += GameTimerEvent;
            GameTimer.Interval = TimeSpan.FromMilliseconds(1);

            LevelUp.Tick += LevelUpEvent;
            LevelUp.Interval = TimeSpan.FromSeconds(20);

            FallingGem.Tick += FallingGemEvent;
            FallingGem.Interval = TimeSpan.FromMilliseconds(1);

            GemIsActive.Tick += GemActivatePropertyChecker;
            GemIsActive.Interval = TimeSpan.FromSeconds(2);

            AcceleratedBall.Tick += AccelerateEvent;
            AcceleratedBall.Interval = TimeSpan.FromSeconds(15);

            ProgressBar.Tick += DispatcherTimer;
            ProgressBar.Interval = TimeSpan.FromSeconds(1);
        }

        private void StopTimers()
        {
            GameTimer.Stop();
            GemStarts.Stop();
            FallingGem.Stop();
            LevelUp.Stop();
            GemIsActive.Stop();
            AcceleratedBall.Stop();
            ProgressBar.Stop();

        }
        private void StartTimers()
        {
            GemStarts.Start();
            GameTimer.Start();
            LevelUp.Start();
            ProgressBar.Start();

        }

        private void LevelUpEvent(object sender, EventArgs e)
        {
            if (paddle.Width > 20)
            {
                ActualLevel += 1;
                paddle.Width -= 10;
                level.Content = $"Level: {ActualLevel}";
            }
        }

        private void SetGemToStartPosition()
        {
            Canvas.SetLeft(gem, -20);
            Canvas.SetTop(gem, 0);
        }

        private void AccelerateEvent(object sender, EventArgs e)
        {
            if (BallSpeedVertical > 0)
            {
                BallSpeedVertical += 1;
            }else
            {
                BallSpeedVertical -= 1;
            }
            if (BallSpeedHorizontal > 0)
            {
                BallSpeedHorizontal += 1;
            }
            else
            {
                BallSpeedHorizontal -= 1;
            }
            
            
        }

        private void FallingGemEvent(object sender, EventArgs e)
        {
            Canvas.SetTop(gem, Canvas.GetTop(gem) + GemSpeed);
            if (CheckItemMeetWithPaddle(gem) || Canvas.GetTop(gem) + (gem.Height) > Application.Current.MainWindow.Height)
            {
                if (CheckItemMeetWithPaddle(gem))
                {
                    RandomPropertyActivator();
                    GemIsActive.Start();     
                }              
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
            int horizontalPosition = rnd.Next(20, ((int)Application.Current.MainWindow.Width - (int)item.Width * 2));
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

        private void BallMoving()
        {
            Canvas.SetLeft(ball, Canvas.GetLeft(ball) - BallSpeedHorizontal);
            Canvas.SetTop(ball, Canvas.GetTop(ball) + BallSpeedVertical);
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            BallMoving();
            if (Canvas.GetLeft(ball) < 8 || Canvas.GetLeft(ball) + (ball.Width + 20) > Application.Current.MainWindow.Width)
            {
               
                BallSpeedHorizontal = -BallSpeedHorizontal;
                OfficialBallSpeedHorizontal = BallSpeedHorizontal;
            }
            else if (Canvas.GetTop(ball) < 8 || Canvas.GetTop(ball) + (ball.Height + 31) > Application.Current.MainWindow.Height)
            {
                BallSpeedVertical = -BallSpeedVertical;
                OfficialBallSpeedVertical = -OfficialBallSpeedVertical;
            }
            if (Canvas.GetTop(ball) + (ball.Height * 2 + 5) >= Application.Current.MainWindow.Height - 50)
            {
                StopTimers();
                str_button.IsEnabled = false;
                MessageBox.Show("Congratulations! You reached " + Score + " points.");
                
            }
            //if (Canvas.GetTop(ball) + (ball.Height) >= Canvas.GetTop(paddle) && Canvas.GetLeft(ball) >= Canvas.GetLeft(paddle) && Canvas.GetLeft(ball) + ball.Width <= Canvas.GetLeft(paddle)+ paddle.Width)
            if(CheckItemMeetWithPaddle(ball))
            {
                CheckPaddleSideMeetWithBall();
                Score += 1;
                score.Content = Score;
                BallSpeedVertical = -BallSpeedVertical;
                OfficialBallSpeedVertical = -OfficialBallSpeedVertical;

            }
        }

        private void CheckPaddleSideMeetWithBall()
        {
            if (Canvas.GetLeft(ball) + ball.Width < Canvas.GetLeft(paddle) + 20)
            {
                if (BallSpeedHorizontal < 0) { BallSpeedHorizontal = -BallSpeedHorizontal; }
            }
            else if (Canvas.GetLeft(ball) > Canvas.GetLeft(paddle) + paddle.Width - 20)
            {
                if (BallSpeedHorizontal > 0) { BallSpeedHorizontal = -BallSpeedHorizontal; }              
            }
            OfficialBallSpeedHorizontal = -OfficialBallSpeedVertical;
        }
        private bool CheckItemMeetWithPaddle(Rectangle item)
        {
            return (Canvas.GetTop(item) + (item.Height) >= Canvas.GetTop(paddle) - 1 &&
                 Canvas.GetLeft(item) >= Canvas.GetLeft(paddle) - item.Width + 1 &&
                 Canvas.GetLeft(item) - 1 <= Canvas.GetLeft(paddle) + paddle.Width);
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
                StopTimers();
                ShowEscapeMessageBox();
            }
            else if (Keyboard.IsKeyDown(Key.Space)) 
            {
                if (GameTimer.IsEnabled) 
                {
                    StopTimers();
                    ShowSpaceMessageBox();
                }
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
            if (Canvas.GetLeft(paddle) + (paddle.Width + 20)  < Application.Current.MainWindow.Width - 10)
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
            MessageBoxResult result = MessageBox.Show("Press SPACE to continue.", "Pause menu");
            if (result == MessageBoxResult.OK) 
            {
                StartTimers();
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

        private void Str_button_Click(object sender, RoutedEventArgs e)
        {
            if (!GameTimer.IsEnabled) 
            {
                if (intermediate.IsChecked == true)
                {
                    GemSpeed = 2;
                    BallSpeedHorizontal = 2;
                    BallSpeedVertical = 2;
                    paddle.Width = 150;
                }
                else if (expert.IsChecked == true)
                {
                    GemSpeed = 3;
                    BallSpeedHorizontal = 3;
                    BallSpeedVertical = 3;
                    paddle.Width = 100;
                    AcceleratedBall.Start();
                }

                DisapleRadiusButtons();

                StartTimers();
            }
        }

        private void Rst_btn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void Ext_btn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void DisapleRadiusButtons() 
        {
            basic.IsEnabled = false;
            intermediate.IsEnabled = false;
            expert.IsEnabled = false;
        }

        private void DispatcherTimer(object sender, EventArgs e)
        {
            myProgressBar.Value += 5;
            if (myProgressBar.Value >= 100)
            {
                myProgressBar.Value = 0;
            }
        }
    }
}
