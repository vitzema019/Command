using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Command.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
       public ObservableCollection<char> usedChars;
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

        private int _fail = 0;
        public int Fail {
            get { return _fail; }
            set { _fail = value; NotifyPropertyChanged(); }
        }
        public ParametrizedRelayCommand<string> GuessCharacterCommand { get; set; }

        public MainViewModel()
        {
            usedChars = new ObservableCollection<char> {};
            GuessCharacterCommand = new ParametrizedRelayCommand<string>(
                (value) =>
                {
                    if (!String.IsNullOrEmpty(value) && !usedChars.Contains(value[0]))
                    {
                        Guess(value[0]);
                        
                        usedChars.Add(value[0]);
                        GuessCharacterCommand.RaiseCanExecureChanged();
                        CheckStatus();
                    }
                },
                
                (p) => { 
                    return p == null ? true : !usedChars.Contains(p[0]); }
                );
            OriginalWord = "Karel";
            HiddenWord = Display(OriginalWord);
            Fail = 8;
        }

        private string Display(string input)
        {
            string displayversion = "";
            for (int i = 0; i < input.Length; i++)
            {
                displayversion += "-";
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
            else
            {
                Fail--;
            }
        }

        public void CheckStatus()
        {
            if(Fail == 0)
            {
                MessageBox.Show("Prohra", "Hangman", MessageBoxButton.OK, MessageBoxImage.Warning);
                Restart();
            }
            else if(HiddenWord == OriginalWord)
            {
                MessageBox.Show("Výhra", "Hangman", MessageBoxButton.OK, MessageBoxImage.Warning);
                Restart();
            }
            
        }

        public void Restart()
        {
            Fail = 8;
            usedChars.Clear();
            HiddenWord = Display(OriginalWord);
            GuessCharacterCommand.RaiseCanExecureChanged();
        }


    }
}
