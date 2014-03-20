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
	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		[Outlet]
		MonoMac.AppKit.NSButton btnAppStoreFetch { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnParseFullSceneName { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnRebuild { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnUpload { get; set; }

		[Outlet]
		MonoMac.AppKit.NSPopUpButton cmbAppStoreSelect { get; set; }

		[Outlet]
		MonoMac.AppKit.NSComboBox cmbComp { get; set; }

		[Outlet]
		MonoMac.AppKit.NSComboBox cmbType { get; set; }

		[Outlet]
		MonoMac.AppKit.NSImageView imgThumbnail { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField lblReleaseGroup { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField lblReleaseMulti { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField lblReleaseName { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField lblReleaseRegType { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField lblReleaseVersion { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextView txtDescriptionCode { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtFullSceneName { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtImageCode { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtTagsCode { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtTitleCode { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (cmbComp != null) {
				cmbComp.Dispose ();
				cmbComp = null;
			}

			if (btnRebuild != null) {
				btnRebuild.Dispose ();
				btnRebuild = null;
			}

			if (btnUpload != null) {
				btnUpload.Dispose ();
				btnUpload = null;
			}

			if (cmbType != null) {
				cmbType.Dispose ();
				cmbType = null;
			}

			if (btnAppStoreFetch != null) {
				btnAppStoreFetch.Dispose ();
				btnAppStoreFetch = null;
			}

			if (btnParseFullSceneName != null) {
				btnParseFullSceneName.Dispose ();
				btnParseFullSceneName = null;
			}

			if (cmbAppStoreSelect != null) {
				cmbAppStoreSelect.Dispose ();
				cmbAppStoreSelect = null;
			}

			if (imgThumbnail != null) {
				imgThumbnail.Dispose ();
				imgThumbnail = null;
			}

			if (lblReleaseGroup != null) {
				lblReleaseGroup.Dispose ();
				lblReleaseGroup = null;
			}

			if (lblReleaseMulti != null) {
				lblReleaseMulti.Dispose ();
				lblReleaseMulti = null;
			}

			if (lblReleaseName != null) {
				lblReleaseName.Dispose ();
				lblReleaseName = null;
			}

			if (lblReleaseRegType != null) {
				lblReleaseRegType.Dispose ();
				lblReleaseRegType = null;
			}

			if (lblReleaseVersion != null) {
				lblReleaseVersion.Dispose ();
				lblReleaseVersion = null;
			}

			if (txtDescriptionCode != null) {
				txtDescriptionCode.Dispose ();
				txtDescriptionCode = null;
			}

			if (txtFullSceneName != null) {
				txtFullSceneName.Dispose ();
				txtFullSceneName = null;
			}

			if (txtImageCode != null) {
				txtImageCode.Dispose ();
				txtImageCode = null;
			}

			if (txtTagsCode != null) {
				txtTagsCode.Dispose ();
				txtTagsCode = null;
			}

			if (txtTitleCode != null) {
				txtTitleCode.Dispose ();
				txtTitleCode = null;
			}
		}
	}

	[Register ("MainWindow")]
	partial class MainWindow
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
