using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pacmanGame
{
    public partial class Form1 : Form
    {
        //control variables
        bool wPressed = false;
        bool aPressed = false;
        bool sPressed = false;
        bool dPressed = false;

        bool spacePressed = false;
        bool escapePressed = false;

        bool isGameOver;

        //game ints
        int pacmanSpeed = 8;
        int score;
        int orangeGhostSpeed;
        int redGhostSpeed;
        int pinkGhostSpeed;

        //array to keep high scores
        int[] Level1Scores = new int[10];
        int[] Level2Scores = new int[10];
        int[] Level3Scores = new int[10];

        //randgen for ghost speeds
        Random randGen = new Random();

        public Form1()
        {
            InitializeComponent();

            resetGame();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = false;
                    break;
                case Keys.A:
                    aPressed = false;
                    break;
                case Keys.S:
                    sPressed = false;
                    break;
                case Keys.D:
                    dPressed = false;
                    break;

                case Keys.Space:
                    spacePressed = false;
                    break;

                case Keys.Escape:
                    escapePressed = false;
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = true;
                    break;
                case Keys.A:
                    aPressed = true;
                    break;
                case Keys.S:
                    sPressed = true;
                    break;
                case Keys.D:
                    dPressed = true;
                    break;

                case Keys.Space:
                    spacePressed = true;
                    break;

                case Keys.Escape:
                    escapePressed = true;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {

            PlayerMovement();
            PlayerIntersections();
            CheckForWin();
            GhostMovement();            

            scoreLabel.Text = $"SCORE: {score}";

            Refresh();
        }

        public void resetGame()
        {
            score = 0;

            scoreLabel.Text = $"SCORE: {score}";

            isGameOver = false;


            gameTimer.Start();

            foreach(Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    x.Visible = true;
                }
            }
        }
        

        public void PlayerMovement()
        {
            //move player
            if (wPressed == true)
            {
                pacman.Top = pacman.Top - pacmanSpeed;
                pacman.Image = Properties.Resources.Up;
            }
            if (sPressed == true)
            {
                pacman.Top = pacman.Top + pacmanSpeed;
                pacman.Image = Properties.Resources.down;
            }
            if (dPressed == true)
            {
                pacman.Left = pacman.Left + pacmanSpeed;
                pacman.Image = Properties.Resources.right;
            }
            if (aPressed == true)
            {
                pacman.Image = Properties.Resources.left;
                pacman.Left = pacman.Left - pacmanSpeed;
            }

            //appear on other side (left and right)
            if (pacman.Left < -30)
            {
                pacman.Left = 680;
            }
            if (pacman.Left > 680)
            {
                pacman.Left = -30;
            }

            //appear on other side (up and down)
            if (pacman.Top < -30)
            {
                pacman.Top = 550;
            }
            if (pacman.Top > 620)
            {
                pacman.Top = -30;
            }
        }
        public void PlayerIntersections()
        {
            //intersects with coin
            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag == "coin")
                {
                    if (pacman.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                    {
                        score++;
                        x.Visible = false;
                    }
                }
            }

            //intersects with wall

            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag == "wall")
                {
                    if (pacman.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                    {
                        gameOver("YOU LOSE!");
                    }
                }
            }

            //intersects with ghost

            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag == "ghost")
                {
                    if (pacman.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                    {
                        gameOver("YOU LOSE!");
                    }
                }
            }
        }
        public void CheckForWin()
        {
            //check if won
            if (score == 50)
            {
                gameOver("YOU WIN!");
            }
        }
        public void GhostMovement()
        {
            int redGhostX = redGhost.Location.X;
            int pinkGhostX = pinkGhost.Location.X;

            orangeGhost.Left = orangeGhost.Left + orangeGhostSpeed;
            pinkGhost.Left = pinkGhost.Left + pinkGhostSpeed;
            redGhost.Left = redGhost.Left + redGhostSpeed;

            if (orangeGhostSpeed == 0)
            {
                orangeGhostSpeed = randGen.Next(0, 10);
            }

            if (pinkGhostSpeed == 0)
            {
                pinkGhostSpeed = randGen.Next(0, 10);
            }

            if (redGhostSpeed == 0)
            {
                redGhostSpeed = randGen.Next(0, 10);
            }

            //orange ghost movement
            //appear on other side (right)
            if (orangeGhost.Left > 680)
            {
                orangeGhost.Left = -30;
                orangeGhostSpeed = randGen.Next(1, 10);
            }

            //pink ghost movement
            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag == "wall")
                {
                    if (pinkGhost.Bounds.IntersectsWith(x.Bounds))
                    {
                        pinkGhostSpeed = -pinkGhostSpeed;
                        pinkGhost.Left = pinkGhostX;
                        if (pinkGhostSpeed < 0)
                        {
                            pinkGhostSpeed = -randGen.Next(1, 10);
                        }
                        else
                        {
                            pinkGhostSpeed = randGen.Next(1, 10);
                        }
                    }
                }
            }

            //red ghost movement
            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag == "wall")
                {
                    if (redGhost.Bounds.IntersectsWith(x.Bounds))
                    {
                        redGhostSpeed = -redGhostSpeed;
                        redGhost.Left = redGhostX;
                        if (redGhostSpeed < 0)
                        {
                            redGhostSpeed = -randGen.Next(1, 10);
                        }
                        else
                        {
                            redGhostSpeed = randGen.Next(1, 10);
                        }
                    }
                }
            }
        }

        public void EasyLevel()
        {
            isGameOver = false;

            pacman.Left = 21;
            pacman.Top = 46;

            redGhost.Left = 370;
            redGhost.Top = 60;

            orangeGhost.Left = 315;
            orangeGhost.Top = 268;

            pinkGhost.Left = 457;
            pinkGhost.Top = 476;

            wall1.Left = 125;
            wall1.Top = 0;

            wall2.Left = 439;
            wall2.Top = 0;

            wall3.Left = 202;
            wall3.Top = 422;

            wall3.Left = 517;
            wall3.Top = 422;
        }

        public void gameOver(string message)
        {
            gameTimer.Stop();

            if (message == "YOU WIN!")
            {
                winLoseLabel.Text = "YOU WIN!";
            }
            if (message == "YOU LOSE!")
            {
                winLoseLabel.Text = "YOU LOSE!";
            }
        }
    }
}
