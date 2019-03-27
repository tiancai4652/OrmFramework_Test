using NLog;
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
    //本程序为测试一下ef和dapper 的性能对比
    //dapper的连接字符串在Dapper内定义
    //EF的连接字符串在App.config内定义

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        ILogger logger;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            Dapper = new Dapper();
            EF = new EF();
            logger = LogManager.GetCurrentClassLogger();
            DataCount = 100;
        }

        int _DataCount=100;
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

        string _txtIni;
        public string txtIni
        {
            get { return _txtIni; }
            set { _txtIni = value; OnPropertyChanged(nameof(txtIni)); }
        }


        string _txtInsertDapper;
        public string txtInsertDapper
        {
            get { return _txtInsertDapper; }
            set { _txtInsertDapper = value; OnPropertyChanged(nameof(txtInsertDapper)); }
        }

        string _txtInsertEF;
        public string txtInsertEF
        {
            get { return _txtInsertEF; }
            set { _txtInsertEF = value; OnPropertyChanged(nameof(txtInsertEF)); }
        }

        Visibility _IsDapperInsert = Visibility.Hidden;
        public Visibility IsDapperInsert
        {
            get { return _IsDapperInsert; }
            set { _IsDapperInsert = value; OnPropertyChanged(nameof(IsDapperInsert)); }
        }

        Visibility _IsEFInsert = Visibility.Hidden;
        public Visibility IsEFInsert
        {
            get { return _IsEFInsert; }
            set { _IsEFInsert = value; OnPropertyChanged(nameof(IsEFInsert)); }
        }

        string _txtFindDapper;
        public string txtFindDapper
        {
            get { return _txtFindDapper; }
            set { _txtFindDapper = value; OnPropertyChanged(nameof(txtFindDapper)); }
        }

        string _txtFindEF;
        public string txtFindEF
        {
            get { return _txtFindEF; }
            set { _txtFindEF = value; OnPropertyChanged(nameof(txtFindEF)); }
        }

        Visibility _IsDapperFind = Visibility.Hidden;
        public Visibility IsDapperFind
        {
            get { return _IsDapperFind; }
            set { _IsDapperFind = value; OnPropertyChanged(nameof(IsDapperFind)); }
        }

        Visibility _IsEFFind = Visibility.Hidden;
        public Visibility IsEFFind
        {
            get { return _IsEFFind; }
            set { _IsEFFind = value; OnPropertyChanged(nameof(IsEFFind)); }
        }

        string _txtDeleteDapper;
        public string txtDeleteDapper
        {
            get { return _txtDeleteDapper; }
            set { _txtDeleteDapper = value; OnPropertyChanged(nameof(txtDeleteDapper)); }
        }

        string _txtDeleteEF;
        public string txtDeleteEF
        {
            get { return _txtDeleteEF; }
            set { _txtDeleteEF = value; OnPropertyChanged(nameof(txtDeleteEF)); }
        }

        Visibility _IsDapperDelete = Visibility.Hidden;
        public Visibility IsDapperDelete
        {
            get { return _IsDapperDelete; }
            set { _IsDapperDelete = value; OnPropertyChanged(nameof(IsDapperDelete)); }
        }

        Visibility _IsEFDelete = Visibility.Hidden;
        public Visibility IsEFDelete
        {
            get { return _IsEFDelete; }
            set { _IsEFDelete = value; OnPropertyChanged(nameof(IsEFDelete)); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtReady = "Ready?";
                DataLists = DataGenerater.GetGroupList(DataCount);
                var groupCount = DataLists.Count;
                int userCount = 0;
                DataLists.Select(t => t.Users).All(t => { userCount += t.Count; return true; });
                int authorityCount = 0;
                var userListGroup = DataLists.Select(t => t.Users);

                var atList= from list in userListGroup from user in list select user.Authorities;
                authorityCount = atList.Count();
                //userListGroup.Select(m => m.All(n =>
                //{ authorityCount += n.Authorities.Count;
                //    return true; }
                //));
                txtReady = "Go";
                txtReady += $"DataCount:{DataCount}"+Environment.NewLine;
                txtReady += $"UserCount:{userCount}" + Environment.NewLine;
                txtReady += $"AuthoritiesCount:{authorityCount}" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            txtInsertDapper = "";
            txtInsertEF = "";
            INSERT_BUTTON.IsEnabled = false;
            int COUNT = 0;
            Thread a = new Thread(() =>
            {
                try
                {
                    IsDapperInsert = Visibility.Visible;
                    DateTime t1 = DateTime.Now;
                    Dapper.InsertDataPerTable(DataLists);
                    DateTime t2 = DateTime.Now;
                    COUNT++;
                    IsDapperInsert = Visibility.Hidden;
                    this.Dispatcher.Invoke(() =>
                    {
                        txtInsertDapper += $"Dapper.Insert({DataLists.Count}):" + (t2 - t1).TotalMilliseconds + " ms";
                        if (COUNT == 2)
                        {
                            INSERT_BUTTON.IsEnabled = true;
                        }
                    });
                }
                catch (Exception ex)
                {
                    INSERT_BUTTON.IsEnabled = true;
                    txtInsertDapper += $"{ex.Message}";
                    logger.Error(ex);
                }
            });
            Thread b = new Thread(() =>
            {
                try
                {
                    IsEFInsert = Visibility.Visible;
                    DateTime t1 = DateTime.Now;
                    EF.InsertDataPerTable(DataLists);
                    DateTime t2 = DateTime.Now;
                    COUNT++;
                    IsEFInsert = Visibility.Hidden;
                    this.Dispatcher.Invoke(() =>
                    {
                        txtInsertEF += $"EF.Insert({DataLists.Count}):" + (t2 - t1).TotalMilliseconds + " ms";
                        if (COUNT == 2)
                        {
                            INSERT_BUTTON.IsEnabled = true;
                        }
                    });
                }
                catch (Exception ex)
                {
                    INSERT_BUTTON.IsEnabled = true;
                    txtInsertDapper += $"{ex.Message}";
                    logger.Error(ex);
                }
            });
            a.IsBackground = true;
            b.IsBackground = true;
            a.Start();
            b.Start();



        }

        private void FIND_BUTTON_Click(object sender, RoutedEventArgs e)
        {

            txtFindDapper = "";
            txtFindEF = "";
            FIND_BUTTON.IsEnabled = false;
            int COUNT = 0;
            Thread a = new Thread(() =>
            {
                try
                {
                    IsDapperFind = Visibility.Visible;
                    DateTime t1 = DateTime.Now;
                    var groupList = Dapper.FindGroupAll();
                    DateTime t2 = DateTime.Now;
                    var userList = Dapper.FindUserAll();
                    DateTime t3 = DateTime.Now;
                    var atList = Dapper.FindAuthorityAll();
                    DateTime t4 = DateTime.Now;

                    COUNT++;
                    IsDapperFind = Visibility.Hidden;
                    this.Dispatcher.Invoke(() =>
                        {
                            txtFindDapper += "Dapper" + Environment.NewLine;
                            txtFindDapper += $"FindGroupAll({groupList.Count}):" + (t2 - t1).TotalMilliseconds + " ms ." + Environment.NewLine;
                            txtFindDapper += $"FindUserAll({groupList.Count}):" + (t3 - t2).TotalMilliseconds + " ms ." + Environment.NewLine;
                            txtFindDapper += $"FindAuthAll({groupList.Count}):" + (t4 - t3).TotalMilliseconds + " ms ." + Environment.NewLine;
                            txtFindDapper += $"FindAll({groupList.Count}):" + (t4 - t1).TotalMilliseconds + " ms ." + Environment.NewLine;
                            if (COUNT == 2)
                            {
                                FIND_BUTTON.IsEnabled = true;
                            }
                        });
                }
                catch (Exception ex)
                {
                    txtFindDapper += $"{ex.Message}";
                    FIND_BUTTON.IsEnabled = true;
                    logger.Error(ex);
                }
            });
            Thread b = new Thread(() =>
            {
                try
                {
                    IsEFFind = Visibility.Visible;
                    DateTime t1 = DateTime.Now;
                    var groupList = EF.FindGroupAll();
                    DateTime t2 = DateTime.Now;
                    var userList = EF.FindUserAll();
                    DateTime t3 = DateTime.Now;
                    var atList = EF.FindAuthorityAll();
                    DateTime t4 = DateTime.Now;
                    COUNT++;
                    IsEFFind = Visibility.Hidden;
                    this.Dispatcher.Invoke(() =>
                    {
                        txtFindDapper += "EF" + Environment.NewLine;
                        txtFindDapper += $"EF.FindGroupAll({groupList.Count}):" + (t2 - t1).TotalMilliseconds + " ms ." + Environment.NewLine; ;
                        txtFindDapper += $"FindUserAll({groupList.Count}):" + (t3 - t2).TotalMilliseconds + " ms ." + Environment.NewLine; ;
                        txtFindDapper += $"FindAuthAll({groupList.Count}):" + (t4 - t3).TotalMilliseconds + " ms ." + Environment.NewLine; ;
                        txtFindDapper += $"FindAll({groupList.Count}):" + (t4 - t1).TotalMilliseconds + " ms ." + Environment.NewLine; ;
                        if (COUNT == 2)
                        {
                            FIND_BUTTON.IsEnabled = true;
                        }
                    });
                }
                catch (Exception ex)
                {
                    txtFindDapper += $"{ex.Message}";
                    FIND_BUTTON.IsEnabled = true;
                    logger.Error(ex);
                }
            });
            a.IsBackground = true;
            b.IsBackground = true;
            a.Start();
            b.Start();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            INI_BUTTON.IsEnabled = false;
            int COUNT = 0;
            txtIni = "";
            Thread a = new Thread(() =>
            {
                try
                {
                    Dapper.DeleteAuthorityAll();
                    Dapper.DeleteUserAll();
                    Dapper.DeleteGroupAll();
                    COUNT++;
                    IsDapperFind = Visibility.Hidden;
                    this.Dispatcher.Invoke(() =>
                        {
                            if (COUNT == 2)
                            {
                                txtIni = "Over";
                                INI_BUTTON.IsEnabled = true;
                            }
                        });
                }
                catch (Exception ex)
                {
                    txtIni += $"{ex.Message}";
                    INI_BUTTON.IsEnabled = true;
                    logger.Error(ex);
                }
            });
            Thread b = new Thread(() =>
            {
                try
                {
                    EF.DeleteAuthorityAll();
                    EF.DeleteUserAll();
                    EF.DeleteGroupAll();
                    COUNT++;
                    IsEFFind = Visibility.Hidden;
                    this.Dispatcher.Invoke(() =>
                        {
                            if (COUNT == 2)
                            {
                                txtIni = "Over";
                                INI_BUTTON.IsEnabled = true;
                            }
                        });
                }
                catch (Exception ex)
                {
                    txtIni += $"{ex.Message}";
                    INI_BUTTON.IsEnabled = true;
                    logger.Error(ex);
                }
            });
            a.IsBackground = true;
            b.IsBackground = true;
            a.Start();
            b.Start();

        }

        private void DELETE_BUTTON_Click(object sender, RoutedEventArgs e)
        {

            txtDeleteDapper = "";
            txtDeleteEF = "";
            DELETE_BUTTON.IsEnabled = false;
            int COUNT = 0;
            Thread a = new Thread(() =>
            {
                try
                {
                    IsDapperDelete = Visibility.Visible;
                    DateTime t1 = DateTime.Now;
                    Dapper.DeleteGroupAll();
                    DateTime t2 = DateTime.Now;
                    Dapper.DeleteUserAll();
                    DateTime t3 = DateTime.Now;
                    Dapper.DeleteAuthorityAll();
                    DateTime t4 = DateTime.Now;

                    COUNT++;
                    IsDapperDelete = Visibility.Hidden;
                    this.Dispatcher.Invoke(() =>
                        {
                    txtDeleteDapper += "Dapper" + Environment.NewLine;
                    txtDeleteDapper += $"DeleteGroupAll:" + (t2 - t1).TotalMilliseconds + " ms ." + Environment.NewLine; ;
                    txtDeleteDapper += $"DeleteUserAll:" + (t3 - t2).TotalMilliseconds + " ms ." + Environment.NewLine; ;
                    txtDeleteDapper += $"DeleteAuthAll:" + (t4 - t3).TotalMilliseconds + " ms ." + Environment.NewLine; ;
                    txtDeleteDapper += $"DeleteAll:" + (t4 - t1).TotalMilliseconds + " ms ." + Environment.NewLine; ;
                    if (COUNT == 2)
                    {
                        DELETE_BUTTON.IsEnabled = true;
                    }
                });
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            });
            Thread b = new Thread(() =>
            {
                try
                {
                    IsEFDelete = Visibility.Visible;
                    DateTime t1 = DateTime.Now;
                    EF.DeleteAuthorityAll();
                    DateTime t2 = DateTime.Now;
                    EF.DeleteUserAll();
                    DateTime t3 = DateTime.Now;
                    EF.DeleteGroupAll();
                    DateTime t4 = DateTime.Now;
                    COUNT++;
                    IsEFDelete = Visibility.Hidden;
                    this.Dispatcher.Invoke(() =>
                        {
                    txtDeleteDapper += "EF" + Environment.NewLine;
                    txtDeleteDapper += $"EF.DeleteAuthAll:" + (t2 - t1).TotalMilliseconds + " ms ." + Environment.NewLine;
                    txtDeleteDapper += $"DeleteUserAll:" + (t3 - t2).TotalMilliseconds + " ms ." + Environment.NewLine;
                    txtDeleteDapper += $"DeleteGroupAll:" + (t4 - t3).TotalMilliseconds + " ms ." + Environment.NewLine;
                    txtDeleteDapper += $"DeleteAll:" + (t4 - t1).TotalMilliseconds + " ms ." + Environment.NewLine;
                    if (COUNT == 2)
                    {
                        DELETE_BUTTON.IsEnabled = true;
                    }
                });
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            });
            a.IsBackground = true;
            b.IsBackground = true;
            a.Start();
            b.Start();

        }
    }
}
