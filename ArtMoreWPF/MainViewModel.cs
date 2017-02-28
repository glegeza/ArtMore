namespace ArtMoreWPF
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows.Input;

    public class MainViewModel : ObservableItem
    {
        private static double DefaultTimer = 14 * 60 * 60;
        private static List<string> fileFormats = new List<string>()
        {
            ".png",
            ".bmp",
            ".jpg"
        };

        private string _totalArtTime = TimeStringHelper.GetHourBasedString(0);
        private FileRecord _currentRecord;

        public event EventHandler SelectionChanged;

        public string TotalArtTime
        {
            get
            {
                var amt = FileList.Sum(r => r.WorkedTime);
                return TimeStringHelper.GetHourBasedString(amt);
            }
        }

        public ICommand DeleteSelectedRecordCommand { get; private set; }

        public ICommand DeleteRecordCommand { get; private set; }

        public ICommand OpenSelectedRecordPath { get; private set; }

        public ICommand OpenSelectedRecordFile { get; private set; }

        public ICommand ChangeFileRecordPath { get; private set; }

        public ObservableCollection<FileRecord> FileList { get; private set; }

        public FileRecord CurrentRecord
        {
            get
            {
                return _currentRecord;
            }
            set
            {
                if (_currentRecord != null)
                {
                    _currentRecord.RecordChanged -= OnSelectedRecordChanged;
                }
                if (value != null)
                {
                    value.RecordChanged += OnSelectedRecordChanged;
                }

                _currentRecord = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("CanAdjustTargetTime");
                TimerModel.UpdateSelectedRecord(_currentRecord);
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public TimerViewModel TimerModel { get; private set; }

        public TargetTimeViewModel TargetTimeModel { get; private set; }

        public bool CanAdjustTargetTime
        {
            get
            {
                return CurrentRecord != null && !TimerModel.TimerIsRunning;
            }
        }

        public MainViewModel()
        {
            FileList = new ObservableCollection<FileRecord>();
            TimerModel = new TimerViewModel();
            TargetTimeModel = new TargetTimeViewModel(this);

            DeleteSelectedRecordCommand = new RelayCommand(
                () => { DeleteItem(CurrentRecord); }, 
                (object o) => { return CanDeleteItem(CurrentRecord); }
                );

            DeleteRecordCommand = new RelayCommand<FileRecord>(DeleteItem, CanDeleteItem);

            OpenSelectedRecordPath = new RelayCommand(
                () => { Process.Start(Path.GetDirectoryName(_currentRecord.Path)); },
                (object o) => { return _currentRecord.FileDirectoryIsValid(); }
                );

            OpenSelectedRecordFile = new RelayCommand(
                () => { Process.Start(_currentRecord.Path); },
                (object o) => { return _currentRecord != null && File.Exists(_currentRecord.Path); }
                );

            ChangeFileRecordPath = new RelayCommand(
                () => { SetNewPath(CurrentRecord); },
                (object o) => { return CurrentRecord != null; }
                );

            FileList.CollectionChanged += (object s, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => { NotifyPropertyChanged("TotalArtTime"); };

            TimerModel.TimerUpdated += OnTimerUpdated;
        }

        private void SetNewPath(FileRecord record)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = Path.GetDirectoryName(record.Path);
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            bool? result = dlg.ShowDialog();
            
            if (result == true)
            {
                string filename = dlg.FileName;
                record.Path = filename;
            }
        }

        private bool CanDeleteItem(FileRecord record)
        {
            return record != null && FileList.Any(r => r.ID.Equals(record.ID));
        }

        private void DeleteItem(FileRecord record)
        {
            FileList.Remove(record);
        }

        public void LoadData(string filename)
        {
            if (File.Exists(filename))
            {
                var json = File.ReadAllText(filename);
                var list = JsonConvert.DeserializeObject<List<SerializableFileRecord>>(json);
                foreach (var item in list)
                {
                    FileList.Add(new FileRecord(item));
                }
            }
        }

        public void SaveData(string filename)
        {
            var list = new List<SerializableFileRecord>();
            foreach (var item in FileList)
            {
                list.Add(new SerializableFileRecord(item));
            }
            var json = JsonConvert.SerializeObject(list);
            File.WriteAllText(filename, json);
        }

        public void HandleFileDrag(string[] files)
        {
            var imageFiles = new List<string>();
            foreach (var file in files)
            {
                var ext = Path.GetExtension(file);
                if (fileFormats.Contains(ext))
                {
                    imageFiles.Add(file);
                }
            }
            if (imageFiles.Count > 0)
            {
                AddImages(imageFiles);
            }
        }

        private void OnTimerUpdated(object sender, EventArgs e)
        {
            NotifyPropertyChanged("CanAdjustTargetTime");
            NotifyPropertyChanged("TotalArtTime");
        }

        private void AddImages(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                var newRecord = new FileRecord(file);
                newRecord.TargetTime = DefaultTimer;
                FileList.Add(newRecord);
            }
        }

        private void OnSelectedRecordChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("TotalArtTime");

        }
    }
}
