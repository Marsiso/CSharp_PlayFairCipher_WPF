using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
    }
}