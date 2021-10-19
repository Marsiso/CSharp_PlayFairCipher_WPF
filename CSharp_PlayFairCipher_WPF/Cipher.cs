using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CSharp_PlayFairCipher_WPF.Annotations;

namespace CSharp_PlayFairCipher_WPF
{
    public class Cipher : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string KeyWord { get; set; }

        public Cipher()
        {
            KeyWord = String.Empty;
        }

        //[NotifyPropertyChangedInvocator]
        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}