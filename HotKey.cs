using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Pin {
	/// <summary>
	/// Interface for registering and unregistering global hotkeys.
	/// </summary>
	public sealed class HotKey : IDisposable {
		#region WinAPI
		[DllImport("User32.dll")]
		private static extern bool RegisterHotKey([In] IntPtr hWnd, [In] int id, [In] uint fsModifiers, [In] uint vk);

		[DllImport("User32.dll")]
		private static extern bool UnregisterHotKey([In] IntPtr hWnd, [In] int id);

		private const int WM_HOTKEY = 0x0312;

		/// <summary>
		/// Modifiers for a hotkey.
		/// </summary>
		[Flags]
		public enum Modifiers : uint {
			/// <summary>
			/// The Alt key.
			/// </summary>
			Alt = 0x0001,
			/// <summary>
			/// The Control key.
			/// </summary>
			Control = 0x0002,
			/// <summary>
			/// The Shift key.
			/// </summary>
			Shift = 0x0004,
			/// <summary>
			/// The Win key.
			/// </summary>
			Win = 0x0008,

			/// <summary>
			/// Disables auto-repeat for this hotkey when it's held down.
			/// </summary>
			NoRepeat = 0x4000
		}
		#endregion

		/// <summary>
		/// Does nothing.
		/// </summary>
		public HotKey() {
		}
		/// <summary>
		/// Initializes the hotkey immediately.
		/// </summary>
		/// <param name="window">The window that'll receive hotkey events.</param>
		/// <param name="id">The index of the hotkey.</param>
		/// <param name="modifiers">Modifiers of the hotkey.</param>
		/// <param name="key">The primary key of the hotkey.</param>
		/// <seealso cref="Register(Window, int, Modifiers, Key)"/>
		public HotKey(Window window, int id, Modifiers modifiers, Key key) {
			Register(window, id, modifiers, key);
		}

		/// <summary>
		/// Handles <c>WndProc</c>.
		/// </summary>
		private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
			if (msg == WM_HOTKEY && wParam.ToInt32() == _id) {
				HotKeyPressed(this, new EventArgs());
				handled = true;
			}
			return IntPtr.Zero;
		}

		/// <summary>
		/// Registers a hotkey, unregistering the previous one if necessary.
		/// </summary>
		/// <param name="window">The window that'll receive hotkey events.</param>
		/// <param name="id">The index of the hotkey.</param>
		/// <param name="modifiers">Modifiers of the hotkey.</param>
		/// <param name="key">The primary key of the hotkey.</param>
		public void Register(Window window, int id, Modifiers modifiers, Key key) {
			Unregister();
			if (window != null) {
				var helper = new WindowInteropHelper(window);
				_id = id;
				_handle = helper.Handle;
				_source = HwndSource.FromHwnd(_handle);
				_source.AddHook(HwndHook);
				if (!RegisterHotKey(_handle, id, (uint)modifiers, (uint)KeyInterop.VirtualKeyFromKey(key))) {
					throw new SystemException("Failed to register hotkey.");
				}
			}
		}
		/// <summary>
		/// Unregisters the current hotkey.
		/// </summary>
		public void Unregister() {
			if (_source != null) {
				_source.RemoveHook(HwndHook);
				UnregisterHotKey(_handle, _id);

				_source.Dispose();
				_source = null;
			}
		}

		/// <summary>
		/// The native window handle.
		/// </summary>
		private IntPtr _handle;
		/// <summary>
		/// The <see cref="HwndSource"/>.
		/// </summary>
		private HwndSource _source;
		/// <summary>
		/// The identifier of this hotkey.
		/// </summary>
		private int _id;

		/// <summary>
		/// Invoked whenever the hotkey is pressed.
		/// </summary>
		public event EventHandler HotKeyPressed;

		public void Dispose() {
			Unregister();
		}
	}
}
