using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;

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
                Application.Current.Dispatcher.BeginInvoke(new Action(() => { TxtMessage.AppendText(message); }));
            });
        }
    }
}
