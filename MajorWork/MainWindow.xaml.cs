﻿using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MajorWork.ViewModels;

namespace MajorWork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainWindowViewModel _mainWindow;
        private int _userLength;
        private readonly BackgroundWorker _worker;

        public MainWindow()
        {
            var mainWindow = new MainWindowViewModel();
            InitializeComponent();
            _mainWindow = mainWindow;

            var backgroundWorker = new BackgroundWorker();
            _worker = backgroundWorker;
            _worker.DoWork += _worker_DoWork;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
            _worker.WorkerReportsProgress = true;
            _mainWindow.OnProgressUpdate += _mainWindow_OnProgressUpdate;
                //Invoke _mainWindow_OnProgressUpdate whenever the OnProgressUpdate event is called in _mainWindow
            PreviewKeyDown += MainWindow_PreviewKeyDown;

            RunTut();
        }

        #region Background Worker

        private void CallBackGroundWorker()
        {
            _worker.RunWorkerAsync();
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e) //Runs algorithms on a separate thread to keep the UI thread idle, and the application responsive
        {
            _mainWindow.Generate(_userLength);
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _mainWindow.DrawGrid(MazeGrid); //Draw the maze once generated

            MazeGrid.Visibility = Visibility.Visible; //Updates UI once the worker has completed its work.
            LoadingBar.Visibility = Visibility.Collapsed;
            LoadingScreen.Visibility = Visibility.Collapsed;
            BtnGenerate.Content = "Clear";
            BtnGenerate.IsEnabled = true;
        }

        void _mainWindow_OnProgressUpdate(int value) //Return to ui thread and increment the loading bar to indivate progress has been made
        {
            Dispatcher.Invoke(delegate { LoadingBar.Value += value; });
        }

        #endregion

        #region Click Methods

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _userLength = Convert.ToInt32(LengthTxt.Text);
                switch ((string) BtnGenerate.Content)
                {
                    case "Generate":
                        Generate();
                        break;

                    case "Clear":
                        Clear();
                        break;
                }
            }

            catch (Exception)
            {
                MessageBox.Show("Please insert a number");
            }
        }


        private void AboutItem_Click(object sender, RoutedEventArgs e) //Opens a support page in the browser that shows documentation, tutorials, source code and other information relevant to the project
        {
            var value = MessageBox.Show("Open your web browser?", "Maze Generator", MessageBoxButton.YesNo);
            if (value == MessageBoxResult.Yes)
                System.Diagnostics.Process.Start("http://shmacktus.github.io/HSC-Major-Work");
        }

        private void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Solve();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
        }

        #endregion

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e) //Handles arrow key press
        {
            if (e.Key == Key.Up | e.Key == Key.Left | e.Key == Key.Right | e.Key == Key.Down)
            {
                e.Handled = true;
                if (_mainWindow.Play(e))
                {
                    MessageBox.Show("Game won!");
                }
            }
        }

        #region Updates to MainWindow

        private void Generate() //UI changes and launches the background worker
        {
            LoadingBar.Visibility = Visibility.Visible;
            LoadingScreen.Visibility = Visibility.Visible;
            BtnSolve.IsEnabled = true;
            BtnGenerate.IsEnabled = false;
            CallBackGroundWorker();
            GenerateGrid();
        }

        private void Clear() //Resets the UI to its initial state
        {
            _mainWindow.Clear();
            MazeGrid.Children.Clear();
            MazeGrid.Visibility = Visibility.Hidden;
            BtnGenerate.Content = "Generate";
            BtnSolve.IsEnabled = false;
            MazeGrid.ColumnDefinitions.Clear();
            MazeGrid.RowDefinitions.Clear();
        }

        private void GenerateGrid()
        {
            //Enable for debugging
            MazeGrid.ShowGridLines = false;

            //Column Definitions
            for (int i = 0; i < _userLength; i++) 
                MazeGrid.ColumnDefinitions.Add(new ColumnDefinition());

            //Row Definitons
            for (int i = 0; i < _userLength; i++)
                MazeGrid.RowDefinitions.Add(new RowDefinition());
        }

        #endregion

        private void RunTut() //Opens the tutorial the first time the application runs on the system, user can then reacess the tutorial at any time by selecting the help button
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings["Tutorial"].Value == "True")
            {
                //Launch message box
                var value =
                    MessageBox.Show(
                        "Hi! Since this is your first time running the application would you like to open the tutorial in your web browser?",
                        "Maze Generator: Tutorial", MessageBoxButton.YesNo);

                if (value == MessageBoxResult.Yes)
                    System.Diagnostics.Process.Start("http://shmacktus.github.io/HSC-Major-Work");

                config.AppSettings.Settings["Tutorial"].Value = "False";
                config.Save(ConfigurationSaveMode.Modified);
            }
        }
    }
}