using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace MyToolkits.Unitities.File.Xml
{
    /// <summary> 将实体对象持久化的基类，子类将继承此基类即可实现持久化的功能
    /// </summary>
    /// 作者：煎饼的归宿
    /// <typeparam name="T"> 子类对象</typeparam>
    [Serializable]
    public class ObjectPersistence<T> where T : ObjectPersistence<T>, new()
    {
        #region XML
        /// <summary> 加载XML文件
        /// </summary>
        /// <returns> 返回对象</returns>
        public static T LoadAsXML()
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            string path = AppDomain.CurrentDomain.BaseDirectory + typeof(T).Name + ".xml";
            if (!System.IO.File.Exists(path))
            {
                FileStream fs = System.IO.File.Create(path);
                fs.Close();
                T c = new T();
                c.SaveAsXML();
            }
            using (StreamReader sr = new StreamReader(path))
            {
                return xs.Deserialize(sr) as T;
            }
        }

        /// <summary> 加载XML文件
        /// </summary>
        /// <returns> 返回对象</returns>
        public static T LoadAsXML(string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            if (!System.IO.File.Exists(path))
            {
                FileStream fs = System.IO.File.Create(path);
                fs.Close();
                T c = new T();
                c.SaveAsXML(path);
            }
            using (StreamReader sr = new StreamReader(path))
            {
                return xs.Deserialize(sr) as T;
            }
        }

        /// <summary> 保存为XML文件
        /// </summary>
        public void SaveAsXML()
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            string path = AppDomain.CurrentDomain.BaseDirectory + typeof(T).Name + ".xml";
            using (StreamWriter sw = new StreamWriter(path))
            {
                xs.Serialize(sw, this); ;
            }
        }

        /// <summary> 保存为XML文件
        /// </summary>
        public void SaveAsXML(string path)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (StreamWriter sw = new StreamWriter(path))
            {
                xs.Serialize(sw, this); ;
            }
        }
        #endregion

        #region Binary

        /// <summary> 加载二进制流文件（该对象必需标记为可序列化）
        /// </summary>
        /// <returns> 返回对象</returns>
        public static T LoadAsBinary()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + typeof(T).Name + ".Binary";
            if (!System.IO.File.Exists(path))
            {
                FileStream fs = System.IO.File.Create(path);
                fs.Close();
                T c = new T();
                c.SaveAsBinary();
            }
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryFormatter format = new BinaryFormatter();
                return format.Deserialize(fs) as T;
            }
        }

        /// <summary> 加载二进制流文件（该对象必需标记为可序列化）
        /// </summary>
        /// <returns> 返回对象</returns>
        public static T LoadAsBinary(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                FileStream fs = System.IO.File.Create(path);
                fs.Close();
                T c = new T();
                c.SaveAsBinary(path);
            }
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryFormatter format = new BinaryFormatter();
                return format.Deserialize(fs) as T;
            }
        }

        /// <summary> 保存为二进制流文件（该对象必需标记为可序列化）
        /// </summary>
        public void SaveAsBinary()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter format = new BinaryFormatter();
                format.Serialize(stream, this);
                string path = AppDomain.CurrentDomain.BaseDirectory + typeof(T).Name + ".Binary";
                byte[] fileByte = stream.GetBuffer();
                stream.Read(fileByte, 0, fileByte.Length);
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                {
                    fs.Write(fileByte, 0, fileByte.Length); //创建这个文件
                }
            }
        }

        /// <summary> 保存为二进制流文件（该对象必需标记为可序列化）
        /// </summary>
        public void SaveAsBinary(string path)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter format = new BinaryFormatter();
                format.Serialize(stream, this);
                byte[] fileByte = stream.GetBuffer();
                stream.Read(fileByte, 0, fileByte.Length);
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                {
                    fs.Write(fileByte, 0, fileByte.Length); //创建这个文件
                }
            }
        }

        /// <summary> 将对象转换为二进制流字节数组（该对象必需标记为可序列化）
        /// </summary>
        /// <returns> 返回字节数组 </returns>
        public byte[] GetBinaryBytes()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter format = new BinaryFormatter();
                format.Serialize(stream, this);
                return stream.GetBuffer();
            }
        }

        /// <summary> 加载二进制流字节数组，并转化为对象（该对象必需标记为可序列化）
        /// </summary>
        /// <param name="bytes"> 二进制流字节数组 </param>
        /// <returns> 返回对象</returns>
        public static T LoadBinaryBytes(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                BinaryFormatter format = new BinaryFormatter();
                return format.Deserialize(stream) as T;
            }
        }
        #endregion

        #region ALL
        /// <summary> 保存所有的保存类型
        /// </summary>
        public void SaveAsAllType()
        {
            this.SaveAsBinary();
            this.SaveAsXML();
        }
        #endregion

        public override bool Equals(object obj)
        {
            return (obj is ObjectPersistence<T>) && ((ObjectPersistence<T>)obj).GetBinaryBytes().SequenceEqual(this.GetBinaryBytes());
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
