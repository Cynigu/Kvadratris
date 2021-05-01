using NLog;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace lab3.Model
{
    public class Kvadratris : ReactiveObject
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #region Consructors
        public Kvadratris(double border1, double border2, double step, double coefficient)
        {
            Border1Y = border1;
            Border2Y = border2;
            Step = step;
            R = coefficient;
            DataList = new ObservableCollection<Point>();
        }
        public Kvadratris()
        {
            Border1Y = -10;
            Border2Y = 10;
            Step = 0.5;
            R = 12;
            DataList = new ObservableCollection<Point>();
        }
        #endregion

        #region Fields
        //public DrawingGroup drawingGroup = new DrawingGroup();
        private double _border1y;
        private double _border2y;
        private double _step;
        private double _R;
        ObservableCollection<Point> _dataList; // Таблица значений
        #endregion

        #region Properties
        public double Border1Y
        {
            get { return _border1y; }
            set
            {
                this.RaiseAndSetIfChanged(ref _border1y, value);
            }
        } // Граница по y 1
        public double Border2Y
        {
            get { return _border2y; }
            set
            {
                this.RaiseAndSetIfChanged(ref _border2y, value);
            }
        } // Граница по y 2
        public double Step
        {
            get { return _step; }
            set
            {
                 this.RaiseAndSetIfChanged(ref _step, value);
            }
        } // шаг    
        public double R
        {
            get { return _R; }
            set
            {
                this.RaiseAndSetIfChanged(ref _R, value);
            }
        } // коэфициент
        public ObservableCollection<Point> DataList 
        { 
            get 
            {
                return _dataList;  
            }
            set
            {
                _dataList = value;
            }
        }
        #endregion

        #region Methods
        public void Clear()
        {
            Border1Y = 0;
            Border2Y = 0;
            Step = 0;
            R = 0;
            DataList.Clear();
            logger.Info("Все данные функции обнулились");
        }
        public void DataFill()
        {
            DataList.Clear();
            logger.Debug("Шаг: " + Step);
            logger.Debug("Границы: " + Border1Y + " " + Border2Y);
            logger.Debug("Коэфициент R: " + R); 
            if (Step > (Math.Abs(Border1Y)+Math.Abs(Border2Y)))
            {
                logger.Info("Шаг больше промежутка. " + "Шаг: " + Step + " Границы: " + Border1Y + " " + Border2Y);
                throw new ArgumentException(String.Format("Шаг больше промежутка"));
            }
            
            int divisionValue = (int)Math.Ceiling((Math.Abs(Border1Y - Border2Y)) / Step) + 1;
            if (Border1Y == Border2Y)
            {
                throw new ArgumentException(String.Format("Границы равны между собой"));
            }
            if (R == 0)
            {
                throw new ArgumentException(String.Format("R не может равняться 0"));
            }
            if (divisionValue == 0 || Step == 0)
            {
                throw new ArgumentNullException();
            }

            double[] x = new double[divisionValue];
            double[] y = new double[divisionValue];
            for (int i = 0; i < divisionValue; i++)
            {
                double tempy = (Border1Y < Border2Y) ? (Border1Y + Step * i) : (Border2Y + Step * i);
                y[i] = Math.Round(tempy, 3);

                double tempx = y[i]*(Math.Cos(Math.PI * y[i] / (2 * R)) / Math.Sin(Math.PI*y[i]/(2*R)));
                x[i] = Math.Round(tempx, 3);
            }
            for (int i = 0; i < divisionValue; i++)
            {
                DataList.Add(new Point(x[i], y[i]));
            }
        }
        #endregion
    }
}
