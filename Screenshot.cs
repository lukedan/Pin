using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Pin {
	/// <summary>
	/// Class used to take screenshots.
	/// </summary>
	public static class Screenshot {
		[DllImport("gdi32.dll")]
		private static extern bool DeleteObject(IntPtr hObject);

		/// <summary>
		/// Takes a screenshot of the entire screen.
		/// </summary>
		/// <returns>The screenshot.</returns>
		public static BitmapSource Take() {
			return Take(new Int32Rect(
				(int)SystemParameters.VirtualScreenLeft,
				(int)SystemParameters.VirtualScreenTop,
				(int)SystemParameters.VirtualScreenWidth,
				(int)SystemParameters.VirtualScreenHeight
			));
		}
		/// <summary>
		/// Takes a screenshot for the specified region.
		/// </summary>
		/// <param name="region">The region of the screenshot.</param>
		/// <returns>The screenshot.</returns>
		public static BitmapSource Take(Int32Rect region) {
			using (Bitmap bmp = new Bitmap(region.Width, region.Height)) {
				using (Graphics g = Graphics.FromImage(bmp)) {
					g.CopyFromScreen(region.X, region.Y, 0, 0, bmp.Size);
					IntPtr hbitmap = bmp.GetHbitmap();
					BitmapSource source = Imaging.CreateBitmapSourceFromHBitmap(
						hbitmap,
						IntPtr.Zero,
						new Int32Rect(0, 0, bmp.Width, bmp.Height),
						BitmapSizeOptions.FromWidthAndHeight(bmp.Width, bmp.Height)
					);
					DeleteObject(hbitmap);
					return source;
				}
			}
		}
	}
}
