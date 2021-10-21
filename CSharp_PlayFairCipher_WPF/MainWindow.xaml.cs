using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

namespace CSharp_PlayFairCipher_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Cipher _cipher;

        public MainWindow()
        {
            InitializeComponent();
            _cipher = new Cipher();
            DataContext = _cipher;
        }

        private void BtnEnglish_OnClick(object sender, RoutedEventArgs e)
        {
            _cipher.Localization = false;
            BtnEnglish.Background = new SolidColorBrush(Color.FromRgb(0, 100, 0));
            BtnCzech.Background = new SolidColorBrush(Color.FromRgb(150, 0, 0));
        }

        private void BtnCzech_OnClick(object sender, RoutedEventArgs e)
        {
            _cipher.Localization = true;
            BtnCzech.Background = new SolidColorBrush(Color.FromRgb(0, 100, 0));
            BtnEnglish.Background = new SolidColorBrush(Color.FromRgb(150, 0, 0));
        }

        private void BtnEncryption_OnClick(object sender, RoutedEventArgs e)
        {
            _cipher.Mode = false;
            BtnEncryption.Background = new SolidColorBrush(Color.FromRgb(0, 100, 0));
            BtnDecryption.Background = new SolidColorBrush(Color.FromRgb(150, 0, 0));
        }

        private void BtnDecryption_OnClick(object sender, RoutedEventArgs e)
        {
            _cipher.Mode = true;
            BtnDecryption.Background = new SolidColorBrush(Color.FromRgb(0, 100, 0));
            BtnEncryption.Background = new SolidColorBrush(Color.FromRgb(150, 0, 0));
        }

        private void TxtBoxKeyWord_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Key.Equals(Key.Enter)) return;
            TxtBoxKeyWord.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void TxtBoxInput_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Key.Equals(Key.Enter)) return;
            TxtBoxInput.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt|Data file (*.dat)|*.dat";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, _cipher.Output);
        }

        private void BtnCopy_OnClick(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(_cipher.Output);
        }

        private void BtnSwitch_OnClick(object sender, RoutedEventArgs e)
        {
            if (_cipher.Mode)
                BtnEncryption_OnClick(BtnEncryption, new RoutedEventArgs(null));
            else
                BtnDecryption_OnClick(BtnDecryption, new RoutedEventArgs(null));

            _cipher.Input = _cipher.Output;
        }


        private void BtnLoad_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text file (*.txt)|*.txt|Data file (*.dat)|*.dat";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
                _cipher.Input = File.ReadAllText(openFileDialog.FileName);
        }
    }
}