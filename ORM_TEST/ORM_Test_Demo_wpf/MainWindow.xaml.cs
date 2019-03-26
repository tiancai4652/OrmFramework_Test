using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ORM_Test_Demo_wpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            Dapper = new Dapper();
            EF = new EF();
        }

        int _DataCount;
        public int DataCount
        {
            get { return _DataCount; }
            set { _DataCount = value; OnPropertyChanged(nameof(DataCount)); }
        }

        List<Group> _DataLists = new List<Group>();
        public List<Group> DataLists
        {
            get { return _DataLists; }
            set { _DataLists = value; OnPropertyChanged(nameof(DataLists)); }
        }

        int _DapperMs;
        public int DapperMs
        {
            get { return _DapperMs; }
            set { _DapperMs = value; OnPropertyChanged(nameof(DapperMs)); }
        }

        int _EFMs;
        public int EFMs
        {
            get { return _EFMs; }
            set { _EFMs = value; OnPropertyChanged(nameof(EFMs)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Dapper Dapper { get; set; }
        public EF EF { get; set; }

        string _txtReady;
        public string txtReady
        {
            get { return _txtReady; }
            set { _txtReady = value; OnPropertyChanged(nameof(txtReady)); }
        }

        string _txtInsert;
        public string txtInsert
        {
            get { return _txtInsert; }
            set { _txtInsert = value; OnPropertyChanged(nameof(txtInsert)); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txtReady = "Ready?";
            DataLists = DataGenerater.GetGroupList(DataCount);
            txtReady = "Go";

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            txtInsert = "";
            INSERT_BUTTON.IsEnabled = false;
            int COUNT = 0;
            Thread a = new Thread(()=> {
                DateTime t1 = DateTime.Now;
                Dapper.InsertDataPerTable(DataLists);
                DateTime t2 = DateTime.Now;
                COUNT++;
                this.Dispatcher.Invoke(()=> {
                    txtInsert += $"Dapper.Insert({DataLists.Count}):" + (t2 - t1).TotalMilliseconds + " ms";
                    if (COUNT == 2)
                    {
                        INSERT_BUTTON.IsEnabled = true;
                    }
                });
            });
            Thread b = new Thread(() => {
                DateTime t1 = DateTime.Now;
                EF.InsertDataPerTable(DataLists);
                DateTime t2 = DateTime.Now;
                COUNT++;
                this.Dispatcher.Invoke(() => {
                    txtInsert += $"EF.Insert({DataLists.Count}):" + (t2 - t1).TotalMilliseconds + " ms";
                    if (COUNT == 2)
                    {
                        INSERT_BUTTON.IsEnabled = true;
                    }
                });
            });
            a.IsBackground = true;
            b.IsBackground = true;
            a.Start();
            b.Start();

        }
    }
}
