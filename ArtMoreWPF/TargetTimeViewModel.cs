namespace ArtMoreWPF
{
    using System.Windows.Input;

    public class TargetTimeViewModel : ObservableItem
    {
        private MainViewModel _model;

        public ICommand ResetTarget { get; private set; }

        public ICommand AddTimeToTarget { get; private set; }

        public TargetTimeViewModel(MainViewModel model)
        {
            _model = model;
            AddTimeToTarget = new RelayCommand<double>(AdjustTargetTime, CanAdjustTargetTime);

            ResetTarget = new RelayCommand(
                () => { _model.CurrentRecord.TargetTime = 0.0; }, 
                (object o) => { return _model.CanAdjustTargetTime; });
        }

        private bool CanAdjustTargetTime(double seconds)
        {
            if (_model.CurrentRecord != null && seconds < 0 && _model.CurrentRecord.RemainingTime + seconds < 0)
            {
                return false;
            }
            return _model.CanAdjustTargetTime;
        }

        private void AdjustTargetTime(double seconds)
        {
            _model.CurrentRecord.TargetTime += seconds;
        }
    }
}
