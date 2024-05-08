using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Media;

namespace Summative_Project_Square_Chasers
{
    
    public partial class SquareChasers : Form
    {
        //Random generators
        Random randGenX = new Random();
        Random randGenY = new Random();

        //Stopwatches
        Stopwatch stopwatch = new Stopwatch();
        Stopwatch stopwatch2 = new Stopwatch();

        //Drawing the players, game and borders
        Rectangle player1 = new Rectangle(45, 185, 20, 20);
        Rectangle player2 = new Rectangle(530, 185, 20, 20);
        Rectangle point = new Rectangle(385, 100, 15, 15);
        Rectangle speedBoost = new Rectangle(185, 300, 15, 15);
        Rectangle antiBoost = new Rectangle(250, 200, 15, 15);
        Rectangle boundary1 = new Rectangle(15, 70, 565, 10);
        Rectangle boundary2 = new Rectangle(15, 320, 565, 10);
        Rectangle boundary3 = new Rectangle(570, 70, 10, 250);
        Rectangle boundary4 = new Rectangle(15, 70, 10, 250);

        //Variables
        int player1Score = 0;
        int player2Score = 0;
        int player1Speed = 6;
        int player2Speed = 6;
        int antiBoostXSpeed = -5;
        int antiBoostYSpeed = -5;
        int coordinateX = 0;
        int coordinateY = 0;

        //Key commands
        bool wPressed = false;
        bool aPressed = false;
        bool sPressed = false;
        bool dPressed = false;
        bool upPressed = false;
        bool downPressed = false;
        bool leftPressed = false;
        bool rightPressed = false;

        //Brush setups 
        SolidBrush blueBrush = new SolidBrush(Color.DodgerBlue);
        SolidBrush greenBrush = new SolidBrush(Color.Green);
        SolidBrush goldBrush = new SolidBrush(Color.Gold);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush purpleBrush = new SolidBrush(Color.Purple);
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        public SquareChasers()
        {
            InitializeComponent();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //Making the keys work
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = true;
                    break;
                case Keys.S:
                    sPressed = true;
                    break;
                case Keys.A:
                    aPressed = true;
                    break;
                case Keys.D:
                    dPressed = true;
                    break;
                case Keys.Up:
                    upPressed = true;
                    break;
                case Keys.Down:
                    downPressed = true;
                    break;
                case Keys.Left:
                    leftPressed = true;
                    break;
                case Keys.Right:
                    rightPressed = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //Making the keys work
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = false;
                    break;
                case Keys.S:
                    sPressed = false;
                    break;
                case Keys.A:
                    aPressed = false;
                    break;
                case Keys.D:
                    dPressed = false;
                    break;
                case Keys.Up:
                    upPressed = false;
                    break;
                case Keys.Down:
                    downPressed = false;
                    break;
                case Keys.Left:
                    leftPressed = false;
                    break;
                case Keys.Right:
                    rightPressed = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move ball
            antiBoost.X = antiBoost.X + antiBoostXSpeed;
            antiBoost.Y = antiBoost.Y + antiBoostYSpeed;

            //check if it hits the top or bottom of the screen
            if (antiBoost.Y <= 82 || antiBoost.Y >= 300)
            {
                antiBoostYSpeed = -antiBoostYSpeed;
            }

            //check if ball hits right side of the screen
            if (antiBoost.X <= 31 || antiBoost.X >= 543)
            {
                antiBoostXSpeed = -antiBoostXSpeed;
            }

            //Speed limit warning
            if (player1Speed == 12 && warningLabel.Text == "")
            {
                warningLabel.Text = "Don't go too fast player 1";
            }

            if (player2Speed == 12 && warningLabel.Text == "")
            {
                warningLabel.Text = "Don't go too fast player 2";
            }

            //Makes the players speed drop if their speed is too high
            if (player1Speed >= 14)
            {
                warningLabel.Text = "You went too fast player 1";
                stopwatch2.Start();
                this.BackColor = Color.Red;

                if (stopwatch2.ElapsedMilliseconds > 1000)
                {
                    player1Speed = 4;
                    this.BackColor = Color.Black;
                    warningLabel.Text = "";
                    stopwatch2.Reset();
                    stopwatch2.Stop();
                }
            }

            if (player2Speed >= 14)
            {
                warningLabel.Text = "You went too fast player 2";
                stopwatch2.Start();
                this.BackColor = Color.Red;

                if (stopwatch2.ElapsedMilliseconds > 1000)
                {
                    player2Speed = 3;
                    this.BackColor = Color.Black;
                    warningLabel.Text = "";
                    stopwatch2.Reset();
                    stopwatch2.Stop();
                }
            }
            
            //Speed boost code
            if (player1.IntersectsWith(speedBoost))
            {
                player1Speed++;
                playerSpeedBoost();
            }

            if (player2.IntersectsWith(speedBoost))
            {
                player2Speed++;
                playerSpeedBoost();
            }

            if (player1.IntersectsWith(antiBoost))
            {
                player1Speed--;
                playerAntiSpeed();
            }

            if (player2.IntersectsWith(antiBoost))
            {
                player2Speed--;
                playerAntiSpeed();
            }

            //Point code
            if (player1.IntersectsWith(point))
            {
                player1Score++;
                player1ScoreLabel.Text = $"Player 1 Score \n {player1Score}";
                pointMethod();
            }

            if (player2.IntersectsWith(point))
            {
                player2Score++;
                player2ScoreLabel.Text = $"Player 2 Score \n {player2Score}";
                pointMethod();
            }

            //Win condition for players
            if (player1Score == 10)
            {
                warningLabel.Text = "Player 1 Wins!!!";
                winMethod();
            }

            if (player2Score == 10)
            {
                warningLabel.Text = "Player 2 Wins!!!";
                winMethod();
            }

            playerMovement();
            Refresh();
        }

        public void playerSpeedBoost()
        {
            SoundPlayer speedPlayer = new SoundPlayer(Properties.Resources.oogleboogle);
            speedPlayer.Play();

            coordinateX = randGenX.Next(30, 565);
            coordinateY = randGenY.Next(90, 315);
            speedBoost.X = coordinateX;
            speedBoost.Y = coordinateY;
        }

        public void playerAntiSpeed()
        {
            SoundPlayer antiSpeedPlayer = new SoundPlayer(Properties.Resources.slowdown);
            antiSpeedPlayer.Play();

            coordinateX = randGenX.Next(30, 565);
            coordinateY = randGenY.Next(90, 315);
            antiBoost.X = coordinateX;
            antiBoost.Y = coordinateY; 
            
            if (player1Speed == 0)
            {
                player1Speed++;
            }
            if (player2Speed == 0)
            {
                player2Speed++;
            }
        }

        public void pointMethod()
        {
            SoundPlayer pointPlayer = new SoundPlayer(Properties.Resources.betterPoint);
            pointPlayer.Play();

            coordinateX = randGenX.Next(30, 565);
            coordinateY = randGenY.Next(90, 315);
            point.X = coordinateX;
            point.Y = coordinateY;
        }

        public void winMethod()
        {
            SoundPlayer winPlayer = new SoundPlayer(Properties.Resources.gong);
            winPlayer.Play();

           // warningLabel.Visible = true;
            gameTimer.Stop();
        }

        public void playerMovement()
        {
            if (wPressed == true && player1.Y > 82)
            {
                player1.Y = player1.Y - player1Speed;
            }

            if (sPressed == true && player1.Y < 300)
            {
                player1.Y = player1.Y + player1Speed;
            }

            if (aPressed == true && player1.X > 31)
            {
                player1.X = player1.X - player1Speed;
            }

            if (dPressed == true && player1.X < 543)
            {
                player1.X = player1.X + player1Speed;
            }

            if (upPressed == true && player2.Y > 82)
            {
                player2.Y = player2.Y - player2Speed;
            }

            if (downPressed == true && player2.Y < 300)
            {
                player2.Y = player2.Y + player2Speed;
            }

            if (leftPressed == true && player2.X > 31)
            {
                player2.X = player2.X - player2Speed;
            }

            if (rightPressed == true && player2.X < 543)
            {
                player2.X = player2.X + player2Speed;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(blueBrush, player1);
            e.Graphics.FillRectangle(greenBrush, player2);
            e.Graphics.FillRectangle(goldBrush, point);
            e.Graphics.FillRectangle(purpleBrush, speedBoost);
            e.Graphics.FillRectangle(redBrush, antiBoost);
            e.Graphics.FillRectangle(whiteBrush, boundary1);
            e.Graphics.FillRectangle(whiteBrush, boundary2);
            e.Graphics.FillRectangle(whiteBrush, boundary3);
            e.Graphics.FillRectangle(whiteBrush, boundary4);
        }
    }
}
