namespace ArtMoreWPF
{
    using System;
    using System.Timers;
    using System.Windows.Input;
    using System.Windows.Media;

    public class TimerViewModel : ObservableItem
    {
        private FileRecord _currentRecord;
        private string _timerString;
        private RecordTimer _currentTimer;
        private Timer _updateTimer;
        private string _remainingTimeString;

        public TimerViewModel()
        {
            CurrentTimerString = TimeStringHelper.GetHourBasedString(0);
            StartTimer = new RelayCommand(ExecuteStartTimer, (object o) => { return CanStartTimer; });
            EndTimer = new RelayCommand(ExecuteEndTimer, (object o) => { return CanEndTimer; });
        }

        public event EventHandler TimerUpdated;

        public string CurrentTimerString
        {
            get
            {
                return _timerString;
            }
            set
            {
                _timerString = value;
                NotifyPropertyChanged();
            }
        }

        public string RemainingTimeString
        {
            get
            {
                return _remainingTimeString;
            }
            set
            {
                _remainingTimeString = value;
                NotifyPropertyChanged();
            }
        }

        public double RemainingTimePercent
        {
            get
            {
                if (_currentRecord == null)
                {
                    return 0;
                }
                var totalTime = _currentRecord.WorkedTime + (_currentTimer != null && _currentTimer.Running ? _currentTimer.ElapsedTime : 0);
                var percent = Math.Min(totalTime / _currentRecord.TargetTime, 100);
                return percent * 100;
            }
        }

        public bool CanStartTimer
        {
            get
            {
                return _currentRecord != null && (_currentTimer == null || !_currentTimer.Running);
            }
        }

        public bool CanEndTimer
        {
            get
            {
                return _currentTimer != null && _currentTimer.Running;
            }
        }

        public bool TimerIsRunning
        {
            get
            {
                return _currentTimer != null && _currentTimer.Running;
            }
        }

        public ICommand StartTimer { get; private set; }
        
        public ICommand EndTimer { get; private set; }

        public void UpdateSelectedRecord(FileRecord newRecord)
        {
            if (newRecord != null)
            {
                newRecord.RecordChanged += OnSelectedRecordChanged;
            }
            if (_currentRecord != null)
            {
                _currentRecord.RecordChanged -= OnSelectedRecordChanged;
            }
            _currentRecord = newRecord;
            TimerStatusUpdated();
            UpdateRemainingTime();
        }

        private void OnSelectedRecordChanged(object sender, EventArgs e)
        {
            UpdateRemainingTime();
            NotifyPropertyChanged("RemainingTimeString");
            NotifyPropertyChanged("RemainingTimePercent");
            NotifyPropertyChanged("CurrentTimerString");
        }

        private void ExecuteStartTimer()
        {
            if (_currentRecord == null || (_currentTimer != null && _currentTimer.Running))
            {
                return;
            }
            _currentTimer = new RecordTimer(_currentRecord);
            _currentTimer.Start();
            _updateTimer = new Timer(10);
            _updateTimer.Start();
            _updateTimer.Elapsed += OnTimerTick;
            TimerStatusUpdated();
        }

        private void ExecuteEndTimer()
        {
            if (_currentTimer == null || !_currentTimer.Running)
            {
                return;
            }
            _currentTimer.End();
            _updateTimer.Elapsed -= OnTimerTick;
            _updateTimer.Stop();
            _updateTimer = null;

            TimerStatusUpdated();
            UpdateRemainingTime();
        }

        private void OnTimerTick(object sender, ElapsedEventArgs e)
        {
            CurrentTimerString = TimeStringHelper.GetHourBasedString(_currentTimer.ElapsedTime);
            var remainingTime = _currentRecord.RemainingTime - _currentTimer.ElapsedTime;
            RemainingTimeString = TimeStringHelper.GetHourBasedString(remainingTime);
        }

        private void UpdateRemainingTime()
        {
            if (_currentRecord != null)
            {
                RemainingTimeString = _currentRecord.PrettyRemainingTime;
            }
            else
            {
                RemainingTimeString = TimeStringHelper.GetHourBasedString(0);
            }
        }

        private void TimerStatusUpdated()
        {
            NotifyPropertyChanged("CanStartTimer");
            NotifyPropertyChanged("CanEndTimer");
            NotifyPropertyChanged("RemainingTimePercent");
            TimerUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
