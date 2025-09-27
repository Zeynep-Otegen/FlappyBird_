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
        int speed = 8;  //oyun hızı
        int score = 0;  //puan
        int gravity = 1; //yerçekimi
        int velocity = 0; //kuşun düşme hızı
        
        Random rand = new Random();  
        
        bool isGameOver = false;   //oyun bitiş kontrolü
        bool isGameStarted = false; //oyun başlangıç kontrolü
        int birdFrame = 0;  //kuşun kanat çırpma animasyonu için sayıcı
        Image[] birdFrames; //kuşun resimleri
        Timer birdFlapTimer = new Timer(); //kanat çırpma için timer

        public Form1()
        {
            InitializeComponent(); 
            this.KeyDown += new KeyEventHandler(gameisdown); //space tuşu ile başlatma ve zıplama

            
            birdFrames = new Image[]  //kuş resim dizisi
            {
                Properties.Resources.flappybird0,
                Properties.Resources.flappybird1, //kanat çırpma animasyonu için resimler
                Properties.Resources.flappybird2
            };

            birdFlapTimer.Interval = 100;  //kanat çırpma hızı (resimleri değiştirme)
            birdFlapTimer.Tick += BirdFlapTimer_Tick; //kanat çırpma metodu
            birdFlapTimer.Start(); //kanat çırpmayı başlatır

            
            gameTimer.Stop(); //oyun başta duruyor
            scoreText.Text = "Başlamak için SPACE tuşuna basın"; 
        }

        private void BirdFlapTimer_Tick(object sender, EventArgs e) 
        {
            birdFrame = (birdFrame + 1) % birdFrames.Length; //döngüsel bir yöntem ile resimler değişir
            bird.Image = birdFrames[birdFrame]; //kuşun resmini değiştirir
        }

        private void gameisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)  //space tuşuna basınca;
            {
                if (!isGameStarted)                  
                {
                    StartGame();       //oyun başlamadıysa başlat,
                }
                else if (!isGameOver)   //oyun başladıysa ve bitmediyse zıplat,
                {
                    velocity = -10;  //yukarı zıplama hızı,
                }
                else if (isGameOver)  //oyun bitiityse yeniden başlat.
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
            bird.Top = 200; //kuşun başlangıç konumu
            pipeDown.Left = 800;  //boruların başlangıç konumu
            pipeUp.Left = 800;
            resetPipes();  //boruları resetle metodu
            scoreText.Text = "SCORE: 0";
            gameTimer.Start(); //oyun zamanlayıcısı başlar
        }

        private void gameTimerEvent(object sender, EventArgs e)
        {
            velocity += gravity; 
            bird.Top += velocity; 

            pipeDown.Left -= speed; 
            pipeUp.Left -= speed;

            scoreText.Text = "SCORE: " + score;
           
            if (pipeDown.Left < -150) //borular ekran dışına çıkınca
            {
                resetPipes(); //boru resetleme metodu çalışır
                score++; //score her geçişte artar 
            }

            Rectangle birdHitbox = new Rectangle(  //kuşun çarpışma alanını azaltma
                bird.Left + 5, //soldan 5 piksel içeri
                bird.Top + 5, //üstten 5 piksel içeri
                bird.Width - 10, //genişlikten 10 piksel eksi
                bird.Height - 10); //yükeeklikten 10 piksel eksi

            if (birdHitbox.IntersectsWith(pipeDown.Bounds) ||
                birdHitbox.IntersectsWith(pipeUp.Bounds) ||    //eğer üst boruya,alt boruya,zemine ya da yukarı çarparsa oyun biter
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

            int pipeHeight = rand.Next(200, 400);  //rastgele random sınıftan boru uzunlukları atar

            pipeDown.Top = pipeHeight; //aşağıdaki boruya yeni yüksekliği atar
            pipeUp.Top = pipeHeight - pipeUp.Height - 150; //yukarıdaki boruya yeni yükseklği atar (borular arası boşluk 150 piksel)
        }

        private void endGame() //oyun bitiş ve skoru gösterme 
        {
            gameTimer.Stop();
            isGameOver = true;
            scoreText.Text = "Game Over!!\nFinal Score:" + score;
        }

        private void RestartGame() //oyunu yeniden başlatma 
        {
            isGameStarted = false;
            scoreText.Text = "Başlamak için SPACE tuşuna basın";
            // Oyun tekrar başlatılacak, space tuşuna basınca StartGame çağrılır
        }

       
    }
}

