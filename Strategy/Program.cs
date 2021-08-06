using System;
using System.Data.SqlClient;
using System.Xml;

namespace Zadanie_3
{
    public interface IDataAccessStategy
    {
        void Connect();
        void ReadData();
        string TransformData();
        void Close();
    }
    public class DataAccessHandler
    {
        private IDataAccessStategy strategy;
        public DataAccessHandler(IDataAccessStategy strategy)
        {
            this.strategy = strategy;
        }
        
        public string Execute()
        {
            strategy.Connect();
            strategy.ReadData();
            string res = strategy.TransformData();
            strategy.Close();
            return res;
        }
    }

    public class DBSumColumn : IDataAccessStategy
    {
        SqlConnection conn;
        SqlCommand command;
        SqlDataReader reader;
        string connStr;
        string table;
        string column;
        public DBSumColumn(string connectionStr, string table, string column)
        {
            this.connStr = connectionStr;
            this.table = table;
            this.column = column;
        }
        public void Connect()
        {
            this.conn = new SqlConnection(this.connStr);
            this.conn.Open();
        }

        public void ReadData()
        {
            string sql = String.Format("SELECT SUM({0}) FROM {1}", this.column, this.table);

            this.command = new SqlCommand(sql, this.conn);
            this.reader = command.ExecuteReader();
        }

        public string TransformData()
        {
            this.reader.Read();
            return this.reader.GetString(0);
        }

        public void Close()
        {
            this.reader.Close();
            this.command.Dispose();
            this.conn.Close();
        }
    }

    public class XMLLongestName : IDataAccessStategy
    {
        XmlDocument doc;
        string path;
        public XMLLongestName(string path)
        {
            this.path = path;
        }
        public void Connect()
        {
            this.doc = new XmlDocument();
        }

        public void ReadData()
        {
            this.doc.Load(this.path);
        }

        public string TransformData()
        {
            int max_len = Int32.MinValue;
            XmlNode max = null;
            XmlNodeList nodeList = this.doc.DocumentElement.SelectNodes("//*");
            foreach(XmlNode elem in nodeList) {
                int len = elem.Name.Length;
                if (len > max_len) {
                    max_len = len;
                    max = elem;
                }
            }
            return max.Name;
        }

        public void Close()
        {
            
        }

    }

    class Program
    {
        static void Main(string[] args)
        {

            DataAccessHandler xml = new DataAccessHandler(new XMLLongestName("books.xml"));

            string res = xml.Execute();

            Console.WriteLine("Node with longest name: {0}", res);
        }
    }
}
