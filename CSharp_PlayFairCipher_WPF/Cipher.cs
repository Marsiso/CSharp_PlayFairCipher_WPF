using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

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
        private bool _mode;

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

        public bool Mode
        {
            get => _mode;
            set => SetMode(value);
        }

        private void SetMode(bool value)
        {
            _mode = value;
            ListFilteredChars = new List<KeyAndValue>();
        }

        public List<KeyAndValue> ListFilteredChars
        {
            get => _listFilteredChars;
            set => SetFilteredChars(ref _listFilteredChars, value);
        }

        public char[,] Matrix { set; get; }

        // ReSharper disable once RedundantAssignment
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
                value.Add(new MatrixRowItem(
                    Matrix[i, 0].ToString(),
                    Matrix[i, 1].ToString(),
                    Matrix[i, 2].ToString(),
                    Matrix[i, 3].ToString(),
                    Matrix[i, 4].ToString()
                ));

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
            _mode = false;
            EncryptionFilterDictionary = new Dictionary<char, string>
            {
                {'A', "A"},
                {'B', "B"},
                {'C', "C"},
                {'D', "D"},
                {'E', "E"},
                {'F', "F"},
                {'G', "G"},
                {'H', "H"},
                {'I', "I"},
                {'J', "I"},
                {'K', "K"},
                {'L', "L"},
                {'M', "M"},
                {'N', "N"},
                {'O', "O"},
                {'P', "P"},
                {'Q', "Q"},
                {'R', "R"},
                {'S', "S"},
                {'T', "T"},
                {'U', "U"},
                {'V', "V"},
                {'W', "W"},
                {'X', "X"},
                {'Y', "Y"},
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
                {'Á', "A"},
                {'Č', "C"},
                {'Ď', "D"},
                {'É', "E"},
                {'Ě', "E"},
                {'Í', "I"},
                {'Ň', "N"},
                {'Ó', "O"},
                {'Ř', "R"},
                {'Š', "S"},
                {'Ť', "T"},
                {'Ú', "U"},
                {'Ů', "U"},
                {'Ý', "Y"},
                {'Ž', "Z"},
                {' ', "XMEZERAX"},
                {'\n', "XMEZERAX"}
            };
            DecryptionFilterDictionary = new Dictionary<char, char>
            {
                {'A', 'A'},
                {'B', 'B'},
                {'C', 'C'},
                {'D', 'D'},
                {'E', 'E'},
                {'F', 'F'},
                {'G', 'G'},
                {'H', 'H'},
                {'I', 'I'},
                {'J', 'I'},
                {'K', 'K'},
                {'L', 'L'},
                {'M', 'M'},
                {'N', 'N'},
                {'O', 'O'},
                {'P', 'P'},
                {'Q', 'Q'},
                {'R', 'R'},
                {'S', 'S'},
                {'T', 'T'},
                {'U', 'U'},
                {'V', 'V'},
                {'W', 'W'},
                {'X', 'X'},
                {'Y', 'Y'},
                {'Z', 'Z'},
                {'Á', 'A'},
                {'Č', 'C'},
                {'Ď', 'D'},
                {'É', 'E'},
                {'Ě', 'E'},
                {'Í', 'I'},
                {'Ň', 'N'},
                {'Ó', 'O'},
                {'Ř', 'R'},
                {'Š', 'S'},
                {'Ť', 'T'},
                {'Ú', 'U'},
                {'Ů', 'U'},
                {'Ý', 'Y'},
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
                EncryptionFilterDictionary['J'] = "J";
                EncryptionFilterDictionary['Q'] = "K";
                EncryptionFilterDictionary['1'] = "XJEDNAX";
                DecryptionFilterDictionary['J'] = 'J';
                DecryptionFilterDictionary['Q'] = 'K';
                PostDecryptionFilterDictionary.Remove("XIEDNAX");
                PostDecryptionFilterDictionary.Add("XJEDNAX", "1");
                KeyWord = _keyWord;
            }
            else
            {
                EncryptionFilterDictionary['Q'] = "Q";
                EncryptionFilterDictionary['J'] = "I";
                EncryptionFilterDictionary['1'] = "XIEDNAX";
                DecryptionFilterDictionary['Q'] = 'Q';
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
            {
                var isFinishedAndEven = i.Equals(inStrBuilder.Length - 1) && Mod(inStrBuilder.Length, 2) == 0;
                var isFinishedAndOdd = i.Equals(inStrBuilder.Length - 1) && Mod(inStrBuilder.Length, 2) > 0;
                var isTwoNearbyCharNonDistinct = !isFinishedAndEven && !isFinishedAndOdd &&
                                                 inStrBuilder[i].Equals(inStrBuilder[i + 1]);
                var c = inStrBuilder[i];

                switch (b1: isFinishedAndEven, b2: isFinishedAndOdd, b3: isTwoNearbyCharNonDistinct, c)
                {
                    case (true, _, _, _):
                        return inStrBuilder.ToString();

                    case (false, true, _, 'X'):
                        inStrBuilder.Append('Y');
                        return inStrBuilder.ToString();

                    case (false, true, _, _):
                        inStrBuilder.Append('X');
                        return inStrBuilder.ToString();

                    case (false, false, true, 'X'):
                        inStrBuilder.Insert(i + 1, 'Y');
                        break;

                    case (false, false, true, _):
                        inStrBuilder.Insert(i + 1, 'X');
                        break;

                    case (false, false, false, _):
                        continue;
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
                return;
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