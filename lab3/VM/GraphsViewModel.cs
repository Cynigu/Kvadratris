using lab3.Commands;
using lab3.Model;
using lab3.Service;
using lab3.View;
using NLog;
using ReactiveUI;
using System;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Input;

namespace lab3.VM
{
    public class GraphsViewModel : ReactiveObject
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region Constructor
        private readonly IDialogService dialogService;
        private readonly IFileService fileService;
        public GraphsViewModel(IDialogService dialogService, IFileService fileService)
        {
            this.dialogService = dialogService;
            this.fileService = fileService;
            Kvad = new Kvadratris();
            MyGraph = new GraphShapeModel();
        }
        #endregion

        #region Fields
        private GraphShapeModel _myGraph ;
        private Kvadratris _kvad;
        //private Image _img;
        #endregion

        #region Properties
        public GraphShapeModel MyGraph 
        {
            get { return _myGraph; }
            set { this.RaiseAndSetIfChanged(ref _myGraph, value); }
        }
        //public Image Img { get { return _img; } set{ this.RaiseAndSetIfChanged(ref _img, value); } }
        public Kvadratris Kvad 
        {
            get
            {
                return _kvad;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _kvad, value);
            }
        }
        #endregion

        #region Command
        private ICommand _clearCommand;
        public ICommand ClearCommand
        {
            get
            {
                return _clearCommand ??
                    (_clearCommand = new RelayCommand(obj  =>
                    {
                        Kvad.Clear();
                        MyGraph.ClearGraph();
                        //dialogService.ShowMessage(Border1.ToString());
                    }));
            }
        }

        private ICommand _startCommand;
        public ICommand StartCommand
        {
            get
            {
                return _startCommand ??
                    (_startCommand = new RelayCommand(obj  =>
                    {
                        try
                        {
                            
                            Kvad.DataFill();

                            MyGraph.CreateGraph(Kvad.DataList);
                        }
                        catch (ArgumentNullException)
                        {
                            return;
                        }
                        catch (ArgumentException e)
                        {

                            dialogService.ShowMessage(e.Message);
                            return;
                        }
                        
                        //for (int i =0; i < 3; i++)
                        //{
                        //    dialogService.ShowMessage(Kvad.TableV.X[i].ToString() + " " + Kvad.TableV.Y[i].ToString());
                        //}
                        
                        //dialogService.ShowMessage(Border1.ToString());
                    }));
            }
        }

        private IAsyncCommand _openFileCommand;
        public IAsyncCommand OpenFileCommand
        {
            get
            {
                return _openFileCommand ??
                  (_openFileCommand = new AsyncCommand(async () =>
                  {
                      if (dialogService.OpenFileDialog() == true)
                      {
                          string data = await fileService.OpenAsync(dialogService.FilePath);
                          string[] dataArr = data.Split(' ');
                          if (dataArr.Length != 4)
                          {
                              //Logger logger = LogManager.GetCurrentClassLogger();
                              logger.Error("Неверно введены исходные данные");
                              dialogService.ShowMessage("Неверно введены исходные данные");
                              return;
                          }
                          for (int i = 0; i < 4; i++)
                          {
                              //Logger logger = LogManager.GetCurrentClassLogger();
                              logger.Error("Неверно введены исходные данные из файла");
                              double temp;
                              if (!double.TryParse(dataArr[i], out temp))
                              {
                                  dialogService.ShowMessage("Неверно введены исходные данные");
                                  return;
                              }
                          }
                          logger.Trace("Файл успешно открыт");
                          Kvad.Border1Y = double.Parse(dataArr[0]);
                          Kvad.Border2Y = double.Parse(dataArr[1]);
                          Kvad.Step = double.Parse(dataArr[2]);
                          Kvad.R = double.Parse(dataArr[3]);
                      }
                  }));
            }
        }

        private RelayCommand _openWindowInformation;
        public ICommand OpenWindowInformation
        {
            get
            {
                return _openWindowInformation ??
                  (_openWindowInformation = new RelayCommand(obj =>
                  {
                      InfoWindow informWindow = new InfoWindow();
                      informWindow.Show();
                  }));
            }
        }

        private IAsyncCommand _saveResaltCommand;
        public IAsyncCommand SaveResaltCommand
        {
            get
            {
                return _saveResaltCommand ??
                  (_saveResaltCommand = new AsyncCommand(async () =>
                  {
                      if (dialogService.SaveFileDialog() == true)
                      {
                          await Task.Run(() => SaveResultToExcel());
                          dialogService.ShowMessage("Файл сохранен");
                      }
                  }));
            }
        }
        private IAsyncCommand _saveOrigCommand;
        public IAsyncCommand SaveOrigCommand
        {
            get
            {
                return _saveOrigCommand ??
                  (_saveOrigCommand = new AsyncCommand(async () =>
                  {
                      if (dialogService.SaveFileDialog() == true)
                      {
                          await Task.Run(() => SaveOrigToExcel());
                          dialogService.ShowMessage("Файл сохранен");
                      }
                  }));
            }
        }
        #endregion

        #region Methods
        private void SaveOrigToExcel()
        {
            Excel.Application xlApp = new Excel.Application();
            //не предлагать пользователю сохранить изменения
            xlApp.DisplayAlerts = false;
            if (xlApp == null)
            {
                dialogService.ShowMessage("Эксель не установлен на устройстве");
                logger.Error("Попытка записи в Эксель провалилась, т.к. Эксель не установлен");
                return;
            }
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            //добавление шаблона книги
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            //Запроса на сохранение для книги не должно быть
            xlWorkBook.Saved = true;
            //Получаем ссылку на лист 1
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "Коэфициент: "; xlWorkSheet.Cells[1, 2] = Kvad.R;
            xlWorkSheet.Cells[2, 1] = "Шаг: ";        xlWorkSheet.Cells[2, 2] = Kvad.Step;
            xlWorkSheet.Cells[3, 1] = "Границы: ";    xlWorkSheet.Cells[3, 2] = Kvad.Border1Y; xlWorkSheet.Cells[3, 3] = Kvad.Border2Y;

            xlWorkBook.SaveAs(dialogService.FilePath, //Имя сохраняемого файла
                Excel.XlFileFormat.xlWorkbookNormal, //Формат сохраняемого файла
                misValue, //Пароль доступа к файлу до 15 символов
                misValue, //Пароль на доступ на запись
                misValue, //При true режим только для чтения 
                misValue,//Создать резервную копию файла при true
                Excel.XlSaveAsAccessMode.xlExclusive,//Режим доступа к рабочей книге
                misValue,//Способ разрешения конфликтов
                misValue,//При true сохраненный документ добавляется в список ранее открытых файлов
                misValue, //Кодовая страница
                misValue, //Направление размещения текста
                misValue);//Идентификатор ExcelApplication 
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
        }

        private void SaveResultToExcel()
        {
            Excel.Application xlApp = new Excel.Application();
            xlApp.DisplayAlerts = false;

            if (xlApp == null)
            {
                dialogService.ShowMessage("Эксель не установлен на устройстве");
                logger.Error("Попытка записи в Эксель провалилась, т.к. Эксель не установлен");
                return;
            }
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkBook.Saved = true;
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "Коэфициент: "; xlWorkSheet.Cells[1, 2] = Kvad.R;
            xlWorkSheet.Cells[2, 1] = "Шаг: "; xlWorkSheet.Cells[2, 2] = Kvad.Step;
            xlWorkSheet.Cells[3, 1] = "Границы: "; xlWorkSheet.Cells[3, 2] = Kvad.Border1Y; xlWorkSheet.Cells[3, 3] = Kvad.Border2Y;
            xlWorkSheet.Cells[4, 1] = "Результат (x(y), y) : ";
            xlWorkSheet.Cells[5, 1] = "X"; xlWorkSheet.Cells[5, 2] = "Y";
            for (int i = 6; i < 6 + Kvad.DataList.Count; i++)
            {
                double tempx = Kvad.DataList[i - 6].X;
                if (double.IsNaN(tempx))
                {
                    continue;
                } 
                else
                {
                    xlWorkSheet.Cells[i, 1] = Kvad.DataList[i - 6].X;
                    xlWorkSheet.Cells[i, 2] = Kvad.DataList[i - 6].Y;
                }
            }
            //-----------------------------------------
           

            Excel.ChartObjects xlCharts = (Excel.ChartObjects)xlWorkSheet.ChartObjects(Type.Missing);
            Excel.ChartObject myChart = xlCharts.Add(10, 80, 300, 250);
            Excel.Chart chart = myChart.Chart;
            Excel.SeriesCollection seriesCollection = (Excel.SeriesCollection)chart.SeriesCollection(Type.Missing);
            Excel.Series series = seriesCollection.NewSeries();
            series.XValues = xlWorkSheet.get_Range("A6", "A" + (5 + Kvad.DataList.Count).ToString());
            series.Values = xlWorkSheet.get_Range("B6", "B" + (5 + Kvad.DataList.Count).ToString());
            chart.ChartType = Excel.XlChartType.xlXYScatterLines;

            //----------------------------------------------
            xlWorkBook.SaveAs(dialogService.FilePath, //Имя сохраняемого файла
               Excel.XlFileFormat.xlWorkbookNormal, //Формат сохраняемого файла
                misValue, //Пароль доступа к файлу до 15 символов
                misValue, //Пароль на доступ на запись
                misValue, //При true режим только для чтения 
                misValue,//Создать резервную копию файла при true
                Excel.XlSaveAsAccessMode.xlExclusive,//Режим доступа к рабочей книге
                misValue,//Способ разрешения конфликтов
                misValue,//При true сохраненный документ добавляется в список ранее открытых файлов
                misValue, //Кодовая страница
                misValue, //Направление размещения текста
                misValue);//Идентификатор ExcelApplication 
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
         
        }
        #endregion
    }
}

