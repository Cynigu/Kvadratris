using NLog;
using ReactiveUI;
using System;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;
using System.Collections.ObjectModel;
using LiveCharts.Defaults;

namespace lab3.Model
{
    public class GraphShapeModel : ReactiveObject
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region Field
        private SeriesCollection _seriesCollection;
        private Func<double, string> _labels;
        private Func<double, string> _yFormatter;

        #endregion

        #region Properties
        public SeriesCollection SeriesCollection 
        {
            get { return _seriesCollection; }
            set { this.RaiseAndSetIfChanged(ref _seriesCollection, value); 
            } 
        }
        public Func<double, string> XLabels
        {
            get { return _labels; }
            set
            {
                this.RaiseAndSetIfChanged(ref _labels, value);
            }
        }
        public Func<double, string> YFormatter
        {
            get { return _yFormatter; }
            set
            {
                this.RaiseAndSetIfChanged(ref _yFormatter, value);
            }
        }
        #endregion

        #region Methods
        public void CreateGraph(ObservableCollection<Point> DataList)
        {
            ChartValues<ObservablePoint> List = new ChartValues<ObservablePoint>();
            for (int i = 0; i < DataList.Count; i++)
            {
                List.Add(new ObservablePoint() { X = DataList[i].X, Y = DataList[i].Y });
            }
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Квадратриса",
                    Values = List,
                    LineSmoothness = 1,
                    PointForeground = Brushes.White,
                    Stroke =  Brushes.White,
                    Fill = System.Windows.Media.Brushes.Transparent
        }
            };

            XLabels = value => value.ToString();

            YFormatter = value => value.ToString(); 
        }
        public void ClearGraph()
        {
            SeriesCollection.Clear();
            XLabels = null;
            YFormatter = null;
        }
        #endregion
    }
}
