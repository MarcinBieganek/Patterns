using System;
using System.IO;
using System.Text;

namespace Zadanie_2
{
    public class CaesarStream : Stream
    {
        private Stream _stream;
        private int _move;
        public CaesarStream(Stream stream, int move) : base()
        {
            this._stream = stream;
            this._move = move;
        }

        private byte Convert(byte b)
        {
            return (byte)(b + _move);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            for (int i = offset; i < (offset+count); i++) {
                buffer[i] = Convert(buffer[i]);
            }
            _stream.Write(buffer, offset, count);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int res = _stream.Read(buffer, offset, count);
            for (int i = offset; i < (offset+count); i++) {
                buffer[i] = Convert(buffer[i]);
            }
            return res;
        }

        // pozostałe metody i własności nie zmienione
        public override void Close()
        {
            _stream.Close();
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override long Position
        {
            get { return _stream.Position; }
            set { _stream.Position = value; }
        }

        public override long Length
        {
            get { return _stream.Length; }
        }

        public override bool CanWrite
        {
            get { return _stream.CanWrite; }
        }

        public override void Flush()
        {
            _stream.Flush();
        }

        public override bool CanSeek
        {
            get { return _stream.CanSeek; }
        }

        public override bool CanRead
        {
            get { return _stream.CanRead; }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            FileStream fileToWrite = File.Create("test.txt");
            CaesarStream caeToWrite = new CaesarStream(fileToWrite, 5);
            StreamWriter w = new StreamWriter(caeToWrite);

            w.Write("abc\nabc");
            w.Close();

            FileStream fileToRead = File.Open("test.txt", FileMode.Open);
            CaesarStream caeToRead = new CaesarStream(fileToRead, -5);
            StreamReader r = new StreamReader(caeToRead);

            Char[] buffer;
            buffer = new Char[(int)r.BaseStream.Length];
            r.Read(buffer, 0, (int)r.BaseStream.Length);

            Console.Write(buffer);

            r.Close();
        }
    }
}
