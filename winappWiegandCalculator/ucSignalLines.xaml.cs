using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.UI;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace winappWiegandCalculator
{
    public sealed partial class ucSignalLines : UserControl
    {
        private double m_DeltaHeight;
        private double m_DeltaWidth;

        private double m_StartOfLinesHeight;

        public List<bool> ListData
        {
            get { return (List<bool>)GetValue(DateValProperty); }
            set { SetValue(DateValProperty, value); }
        }

        public static DependencyProperty DateValProperty =
          DependencyProperty.Register("ListData", typeof(List<bool>), typeof(ucSignalLines),
          new PropertyMetadata(new List<bool>()));

        public ucSignalLines()
        {
            this.InitializeComponent();

        }  // of ucSignalLines()

        public void ClearDrawing()
        {
            canvasLines.Children.Clear();

        }  // of ClearDrawing()

        public void DrawLinesAndText()
        {
            double l_CanvasHeight = canvasLines.ActualHeight;
            double l_CanvasWidth = canvasLines.ActualWidth;

            m_DeltaHeight = l_CanvasHeight / 5;
            m_DeltaWidth = l_CanvasWidth / 8;

            m_StartOfLinesHeight = 85;

            // add legend for protocol and number of bits
            TextBlock l_tbkNumberOfBits = new TextBlock();
            l_tbkNumberOfBits.Text = string.Format("Signal with {0} bits", ListData.Count);
            SolidColorBrush l_Brush = new SolidColorBrush(Colors.Black);
            l_tbkNumberOfBits.Foreground = l_Brush;
            l_tbkNumberOfBits.Style = (Style)Application.Current.Resources["BasicTextStyle"];
            Windows.UI.Xaml.Controls.Canvas.SetLeft(l_tbkNumberOfBits, 10);
            Windows.UI.Xaml.Controls.Canvas.SetTop(l_tbkNumberOfBits, 20);
            canvasLines.Children.Add(l_tbkNumberOfBits);

            // legend for data lines
            Line l_LegendData0Line = new Line();
            l_LegendData0Line.X1 = 60;
            l_LegendData0Line.Y1 = 55;
            l_LegendData0Line.X2 = 70;
            l_LegendData0Line.Y2 = 55;
            l_LegendData0Line.Stroke = new SolidColorBrush(Colors.Blue);
            canvasLines.Children.Add(l_LegendData0Line);
            TextBlock l_tbkData0 = new TextBlock();
            l_tbkData0.Text = "Data 0";
            l_tbkData0.Foreground = new SolidColorBrush(Colors.Black);
            l_tbkData0.Style = (Style)Application.Current.Resources["BasicTextStyle"];
            Windows.UI.Xaml.Controls.Canvas.SetLeft(l_tbkData0, 10);
            Windows.UI.Xaml.Controls.Canvas.SetTop(l_tbkData0, 45);
            canvasLines.Children.Add(l_tbkData0);

            Line l_LegendData1Line = new Line();
            l_LegendData1Line.X1 = 60;
            l_LegendData1Line.Y1 = 75;
            l_LegendData1Line.X2 = 70;
            l_LegendData1Line.Y2 = 75;
            l_LegendData1Line.Stroke = new SolidColorBrush(Colors.Red);
            canvasLines.Children.Add(l_LegendData1Line);
            TextBlock l_tbkData1 = new TextBlock();
            l_tbkData1.Text = "Data 1";
            l_tbkData1.Foreground = new SolidColorBrush(Colors.Black);
            l_tbkData1.Style = (Style)Application.Current.Resources["BasicTextStyle"];
            Windows.UI.Xaml.Controls.Canvas.SetLeft(l_tbkData1, 10);
            Windows.UI.Xaml.Controls.Canvas.SetTop(l_tbkData1, 65);
            canvasLines.Children.Add(l_tbkData1);

            // add horizontal line
            Line l_HorizontalLine = new Line();
            l_HorizontalLine.X1 = 0;
            l_HorizontalLine.Y1 = m_StartOfLinesHeight;
            l_HorizontalLine.X2 = l_CanvasWidth;
            l_HorizontalLine.Y2 = l_HorizontalLine.Y1;
            l_HorizontalLine.Stroke = new SolidColorBrush(Colors.DarkMagenta);
            canvasLines.Children.Add(l_HorizontalLine);

            if (ListData.Count == 0)
                return;

            // for each bit add a vertical line
            double l_HorizontalInterval = (6 * m_DeltaWidth) / ListData.Count;
            double l_StartX = m_DeltaWidth;
            foreach (bool b in ListData)
            {
                l_StartX += l_HorizontalInterval;
                AddVerticalLine(l_StartX, (b) ? Colors.Red : Colors.Blue);

            }  // of foreach        
        
        }  // of DrawLinesAndText()

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DrawLinesAndText();

        }  // of UserControl_Loaded()

        private void AddVerticalLine(double p_X, Color p_Color)
        {
            Line l_VerticalLine = new Line();
            l_VerticalLine.X1 = p_X;
            l_VerticalLine.Y1 = m_StartOfLinesHeight;
            l_VerticalLine.X2 = p_X;
            l_VerticalLine.Y2 = 4.0 * m_DeltaHeight;
            SolidColorBrush l_Brush = new SolidColorBrush(p_Color);
            l_VerticalLine.Stroke = l_Brush;

            canvasLines.Children.Add(l_VerticalLine);

        }  // of AddVerticalLine()

    }  // of class ucSignalLines

}  // of namespace winappWiegandCalculator
