using System;
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
        private int step = 1;
        private double a, b, c, d;
        private double E = 1.41421;
        public MainWindow()
        {
            InitializeComponent(); 
        }
        private void DrawPoint(Vector point)
        {
            double x = point.X * step + canvasSize / 2;
            double y = point.Y * step + canvasSize / 2;
            if (0 <= x && x <= canvasSize && 0 <= y && y <= canvasSize)
            {
                Point _point = new Point(x, y);
                Ellipse elipse = new Ellipse();

                elipse.Width = 1;
                elipse.Height = 1;

                elipse.StrokeThickness = 1;
                elipse.Stroke = Brushes.Black;
                elipse.Margin = new Thickness(_point.X, _point.Y, 0, 0);

                canvas.Children.Add(elipse);
            }
        }

        private Vector GetCentreHyperbola()
        {
            return new Vector(0,d-b*c);
        }

        private double GetDistanceFromCentreToTop()
        {
            return E * Math.Sqrt(Math.Abs(a * c));
        }

        private Vector[] getNeighbors(Vector point)
        {
            return new Vector[] 
            { 
                new Vector(point.X+1.0/step, point.Y),
                new Vector(point.X-1.0/step, point.Y),
                new Vector(point.X, point.Y+1.0/step),
                new Vector(point.X, point.Y-1.0/step)
            }; 
        }
        private Vector getBestNeighbor(Vector point, Vector last, Vector F1, Vector F2, double distance)
        {
            var neighbors = getNeighbors(point);
            var results = new double[neighbors.Length];
            for (int i = 0; i < neighbors.Length; i++ )
            {
                results[i] = Math.Abs(Math.Abs(new Segment(neighbors[i], F1).Length() - new Segment(neighbors[i], F2).Length()) - 2 * distance);
            }
            int idOfSmallest = 0;
            for (int i = 1; i < neighbors.Length; i++ )
                if (results[i] < results[idOfSmallest] && !neighbors[i].Equals(last) && InCanvas(neighbors[i]))
                    idOfSmallest = i;
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
            }
            catch (FormatException)
            {
                MessageBox.Show("Text must be Double");
                return;
            }
            var centreHyperbola = GetCentreHyperbola();
            var distance = GetDistanceFromCentreToTop();
            var F1 = new Vector(-E * Math.Sqrt(a * c), -E * Math.Sqrt(a * c) - b * c + d);
            var F2 = new Vector(E * Math.Sqrt(a * c), E * Math.Sqrt(a * c) - b * c + d);
            var top1 = new Segment(centreHyperbola, F1).ResizeTo(distance).End;
            var top2 = new Segment(centreHyperbola, F2).ResizeTo(distance).End;

            int startX = -canvasSize / 2 / step;
            Vector stepPoint = new Vector(startX, centreHyperbola.Y);
            Vector lastPoint = new Vector(startX, centreHyperbola.Y);
            while (stepPoint.Equals(top1))
            {
                DrawPoint(stepPoint);
                Vector tmp = stepPoint;
                stepPoint = getBestNeighbor(stepPoint, lastPoint, F1, F2, distance);
                lastPoint = tmp;
            }

            int endX = canvasSize / 2 / step;
            stepPoint = new Vector(endX, centreHyperbola.Y);
            lastPoint = new Vector(endX, centreHyperbola.Y);
            while (InCanvas(stepPoint))
            {
                DrawPoint(stepPoint);
                Vector tmp = stepPoint;
                stepPoint = getBestNeighbor(stepPoint, lastPoint, F1, F2, distance);
                lastPoint = tmp;
            }
        }
    }
}
