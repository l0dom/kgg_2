﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading; 
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KGG_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int canvasSize = 600;
        private int step = 10;
        private double a, b, c, d;
        private double E = 1.41421;
        private bool reverse;
        public MainWindow()
        {
            InitializeComponent(); 
        }
        private void DrawPoint(Vector point)
        {
            double x = Math.Round(point.X * step + canvasSize / 2);
            double y = Math.Round(point.Y * step + canvasSize / 2);
            Point _point = new Point(x, y);
            Ellipse elipse = new Ellipse();

            elipse.Width = 1;
            elipse.Height = 1;

            elipse.StrokeThickness = 1;
            elipse.Stroke = Brushes.Black;
            elipse.Margin = new Thickness(_point.X, _point.Y, 0, 0);

            canvas.Children.Add(elipse);
        }

        private Vector GetCentreHyperbola()
        {
            return new Vector(0,d-b*c);
        }
        
        private Vector[] GetNeighbors(Vector point)
        {
            //var result = new List<Vector>();
            //for (double i = -1; i <= 1; i+=1)
            //    for (double j = -1; j <= 1; j+=1)
            //        if (i != 0 || j != 0)
            //            result.Add(new Vector(point.X + i / step, point.Y + j / step));
            //return result.ToArray();
            return new Vector[] 
            { 
                new Vector(point.X+1.0/step, point.Y),
                new Vector(point.X, point.Y +(reverse ? -1.0 : 1.0)/step)
            }; 
        }
        private double CheckFunction (Vector point)
        {
            return Math.Abs(point.Y * point.X + (d - b * c) * point.X + c * a);
        }
        private Vector GetBestNeighbor(Vector point, Vector last)
        {
            var neighbors = GetNeighbors(point);
            var idOfSmallest = 0;
            var smallestValue = CheckFunction(neighbors[0]);
            for (int i = 1; i < neighbors.Length; i++)
            {
                if (neighbors[i].Equals(last))
                    continue;
                var potencialSmallest = CheckFunction(neighbors[i]);
                if (potencialSmallest < smallestValue)
                {
                    smallestValue = potencialSmallest;
                    idOfSmallest = i;
                }
            }
            return neighbors[idOfSmallest];            
        }

        private bool InCanvas(Vector point)
        {
            return -canvasSize / 2 / step <= point.X && point.X <= canvasSize / 2 / step &&
                -canvasSize / 2 / step <= point.X && point.Y <= canvasSize / 2 / step;
        }

        private void BDraw_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            try
            {
                a = Convert.ToDouble(TA.Text);
                b = Convert.ToDouble(TB.Text);
                c = Convert.ToDouble(TC.Text);
                d = Convert.ToDouble(TD.Text);
                reverse = a<0 ^ c<0;
            }
            catch (FormatException)
            {
                MessageBox.Show("Text must be Double");
                return;
            }
            var centreHyperbola = GetCentreHyperbola();

            int startX = -canvasSize / 2 / step;
            var stepPoint = new Vector(startX, centreHyperbola.Y);
            var lastPoint = stepPoint;
            int i = 0;

            while (InCanvas(stepPoint) && i<600)
            {
                DrawPoint(stepPoint);
                DrawPoint(new Vector(-stepPoint.X, 2 * centreHyperbola.Y - stepPoint.Y));
                Vector tmp = stepPoint;
                stepPoint = GetBestNeighbor(stepPoint, lastPoint);
                lastPoint = tmp;
                i++;
            }

        }
    }
}
