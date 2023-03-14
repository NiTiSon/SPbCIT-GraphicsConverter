using Microsoft.Win32;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System.Globalization;
using System;
using System.Windows;
using System.Windows.Media;

namespace SPbCIT.NiTiSon.GraphicsConverter;

public partial class MainWindow : Window
{
	private WpfDrawingSettings settings;
	private FileSvgReader reader;
	private ImageSvgConverter converter;
	private string? imagePath;

	public MainWindow()
	{
		InitializeComponent();
		settings = new();
		reader = new(settings)
		{
			SaveXaml = false,
			SaveZaml = false,
		};
		converter = new(settings)
		{
			EncoderType = ImageEncoderType.PngBitmap,
		};
	}

	private void ConvertButtonClick(object sender, RoutedEventArgs e)
	{
		if (imagePath is null)
			return;

		SaveFileDialog saveFileDialog = new()
		{
			Filter = "Portable Network Graphics|*.png"
		};

		if (saveFileDialog.ShowDialog() == true)
		{
			if (!converter.Convert(imagePath, saveFileDialog.FileName))
			{
				MessageBox.Show("Unable to convert vector image", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
			else
			{
				MessageBox.Show($"Image saved at path: {saveFileDialog.FileName}", "Success", MessageBoxButton.OK, MessageBoxImage.None);
			}
		}
	}
	private void SelectButtonClick(object sender, RoutedEventArgs e)
	{
		OpenFileDialog dialog = new()
		{
			Filter = "SVG files|*.svg",
		};

		if (dialog.ShowDialog() == true)
		{
			DrawingGroup dg = reader.Read(dialog.FileName);

			canvas.Background = Brushes.Transparent;

			canvas.UnloadDiagrams();

			if (dg != null)
			{
				zoomPan.ZoomTo(dg.Bounds);
				canvas.RenderSize = new(400, 400);
				canvas.RenderDiagrams(dg);

				imagePath = dialog.FileName;
			}
		}
	}
}
