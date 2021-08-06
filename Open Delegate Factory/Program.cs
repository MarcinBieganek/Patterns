using System;
using System.Collections.Generic;

namespace Zadanie_2
{
    public interface IShape
    {
        
    }  

    public class SquareShape : IShape
    {
        public object size;
        public SquareShape(params object[] parameters)
        {
            this.size = parameters[0];
        }
    }

    public class CircleShape : IShape
    {
        public object size;
        public CircleShape(params object[] parameters)
        {
            this.size = parameters[0];
        }
    }

    public interface IShapeFactoryWorker
    {
        bool AcceptsParameters(string ShapeName, params object[] parameters);
        IShape Create(string ShapeName, params object[] parameters);
    }
    public class SquareFactoryWorker : IShapeFactoryWorker
    {
        public bool AcceptsParameters(string ShapeName, params object[] parameters)
        {
            return ShapeName == "Square" && parameters.Length == 1;
        }

        public IShape Create(string ShapeName, params object[] parameters)
        {
            return new SquareShape(parameters);
        }
    }

    public class CircleFactoryWorker : IShapeFactoryWorker
    {
        public bool AcceptsParameters(string ShapeName, params object[] parameters)
        {
            return ShapeName == "Circle" && parameters.Length == 1;
        }

        public IShape Create(string ShapeName, params object[] parameters)
        {
            return new CircleShape(parameters);
        }
    }
    public class ShapeFactory
    {
        private List<IShapeFactoryWorker> _workers = new List<IShapeFactoryWorker>();

        public ShapeFactory()
        {
            this._workers.Add(new SquareFactoryWorker());
            this._workers.Add(new CircleFactoryWorker());
        }

        public void RegisterWorker(IShapeFactoryWorker worker)
        {
            _workers.Add(worker);
        }
        public IShape CreateShape(string ShapeName, params object[] parameters)
        {
            foreach (var worker in _workers)
            {
                if (worker.AcceptsParameters(ShapeName, parameters))
                {
                    return worker.Create(ShapeName, parameters);
                }
            }
            
            throw new ArgumentNullException();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
