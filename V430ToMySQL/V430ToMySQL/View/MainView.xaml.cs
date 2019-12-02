using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using V430ToMySQL.Service;

namespace V430ToMySQL.View
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            Messenger.Default.Register<String>(this, "Message", message =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { AppendText(message); }));
            });
            Messenger.Default.Register<CommandItem>(this, "Command", command =>
            {
                //Application.Current.Dispatcher.BeginInvoke(new Action(() => { Refresh(command); }));
                Refresh(command);
            });
        }
        CommandItem _command;
        void Refresh(CommandItem command)
        {
            Task.Factory.StartNew(() =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => 
                {
                    try
                    {
                        _command = command;
                        AppendText($"{DateTime.Now.ToString(" HH:mm:ss.fff")} | Main | select * from {_command.TableName}\r\n");
                        MySQLService _db = new MySQLService($"Server={_command.MySqlIp};" +
                                $"Port={_command.MySqlPort};" +
                                $"Database={_command.DatabaseName};" +
                                $"Uid={_command.UserName};" +
                                $"Pwd={_command.Password};" +
                                $"charset=utf8;Convert Zero Datetime=True");
                        DtProduct.ItemsSource = _db.GetTable($"select * from {_command.TableName}").AsDataView();
                    }
                    catch (Exception ex)
                    {
                        AppendText($"{DateTime.Now.ToString(" HH:mm:ss.fff")} | Main | {ex.Message}");
                    }
                    AppendText($"{DateTime.Now.ToString(" HH:mm:ss.fff")} | Main | Done\r\n");
                }));
            });
        }
        /// <summary>
        /// 修改时间列显示格式,也可用sql语句转换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DtProduct_AutoGeneratingColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "yyyy-MM-dd HH:mm:ss.fff";
        }
        void AppendText(string msg)
        {
            TxtMessage.AppendText(msg);
        }
    }
}
