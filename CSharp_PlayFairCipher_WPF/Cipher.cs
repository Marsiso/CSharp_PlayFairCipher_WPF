using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Annotations.Storage;

namespace CSharp_PlayFairCipher_WPF
{
    public class Cipher : INotifyPropertyChanged
    {
        private string _keyWord;
        private string _input;
        private string _output;
        private bool _localization;
        private List<MatrixRowItem> _listMatrixChars;
        private List<KeyAndValue> _listFilteredChars;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Localization
        {
            get => _localization;
            set
            {
                if (_localization.Equals(value)) return;
                _localization = value;
                SetLocalization(value);
            }
        }

        public Dictionary<char, string> EncryptionFilterDictionary { get; }
        public Dictionary<char, char> DecryptionFilterDictionary { get; }
        public Dictionary<string, string> PostDecryptionFilterDictionary { get; }
        public Dictionary<string, string> EncryptionDictionary { get; set; }
        public Dictionary<string, string> DecryptionDictionary { get; set; }

        public bool Mode { get; set; }

        public List<KeyAndValue> ListFilteredChars
        {
            get => _listFilteredChars;
            set => SetFilteredChars(ref _listFilteredChars, value);
        }

        public char[,] Matrix { set; get; }

        public void SetFilteredChars(ref List<KeyAndValue> store, List<KeyAndValue> value,
            [CallerMemberName] string name = null)
        {
            value ??= new List<KeyAndValue>();
            value.AddRange(Mode
                ? DecryptionFilterDictionary.Select(pair => new KeyAndValue(pair.Key.ToString(), pair.Value.ToString()))
                : EncryptionFilterDictionary.Select(pair => new KeyAndValue(pair.Key.ToString(), pair.Value)));
            store = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void SetListMatrixChars(ref List<MatrixRowItem> store, List<MatrixRowItem> value,
            [CallerMemberName] string name = null)
        {
            value ??= new List<MatrixRowItem>(5);
            for (var i = 0; i < 5; i++)
            {
                value.Add(new MatrixRowItem(
                    Matrix[i, 0].ToString(),
                    Matrix[i, 1].ToString(),
                    Matrix[i, 2].ToString(),
                    Matrix[i, 3].ToString(),
                    Matrix[i, 4].ToString()
                ));
            }

            store = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public List<MatrixRowItem> ListMatrixChars
        {
            get => _listMatrixChars;
            set => SetListMatrixChars(ref _listMatrixChars, value);
        }

        public class KeyAndValue
        {
            public string Key { get; set; }
            public string Value { get; set; }

            public KeyAndValue(string key, string value)
            {
                Key = key;
                Value = value;
            }

            public override string ToString()
            {
                return $"{Key}-->{Value}";
            }
        }

        public class MatrixRowItem
        {
            public string Char0 { get; set; }
            public string Char1 { get; set; }
            public string Char2 { get; set; }
            public string Char3 { get; set; }
            public string Char4 { get; set; }

            public MatrixRowItem(string char0, string char1, string char2, string char3, string char4)
            {
                Char0 = char0;
                Char1 = char1;
                Char2 = char2;
                Char3 = char3;
                Char4 = char4;
            }

            public override string ToString()
            {
                return $"{Char0}  {Char1}  {Char2}  {Char3}  {Char4}";
            }
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
            set
            {
                if (Mode)
                {
                    Decrypt(ref _output, value);
                    return;
                }

                Encrypt(ref _output, value);
            }
        }

        public string KeyWord
        {
            get => _keyWord;
            set => SetKeyWord(ref _keyWord, value);
        }

        public Cipher()
        {
            _keyWord = string.Empty;
            _input = string.Empty;
            _output = string.Empty;
            _listMatrixChars = new List<MatrixRowItem>();
            _listFilteredChars = new List<KeyAndValue>();
            _localization = false;
            Mode = false;
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
            EncryptionDictionary = new Dictionary<string, string>();
            DecryptionDictionary = new Dictionary<string, string>();
            KeyWord = string.Empty;
            BuildCharsMatrix();
            ListMatrixChars = new List<MatrixRowItem>();
            ListFilteredChars = new List<KeyAndValue>();
            Input = @"TYPE HERE ...";
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

            store = stringBuilder.ToString();
            BuildCharsMatrix();
            ListMatrixChars = new List<MatrixRowItem>();
            ListFilteredChars = new List<KeyAndValue>();
            EncryptionDictionary.Clear();
            DecryptionDictionary.Clear();
            Input = _input;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void SetValue<T>(ref T store, T value, [CallerMemberName] string name = null)
        {
            if (Equals(store, value)) return;
            store = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void SetLocalization(bool value)
        {
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
                KeyWord = _keyWord;
            }
            else
            {
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
                KeyWord = _keyWord;
            }
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

        public void Encrypt(ref string store, string value, [CallerMemberName] string name = null)
        {
            value = PrepareOpenText(value);
            var cipherBuilder = new StringBuilder(value.Length);
            for (var i = 0; i < value.Length; i += 2)
            {
                if (EncryptionDictionary.TryGetValue($"{value[i]}{value[i + 1]}", out var val))
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

                    if (value[i].Equals(Matrix[k, l])) break;
                    ++l;
                }

                while (m < 5)
                {
                    if (n.Equals(5))
                    {
                        n = 0;
                        ++m;
                    }

                    if (value[i + 1].Equals(Matrix[m, n])) break;
                    ++n;
                }

                var resultEncrypt = Encrypt(ref k, ref l, ref m, ref n);
                EncryptionDictionary.Add($"{value[i]}{value[i + 1]}", resultEncrypt);
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

        public void Decrypt(ref string store, string value, [CallerMemberName] string name = null)
        {
            var cipher = PrepareCipher(value);
            if (Mod(cipher.Length, 2) > 0)
            {
                store = string.Empty;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }

            var cipherBuilder = new StringBuilder(cipher.Length);
            for (var i = 0; i < cipher.Length; i += 2)
            {
                if (DecryptionDictionary.TryGetValue($"{cipher[i]}{cipher[i + 1]}", out var val))
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

                    if (cipher[i].Equals(Matrix[k, l])) break;
                    ++l;
                }

                while (m < 5)
                {
                    if (n.Equals(5))
                    {
                        n = 0;
                        ++m;
                    }

                    if (cipher[i + 1].Equals(Matrix[m, n])) break;
                    ++n;
                }

                var resultDecrypt = Decrypt(ref k, ref l, ref m, ref n);
                DecryptionDictionary.Add($"{cipher[i]}{cipher[i + 1]}", resultDecrypt);
                cipherBuilder.Append(resultDecrypt);
            }

            foreach (var (key, val) in PostDecryptionFilterDictionary) cipherBuilder.Replace(key, val);

            store = cipherBuilder.ToString();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string Decrypt(ref int x1, ref int x2, ref int y1, ref int y2)
        {
            if (x1.Equals(y1)) return $"{Matrix[x1, Mod(x2 - 1, 5)]}{Matrix[y1, Mod(y2 - 1, 5)]}";

            return x2.Equals(y2)
                ? $"{Matrix[Mod(x1 - 1, 5), x2]}{Matrix[Mod(y1 - 1, 5), y2]}"
                : $"{Matrix[x1, y2]}{Matrix[y1, x2]}";
        }

        private string PrepareCipher(string input)
        {
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
            var inStrBuilder = new StringBuilder(input.Length);
            foreach (var c in input.Where(c => enumChars.Contains(char.ToUpper(c))))
                inStrBuilder.Append(char.ToUpper(c));

            return inStrBuilder.ToString();
        }
    }
}