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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pin {
	/// <summary>
	/// Interaction logic for SettingsWindow.xaml
	/// </summary>
	public partial class SettingsWindow : Window {
		/// <summary>
		/// Initializes <see cref="_hotkey"/>.
		/// </summary>
		public SettingsWindow() {
			InitializeComponent();
		}

		protected override void OnSourceInitialized(EventArgs e) {
			base.OnSourceInitialized(e);
			_hotkey = new HotKey(this, 1, HotKey.Modifiers.Alt | HotKey.Modifiers.Shift | HotKey.Modifiers.Control, Key.P);
			_hotkey.HotKeyPressed += _hotkeyPressed;
		}
		protected override void OnClosed(EventArgs e) {
			base.OnClosed(e);
			_hotkey.Dispose();
			_clipWnd.Close();
		}

		/// <summary>
		/// Takes a new snapshot.
		/// </summary>
		private void _hotkeyPressed(object sender, EventArgs e) {
			_clipWnd.Show();
		}

		/// <summary>
		/// The hotkey trigger.
		/// </summary>
		private HotKey _hotkey;
		/// <summary>
		/// The window used for clipping.
		/// </summary>
		private ClipWindow _clipWnd = new ClipWindow();
	}
}
