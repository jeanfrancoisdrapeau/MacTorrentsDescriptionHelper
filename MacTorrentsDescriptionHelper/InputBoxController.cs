using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace MacTorrentsDescriptionHelper
{
	public partial class InputBoxController : MonoMac.AppKit.NSWindowController
	{
		#region Constructors

		// Called when created from unmanaged code
		public InputBoxController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public InputBoxController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		// Call to load from the XIB/NIB file
		public InputBoxController () : base ("InputBox")
		{
			Initialize ();
		}
		// Shared initialization code
		void Initialize ()
		{
		}

		#endregion

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();

			Window.Title = TitleText;

			btnOk.Activated += event_btnOk_Activated;

		}
		//strongly typed window accessor
		public new InputBox Window {
			get {
				return (InputBox)base.Window;
			}
		}

		public string InputText = string.Empty;
		public string TitleText = string.Empty;

		public void event_btnOk_Activated(object sender, EventArgs e)
		{
			InputText = txtInput.StringValue;
			NSApplication.SharedApplication.StopModal ();
			this.Close ();
		}
	}
}

