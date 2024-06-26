﻿using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool eraseMode = false;
        private bool selectMode = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClearCanvas(object sender, RoutedEventArgs e)
        {
            inkCanvas.Strokes.Clear();
        }

        private void EraseModeChange(object sender, RoutedEventArgs e)
        {
            eraseMode = !eraseMode;

            if (eraseMode)
            {
                ((Button)sender).Background = new SolidColorBrush(Colors.Gray);
                inkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            }
            else
            {
                ((Button)sender).Background = new SolidColorBrush(Colors.Black);
                inkCanvas.DefaultDrawingAttributes.Color = ColorPicker.SelectedColor.Value;
                inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            }
        }

        private void Select(object sender, RoutedEventArgs e)
        {
            selectMode = !selectMode;

            if (selectMode)
            {
                ((Button)sender).Background = new SolidColorBrush(Colors.Gray);
                inkCanvas.EditingMode = InkCanvasEditingMode.Select;
            }
            else
            {
                ((Button)sender).Background = new SolidColorBrush(Colors.Black);
                inkCanvas.DefaultDrawingAttributes.Color = ColorPicker.SelectedColor.Value;
                inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            }
        }

        private void SelectedColorChange(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (inkCanvas != null && ColorPicker.SelectedColor.HasValue)
            {
                inkCanvas.DefaultDrawingAttributes.Color = ColorPicker.SelectedColor.Value;
            }
        }

        private void ThicknessChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (inkCanvas != null && ((Slider)sender) != null)
            {
                inkCanvas.DefaultDrawingAttributes.Width = ((Slider)sender).Value;
                inkCanvas.DefaultDrawingAttributes.Height = ((Slider)sender).Value;
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Pliki obrazów|*.jpg;*.png;*.bmp;";
            if (saveFileDialog.ShowDialog() == true)
            {
                SaveAsImage(saveFileDialog.FileName);
            }
        }

        private void SaveAsImage(string fileName)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)inkCanvas.ActualWidth, (int)inkCanvas.ActualHeight, 96d, 96d, PixelFormats.Default);
            rtb.Render(inkCanvas);

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                encoder.Save(fs);
            }
        }
    }
}