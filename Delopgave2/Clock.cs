using System;
using System.ComponentModel;

namespace Delopgave2
{
    class Clock : INotifyPropertyChanged
    {
        private string _date;
        private string _time;

        public Clock()
        {
            Update();
        }

        public void Update()
        {
            Date = DateTime.Now.ToLongDateString();
            Time = DateTime.Now.ToLongTimeString();
        }

       

        public string Date
        {
            get { return _date; }
            private set
            {
                if (_date == value) return;
                _date = value;
                Notify("Date");
            } 
        }

        public string Time
        {
            get { return _time; }
            private set
            {
                if (_time == value) return;
                _time = value;
                Notify("Time");
            }
        }

        private void Notify(string propName)
        {

             /* 
            if (PropertyChanged != null)
                 PropertyChanged(this, new PropertyChangedEventArgs(propName));
             */
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
