using System;
using System.Data.SqlClient;
using System.Xml;

namespace Zadanie_2
{
    public abstract class DataAccessHandler
    {
        public abstract void Connect();
        public abstract void ReadData();
        public abstract string TransformData();
        public abstract void Close();

        public string Execute()
        {
            this.Connect();
            this.ReadData();
            string res = this.TransformData();
            this.Close();
            return res;
        }
    }

    public class DBSumColumn : DataAccessHandler
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
        public override void Connect()
        {
            this.conn = new SqlConnection(this.connStr);
            this.conn.Open();
        }

        public override void ReadData()
        {
            string sql = String.Format("SELECT SUM({0}) FROM {1}", this.column, this.table);

            this.command = new SqlCommand(sql, this.conn);
            this.reader = command.ExecuteReader();
        }

        public override string TransformData()
        {
            this.reader.Read();
            return this.reader.GetString(0);
        }

        public override void Close()
        {
            this.reader.Close();
            this.command.Dispose();
            this.conn.Close();
        }
    }

    public class XMLLongestName : DataAccessHandler
    {
        XmlDocument doc;
        string path;
        public XMLLongestName(string path)
        {
            this.path = path;
        }
        public override void Connect()
        {
            this.doc = new XmlDocument();
        }

        public override void ReadData()
        {
            this.doc.Load(this.path);
        }

        public override string TransformData()
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

        public override void Close()
        {
            
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            XMLLongestName xml = new XMLLongestName("books.xml");

            string res = xml.Execute();

            Console.WriteLine("Node with longest name: {0}", res);
        }
    }
}
