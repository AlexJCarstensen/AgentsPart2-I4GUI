using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;


namespace Delopgave4
{
    public class Agents : ObservableCollection<Agent>, INotifyPropertyChanged
    {
        private string _fileName = "";

        public Agents()
        {
            if (!(bool) (DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof (DependencyObject)).DefaultValue))
                return;
            Add(new Agent("001", "Nina", "Assassination", "UpperVolta"));
            Add(new Agent("007", "James Bond", "Martinis", "North Korea"));
            Add(new Agent("005", "Alex", "Computer Engineering", "Making it work!"));
        }

        #region Commands

        private ICommand _addCommand;
        public ICommand AddCommand => _addCommand ?? (_addCommand = new RelayCommand(AddAgent));

        private void AddAgent()
        {
            Add(new Agent());
            NotifyPropertyChanged("Count");
            CurrentIndex = Count - 1;
        }

        private ICommand _deleteCommand;

        public ICommand DeleteCommand
            => _deleteCommand ?? (_deleteCommand = new RelayCommand(AgentDelete, AgentDelete_Cane));

        private bool AgentDelete_Cane()
        {
            if (Count > 0 && CurrentIndex >= 0)
                return true;
            else
                return false;
        }

        private void AgentDelete()
        {
            RemoveAt(CurrentIndex);
            NotifyPropertyChanged("Count");

        }


        private ICommand _nextCommand;

        public ICommand NextCommand
        {
            get
            {
                return _nextCommand ?? (_nextCommand = new RelayCommand(
                    () => ++CurrentIndex,
                    () => CurrentIndex < (Count - 1)));
            }
        }

        private ICommand _prevCommand;

        public ICommand PrevCommand
            => _prevCommand ?? (_prevCommand = new RelayCommand(PreviousCommandExecute, PreviousCommandCanExecute));

        private bool PreviousCommandCanExecute()
        {
            if (CurrentIndex > 0)
                return true;
            else
                return false;

        }

        private void PreviousCommandExecute()
        {
            if (CurrentIndex > 0)
                --CurrentIndex;
        }

        private ICommand _newCommand;

        public ICommand NewCommand => _newCommand ?? (_newCommand = new RelayCommand(NewCommand_Execute));

        private void NewCommand_Execute()
        {
            MessageBoxResult res = MessageBox.Show("Any unsaved data will be lost! are you sure you want to continue",
                "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (res == MessageBoxResult.Yes)
            {
                Clear();
                _fileName = "";
            }
        }

        private ICommand _openCommand;
        public ICommand OpenCommand => _openCommand ?? (_openCommand = new RelayCommand<string>(OpenCommand_Execute));

        private void OpenCommand_Execute(string argFileName)
        {
            if (argFileName == "")
            {
                MessageBox.Show("You must enter a filename in the file name textbox!", "unable to save file",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                _fileName = argFileName;
                Agents tempAgents = new Agents();

                XmlSerializer serializer = new XmlSerializer(typeof(Agents));
                try
                {
                    TextReader reader = new StreamReader(_fileName);

                    tempAgents = (Agents) serializer.Deserialize(reader);
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Unable to open file", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Clear();
                foreach (var agent in tempAgents)
                {
                    Add(agent);
                }
            }
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(SaveCommand_Execute, SaveCommand_CanExecute));

        private bool SaveCommand_CanExecute()
        {
            return (_fileName != "") && (Count > 0);
        }

        private void SaveCommand_Execute()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Agents));
            TextWriter writer = new StreamWriter(_fileName);

            serializer.Serialize(writer,this);
            writer.Close();
        }
        private ICommand _saveAsCommand;
        public ICommand SaveAsCommand => _saveAsCommand ?? (_saveAsCommand = new RelayCommand<string>(SaveAsCommand_Execute));

        private void SaveAsCommand_Execute(string argFileName)
        {
            if (argFileName == "")
            {
                MessageBox.Show("You must enter a filename in the file name textbox!", "unable to save file",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                _fileName = argFileName;
                SaveCommand_Execute();
            }

        }

        private ICommand _exitCommand;

        public ICommand ExitCommand => _exitCommand ?? (_exitCommand = new RelayCommand(ExitCommand_Execute));

        private void ExitCommand_Execute()
        {
            Application.Current.MainWindow.Close();
        }

        private ICommand _colorCommand;
        public ICommand ColorCommand => _colorCommand ?? (_colorCommand = new RelayCommand<string>(ColorCommand_Execute));

        private void ColorCommand_Execute(string colorStr)
        {
            SolidColorBrush newBrush = SystemColors.WindowBrush;

            try
            {
                if (colorStr != null)
                {
                    if(colorStr != "Default")
                        newBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorStr));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unknown color name, default color is used", "program Error");
            }
            Application.Current.MainWindow.Resources["myBrush"] = newBrush;
        }

        #endregion //Commands

#region Properties

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set
            {
                if (_currentIndex != value)
                {
                    _currentIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }
#endregion //Properties

#region INotifyPropertyChanged implemetation

        public new event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
#endregion // INotifyPropertyChanged implemetation

#region Fields

        int _currentIndex = -1;

#endregion //Fields
    }  // Just to reference it from xaml

    

 

}

