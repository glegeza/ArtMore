namespace ArtMoreWPF
{
    using System;

    public class RecordTimer
    {
        private double _elapsedTime = 0;

        public RecordTimer(FileRecord record)
        {
            StartTime = null;
            FinishTime = null;
            Record = record;
        }

        public event EventHandler TimerStarted;

        public event EventHandler TimerStopped;

        public FileRecord Record
        {
            get; private set;
        }

        public DateTime? StartTime
        {
            get; private set;
        }

        public DateTime? FinishTime
        {
            get; private set;
        }

        public bool Running
        {
            get { return !Finished && StartTime != null; }
        }

        public bool Finished
        {
            get { return FinishTime != null; }
        }

        public double ElapsedTime
        {
            get
            {
                if (StartTime == null && FinishTime == null)
                {
                    return 0;
                }
                return Finished ? _elapsedTime : (DateTime.Now - StartTime.Value).TotalSeconds;
            }
        }

        public void Start()
        {
            if (Running)
            {
                throw new Exception("Trying to start timer while it is already running.");
            }
            StartTime = DateTime.Now;
            FinishTime = null;
            TimerStarted?.Invoke(this, EventArgs.Empty);
        }

        public void End(bool update=true)
        {
            if (!Running)
            {
                throw new Exception("Ending timer before starting it");
            }
            FinishTime = DateTime.Now;
            _elapsedTime = (FinishTime.Value - StartTime.Value).TotalSeconds;
            if (update)
            {
                Record.WorkedTime += _elapsedTime;
            }
            TimerStopped?.Invoke(this, EventArgs.Empty);
        }
    }
}
