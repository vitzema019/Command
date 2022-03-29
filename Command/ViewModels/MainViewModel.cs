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
        private string _originalWord = "Karel";
        public string _hiddenWord = "_____";
        public ParametrizedRelayCommand<string> GuessCharacterCommand { get; set; }

        public MainViewModel()
        {
            _usedChars = new ObservableCollection<char> {};
            GuessCharacterCommand = new ParametrizedRelayCommand<string>(
                (value) =>
                {
                    if (!String.IsNullOrEmpty(value) && !_usedChars.Contains(value[0]))
                    {
                        _usedChars.Add(value[0]);
                        GuessCharacterCommand.RaiseCanExecureChanged();
                    }
                },
                
                (p) => { 
                    return p == null ? true : !_usedChars.Contains(p[0]); }
                );
        }

        public string Word
        {
            get
            {
                return _hiddenWord;
            }
            set
            {
                _hiddenWord = value;
                NotifyPropertyChanged();
            }
        }
    }
}
