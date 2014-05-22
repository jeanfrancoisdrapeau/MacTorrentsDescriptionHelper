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
		MonoMac.AppKit.NSButton btnDesuraStoreFetch { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnFindNfo { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnOpenInBrowser { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnParseDesuraUrl { get; set; }

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
		MonoMac.AppKit.NSTextView txtDescription { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextView txtDescriptionCode { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtFullSceneName { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtImageCode { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtNfoFilename { get; set; }

		[Outlet]
		MonoMac.AppKit.NSSecureTextField txtPassWord { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtSearchTerms { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtTagsCode { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtTitleCode { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField txtUserName { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (txtSearchTerms != null) {
				txtSearchTerms.Dispose ();
				txtSearchTerms = null;
			}

			if (btnAppStoreFetch != null) {
				btnAppStoreFetch.Dispose ();
				btnAppStoreFetch = null;
			}

			if (btnDesuraStoreFetch != null) {
				btnDesuraStoreFetch.Dispose ();
				btnDesuraStoreFetch = null;
			}

			if (btnFindNfo != null) {
				btnFindNfo.Dispose ();
				btnFindNfo = null;
			}

			if (btnOpenInBrowser != null) {
				btnOpenInBrowser.Dispose ();
				btnOpenInBrowser = null;
			}

			if (btnParseDesuraUrl != null) {
				btnParseDesuraUrl.Dispose ();
				btnParseDesuraUrl = null;
			}

			if (btnParseFullSceneName != null) {
				btnParseFullSceneName.Dispose ();
				btnParseFullSceneName = null;
			}

			if (btnRebuild != null) {
				btnRebuild.Dispose ();
				btnRebuild = null;
			}

			if (btnUpload != null) {
				btnUpload.Dispose ();
				btnUpload = null;
			}

			if (cmbAppStoreSelect != null) {
				cmbAppStoreSelect.Dispose ();
				cmbAppStoreSelect = null;
			}

			if (cmbComp != null) {
				cmbComp.Dispose ();
				cmbComp = null;
			}

			if (cmbType != null) {
				cmbType.Dispose ();
				cmbType = null;
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

			if (txtDescription != null) {
				txtDescription.Dispose ();
				txtDescription = null;
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

			if (txtNfoFilename != null) {
				txtNfoFilename.Dispose ();
				txtNfoFilename = null;
			}

			if (txtPassWord != null) {
				txtPassWord.Dispose ();
				txtPassWord = null;
			}

			if (txtTagsCode != null) {
				txtTagsCode.Dispose ();
				txtTagsCode = null;
			}

			if (txtTitleCode != null) {
				txtTitleCode.Dispose ();
				txtTitleCode = null;
			}

			if (txtUserName != null) {
				txtUserName.Dispose ();
				txtUserName = null;
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
