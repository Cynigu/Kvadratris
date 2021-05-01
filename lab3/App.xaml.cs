using Autofac;
using System.Windows;
using lab3.VM;
using lab3.Service;
using System.Windows.Threading;
using NLog;

namespace lab3
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public App()
        {
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);

        }
        protected override void OnStartup(StartupEventArgs e)
        {
             // ContainerBuiler позволяет произвести необходимую привязку Producer-Consumer
            var builder = new ContainerBuilder();
            builder.RegisterType<DefaultDialogService>().As<IDialogService>();
            builder.RegisterType<TextFileService>().As<IFileService>();
            //builder.RegisterType<ExcelFileService>().As<IFileService>();
            builder.RegisterType<GraphsViewModel>().AsSelf();
            // осуществляем необходимое связывание и получаем контейнер,
            // с помощью которого можем получать нужные нам данные;
            var container = builder.Build();
            var viewmodel = container.Resolve<GraphsViewModel>();
            var view = new MainWindow { DataContext = viewmodel };
            view.Show();
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            logger.Fatal( e.Exception.StackTrace + " " +"Исключение: " + e.Exception.GetType().ToString() + " " + e.Exception.Message);
            // Prevent default unhandled exception processing
            //e.Handled = true;
        }
    }
}
