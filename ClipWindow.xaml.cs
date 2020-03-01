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
using System.Windows.Threading;

namespace Pin {
	/// <summary>
	/// Interaction logic for ClipWindow.xaml
	/// </summary>
	public partial class ClipWindow : Window {
		/// <summary>
		/// Constructor.
		/// </summary>
		public ClipWindow() {
			InitializeComponent();
		}

		/// <summary>
		/// The starting position of the selection.
		/// </summary>
		private Point? _startPosition;

		/// <summary>
		/// Resets <see cref="_startPosition"/> and hides this window.
		/// </summary>
		public void FinishClip() {
			_startPosition = null;
			SetPreview(null);
			Hide();
		}

		/// <summary>
		/// Sets the preview region.
		/// </summary>
		/// <param name="region">The preview region.</param>
		private void SetPreview(Rect? region) {
			if (region.HasValue) {
				selection.Visibility = Visibility.Visible;
				leftCol.Width = new GridLength(region.Value.X);
				midCol.Width = new GridLength(region.Value.Width);
				topRow.Height = new GridLength(region.Value.Y);
				midRow.Height = new GridLength(region.Value.Height);
			} else {
				selection.Visibility = Visibility.Collapsed;
				leftCol.Width = midCol.Width = topRow.Height = midRow.Height = new GridLength(0.0);
			}
		}

		protected override void OnKeyDown(KeyEventArgs e) {
			base.OnKeyDown(e);
			if (e.Key == Key.Escape) {
				FinishClip();
			}
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			if (_startPosition.HasValue) {
				SetPreview(new Rect(_startPosition.Value, e.GetPosition(this)));
			} else {
				SetPreview(null);
			}
		}
		protected override void OnMouseDown(MouseButtonEventArgs e) {
			base.OnMouseDown(e);
			if (e.ChangedButton == MouseButton.Left) {
				_startPosition = PointToScreen(e.GetPosition(this));
			}
		}
		protected override void OnMouseUp(MouseButtonEventArgs e) {
			base.OnMouseUp(e);
			if (e.ChangedButton == MouseButton.Left) {
				if (_startPosition.HasValue) {
					// TODO dpi
					Rect r = new Rect(_startPosition.Value, PointToScreen(e.GetPosition(this)));
					int
						left = (int)Math.Floor(r.Left), width = (int)Math.Ceiling(r.Right) - left,
						top = (int)Math.Floor(r.Top), height = (int)Math.Ceiling(r.Bottom) - top;

					// now that we have the region, close this window so that it won't be in the capture
					FinishClip();

					if (width > 0 && height > 0) {
						PinWindow newWnd = new PinWindow();
						newWnd.SetScreenshot(new Int32Rect(left, top, width, height));
						newWnd.Show();
					}
				}
			}
		}
	}
}
