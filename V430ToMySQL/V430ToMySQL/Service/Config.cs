using MyToolkits.Unitities.File.Xml;
using System.Collections.Generic;

namespace V430ToMySQL.Service
{
    public class Config : ObjectPersistence<Config>
    {
        public string V430Ip { get; set; }
        public int V430Port { get; set; }
        public string MySqlIp { get; set; }
        public int MySqlPort { get; set; }
        public string DatabaseName { get; set; }
        public string TableName { get; set; }
        public string LocalIp { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool[] IsEnable { get; set; } = new bool[16];

    }
}
