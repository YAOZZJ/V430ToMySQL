using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using V430ToMySQL.Service;

namespace V430ToMySQL.ViewModel
{
    public class MainViewModel : MyViewModelBase
    {
        public MainViewModel()
        {
            _mySqlIp = "127.0.0.1";
            _mySqlPort = 3306;
            _databaseName = "database1";
            _userName = "root";
            _password = "1226";
            
        }

        #region Action
        void ConnectToMySql()
        {
            try
            {
                _db = new MySQLService($"Server={_mySqlIp};" +
                    $"Port={_mySqlPort};" +
                    $"Database={_databaseName};" +
                    $"Uid={_userName};" +
                    $"Pwd={_password};" +
                    $"charset=utf8;Convert Zero Datetime=True");
                MySqlConnected = true;
            }
            catch(Exception ex)
            {
                MySqlConnected = false;
                throw ex;
            }
        }
        void DisConnectToMySql()
        {
            try 
            {
                if (_db != null || MySqlConnected) _db.CloseConnection();
                MySqlConnected = false;
            }
            catch (Exception ex)
            {
                MySqlConnected = false;
                throw ex;
            }
        }
        void Action1() 
        {
            ConnectToMySql();
            _db.Insert(@"INSERT INTO `table1` (`Code`, `Time`) VALUES ('1234', '20191130');");
            //Message.WriteLine(MethodBase.GetCurrentMethod().Name);

            //var table = _db.Select("SELECT * FROM `table1`");
            //foreach(var item in table)
            //{
            //    Message.Write(item["ID"].ToString(),timestamp:false); Message.Write("  ", timestamp: false);
            //    Message.Write(item["Code"].ToString(), timestamp: false); Message.Write("  ", timestamp: false);
            //    Message.Write(item["Time"].ToString(), timestamp: false);
            //    Message.WriteLine(" ", timestamp: false);
            //}
        }
        void Action2() 
        {
            //Message.WriteLine(MethodBase.GetCurrentMethod().Name); 
        }
        void Action3() { Message.WriteLine(MethodBase.GetCurrentMethod().Name); }
        void Action4() { Message.WriteLine(MethodBase.GetCurrentMethod().Name); }
        void Action5() { Message.WriteLine(MethodBase.GetCurrentMethod().Name); }
        void Action6() { Message.WriteLine(MethodBase.GetCurrentMethod().Name); }
        #endregion

        #region 私有成员
        RelayCommand cmd1;
        RelayCommand cmd2;
        RelayCommand cmd3;
        RelayCommand cmd4;
        RelayCommand cmd5;
        RelayCommand cmd6;

        string _v430Ip;
        int _v430Port;
        string _mySqlIp;
        int _mySqlPort;
        string _databaseName;
        string _tableName;
        string _localIp;
        string _userName;
        string _password;

        bool _mySqlConnected = false;

        MySQLService _db;

        #endregion

        #region 公共成员
        public RelayCommand Cmd1 { get => cmd1?? (cmd1 = new RelayCommand(Action1)); }
        public RelayCommand Cmd2 { get => cmd2?? (cmd2 = new RelayCommand(Action2)); }
        public RelayCommand Cmd3 { get => cmd3?? (cmd3 = new RelayCommand(Action3)); }
        public RelayCommand Cmd4 { get => cmd4?? (cmd4 = new RelayCommand(Action4)); }
        public RelayCommand Cmd5 { get => cmd5?? (cmd5 = new RelayCommand(Action5)); }
        public RelayCommand Cmd6 { get => cmd6?? (cmd6 = new RelayCommand(Action6)); }
        public string V430Ip { get => _v430Ip; set { _v430Ip = value; OnPropertyChanged(()=> V430Ip); } }
        public int V430Port { get => _v430Port; set { _v430Port = value; OnPropertyChanged(() => V430Port); } }
        public string MySqlIp { get => _mySqlIp; set { _mySqlIp = value; OnPropertyChanged(() => MySqlIp); } }
        public int MySqlPort { get => _mySqlPort; set { _mySqlPort = value; OnPropertyChanged(() => MySqlPort); } }
        public string DatabaseName { get => _databaseName; set { _databaseName = value; OnPropertyChanged(() => DatabaseName); } }
        public string TableName { get => _tableName; set { _tableName = value; OnPropertyChanged(() => TableName); } }
        public string LocalIp { get => _localIp; set { _localIp = value; OnPropertyChanged(() => LocalIp); } }
        public string UserName { get => _userName; set { _userName = value; OnPropertyChanged(() => UserName); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(() => Password); } }
        public bool MySqlConnected { get => _mySqlConnected; set { _mySqlConnected = value; OnPropertyChanged(() => MySqlConnected); } }


        public ObservableCollection<string> V430IpList { get; set; }
        public ObservableCollection<string> MySqlIpList { get; set; }
        public ObservableCollection<string> DatabaseNameList { get; set; }
        public ObservableCollection<string> TableNameList { get; set; }
        public ObservableCollection<int> V430PortList { get; set; }
        public ObservableCollection<int> MySqlPortList { get; set; }

        #endregion
    }
}