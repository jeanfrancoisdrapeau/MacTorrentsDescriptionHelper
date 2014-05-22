// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;
using System.CodeDom.Compiler;

namespace MacTorrentsDescriptionHelper
{
	[Register ("InputBoxController")]
	partial class InputBoxController
	{
		[Outlet]
		MonoMac.AppKit.NSButton btnOk { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtInput { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (txtInput != null) {
				txtInput.Dispose ();
				txtInput = null;
			}

			if (btnOk != null) {
				btnOk.Dispose ();
				btnOk = null;
			}
		}
	}

	[Register ("InputBox")]
	partial class InputBox
	{
		[Outlet]
		MonoMac.AppKit.NSButton btnOk { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtInput { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnOk != null) {
				btnOk.Dispose ();
				btnOk = null;
			}

			if (txtInput != null) {
				txtInput.Dispose ();
				txtInput = null;
			}
		}
	}
}
