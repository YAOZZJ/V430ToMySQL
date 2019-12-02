using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using V430ToMySQL.Service;

namespace V430ToMySQL.ViewModel
{
    public class MainViewModel : MyViewModelBase
    {
        public MainViewModel()
        {
            _time = DateTime.Now.ToLocalTime();
            _Config = Config.LoadAsXML();
        }

        #region Action
        /// <summary>
        /// 窗体关闭操作
        /// </summary>
        void Closing()
        {
            _Config.SaveAsXML();
        }
        /// <summary>
        /// 向MySql插入数据
        /// </summary>
        void Insert()
        {
            string sql = $"INSERT INTO `{TableName}` (`Code`, `Time`) VALUES ('{Code}', '{Time}');";
            Message.WriteLine(sql, "Main");
            if (ConnectToMySql())
            _db.Insert(sql);
            Message.WriteLine("Done", "Main");
        }
        /// <summary>
        /// 连接MySql
        /// </summary>
        /// <returns></returns>
        bool ConnectToMySql()
        {
            try
            {
                _db = new MySQLService($"Server={MySqlIp};" +
                    $"Port={MySqlPort};" +
                    $"Database={DatabaseName};" +
                    $"Uid={UserName};" +
                    $"Pwd={Password};" +
                    $"charset=utf8;Convert Zero Datetime=True");
                MySqlConnected = true;
                return true;
            }
            catch(Exception ex)
            {
                MySqlConnected = false;
                //throw ex;
                Message.WriteLine(ex.Message, "Main");
                return false;
            }
        }
        //void DisConnectToMySql()
        //{
        //    try 
        //    {
        //        if (_db != null || MySqlConnected) _db.CloseConnection();
        //        MySqlConnected = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        MySqlConnected = false;
        //        throw ex;
        //    }
        //}
        /// <summary>
        /// 获取Table数据
        /// </summary>
        void Action1() 
        {
            Messenger.Default.Send<CommandItem>
                (new CommandItem()
                {
                    MySqlIp = this.MySqlIp,
                    MySqlPort  = this.MySqlPort,
                    DatabaseName = this.DatabaseName,
                    TableName = this.TableName,
                    UserName = this.UserName,
                    Password = this.Password
                }
                , "Command");
        }
        void Action2() 
        {
            Message.WriteLine(MethodBase.GetCurrentMethod().Name);

        }
        #endregion

        #region 私有成员
        RelayCommand cmd1;
        RelayCommand cmd2;
        RelayCommand insertCmd;
        RelayCommand closingcmd;

        int _id;
        string _code;
        DateTime _time;

        bool _mySqlConnected = false;

        MySQLService _db;
        readonly Config _Config;

        #endregion

        #region 公共成员
        public RelayCommand Cmd1 { get => cmd1?? (cmd1 = new RelayCommand(Action1)); }
        public RelayCommand Cmd2 { get => cmd2?? (cmd2 = new RelayCommand(Action2)); }
        public RelayCommand InsertCmd { get => insertCmd ?? (insertCmd = new RelayCommand(Insert)); }
        public RelayCommand Closingcmd { get => closingcmd ?? (closingcmd = new RelayCommand(Closing)); }

        public string V430Ip { get => _Config.V430Ip; set { _Config.V430Ip = value; OnPropertyChanged(()=> V430Ip); } }
        public int V430Port { get => _Config.V430Port; set { _Config.V430Port = value; OnPropertyChanged(() => V430Port); } }
        public string MySqlIp { get => _Config.MySqlIp; set { _Config.MySqlIp = value; OnPropertyChanged(() => MySqlIp); } }
        public int MySqlPort { get => _Config.MySqlPort; set { _Config.MySqlPort = value; OnPropertyChanged(() => MySqlPort); } }
        public string DatabaseName { get => _Config.DatabaseName; set { _Config.DatabaseName = value; OnPropertyChanged(() => DatabaseName); } }
        public string TableName { get => _Config.TableName; set { _Config.TableName = value; OnPropertyChanged(() => TableName); } }
        public string LocalIp { get => _Config.LocalIp; set { _Config.LocalIp = value; OnPropertyChanged(() => LocalIp); } }
        public string UserName { get => _Config.UserName; set { _Config.UserName = value; OnPropertyChanged(() => UserName); } }
        public string Password { get => _Config.Password; set { _Config.Password = value; OnPropertyChanged(() => Password); } }
        public bool MySqlConnected { get => _mySqlConnected; set { _mySqlConnected = value; OnPropertyChanged(() => MySqlConnected); } }

        public int Id { get => _id; set { _id = value; OnPropertyChanged(() => Id); } }
        public string Code { get => _code; set { _code = value; OnPropertyChanged(() => Code); } }
        public string Time { get => _time.ToString("yyyy-MM-dd HH:mm:ss.ffff"); set { _time = Convert.ToDateTime(value); OnPropertyChanged(() => Time); } }

        public ObservableCollection<string> V430IpList { get; set; }
        public ObservableCollection<string> MySqlIpList { get; set; }
        public ObservableCollection<string> DatabaseNameList { get; set; }
        public ObservableCollection<string> TableNameList { get; set; }
        public ObservableCollection<int> V430PortList { get; set; }
        public ObservableCollection<int> MySqlPortList { get; set; }


        #endregion
    }
}