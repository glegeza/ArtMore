namespace ArtMoreWPF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;
    using System.IO;

    public class FileRecord : ObservableItem, IEquatable<FileRecord>
    {
        private string _name;
        private string _path;
        private double _timeWorked;
        private double _targetTime;
        private BitmapImage _thumbnail;
        private Guid _id;
        private DateTime _lastUpdate;

        public event EventHandler RecordChanged;

        public FileRecord(SerializableFileRecord loadedRecord)
        {
            _name = loadedRecord.Name;
            _path = loadedRecord.Path;
            _timeWorked = loadedRecord.WorkTime;
            _targetTime = loadedRecord.TargetTime;
            _id = Guid.Parse(loadedRecord.ID);
            try
            {
                _thumbnail = new BitmapImage(new Uri(_path));
            }
            catch (Exception e)
            {

            }
        }

        public FileRecord(string path)
        {
            _name = System.IO.Path.GetFileNameWithoutExtension(path);
            _path = path;
            _timeWorked = 0.0;
            _targetTime = 0.0;
            _id = Guid.NewGuid();
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("FullName");
                OnRecordChanged();
            }
        }

        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                SetThumbnail();
                NotifyPropertyChanged();
                NotifyPropertyChanged("FullName");
                OnRecordChanged();
            }
        }

        public Guid ID { get; private set; }

        public DateTime LastUpdate
        {
            get
            {
                return _lastUpdate;
            }
            set
            {
                _lastUpdate = value;
                NotifyPropertyChanged();
            }
        }

        public double WorkedTime
        {
            get
            {
                return _timeWorked;
            }
            set
            {
                _timeWorked = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("RemainingTime");
                NotifyPropertyChanged("PrettyWorkTime");
                NotifyPropertyChanged("PrettyTargetTime");
                NotifyPropertyChanged("PrettyRemainingTime");
                OnRecordChanged();
            }
        }

        public double TargetTime
        {
            get
            {
                return _targetTime;
            }
            set
            {
                _targetTime = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("RemainingTime");
                NotifyPropertyChanged("PrettyTargetTime");
                NotifyPropertyChanged("PrettyRemainingTime");
                OnRecordChanged();
            }
        }

        public double RemainingTime
        {
            get
            {
                return TargetTime - WorkedTime;
            }
        }

        public string PrettyWorkTime
        {
            get
            {
                return TimeStringHelper.GetHourBasedString(WorkedTime);
            }
        }

        public string PrettyTargetTime
        {
            get
            {
                return TimeStringHelper.GetHourBasedString(TargetTime);
            }
        }

        public string PrettyRemainingTime
        {
            get
            {
                return TimeStringHelper.GetHourBasedString(RemainingTime);
            }
        }

        public string FullName
        {
            get
            {
                return String.Format("{0} ({1})", Name, Path);
            }
        }

        public BitmapImage Thumbnail
        {
            get
            {
                return _thumbnail;
            }
        }

        public bool FileIsValid()
        {
            return File.Exists(Path);
        }

        public bool FileDirectoryIsValid()
        {
            return Directory.Exists(new FileInfo(Path).DirectoryName);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FileRecord))
            {
                return false;
            }
            return Equals(obj as FileRecord);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public bool Equals(FileRecord other)
        {
            return other.ID.Equals(ID);
        }

        private void OnRecordChanged()
        {
            RecordChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SetThumbnail()
        {
            try
            {
                _thumbnail = new BitmapImage(new Uri(Path));
            }
            catch (Exception e)
            {

            }
            NotifyPropertyChanged("Thumbnail");
        }
    }
}
