using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CouplesGame
{
    public partial class Form1 : Form
    {
        private Random random = new Random();
        private List<char> characters = new List<char>();
        private Button firstSelectedButton = null;
        private Button secondSelectedButton = null;
        private Timer checkForMatchTimer;
        private Timer gameTimer;
        private Label timeLabel;
        private int timeElapsed;
        private Timer hideFirstSelectionTimer;

        public Form1()
        {
            InitializeComponent();
            InitializeCharacters();
            InitializeTimer();
            hideFirstSelectionTimer = new Timer
            {
                Interval = 3000,
                Enabled = false
            };
            hideFirstSelectionTimer.Tick += HideFirstSelectionTimer_Tick;
            timeElapsed = 0;
            timeLabel = new Label
            {
                Text = "Segundos: 0",
                Dock = DockStyle.Top
            };
            this.Controls.Add(timeLabel);

            gameTimer = new Timer
            {
                Interval = 1000,
                Enabled = true
            };
            gameTimer.Tick += GameTimer_Tick;
            for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
            {
                for (int j = 0; j < tableLayoutPanel1.ColumnCount; j++)
                {
                    Button button = new Button
                    {
                        Dock = DockStyle.Fill,
                        Font = new Font("Webdings", 48F, FontStyle.Bold),
                        Tag = GetRandomCharacter()
                    };
                    button.Click += Button_Click;
                    tableLayoutPanel1.Controls.Add(button, j, i);
                }
            }
        }

        private void InitializeTimer()
        {
            checkForMatchTimer = new Timer
            {
                Interval = 1000,
                Enabled = false
            };
            checkForMatchTimer.Tick += CheckForMatchTimer_Tick;
        }

        private void CheckForMatchTimer_Tick(object sender, EventArgs e)
        {
            HideIfNotMatch();
            checkForMatchTimer.Stop();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            if (clickedButton == firstSelectedButton || clickedButton == secondSelectedButton)
            {
                return;
            }
            clickedButton.Text = clickedButton.Tag.ToString();
            if (firstSelectedButton == null)
            {
                firstSelectedButton = clickedButton;
                hideFirstSelectionTimer.Start();
                return;
            }
            secondSelectedButton = clickedButton;
            hideFirstSelectionTimer.Stop();
            if (!IsMatch())
            {
                checkForMatchTimer.Start();
            }
            else
            {
                ResetSelection();
            }
        }

        private bool IsMatch()
        {
            return firstSelectedButton.Tag.ToString() == secondSelectedButton.Tag.ToString();
        }

        private void HideIfNotMatch()
        {
            if (!IsMatch())
            {
                firstSelectedButton.Text = "";
                secondSelectedButton.Text = "";
            }
            ResetSelection();
        }

        private void ResetSelection()
        {
            firstSelectedButton = null;
            secondSelectedButton = null;
        }
        private char GetRandomCharacter()
        {
            int index = random.Next(characters.Count);
            char c = characters[index];
            characters.RemoveAt(index);
            return c;
        }
        private char GetRandomChar()
        {
            int num = random.Next(0, 26);
            char letter = (char)('A' + num);
            return letter;
        }
        private void InitializeCharacters()
        {
            for (int i = 0; i < 8; i++)
            {
                char c = GetRandomChar();
                characters.Add(c);
                characters.Add(c);
            }
        }
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            timeElapsed++;
            timeLabel.Text = "Segundos: " + timeElapsed;
        }
        private void StartGame()
        {
            timeElapsed = 0;
            gameTimer.Start();
        }

        private void EndGame()
        {
            gameTimer.Stop();
        }
        private void HideFirstSelectionTimer_Tick(object sender, EventArgs e)
        {
            if (firstSelectedButton != null)
            {
                firstSelectedButton.Text = "";
                ResetSelection();
            }
            hideFirstSelectionTimer.Stop();
        }
    }
}
