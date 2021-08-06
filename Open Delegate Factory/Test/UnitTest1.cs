using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Zadanie_2
{
    public class RectangleShape : IShape
    {
        public object size_x;
        public object size_y;
        public RectangleShape(params object[] parameters)
        {
            this.size_x = parameters[0];
            this.size_y = parameters[1];
        }
    }

    public class RectangleFactoryWorker : IShapeFactoryWorker
    {
        public bool AcceptsParameters(string ShapeName, params object[] parameters)
        {
            return ShapeName == "Rectangle" && parameters.Length == 2;
        }

        public IShape Create(string ShapeName, params object[] parameters)
        {
            return new RectangleShape(parameters);
        }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestNotNull()
        {
            var factory = new ShapeFactory();
            IShape square = factory.CreateShape("Square", 5);

            Assert.IsNotNull(square);

            factory.RegisterWorker(new RectangleFactoryWorker());
            IShape rect = factory.CreateShape("Rectangle", 3, 5);

            Assert.IsNotNull(rect);
        }

        [TestMethod]
        public void TestType()
        {
            var factory = new ShapeFactory();
            IShape square = factory.CreateShape("Square", 5);

            Assert.IsInstanceOfType(square, typeof(SquareShape));

            factory.RegisterWorker(new RectangleFactoryWorker());
            IShape rect = factory.CreateShape("Rectangle", 3, 5);

            Assert.IsInstanceOfType(rect, typeof(RectangleShape));
        }

        [TestMethod]
        public void TestInvalidShapeName()
        {
            var factory = new ShapeFactory();

            Assert.ThrowsException<ArgumentNullException>(
                () =>
                {
                    IShape square = factory.CreateShape("Rectangle", 5, 4);   
                });
        }

        [TestMethod]
        public void TestInvalidParams()
        {
            var factory = new ShapeFactory();

            Assert.ThrowsException<ArgumentNullException>(
                () =>
                {
                    IShape square = factory.CreateShape("Square", 5, 6);   
                });
        }
    }
}
