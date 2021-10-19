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
        private bool _localization;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Localization
        {
            get => _localization;
            set => SetLocalization(ref _localization, value);
        }

        public Dictionary<char, string> EncryptionFilterDictionary { get; private set; }
        public Dictionary<char, char> DecryptionFilterDictionary { get; private set; }
        public Dictionary<string, string> PostDecryptionFilterDictionary { get; private set; }
        public bool Mode { get; set; }

        public char[,] Matrix { set; get; }

        public List<string> GetMatrixAsList()
        {
            var listS = new List<string>();
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    listS.Append(Matrix[i, j].ToString());
                }
            }

            return listS;
        }

        public string Input
        {
            get => _input;
            set
            {
                SetValue(ref _input, value);
                Output = value;
            }
        }

        public string Output
        {
            get => _output;
            set => Encrypt(ref _output, value);
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
            _input = string.Empty;
            _output = string.Empty;
            BuildCharsMatrix();
            Input = @"Type here ...";
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
            BuildCharsMatrix();
            EncryptionDictionary.Clear();
            Input = Input;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void SetValue<T>(ref T store, T value, [CallerMemberName] string name = null)
        {
            if (Equals(store, value)) return;
            store = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void SetLocalization(ref bool store, bool value)
        {
            if (Localization.Equals(value)) return;
            _localization = value;
            if (value)
            {
                EncryptionFilterDictionary['j'] = "J";
                EncryptionFilterDictionary['J'] = "J";
                EncryptionFilterDictionary['q'] = "K";
                EncryptionFilterDictionary['Q'] = "K";
                EncryptionFilterDictionary['1'] = "XJEDNAX";
                DecryptionFilterDictionary['j'] = 'J';
                DecryptionFilterDictionary['J'] = 'J';
                DecryptionFilterDictionary['q'] = 'K';
                DecryptionFilterDictionary['Q'] = 'K';
                PostDecryptionFilterDictionary.Remove("XIEDNAX");
                PostDecryptionFilterDictionary.Add("XJEDNAX", "1");
                KeyWord = KeyWord;
                //TODO Start rebuilding
                return;
            }

            EncryptionFilterDictionary['q'] = "Q";
            EncryptionFilterDictionary['Q'] = "Q";
            EncryptionFilterDictionary['j'] = "I";
            EncryptionFilterDictionary['J'] = "I";
            EncryptionFilterDictionary['1'] = "XIEDNAX";
            DecryptionFilterDictionary['q'] = 'Q';
            DecryptionFilterDictionary['Q'] = 'Q';
            DecryptionFilterDictionary['j'] = 'I';
            DecryptionFilterDictionary['J'] = 'I';
            PostDecryptionFilterDictionary.Remove("XJEDNAX");
            PostDecryptionFilterDictionary.Add("XIEDNAX", "1");
            KeyWord = KeyWord;
            //TODO Start rebuilding
        }

        public void BuildCharsMatrix()
        {
            Matrix = new char[5, 5];
            IEnumerable<char> enumChars = Localization
                ? new[] // CZ
                {
                    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
                    'M', 'N', 'O', 'P', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
                }
                : new[] // EN
                {
                    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'K', 'L', 'M',
                    'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
                };

            int i = 0, j = 0;
            foreach (var entry in KeyWord)
            {
                Matrix[i, j] = entry;
                ++j;
                if (!j.Equals(5)) continue;
                ++i;
                j = 0;
            }

            var rest = enumChars.Except(KeyWord);
            foreach (var entry in rest)
            {
                Matrix[i, j] = entry;
                ++j;
                if (!j.Equals(5)) continue;
                ++i;
                j = 0;
            }
        }

        private int Mod(int a, int m)
        {
            var result = a % m;
            return result < 0
                ? result + m
                : result;
        }

        public Dictionary<string, string> EncryptionDictionary { get; set; } = new();

        public void Encrypt(ref string store, string value, [CallerMemberName] string name = null)
        {
            var openText = PrepareOpenText(value);
            var cipherBuilder = new StringBuilder(openText.Length);
            for (var i = 0; i < openText.Length; i += 2)
            {
                if (EncryptionDictionary.TryGetValue($"{openText[i]}{openText[i + 1]}", out var val))
                {
                    cipherBuilder.Append(val);
                    continue;
                }

                int k = 0, l = 0, m = 0, n = 0;
                while (k < 5)
                {
                    if (l.Equals(5))
                    {
                        l = 0;
                        ++k;
                    }

                    if (openText[i].Equals(Matrix[k, l])) break;
                    ++l;
                }

                while (m < 5)
                {
                    if (n.Equals(5))
                    {
                        n = 0;
                        ++m;
                    }

                    if (openText[i + 1].Equals(Matrix[m, n])) break;
                    ++n;
                }

                var resultEncrypt = Encrypt(ref k, ref l, ref m, ref n);
                EncryptionDictionary.Add($"{openText[i]}{openText[i + 1]}", resultEncrypt);
                cipherBuilder.Append(resultEncrypt);
            }

            store = cipherBuilder.ToString();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string Encrypt(ref int x1, ref int x2, ref int y1, ref int y2)
        {
            if (x1.Equals(y1)) return $"{Matrix[x1, Mod(x2 + 1, 5)]}{Matrix[y1, Mod(y2 + 1, 5)]}";

            return x2.Equals(y2)
                ? $"{Matrix[Mod(x1 + 1, 5), x2]}{Matrix[Mod(y1 + 1, 5), y2]}"
                : $"{Matrix[x1, y2]}{Matrix[y1, x2]}";
        }

        private string PrepareOpenText(string input)
        {
            var inStrBuilder = new StringBuilder(input.Length << 1);
            foreach (var key in input)
                if (EncryptionFilterDictionary.TryGetValue(key, out var value))
                    inStrBuilder.Append(value);

            for (var i = 0; i < inStrBuilder.Length; ++i)
                // Merge ifs and switches
                if (i.Equals(inStrBuilder.Length - 1) && Mod(inStrBuilder.Length, 2) == 0)
                {
                    return inStrBuilder.ToString();
                }
                else if (i.Equals(inStrBuilder.Length - 1) && Mod(inStrBuilder.Length, 2) > 0)
                {
                    var c = inStrBuilder[^1];
                    switch (c)
                    {
                        case 'X':
                            inStrBuilder.Append('Y');
                            return inStrBuilder.ToString();

                        default:
                            inStrBuilder.Append('X');
                            return inStrBuilder.ToString();
                    }
                }
                else if (inStrBuilder[i].Equals(inStrBuilder[i + 1]))
                {
                    var c = inStrBuilder[i];
                    switch (c)
                    {
                        case 'X':
                            inStrBuilder.Insert(i + 1, 'Y');
                            break;

                        default:
                            inStrBuilder.Insert(i + 1, 'X');
                            break;
                    }
                }

            return inStrBuilder.ToString();
        }
    }
}