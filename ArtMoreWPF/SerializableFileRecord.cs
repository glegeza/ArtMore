namespace ArtMoreWPF
{
    public class SerializableFileRecord
    {
        public SerializableFileRecord(FileRecord fileRecord)
        {
            Name = fileRecord.Name;
            Path = fileRecord.Path;
            ID = fileRecord.ID.ToString();
            WorkTime = fileRecord.WorkedTime;
            TargetTime = fileRecord.TargetTime;
        }

        public SerializableFileRecord()
        {

        }

        public string Name;
        public string Path;
        public string ID;
        public double WorkTime;
        public double TargetTime;
    }
}
