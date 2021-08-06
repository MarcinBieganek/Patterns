using System;
using System.IO;
using System.Net;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Zadanie_1
{
    public interface ICommand
    {
        void Execute();
    }

    public class FTPDownloadCommand : ICommand
    {
        private Receiver _receiver;
        private string _url;
        private string _email;
        private string _path;
        public FTPDownloadCommand(Receiver receiver, string url, string email, string path)
        {
            this._receiver = receiver;
            this._url = url;
            this._email = email;
            this._path = path;
        }

        public void Execute()
        {
            this._receiver.FTPDownload(this._url, this._email, this._path);
        }

    }

    public class HTTPDownloadCommand : ICommand
    {
        private Receiver _receiver;
        private string _url;
        private string _path;

        public HTTPDownloadCommand(Receiver receiver, string url, string path)
        {
            this._receiver = receiver;
            this._url = url;
            this._path = path;
        }
        public void Execute()
        {
            this._receiver.HTTPDownload(this._url, this._path);
        }
    }

    public class RandomFileCommand : ICommand
    {
        private Receiver _receiver;
        private string _path;
        private uint _size;

        public RandomFileCommand(Receiver receiver, string path, uint size)
        {
            this._receiver = receiver;
            this._path = path;
            this._size = size;
        }
        public void Execute()
        {
            this._receiver.RandomFile(this._path, this._size);
        }
    }

    public class FileCopyCommand : ICommand
    {
        private Receiver _receiver;
        private string _curr_path;
        private string _new_path;

        public FileCopyCommand(Receiver receiver, string curr_path, string new_path)
        {
            this._receiver = receiver;
            this._curr_path = curr_path;
            this._new_path = new_path;
        }
        public void Execute()
        {
            this._receiver.FileCopy(this._curr_path, this._new_path);
        }
    }

    public class Receiver
    {
        public void FTPDownload(string url, string email, string path)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            request.Credentials = new NetworkCredential("anonymous", email);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            FileStream file = File.Create(path);
            StreamWriter writer = new StreamWriter(file);

            writer.WriteLine(reader.ReadToEnd());

            reader.Close();
            writer.Close();
            file.Close();
            response.Close();
        }

        public void HTTPDownload(string url, string path)
        {
            WebClient myWebClient = new WebClient();
            myWebClient.DownloadFile(url, path);
        }

        public void RandomFile(string path, uint size)
        {
            byte[] data = new byte[size];
            Random rng = new Random();
            rng.NextBytes(data);

            File.WriteAllBytes(path, data);
        }

        public void FileCopy(string curr_path, string new_path)
        {
            File.Copy(curr_path, new_path, true);
        }
    }
    
    public class Invoker
    {
        ConcurrentQueue<ICommand> cq;
        Thread retriver1;
        Thread retriver2;

        public Invoker()
        {
            this.cq = new ConcurrentQueue<ICommand>();
            this.retriver1 = new Thread(this.Retrive);
            this.retriver2 = new Thread(this.Retrive);
        }

        private void StartRetrivers()
        {
            if (this.retriver1.ThreadState == ThreadState.Unstarted) {
                this.retriver1.Start();
            } else if (this.retriver1.ThreadState == ThreadState.Stopped) {
                this.retriver1.Join();
                this.retriver1 = new Thread(this.Retrive);
                this.retriver1.Start();
            }
            if (this.retriver2.ThreadState == ThreadState.Unstarted) {
                this.retriver2.Start();   
            } else if (this.retriver2.ThreadState == ThreadState.Stopped) {
                this.retriver2.Join();
                this.retriver2 = new Thread(this.Retrive);
                this.retriver2.Start();
            }
        }

        public void AddCommand(ICommand new_command)
        {
            this.cq.Enqueue(new_command);
            this.StartRetrivers(); 
        }

        public void Retrive()
        {
            ICommand curr_command;
            while(this.cq.TryDequeue(out curr_command)) {
                curr_command.Execute();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Invoker inv = new Invoker();
            
            Receiver r = new Receiver();

            inv.AddCommand(new RandomFileCommand(r, "test.txt", 1024));
            inv.AddCommand(new RandomFileCommand(r, "test2.txt", 512));
            inv.AddCommand(new RandomFileCommand(r, "test3.txt", 16));
            
            inv.AddCommand(new FileCopyCommand(r, "test.txt", "copy_test.txt"));
            inv.AddCommand(new FileCopyCommand(r, "test2.txt", "copy_test2.txt"));

            inv.AddCommand(new HTTPDownloadCommand(r, "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f6/Wikipedia-logo-v2-wordmark.svg/800px-Wikipedia-logo-v2-wordmark.svg.png", "zdj.png"));
        }
    }
}
