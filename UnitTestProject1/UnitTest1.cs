using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using lab3.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Kvadratris kvad = new Kvadratris();
            kvad.Border1Y = -2;
            kvad.Border2Y = 2;
            kvad.R = 12;
            kvad.Step = 1;
            kvad.DataFill();
            ObservableCollection<Point> fact = new ObservableCollection<Point>() { 
                new Point(7.464, -2), 
                new Point(7.596, -1),
                new Point(double.NaN, 0),
                new Point(7.596, 1),
                new Point(7.464, 2) 
            };
            Assert.AreEqual(kvad.DataList.Count, fact.Count);
            for (int i = 0; i < kvad.DataList.Count;i++)
            {
                Assert.AreEqual(kvad.DataList[i].X, fact[i].X);
                Assert.AreEqual(kvad.DataList[i].Y, fact[i].Y);
            }
        }
        [TestMethod]
        public void TestMethod2()
        {
            Kvadratris kvad = new Kvadratris();
            kvad.Border1Y = -1;
            kvad.Border2Y = 1;
            kvad.R = 1;
            kvad.Step = 1;
            kvad.DataFill();
            ObservableCollection<Point> fact = new ObservableCollection<Point>() {
                new Point(0, -1),
                new Point(double.NaN, 0),
                new Point(0, 1),
            };
            Assert.AreEqual(kvad.DataList.Count, fact.Count);
            for (int i = 0; i < kvad.DataList.Count; i++)
            {
                Assert.AreEqual(kvad.DataList[i].X, fact[i].X);
                Assert.AreEqual(kvad.DataList[i].Y, fact[i].Y);
            }
        }
        [TestMethod]
        public void TestMethod3()
        {
            Kvadratris kvad = new Kvadratris();
            kvad.Border1Y = 0;
            kvad.Border2Y = -0.5;
            kvad.R = 1;
            kvad.Step = 0.1;
            kvad.DataFill();
            ObservableCollection<Point> fact = new ObservableCollection<Point>() {
                new Point(0.5, -0.5),
                new Point(0.551, -0.4),
                new Point(0.589, -0.3),
                new Point(0.616, -0.2),
                new Point(0.631, -0.1),
                new Point(double.NaN, 0),
            };
            Assert.AreEqual(kvad.DataList.Count, fact.Count);
            for (int i = 0; i < kvad.DataList.Count; i++)
            {
                Assert.AreEqual(kvad.DataList[i].X, fact[i].X);
                Assert.AreEqual(kvad.DataList[i].Y, fact[i].Y);
            }
        }
    }
}
