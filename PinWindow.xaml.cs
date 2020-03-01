using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pin {
	/// <summary>
	/// Interaction logic for ClipWindow.xaml
	/// </summary>
	public partial class PinWindow : Window {
		/// <summary>
		/// Constructor.
		/// </summary>
		public PinWindow() {
			InitializeComponent();
		}

		/// <summary>
		/// The desired width of this window.
		/// </summary>
		public double DesiredWindowWidth {
			get => image.Source.Width + imageEffect.BlurRadius * 2 + imageEffect.ShadowDepth * 2;
		}
		/// <summary>
		/// The desired height of this window.
		/// </summary>
		public double DesiredWindowHeight {
			get => image.Source.Height + imageEffect.BlurRadius * 2 + imageEffect.ShadowDepth * 2;
		}

		/// <summary>
		/// Sets the bitmap displayed by this window.
		/// </summary>
		/// <param name="bmp">The bitmap.</param>
		public void SetBitmap(BitmapSource bmp) {
			image.Source = bmp;
			Width = DesiredWindowWidth;
			Height = DesiredWindowHeight;
		}

		/// <summary>
		/// Places the image at the specified point.
		/// </summary>
		/// <param name="pt"></param>
		public void Place(Point pt) {
			Left = pt.X - 0.5 * (DesiredWindowWidth - image.Source.Width);
			Top = pt.Y - 0.5 * (DesiredWindowHeight - image.Source.Height);
		}

		/// <summary>
		/// Sets the image to be the screenshot at the given region, and moves the window so that the image overlaps
		/// that region.
		/// </summary>
		public void SetScreenshot(Int32Rect region) {
			SetBitmap(Screenshot.Take(region));
			Place(new Point(region.X, region.Y));
		}

		protected override void OnMouseDown(MouseButtonEventArgs e) {
			base.OnMouseDown(e);
			switch (e.ChangedButton) {
				case MouseButton.Left:
					DragMove();
					break;
				case MouseButton.Middle:
					Close();
					break;
			}
		}
	}
}
