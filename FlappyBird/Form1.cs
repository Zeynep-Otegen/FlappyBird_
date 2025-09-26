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
        int speed = 8;
        int score = 0;
        int gravity = 1;
        int velocity = 0;
        Random rand = new Random();
        bool isGameOver = false;
        bool isGameStarted = false;
        int birdFrame = 0;
        Image[] birdFrames;
        Timer birdFlapTimer = new Timer();

        public Form1()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(gameisdown);

            // Kuş kanat çırpıyor
            birdFrames = new Image[]
            {
                Properties.Resources.flappybird0,
                Properties.Resources.flappybird1,
                Properties.Resources.flappybird2
            };

            birdFlapTimer.Interval = 100; // 100ms'de bir frame değiştir
            birdFlapTimer.Tick += BirdFlapTimer_Tick;
            birdFlapTimer.Start();

            // Oyun başta duruyor
            gameTimer.Stop();
            scoreText.Text = "Başlamak için SPACE tuşuna basın";
        }

        private void BirdFlapTimer_Tick(object sender, EventArgs e)
        {
            birdFrame = (birdFrame + 1) % birdFrames.Length;
            bird.Image = birdFrames[birdFrame];
        }

        private void gameisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (!isGameStarted)
                {
                    StartGame();
                }
                else if (!isGameOver)
                {
                    velocity = -10;
                }
                else if (isGameOver)
                {
                    RestartGame();
                }
            }
        }

        private void StartGame()
        {
            isGameStarted = true;
            isGameOver = false;
            score = 0;
            velocity = 0;
            bird.Top = 200;
            pipeDown.Left = 800;
            pipeUp.Left = 800;
            resetPipes();
            scoreText.Text = "SCORE: 0";
            gameTimer.Start();
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

            Rectangle birdHitbox = new Rectangle(
                bird.Left + 5,
                bird.Top + 5,
                bird.Width - 10,
                bird.Height - 10);

            if (birdHitbox.IntersectsWith(pipeDown.Bounds) ||
                birdHitbox.IntersectsWith(pipeUp.Bounds) ||
                birdHitbox.IntersectsWith(ground.Bounds) ||
                bird.Top < 0)
            {
                endGame();
            }
        }

        private void resetPipes()
        {
            pipeDown.Left = 800;
            pipeUp.Left = 800;

            int pipeHeight = rand.Next(200, 400);

            pipeDown.Top = pipeHeight;
            pipeUp.Top = pipeHeight - pipeUp.Height - 150;
        }

        private void endGame()
        {
            gameTimer.Stop();
            isGameOver = true;
            scoreText.Text = "Game Over!!\nFinal Score:" + score;
        }

        private void RestartGame()
        {
            isGameStarted = false;
            scoreText.Text = "Başlamak için SPACE tuşuna basın";
            // Oyun tekrar başlatılacak, space tuşuna basınca StartGame çağrılır
        }

        private void scoreText_Click(object sender, EventArgs e)
        {

        }
    }
}

