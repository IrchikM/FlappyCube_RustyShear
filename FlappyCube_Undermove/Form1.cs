using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlappyCube_Undermove
{
    public partial class Form1 : Form
    {
        // Сила тяжести. 
        //Чем она выше тем быстрее игрок набирает скорость
        const int gravity = 1;
        // Толщина линии обводки
        const int minWidth = 2;

        // Переменная которая хранит в себе текщий кадр
        Bitmap bmp;
        // Определяет стиль контуров
        Pen pen;
        // Генерирует высоту появления труб
        Random rnd = new Random();
        // Шрифт очков в правом верхнем углу
        Font f = SystemFonts.DefaultFont;

        // Описание игрока
        Rectangle player;
        // Скорость игрока
        int playerVelocity = 0;
        // Количество набранных очков
        int score = 0;

        bool isGlowingOn = true;

        // Трубы
        Rectangle tube1;
        Rectangle tube2;
        Rectangle tube3;
        Rectangle tube4;
        Rectangle tube5;
        Rectangle tube6;
        // Расстояние между трубами
        int space = 150;
        // Скорость движения труб
        int tubesVelocity = -3;

        SoundPlayer soundPlayer;

        public Form1()
        {
            InitializeComponent();
            // Создаем кадр
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            // Задаем стиль линий и цвет
            pen = new Pen(Brushes.Aqua);
            // Размещаем игрока и задаем размеры
            player = new Rectangle(30, 30, 30, 30);
            // размещаем трубы так чтобы верние трубы 
            // располагались относительно нижних труб
            // на space пикселей выше
            tube1 = new Rectangle(500, 300, 80, 500);
            tube2 = new Rectangle(tube1.X, tube1.Y - tube1.Height - space, 80, 500);
            tube3 = new Rectangle(700, 400, 80, 500);
            tube4 = new Rectangle(tube3.X, tube3.Y - tube3.Height - space, 80, 500);
            tube5 = new Rectangle(900, 200, 80, 500);
            tube6 = new Rectangle(tube5.X, tube5.Y - tube5.Height - space, 80, 500);
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream resourceStream = assembly.GetManifestResourceStream(@"FlappyCube_Undermove.Music_RustyShear.wav");
            soundPlayer = new SoundPlayer(resourceStream);
            soundPlayer.PlayLooping();

        }

        // Главный цикл игры, 
        // в нем происходит отрисовка и игровая логика
        private void timer1_Tick(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);

            if(pen.Width > minWidth)
            {
                pen.Width--;
            }

            Draw(g);

            pictureBox1.Image = bmp;
            g.Dispose();
        }

        // Метод отрисовки. 
        // Если мы хотим, чтобы что-то отобразилось на экране,
        // то мы должны добавить в него соотвествующую строку.
        private void Draw(Graphics g)
        {
           
            g.DrawRectangle(pen, player);
            g.DrawRectangle(pen, tube1);
            g.DrawRectangle(pen, tube2);
            g.DrawRectangle(pen, tube3);
            g.DrawRectangle(pen, tube4);
            g.DrawRectangle(pen, tube5);
            g.DrawRectangle(pen, tube6);
            g.DrawString(score.ToString(), f, Brushes.White, 400, 20);
        }

        private void TubesLogic()
        {
            // Двигаем первую пару труб
            tube1.X += tubesVelocity;
            tube2.X = tube1.X;

            // Двигаем вторую пару труб
            tube3.X += tubesVelocity;
            tube4.X = tube3.X;

            // Двигаем третью пару труб
            tube5.X += tubesVelocity;
            tube6.X = tube5.X;

            // Возвращаем назад первую пару труб
            if (tube1.Right <= 0)
            {
                tube1.X = pictureBox1.Right;
                tube1.Y = rnd.Next(200, 450);
                tube2.Y = tube1.Y - tube1.Height - space;
            }

            // Возвращаем назад вторую пару труб
            if (tube3.Right <= 0)
            {
                tube3.X = pictureBox1.Right;
                tube3.Y = rnd.Next(200, 450);
                tube4.Y = tube3.Y - tube3.Height - space;
            }

            // Возвращаем назад третью пару труб
            if (tube5.Right <= 0)
            {
                tube5.X = pictureBox1.Right;
                tube5.Y = rnd.Next(200, 450);
                tube6.Y = tube5.Y - tube5.Height - space;
            }
        }

        private void PlayerLogic()
        {
            // Добавляем очки игроку
            score++;

            // Алгоритм движения игрока
            // Скорость увеличвается в 
            // зависимости от величины гравитации
            // Игрок за один тик таймера пермещается на 
            // расстояние равное скорости
            playerVelocity += gravity;
            player.Y += playerVelocity;

            // Если игрок столкнулся с нижней частью, 
            // то переместить его наверх и сбросить скорость
            // иначе, если игрок столкнулся с верхней частью, 
            // то погасить скорость и не дать ему выйти за пределы.
            if (player.Bottom > pictureBox1.Bottom)
            {
                player.Y = 0;
                playerVelocity = 0;
                score = 0;
            }
            else if (player.Y < 0)
            {
                player.Y = 0;
                playerVelocity = 0;
            }

            // Логика столновений перовй пары
            //////////////////////////////////
            if(player.Right >= tube1.Left && 
               player.Bottom > tube1.Top &&
               player.Left <= tube1.Right)
            {
                player.Y = 0;
                playerVelocity = 0;
                score = 0;
            }

            if (player.Right >= tube2.Left &&
                player.Top <= tube2.Bottom &&
                player.Left <= tube2.Right)
            {
                player.Y = 0;
                playerVelocity = 0;
                score = 0;
            }
            /////////////////////////////////////

            // Логика столкновений второй пары труб
            /////////////////////////////////////
            if (player.Right >= tube3.Left &&
                player.Bottom > tube3.Top &&
                player.Left <= tube3.Right)
            {
                player.Y = 0;
                playerVelocity = 0;
                score = 0;
            }

            if (player.Right >= tube4.Left &&
                player.Top <= tube4.Bottom &&
                player.Left <= tube4.Right)
            {
                player.Y = 0;
                playerVelocity = 0;
                score = 0;
            }
            ///////////////////////////////////

            // Логика столкновений третьей пары труб
            /////////////////////////////////////
            if (player.Right >= tube5.Left &&
                player.Bottom > tube5.Top &&
                player.Left <= tube5.Right)
            {
                player.Y = 0;
                playerVelocity = 0;
                score = 0;
            }

            if (player.Right >= tube6.Left &&
                player.Top <= tube6.Bottom &&
                player.Left <= tube6.Right)
            {
                player.Y = 0;
                playerVelocity = 0;
                score = 0;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            // При нажатии на пробел запускаем игру и 
            // придаем игроку ускорение вверх
            if(e.KeyCode == Keys.Space)
            {
                
                pen = new Pen(Brushes.Aqua);
                if (isGlowingOn)
                {
                    pen.Width = 10;
                    
                }
                playerVelocity -= 20;
                drawTimer.Start();
                playerTimer.Start();
                tubesTimer.Start();
                
            }
            else if(e.KeyCode == Keys.Escape)
            {
                drawTimer.Enabled = !drawTimer.Enabled;
                playerTimer.Enabled = !playerTimer.Enabled;
                tubesTimer.Enabled = !tubesTimer.Enabled;
                button1.Visible = !button1.Visible;
                button2.Visible = !button2.Visible;
                button3.Visible = !button3.Visible;
                button4.Visible = !button4.Visible;
                button1.Enabled = !button1.Enabled;
                button2.Enabled = !button2.Enabled;
                button3.Enabled = !button3.Enabled;
                button4.Enabled = !button4.Enabled;
            }
            else if(e.KeyCode == Keys.L)
            {
                drawTimer.Stop();
                playerTimer.Stop();
                tubesTimer.Stop();
                new LeaderBoardForm(score).Show();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Отрисовываем первый кадр, 
            // чтобы экран не был пустым на старте
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);

            Draw(g);

            pictureBox1.Image = bmp;
            g.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            drawTimer.Enabled = true;
            playerTimer.Enabled = true;
            tubesTimer.Enabled = true;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            drawTimer.Stop();
            new LeaderBoardForm(score).Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            drawTimer.Stop();
            new SettingsForm().Show();
        }

        private void tubesTimer_Tick(object sender, EventArgs e)
        {
            TubesLogic();
        }

        private void playerTimer_Tick(object sender, EventArgs e)
        {
            PlayerLogic();
        }

        private void settingsUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                string[] settings;
                settings = File.ReadAllLines("settings");
                if (settings.Length >= 3)
                {
                    playerTimer.Interval = Convert.ToInt32(settings[0]);
                    tubesTimer.Interval = Convert.ToInt32(settings[1]);
                    isGlowingOn = Convert.ToBoolean(settings[2]);
                }
            }
            catch (FileNotFoundException)
            {
                File.Create("settings");
                
                
            }
            catch (Exception)
            {

            }
            
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space)
            {
                pen = new Pen(Brushes.MediumVioletRed);
            }
            
        }
    }
}
