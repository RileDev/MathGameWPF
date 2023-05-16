using MathGame.Classes;
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

namespace MathGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameManager mgr;
        bool isStarted;
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            mgr = new GameManager(Difficulty.Easy, Operator.Multiply);
            mgr.RestartQuestion();
            lblQuestion.Content = "Click Generate";
            isStarted = false;
            lblMessage.Content = String.Empty;
            lblMessage.Foreground = new SolidColorBrush(Colors.White);
            btnGenerate.IsEnabled = true;
            btnCheck.IsEnabled = false;
            pbGuess.Value = mgr.Guesses;
            tbUserInput.Clear();
            ToggleButtonVisibility(isStarted);
            UpdateDifficultySettings();
            UpdateOperatorSettings();
        }

        private void ToggleButtonVisibility(bool visible)
        {
            //Operator buttons
            btnMultiply.IsEnabled = visible;
            btnDivide.IsEnabled = visible;
            btnSubstract.IsEnabled = visible;
            btnAdd.IsEnabled = visible;
            btnCheck.IsEnabled = visible;
            //Difficulty Buttons
            btnDiffEasy.IsEnabled = !visible;
            btnDiffMedium.IsEnabled = !visible;
            btnDiffHard.IsEnabled = !visible;
        }

        private void UpdateOperatorSettings()
        {
            lblOperatorChosen.Content = mgr.Operator.ToString();
        }

        private void UpdateDifficultySettings()
        {
            lblDifficultyChosen.Content = mgr.Difficulty.ToString();
            lblTrials.Content = $"Trials: {mgr.Trials.ToString()}";
            pbGuess.Maximum = mgr.MaxGuesses;
        }

        private void btnDiffEasy_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button)
            {
                Button btn = (Button)sender;
                mgr.Difficulty = (Difficulty)Int32.Parse(btn.Uid);
                UpdateDifficultySettings();
            }
        }

        private void btnMultiply_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                mgr.Operator = (Operator)Int32.Parse(btn.Uid);
                lblQuestion.Content = mgr.DisplayQuestion();
                UpdateOperatorSettings();
                GenerateUserQuestion();
            }
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            isStarted = true;
            GenerateUserQuestion();
            ToggleButtonVisibility(isStarted);
        }

        private void GenerateUserQuestion()
        {
            mgr.GenerateQuestion();
            lblQuestion.Content = mgr.DisplayQuestion();
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            if (mgr.CheckAnswer(tbUserInput.Text))
            {
                CorrectAnswer();
            }
            else
            {
                WrongAnswer();
            }
        }

        private void WrongAnswer()
        {
            if(mgr.Trials > 0)
            {
                mgr.RemoveTrial();
                GenerateUserQuestion();
                lblTrials.Content = $"Trials: {mgr.Trials}";
                tbUserInput.Clear();
            }

            if(mgr.Trials == 0)
            {
                lblMessage.Content = "You Lost";
                lblMessage.Foreground = new SolidColorBrush(Colors.Red);
                btnGenerate.IsEnabled = false;
            }
        }

        private void CorrectAnswer()
        {
            if(mgr.Guesses != mgr.MaxGuesses - 1)
            {
                mgr.AddGuess(); 
                GenerateUserQuestion();
                pbGuess.Value = mgr.Guesses;
                tbUserInput.Clear();
            }
            else
            {
                pbGuess.Value++;
                lblMessage.Content = "You Won";
                lblMessage.Foreground = new SolidColorBrush(Colors.Green);
                btnGenerate.IsEnabled = false;
            }
        }

        private void tbUserInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnCheck.IsEnabled = (tbUserInput.Text.Length > 0);
        }
    }
}
