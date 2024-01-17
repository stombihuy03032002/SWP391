using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;

namespace ReadFile
{
    public class MyDataModel
    {
        public string Province { get; set; }
        public string Mathematics { get; set; }

    }

    public sealed class MyDataModelMap : ClassMap<MyDataModel>
    {
        public MyDataModelMap()
        {
            Map(m => m.Province).Name("province");
            Map(m => m.Mathematics).Name("mathematics");
        }
    }

    public class ProvinceMathematicsAverage
    {
        public string Province { get; set; }
        public double AverageMathematicsScore { get; set; }
        public int TotalCandidates { get; set; }
    }

    public partial class MainWindow : Window
    {
        public List<MyDataModel> DataList { get; set; }
        public List<ProvinceMathematicsAverage> AverageList { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            DataList = new List<MyDataModel>();
            AverageList = new List<ProvinceMathematicsAverage>();
        }

        private void LoadCsvButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string csvFilePath = openFileDialog.FileName;

                // Read CSV file using CsvHelper with custom mapping
                DataList = ReadCsvFile<MyDataModel, MyDataModelMap>(csvFilePath);
                ComputeAverageMathematicsScore();

                // Bind the data to the ListView
                csvListView.ItemsSource = AverageList;
            }
        }

        private List<T> ReadCsvFile<T, TMap>(string filePath)
            where T : class
            where TMap : ClassMap<T>, new()
        {
            List<T> records;

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.Context.RegisterClassMap<TMap>();
                records = csv.GetRecords<T>().ToList();
            }

            return records;
        }

        private void ComputeAverageMathematicsScore()
        {
            var groupedData = DataList
                .GroupBy(d => d.Province)
                .Select(group =>
                {
                    var totalCandidates = group.Count();
                    var totalMathematicsStudents = group.Count(item => !string.IsNullOrEmpty(item.Mathematics));
                    var averageScore = group.Average(item => double.TryParse(item.Mathematics, out var score) ? score : 0);
                    return new ProvinceMathematicsAverage
                    {
                        Province = group.Key,
                        AverageMathematicsScore = averageScore,
                        TotalCandidates = totalCandidates
                    };
                })
                .OrderByDescending(item => item.AverageMathematicsScore) // Sort in descending order
                .ToList();

            AverageList = groupedData;
        }
    }
}
