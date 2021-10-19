using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using CSharp_PlayFairCipher_WPF.Annotations;

namespace CSharp_PlayFairCipher_WPF
{
    public class Cipher : INotifyPropertyChanged
    {
        private string _keyWord;
        private string _input;
        private string _output;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Localization { get; set; }
        public Dictionary<char, string> EncryptionFilterDictionary { get; private set; }
        public Dictionary<char, char> DecryptionFilterDictionary { get; private set; }
        public Dictionary<string, string> PostDecryptionFilterDictionary { get; private set; }
        public bool Mode { get; set; }

        public string Input
        {
            get => _input;
            set => SetValue(ref _input, value);
        }

        public string Output
        {
            get => _output;
            set => SetValue(ref _output, value);
        }

        public string KeyWord
        {
            get => _keyWord;
            set => SetKeyWord(ref _keyWord, value);
        }

        public Cipher()
        {
            _keyWord = string.Empty;
            KeyWord = string.Empty;
            EncryptionFilterDictionary = new Dictionary<char, string>
            {
                {'a', "A"},
                {'A', "A"},
                {'b', "B"},
                {'B', "B"},
                {'c', "C"},
                {'C', "C"},
                {'d', "D"},
                {'D', "D"},
                {'e', "E"},
                {'E', "E"},
                {'f', "F"},
                {'F', "F"},
                {'g', "G"},
                {'G', "G"},
                {'h', "H"},
                {'H', "H"},
                {'i', "I"},
                {'I', "I"},
                {'j', "I"},
                {'J', "I"},
                {'k', "K"},
                {'K', "K"},
                {'l', "L"},
                {'L', "L"},
                {'m', "M"},
                {'M', "M"},
                {'n', "N"},
                {'N', "N"},
                {'o', "O"},
                {'O', "O"},
                {'p', "P"},
                {'P', "P"},
                {'q', "Q"},
                {'Q', "Q"},
                {'r', "R"},
                {'R', "R"},
                {'s', "S"},
                {'S', "S"},
                {'t', "T"},
                {'T', "T"},
                {'u', "U"},
                {'U', "U"},
                {'v', "V"},
                {'V', "V"},
                {'w', "W"},
                {'W', "W"},
                {'x', "X"},
                {'X', "X"},
                {'y', "Y"},
                {'Y', "Y"},
                {'z', "Z"},
                {'Z', "Z"},
                {'0', "XNULAX"},
                {'1', "XIEDNAX"},
                {'2', "XDVAX"},
                {'3', "XTRIX"},
                {'4', "XCTYRYX"},
                {'5', "XPETX"},
                {'6', "XSESTX"},
                {'7', "XSEDUMX"},
                {'8', "XOSUMX"},
                {'9', "XDEVETX"},
                {'á', "A"},
                {'Á', "A"},
                {'č', "C"},
                {'Č', "C"},
                {'ď', "D"},
                {'Ď', "D"},
                {'é', "E"},
                {'É', "E"},
                {'ě', "E"},
                {'Ě', "E"},
                {'í', "I"},
                {'Í', "I"},
                {'ň', "N"},
                {'Ň', "N"},
                {'ó', "O"},
                {'Ó', "O"},
                {'ř', "R"},
                {'Ř', "R"},
                {'š', "S"},
                {'Š', "S"},
                {'ť', "T"},
                {'Ť', "T"},
                {'ú', "U"},
                {'Ú', "U"},
                {'ů', "U"},
                {'Ů', "U"},
                {'ý', "Y"},
                {'Ý', "Y"},
                {'ž', "Z"},
                {'Ž', "Z"},
                {' ', "XMEZERAX"},
                {'\n', "XMEZERAX"}
            };
            DecryptionFilterDictionary = new Dictionary<char, char>
            {
                {'a', 'A'},
                {'A', 'A'},
                {'b', 'B'},
                {'B', 'B'},
                {'c', 'C'},
                {'C', 'C'},
                {'d', 'D'},
                {'D', 'D'},
                {'e', 'E'},
                {'E', 'E'},
                {'f', 'F'},
                {'F', 'F'},
                {'g', 'G'},
                {'G', 'G'},
                {'h', 'H'},
                {'H', 'H'},
                {'i', 'I'},
                {'I', 'I'},
                {'j', 'I'},
                {'J', 'I'},
                {'k', 'K'},
                {'K', 'K'},
                {'l', 'L'},
                {'L', 'L'},
                {'m', 'M'},
                {'M', 'M'},
                {'n', 'N'},
                {'N', 'N'},
                {'o', 'O'},
                {'O', 'O'},
                {'p', 'P'},
                {'P', 'P'},
                {'q', 'Q'},
                {'Q', 'Q'},
                {'r', 'R'},
                {'R', 'R'},
                {'s', 'S'},
                {'S', 'S'},
                {'t', 'T'},
                {'T', 'T'},
                {'u', 'U'},
                {'U', 'U'},
                {'v', 'V'},
                {'V', 'V'},
                {'w', 'W'},
                {'W', 'W'},
                {'x', 'X'},
                {'X', 'X'},
                {'y', 'Y'},
                {'Y', 'Y'},
                {'z', 'Z'},
                {'Z', 'Z'},
                {'á', 'A'},
                {'Á', 'A'},
                {'č', 'C'},
                {'Č', 'C'},
                {'ď', 'D'},
                {'Ď', 'D'},
                {'é', 'E'},
                {'É', 'E'},
                {'ě', 'E'},
                {'Ě', 'E'},
                {'í', 'I'},
                {'Í', 'I'},
                {'ň', 'N'},
                {'Ň', 'N'},
                {'ó', 'O'},
                {'Ó', 'O'},
                {'ř', 'R'},
                {'Ř', 'R'},
                {'š', 'S'},
                {'Š', 'S'},
                {'ť', 'T'},
                {'Ť', 'T'},
                {'ú', 'U'},
                {'Ú', 'U'},
                {'ů', 'U'},
                {'Ů', 'U'},
                {'ý', 'Y'},
                {'Ý', 'Y'},
                {'ž', 'Z'},
                {'Ž', 'Z'}
            };
            PostDecryptionFilterDictionary = new Dictionary<string, string>()
            {
                {"XNULAX", "0"},
                {"XIEDNAX", "1"},
                {"XDVAX", "2"},
                {"XTRIX", "3"},
                {"XCTYRYX", "4"},
                {"XPETX", "5"},
                {"XSESTX", "6"},
                {"XSEDUMX", "7"},
                {"XOSUMX", "8"},
                {"XDEVETX", "9"},
                {"XMEZERAX", " "}
            };
            Localization = false;
            Input = @"Type here ...";
            Output = String.Empty;
        }

        public void SetKeyWord(ref string store, string value, [CallerMemberName] string name = null)
        {
            var counterChars = new Dictionary<char, int>();
            var stringBuilder = new StringBuilder(25);
            foreach (var key in (value ?? string.Empty).TakeWhile(_ => stringBuilder.Length != 25))
            {
                if (!DecryptionFilterDictionary.TryGetValue(key, out var val)) continue;
                if (counterChars.ContainsKey(val)) continue;
                counterChars.Add(val, 1);
                stringBuilder.Append(val);
            }

            var s = stringBuilder.ToString();
            if (store.Equals(s)) return;

            store = s;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void SetValue<T>(ref T store, T value, [CallerMemberName] string name = null)
        {
            if (Equals(store, value)) return;
            store = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}