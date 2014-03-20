using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace MacTorrentsDescriptionHelper
{
	public partial class MainWindowController : MonoMac.AppKit.NSWindowController
	{
		#region Constructors

		// Called when created from unmanaged code
		public MainWindowController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MainWindowController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		// Call to load from the XIB/NIB file
		public MainWindowController () : base ("MainWindow")
		{
			Initialize ();
		}
		// Shared initialization code
		void Initialize ()
		{
		}

		#endregion

		//strongly typed window accessor
		public new MainWindow Window {
			get {
				return (MainWindow)base.Window;
			}
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
			btnParseFullSceneName.Activated += event_btnParseFullSceneName_Activated;
			btnAppStoreFetch.Activated += event_btnAppStoreFetch_Activated;
			cmbAppStoreSelect.Activated += event_cmbAppStoreSelect_Activated;
			btnRebuild.Activated += event_btnRebuild_Activated;
			btnUpload.Activated += event_btnUpload_Activated;

			cmbAppStoreSelect.RemoveAllItems ();
		}

		public class ReleaseInfo
		{
			public bool releaseIsCracked { get; set; }
			public bool releaseIsRetail { get; set; }
			public bool releaseIsKeygen { get; set; }

			public bool releaseIsMultilingual { get; set; }

			public string releaseName { get; set; }
			public string releaseVersion { get; set; }
			public string releaseGroup { get; set; }
		}
		public ReleaseInfo _releaseInfo;

		public class iTunesResult
		{
			public iTuneJsonResults[] results { get; set; }
		}
			
		public class iTuneJsonResults
		{
			public string trackId { get; set; }
			public string trackName { get; set; }
			public string artistName { get; set; }
			public string version { get; set; }
			public string description { get; set; }
			public string primaryGenreName { get; set; }
			public string artworkUrl60 { get; set; }
			public string trackViewUrl { get; set; }
			public string releaseNotes { get; set; }
			public string averageUserRatingForCurrentVersion { get; set; }
			public string userRatingCountForCurrentVersion { get; set; }
		}
		public iTunesResult _itunesObj;

		public void event_btnAppStoreFetch_Activated(object sender, EventArgs e)
		{
			//https://itunes.apple.com/search?term=apalon+weather+live&limit=10&media=software&entity=macSoftware
			//Build mac app store fetch string
			string fetchString = "https://itunes.apple.com/search?term=";

			fetchString += _releaseInfo.releaseName.Replace (' ', '+');

			fetchString += "&limit=10&media=software&entity=macSoftware";

			WebClient client = new WebClient();

			// Download string.
			string json = client.DownloadString(fetchString);

			_itunesObj = new iTunesResult();
			_itunesObj = JsonConvert.DeserializeObject<iTunesResult>(json);

			cmbAppStoreSelect.RemoveAllItems ();
			foreach(iTuneJsonResults item in _itunesObj.results)
			{
				cmbAppStoreSelect.AddItem (item.trackId + " : " + item.version + " : " + item.artistName + " " + item.trackName);
			}
		}

		public class WebClientEx : WebClient
		{
			public CookieContainer CookieContainer { get; private set; }

			public WebClientEx()
			{
				CookieContainer = new CookieContainer();
			}

			protected override WebRequest GetWebRequest(Uri address)
			{
				var request = base.GetWebRequest(address);
				if (request is HttpWebRequest)
				{
					(request as HttpWebRequest).CookieContainer = CookieContainer;
				}
				return request;
			}
		}

		public void event_btnUpload_Activated(object sender, EventArgs e)
		{
			FileInfo fInfo = new FileInfo(txtFullSceneName.StringValue);
			long numBytes = fInfo.Length;

			FileStream fStream = new FileStream(txtFullSceneName.StringValue, FileMode.Open, FileAccess.Read);
			BinaryReader br = new BinaryReader(fStream);

			FormUpload.FileParameter fp = new FormUpload.FileParameter (br.ReadBytes((int)numBytes),
				Path.GetFileName (txtFullSceneName.StringValue),
				"application/octet-stream");

			br.Close();
			fStream.Close();

			string releaseType = string.Empty;
			if (cmbType.StringValue == "App")
				releaseType = "1";
			if (cmbType.StringValue == "Game")
				releaseType = "2";

			string releaseVersion = string.Empty;
			if (cmbType.StringValue == "10.6")
				releaseVersion = "1";
			if (cmbType.StringValue == "10.7")
				releaseVersion = "2";
			if (cmbType.StringValue == "10.8")
				releaseVersion = "3";
			if (cmbType.StringValue == "10.9")
				releaseVersion = "4";

			var client = new WebClientEx ();
			var values = new NameValueCollection
			{
				{ "username", "eluder" },
				{ "password", "$N0feaR!" },
			};

			// Authenticate
			client.UploadValues("https://mac-torrents.me/login.php", values);

			// Upload torrent
			Dictionary<string, object> paramsDic = new Dictionary<string, object> ();
			paramsDic.Add ("submit", "true");
			paramsDic.Add ("auth", "1429ad318a720b1a04d9565f7e407fa0");
			paramsDic.Add ("type", releaseType);
			paramsDic.Add ("title", txtTitleCode.StringValue);
			paramsDic.Add ("macos[]", releaseVersion);
			paramsDic.Add ("tags", txtTagsCode.StringValue);
			paramsDic.Add ("image", txtImageCode.StringValue);
			paramsDic.Add ("desc", txtDescriptionCode.Value);
			paramsDic.Add ("file_input", fp);
				
			//Upload torrent
			FormUpload.MultipartFormDataPost (
				"https://mac-torrents.me/upload.php",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_2) " +
					"AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.149 Safari/537.36",
				paramsDic,
				client.CookieContainer);

			var alert = new NSAlert {
				MessageText = "Done!",
				AlertStyle = NSAlertStyle.Informational
			};

			alert.AddButton ("Ok");

			alert.RunModal();
		}

		public void event_btnRebuild_Activated(object sender, EventArgs e)
		{
			rebuildDescCode ();
		}

		public void event_cmbAppStoreSelect_Activated(object sender, EventArgs e)
		{
			string id = cmbAppStoreSelect.TitleOfSelectedItem.Split (':')[0].Trim();

			iTuneJsonResults first = _itunesObj.results.First(o => o.trackId.Equals(id));

			//txtCompCode.StringValue = string.Empty;
			txtTagsCode.StringValue = first.primaryGenreName;
			txtImageCode.StringValue = first.artworkUrl60;

			//rebuildDescCode ();
		}

		public void rebuildDescCode()
		{
			if (cmbAppStoreSelect.TitleOfSelectedItem == null)
				return;

			string id = cmbAppStoreSelect.TitleOfSelectedItem.Split (':')[0].Trim();

			iTuneJsonResults first = _itunesObj.results.First(o => o.trackId.Equals(id));

			//MacTorrents Code
			string finalCode = "[b]Title:[/b] " + _releaseInfo.releaseName + "\n";
			finalCode += "[b]Version:[/b] " + _releaseInfo.releaseVersion + "\n";
			finalCode += "[b]What's New:[/b] \n" + first.releaseNotes + "\n";
			finalCode += "\n";
			finalCode += "[b]Group:[/b] " + _releaseInfo.releaseGroup + "\n";
			finalCode += "[b]Crack:[/b] " + (_releaseInfo.releaseIsCracked ? "Yes" : "No") + "\n";
			finalCode += "[b]Keygen:[/b] " + (_releaseInfo.releaseIsKeygen ? "Yes" : "No") + "\n";
			finalCode += "[b]Retail:[/b] " + (_releaseInfo.releaseIsRetail ? "Yes" : "No") + "\n";
			finalCode += "[b]Original Scene Files:[/b] Yes\n";
			finalCode += "\n";
			finalCode += "[b]Info Link:[/b] " + first.trackViewUrl + "\n";
			finalCode += "[b]System Reqs:[/b] Mac OSX " + cmbComp.StringValue + "\n";
			finalCode += "[b]Rating:[/b] " + first.averageUserRatingForCurrentVersion +
				" from " + first.userRatingCountForCurrentVersion + " users\n";
			finalCode += "\n";
			finalCode += "[b]Info:[/b]\n" + first.description + "\n";

			txtDescriptionCode.Value = finalCode;
		}

		public void event_btnParseFullSceneName_Activated(object sender, EventArgs e)
		{
			var openPanel = new NSOpenPanel();
			openPanel.ReleasedWhenClosed = true;
			openPanel.Prompt = "Select file";
			openPanel.AllowedFileTypes = new string[] {"torrent"};
			openPanel.AllowsMultipleSelection = false;
			openPanel.CanChooseDirectories = false;
			openPanel.CanCreateDirectories = false;

			var result = openPanel.RunModal();
			if (result == 1) {
				txtFullSceneName.StringValue = openPanel.Url.Path;
			} else {
				return;
			}

			_releaseInfo = new ReleaseInfo ();

			string fullSceneName = Path.GetFileNameWithoutExtension(txtFullSceneName.StringValue);

			fullSceneName = fullSceneName.Replace ('.', ' ');
			fullSceneName = fullSceneName.Replace ('_', ' ');
			fullSceneName = fullSceneName.Replace ('-', ' ');

			//Multilingual?
			_releaseInfo.releaseIsMultilingual = fullSceneName.IndexOf ("Multilingual") == -1 && 
				fullSceneName.IndexOf ("Bilingual") == -1 ? false : true;
			fullSceneName = fullSceneName.Replace ("Multilingual", "");
			fullSceneName = fullSceneName.Replace ("Bilingual", "");

			//Cracked, regged, retail, etc.
			_releaseInfo.releaseIsCracked = fullSceneName.IndexOf ("Cracked") == -1 ? false : true;
			_releaseInfo.releaseIsRetail = fullSceneName.IndexOf ("Retail") == -1 ? false : true;
			_releaseInfo.releaseIsKeygen = fullSceneName.IndexOf ("Keygen") == -1 ? false : true;

			int whereIsMacosx = fullSceneName.IndexOf ("MacOSX");
			_releaseInfo.releaseName = string.Empty;
			_releaseInfo.releaseVersion = string.Empty;
			bool parsingVersion = false;

			for (int i = 0; i < whereIsMacosx - 1; i++) {
				if (fullSceneName [i] == 'v' && char.IsNumber(fullSceneName [i + 1]))
					parsingVersion = true;

				if (parsingVersion) {
					_releaseInfo.releaseVersion += fullSceneName [i];
				} else {
					_releaseInfo.releaseName += fullSceneName [i];
				}
			}
			_releaseInfo.releaseName = _releaseInfo.releaseName.Trim ();
			_releaseInfo.releaseVersion = _releaseInfo.releaseVersion.Substring(1).Trim().Replace(' ','.');
				
			string[] fullSceneNameArray = fullSceneName.Split (' ');
			_releaseInfo.releaseGroup = fullSceneNameArray [fullSceneNameArray.Length - 1];

			lblReleaseName.StringValue = _releaseInfo.releaseName;
			lblReleaseVersion.StringValue = _releaseInfo.releaseVersion;
			lblReleaseGroup.StringValue = _releaseInfo.releaseGroup;
			lblReleaseMulti.StringValue = "Multi: " + _releaseInfo.releaseIsMultilingual.ToString ();
			lblReleaseRegType.StringValue = "Reg. Type: ";
			if (_releaseInfo.releaseIsCracked) lblReleaseRegType.StringValue += "Cracked ";
			if (_releaseInfo.releaseIsRetail) lblReleaseRegType.StringValue += "Retail ";
			if (_releaseInfo.releaseIsKeygen) lblReleaseRegType.StringValue += "Keygen ";

			string crackType = string.Empty;
			if (_releaseInfo.releaseIsCracked)
				crackType = "Crack";
			else if (_releaseInfo.releaseIsKeygen)
				crackType = "Keygen";
			else
				crackType = "Retail";

			txtTitleCode.StringValue = _releaseInfo.releaseName + 
				" [Intel/"+ crackType +"] ["+ _releaseInfo.releaseVersion +"]";
		}
	}
}

