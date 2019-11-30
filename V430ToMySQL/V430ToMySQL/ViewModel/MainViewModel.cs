using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using System.Reflection;
using V430ToMySQL.Service;

namespace V430ToMySQL.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {

        }

        #region Action
        void Action1() 
        { 
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
            var db = new MySQLService("Server=localhost;Port=3306;Database=database1;Uid=root;Pwd=1226;charset=utf8;Convert Zero Datetime=True");
            var table = db.Select("SELECT * FROM `table1`");
            foreach(var item in table)
            {
                Debug.Write(item["ID"]); Debug.Write("  ");
                Debug.Write(item["Code"]); Debug.Write("  ");
                Debug.Write(item["Time"]);
                Debug.WriteLine("");
            }
        }
        void Action2() { Debug.WriteLine(MethodBase.GetCurrentMethod().Name); }
        void Action3() { Debug.WriteLine(MethodBase.GetCurrentMethod().Name); }
        void Action4() { Debug.WriteLine(MethodBase.GetCurrentMethod().Name); }
        void Action5() { Debug.WriteLine(MethodBase.GetCurrentMethod().Name); }
        void Action6() { Debug.WriteLine(MethodBase.GetCurrentMethod().Name); }
        #endregion

        #region 私有成员
        RelayCommand cmd1;
        RelayCommand cmd2;
        RelayCommand cmd3;
        RelayCommand cmd4;
        RelayCommand cmd5;
        RelayCommand cmd6;

        string _ipV430;
        int _portV430;
        string _ipMySql;
        int _portSql;
        string _nameDatabase;
        string _nameTable;

        #endregion

        #region 公共成员
        public RelayCommand Cmd1 { get => cmd1?? (cmd1 = new RelayCommand(Action1)); }
        public RelayCommand Cmd2 { get => cmd2?? (cmd2 = new RelayCommand(Action2)); }
        public RelayCommand Cmd3 { get => cmd3?? (cmd3 = new RelayCommand(Action3)); }
        public RelayCommand Cmd4 { get => cmd4?? (cmd4 = new RelayCommand(Action4)); }
        public RelayCommand Cmd5 { get => cmd5?? (cmd5 = new RelayCommand(Action5)); }
        public RelayCommand Cmd6 { get => cmd6?? (cmd6 = new RelayCommand(Action6)); }
        public string IpV430 { get => _ipV430; set => _ipV430 = value; }
        public int PortV430 { get => _portV430; set => _portV430 = value; }
        public string IpMySql { get => _ipMySql; set => _ipMySql = value; }
        public int PortSql { get => _portSql; set => _portSql = value; }
        public string NameDatabase { get => _nameDatabase; set => _nameDatabase = value; }
        public string NameTable { get => _nameTable; set => _nameTable = value; }

        #endregion
    }
}