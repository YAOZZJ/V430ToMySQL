using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MyToolkits.Sockets;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
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
        
        #region MySql
        /// <summary>
        /// 向MySql插入数据
        /// </summary>
        void Insert()
        {
            _time = DateTime.Now.ToLocalTime();
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
        #endregion MySql

        #region Client
        /// <summary>
        /// 连接/断开到端口
        /// </summary>
        void ConnectAction()
        {
            ConnectToServer();
        }

        void SendAction()
        {
            if (_client == null || string.IsNullOrEmpty(Code)) return;
            _client.Send(Code);
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        void ConnectToServer()
        {
            if (_client == null)
            {
                _client = new SocketClient(V430Ip, V430Port);

                //连接成功事件
                _client.HandleClientStarted = new Action<SocketClient>((theClient) =>
                {
                    IsConnect = true;
                    Message.WriteLine($"MyClient |Connected with {_client.RemoteIPEndPoint.Address} {_client.RemoteIPEndPoint.Port}");
                });

                //断开连接事件
                _client.HandleClientClose = new Action<SocketClient>((theClient) =>
                {
                    IsConnect = false;
                    Message.WriteLine($"MyClient |Disonnected with {V430Ip} {V430Port}");

                });

                //Error
                _client.HandleException = new Action<Exception>((ex) =>
                {
                    IsConnect = false;
                    Message.WriteLine($"MyClient |{ex.Message}");
                });

                //当收到服务器发送的消息后的处理事件
                _client.HandleRecMsg = new Action<byte[], SocketClient>((bytes, theClient) =>
                {
                    string msg = Encoding.Default.GetString(bytes);
                    Message.WriteLine($"MyClient |收到消息:{msg}");
                    Code = msg;
                    if (ChkMySqlAutoInsert) Insert();
                    //theClient.Send($"MyClient |收到消息:{msg}", Encoding.Default);
                });
            }
            if (!_client.Connected)
                _client.StartClient();
            else _client.Close();
        }
        #endregion Client

        #region OtherAction

        void LoadedAction()
        {
            Message.WriteLine("Load", "Main");
            if (ChkV430AutoConnect) ConnectAction();
        }

        /// <summary>
        /// 窗体关闭操作
        /// </summary>
        void Closing()
        {
            _Config.SaveAsXML();
        }

        void Action2() 
        {
            Message.WriteLine(MethodBase.GetCurrentMethod().Name);

        }
        #endregion OtherAction
        #endregion Action

        #region 私有成员
        RelayCommand cmd1;
        RelayCommand cmd2;
        RelayCommand insertCmd;
        RelayCommand closingcmd;
        //RelayCommand refreshCmd;
        RelayCommand connectCmd;
        RelayCommand sendCmd;
        RelayCommand loadedCmd;

        int _id;
        string _code;
        DateTime _time;

        bool _isConnect = false;

        bool _mySqlConnected = false;

        MySQLService _db;
        readonly Config _Config;
        SocketClient _client;

        #endregion

        #region 公共成员
        public RelayCommand Cmd1 { get => cmd1?? (cmd1 = new RelayCommand(Action1)); }
        public RelayCommand Cmd2 { get => cmd2?? (cmd2 = new RelayCommand(Action2)); }
        public RelayCommand InsertCmd { get => insertCmd ?? (insertCmd = new RelayCommand(Insert)); }
        public RelayCommand Closingcmd { get => closingcmd ?? (closingcmd = new RelayCommand(Closing)); }
        //public RelayCommand RefreshCmd { get => refreshCmd ?? (refreshCmd = new RelayCommand());}
        public RelayCommand ConnectCmd { get => connectCmd ?? (connectCmd = new RelayCommand(ConnectAction));}
        public RelayCommand SendCmd { get => sendCmd ?? (sendCmd = new RelayCommand(SendAction));}
        public RelayCommand LoadedCmd { get => loadedCmd ?? (loadedCmd = new RelayCommand(LoadedAction));  }


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

        public bool IsConnect { get => _isConnect; set { _isConnect = value; OnPropertyChanged(() => IsConnect); } }

        public bool ChkIdEnable { get => _Config.IsEnable[00]; set { _Config.IsEnable[00] = value; OnPropertyChanged(() => ChkIdEnable); } }
        public bool ChkCodeEnable { get => _Config.IsEnable[01]; set { _Config.IsEnable[01] = value; OnPropertyChanged(() => ChkCodeEnable); } }
        public bool ChkTimeEnable { get => _Config.IsEnable[02]; set { _Config.IsEnable[02] = value; OnPropertyChanged(() => ChkTimeEnable); } }
        public bool ChkIdManual { get => _Config.IsEnable[03]; set { _Config.IsEnable[03] = value; OnPropertyChanged(() => ChkIdManual); } }
        public bool ChkCodeManual { get => _Config.IsEnable[04]; set { _Config.IsEnable[04] = value; OnPropertyChanged(() => ChkCodeManual); } }
        public bool ChkTimeManual { get => _Config.IsEnable[05]; set { _Config.IsEnable[05] = value; OnPropertyChanged(() => ChkTimeManual); } }
        public bool ChkV430AutoConnect { get => _Config.IsEnable[06]; set { _Config.IsEnable[06] = value; OnPropertyChanged(() => ChkV430AutoConnect); } }
        public bool ChkMySqlAutoInsert { get => _Config.IsEnable[07]; set { _Config.IsEnable[07] = value; OnPropertyChanged(() => ChkMySqlAutoInsert); } }


        //public ObservableCollection<string> V430IpList { get; set; }
        //public ObservableCollection<string> MySqlIpList { get; set; }
        //public ObservableCollection<string> DatabaseNameList { get; set; }
        //public ObservableCollection<string> TableNameList { get; set; }
        //public ObservableCollection<int> V430PortList { get; set; }
        //public ObservableCollection<int> MySqlPortList { get; set; }


        #endregion
    }
}