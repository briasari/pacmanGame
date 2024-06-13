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
        bool wPressed = false;
        bool aPressed = false;
        bool sPressed = false;
        bool dPressed = false;

        bool spacePressed = false;
        bool escapePressed = false;

        bool isGameOver;

        int pacmanSpeed = 10;
        int score;
        int orangeGhostSpeed;
        int redGhostSpeed;
        int pinkGhostSpeed;

        List<PictureBox> ghostXList = new List<PictureBox>();
        List<PictureBox> ghostYList = new List<PictureBox>();


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


            //intersects with coin
            foreach(Control x in this.Controls)
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

            //check if won
            if (score == 50)
            {
                gameOver("YOU WIN!");
            }


            //ghost movement
            

                scoreLabel.Text = $"SCORE: {score}";

            Refresh();
        }

        private void resetGame()
        {
            score = 0;

            scoreLabel.Text = $"SCORE: {score}";

            isGameOver = false;

            pacman.Left = 21;
            pacman.Top = 46;

            redGhost.Left = 370;
            redGhost.Top = 60;

            orangeGhost.Left = 315;
            orangeGhost.Top = 268;

            pinkGhost.Left = 457;
            pinkGhost.Top = 476;

            gameTimer.Start();

            foreach(Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    x.Visible = true;
                }
            }
        }

        private void gameOver(string message)
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
