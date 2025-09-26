using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlappyBird
{
    public partial class Form1 : Form
    {
        int speed=8;
        int score = 0;
        int gravity = 2;
        int velocity = 0;
        Random rand = new Random();
        public Form1()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(gameisdown);
            
        }
         private void gameisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                velocity = -10;
            }
        }

        private void gameTimerEvent(object sender, EventArgs e)
        {
            velocity += gravity;       
            bird.Top += velocity;

            pipeDown.Left -= speed;
            pipeUp.Left -= speed;

            scoreText.Text = "SCORE: " + score;
           
            if (pipeDown.Left < -150)
            {
                resetPipes();
                score++;
            }
            
            if (bird.Bounds.IntersectsWith(pipeDown.Bounds) || bird.Bounds.IntersectsWith(pipeUp.Bounds) || bird.Bounds.IntersectsWith(ground.Bounds) || bird.Top < 0)
            {
                endGame();
            }
        }
        private void resetPipes()
        {
            pipeDown.Left = 800;
            pipeUp.Left = 800;

            // Rastgele yükseklik oluştur
            int pipeHeight = rand.Next(200, 400);

            pipeDown.Top = pipeHeight;
            pipeUp.Top = pipeHeight - pipeUp.Height - 150; // Borular arası boşluk
        }
        private void endGame()
        {
            gameTimer.Stop();
            scoreText.Text = "Game Over!!\nFinal Score:"+score ;
           
        }

        private void scoreText_Click(object sender, EventArgs e)
        {

        }
    }
    }

