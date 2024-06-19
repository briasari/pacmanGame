﻿using System;
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
        double[] Level1Scores = new double[10];
        double[] Level2Scores = new double[10];
        double[] Level3Scores = new double[10];

        //randgen for ghost speeds
        Random randGen = new Random();

        //screen number
        int screenNum = 0;

        public Form1()
        {
            InitializeComponent();

            backgroundTimer.Enabled = true;
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


            foreach (Control x in this.Controls)
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
            scoreLabel.Left = 13;
            scoreLabel.Top = 13;

            isGameOver = false;

            pacman.Left = 36;
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

            wall4.Left = 517;
            wall4.Top = 422;

            //coin placements

            //pacman coin
            coin50.Left = 48;
            coin50.Top = 142;

            //red ghost coins
            coin1.Left = 201;
            coin1.Top = 18;

            coin2.Left = 258;
            coin2.Top = 18;

            coin3.Left = 318;
            coin3.Top = 18;

            coin4.Left = 376;
            coin4.Top = 18;

            coin5.Left = 204;
            coin5.Top = 142;

            coin6.Left = 261;
            coin6.Top = 142;

            coin7.Left = 332;
            coin7.Top = 142;

            coin8.Left = 379;
            coin8.Top = 142;

            //top right corner
            coin9.Left = 521;
            coin9.Top = 18;

            coin10.Left = 582;
            coin10.Top = 18;

            coin11.Left = 639;
            coin11.Top = 18;

            coin12.Left = 521;
            coin12.Top = 80;

            coin13.Left = 582;
            coin13.Top = 80;

            coin14.Left = 639;
            coin14.Top = 80;

            coin15.Left = 521;
            coin15.Top = 142;

            coin16.Left = 582;
            coin16.Top = 142;

            coin17.Left = 639;
            coin17.Top = 142;

            //middle row (orange ghost coins)
            coin18.Left = 65;
            coin18.Top = 286;

            coin19.Left = 131;
            coin19.Top = 286;

            coin20.Left = 197;
            coin20.Top = 286;

            coin21.Left = 263;
            coin21.Top = 286;

            coin22.Left = 322;
            coin22.Top = 286;

            coin23.Left = 385;
            coin23.Top = 286;

            coin24.Left = 451;
            coin24.Top = 286;

            coin25.Left = 517;
            coin25.Top = 286;

            coin26.Left = 583;
            coin26.Top = 286;

            //bottom left corner
            coin27.Left = 28;
            coin27.Top = 432;

            coin28.Left = 89;
            coin28.Top = 432;

            coin29.Left = 146;
            coin29.Top = 432;

            coin30.Left = 28;
            coin30.Top = 494;

            coin31.Left = 89;
            coin31.Top = 494;

            coin32.Left = 146;
            coin32.Top = 494;

            coin33.Left = 28;
            coin33.Top = 556;

            coin34.Left = 89;
            coin34.Top = 556;

            coin35.Left = 146;
            coin35.Top = 556;

            //pink ghost coins
            coin36.Left = 285;
            coin36.Top = 432;

            coin37.Left = 342;
            coin37.Top = 432;

            coin38.Left = 403;
            coin38.Top = 432;

            coin39.Left = 460;
            coin39.Top = 432;

            coin40.Left = 288;
            coin40.Top = 556;

            coin41.Left = 345;
            coin41.Top = 556;

            coin42.Left = 406;
            coin42.Top = 556;

            coin43.Left = 463;
            coin43.Top = 556;

            //rightmost coins
            coin44.Left = 590;
            coin44.Top = 432;

            coin45.Left = 647;
            coin45.Top = 432;

            coin46.Left = 590;
            coin46.Top = 495;

            coin47.Left = 647;
            coin47.Top = 495;

            coin48.Left = 590;
            coin48.Top = 556;

            coin49.Left = 647;
            coin49.Top = 556;
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

        public void ScreenChange()
        {
            if (spacePressed)
            {
                screenNum++;
            }
            subtitleLabel.Text = "PRESS SPACE TO PROCEED\nOR PRESS ESC TO EXIT";
            if (spacePressed == true && screenNum == 0)
            {
                screenNum = 1;
            }
            if (screenNum == 1)
            {
                titleLabel.Left = 2;
                titleLabel.Top = 3;

                subtitleLabel.Left = 162;
                subtitleLabel.Top = 110;

                subtitleLabel.Text = "LEVEL SELECTION";

                easyLabel.Visible = true;
                mediumLabel.Visible = true;
                hardLabel.Visible = true;

                easyButton.Visible = true;
                mediumButton.Visible = true;
                hardButton.Visible = true;
            }
            if (spacePressed == true && screenNum == 2)
            {
                screenNum = 3;

                if (screenNum == 3)
                {
                    instructionLabel.Visible = false;
                    titleLabel.Visible = false;
                    subtitleLabel.Visible = false;

                    resetGame();
                    EasyLevel();
                }
            }

            if (escapePressed == true)
            {
                Application.Exit();
            }
        }

        private void easyButton_Click(object sender, EventArgs e)
        {
            screenNum = 2;

            if (screenNum == 2)
            {
                titleLabel.Text = "BEFORE YOU BEGIN:";
                instructionLabel.Visible = true;
                subtitleLabel.Top = 500;

                easyLabel.Visible = false;
                mediumLabel.Visible = false;
                hardLabel.Visible = false;

                easyButton.Visible = false;
                mediumButton.Visible = false;
                hardButton.Visible = false;
            }
        }
            private void backgroundTimer_Tick(object sender, EventArgs e)
            {
                ScreenChange();

                Refresh();
            }
        }
    }
