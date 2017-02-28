namespace ArtMoreWPF
{
    using System.Windows;

    public partial class MainWindow : Window
    {
        private string Filename = "art.json";

        private MainViewModel _model;

        public MainWindow()
        {
            InitializeComponent();

            _model = DataContext as MainViewModel;
            Closing += OnWindowClosing;
            _model.LoadData(Filename);
        }

        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _model.SaveData(Filename);
        }

        private void OnFileDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                _model.HandleFileDrag(files);
            }
        }
    }
}
