using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Command.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private ObservableCollection<char> _usedChars;
        private string _originalWord;
        public string OriginalWord
        {
            get { return _originalWord; }
            set { _originalWord = value; NotifyPropertyChanged(); }
        }
        private string _hiddenword;
        public string HiddenWord
        {
            get { return _hiddenword; }
            set { _hiddenword = value; NotifyPropertyChanged(); }
        }
        public ParametrizedRelayCommand<string> GuessCharacterCommand { get; set; }

        public MainViewModel()
        {
            _usedChars = new ObservableCollection<char> {};
            GuessCharacterCommand = new ParametrizedRelayCommand<string>(
                (value) =>
                {
                    if (!String.IsNullOrEmpty(value) && !_usedChars.Contains(value[0]))
                    {
                        Guess(value[0]);
                        _usedChars.Add(value[0]);
                        GuessCharacterCommand.RaiseCanExecureChanged();
                    }
                },
                
                (p) => { 
                    return p == null ? true : !_usedChars.Contains(p[0]); }
                );
            OriginalWord = "Karel";
            HiddenWord = Display(OriginalWord);
        }

        private string Display(string input)
        {
            string displayversion = "";
            for (int i = 0; i < input.Length; i++)
            {
                displayversion += "_";
            }
            return displayversion;

        }

        public void Guess (char input)
        {
            StringBuilder ch = new StringBuilder(HiddenWord);
            int replace = 0;
            for(int i = 0; i < _originalWord.Length; i++)
            {
                char compare = _originalWord[i];
                if(char.ToUpper(compare) == char.ToUpper(input))
                {
                    ch[i] = OriginalWord[i];
                    replace++;
                }
            }
            if(replace > 0)
            {
                HiddenWord = ch.ToString();
            }
        }


    }
}
