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

			Window.Title += " " + NSBundle.MainBundle.InfoDictionary ["CFBundleVersion"];

			btnParseFullSceneName.Activated += event_btnParseFullSceneName_Activated;
			btnAppStoreFetch.Activated += event_btnAppStoreFetch_Activated;
			btnDesuraStoreFetch.Activated += event_btnDesuraStoreFetch_Activated;
			btnParseDesuraUrl.Activated += event_btnParseDesuraUrl_Activated;
			cmbAppStoreSelect.Activated += event_cmbAppStoreSelect_Activated;
			btnRebuild.Activated += event_btnRebuild_Activated;
			btnUpload.Activated += event_btnUpload_Activated;
			btnOpenInBrowser.Activated += event_btnOpenInBrowser_Activated;
			btnFindNfo.Activated += event_btnFindNfo_Activated;

			cmbAppStoreSelect.RemoveAllItems ();
		}
			
		public void event_btnOpenInBrowser_Activated(object sender, EventArgs e)
		{
			string id = cmbAppStoreSelect.TitleOfSelectedItem.Split (':')[0].Trim();

			//https://itunes.apple.com/ca/app/id795396190
			string url = "https://itunes.apple.com/ca/app/id" + id;
			System.Diagnostics.Process.Start(url);
		
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

			public string releaseWhatsNew { get; set; }
			public string releaseUrl { get; set; }
			public string releaseRating { get; set; }
			public string releaseRatingNb { get; set; }
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
			public string artworkUrl100 { get; set; }
			public string trackViewUrl { get; set; }
			public string releaseNotes { get; set; }
			public string averageUserRatingForCurrentVersion { get; set; }
			public string userRatingCountForCurrentVersion { get; set; }
			public string averageUserRating { get; set; }
			public string userRatingCount { get; set; }
		}
		public iTunesResult _itunesObj;

		public void event_btnFindNfo_Activated(object sender, EventArgs e)
		{
			var openPanel = new NSOpenPanel();
			openPanel.ReleasedWhenClosed = true;
			openPanel.Prompt = "Select file";
			openPanel.AllowedFileTypes = new string[] {"nfo"};
			openPanel.AllowsMultipleSelection = false;
			openPanel.CanChooseDirectories = false;
			openPanel.CanCreateDirectories = false;

			var result = openPanel.RunModal();
			if (result == 1) {
				txtNfoFilename.StringValue = openPanel.Url.Path;
			} else {
				return;
			}
		}

		public void event_btnDesuraStoreFetch_Activated(object sender, EventArgs e)
		{
			string s = _releaseInfo.releaseName.Replace (' ', '+');
			string url = "http://www.desura.com/search?q=" + s;
			System.Diagnostics.Process.Start(url);
		}

		public void event_btnParseDesuraUrl_Activated(object sender, EventArgs e)
		{
			InputBoxController ibc = new InputBoxController();
			ibc.TitleText = "Enter Desura app URL";
			ibc.Window.SetFrame (new System.Drawing.RectangleF (
				new System.Drawing.PointF(
					this.Window.Frame.X + (this.Window.Frame.Width / 2) - (ibc.Window.Frame.Width / 2),
					this.Window.Frame.Y + (this.Window.Frame.Height / 2) - (ibc.Window.Frame.Height / 2)),
				new System.Drawing.SizeF(436, 75)),
				true);
			NSApplication.SharedApplication.RunModalForWindow(ibc.Window);

			string inputText = ibc.InputText;
			if (string.IsNullOrEmpty(inputText))
				return;

			
		}

		public void event_btnAppStoreFetch_Activated(object sender, EventArgs e)
		{
			//https://itunes.apple.com/search?term=apalon+weather+live&limit=10&media=software&entity=macSoftware
			//Build mac app store fetch string
			string fetchString = "https://itunes.apple.com/search?term=";

			fetchString += txtSearchTerms.StringValue.Replace (' ', '+');

			fetchString += "&limit=10&media=software&entity=macSoftware";

			WebClient client = new WebClient();
			client.DownloadStringCompleted += event_downloadStringAsyncCompleted;
			client.DownloadStringAsync (fetchString);
		}

		public void event_downloadStringAsyncCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			// Download string.
			string json = e.Result;

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
			if (string.IsNullOrEmpty (txtUserName.StringValue) || string.IsNullOrEmpty (txtPassWord.StringValue)) {
				var alert = new NSAlert {
					MessageText = "Username or password empty.",
					AlertStyle = NSAlertStyle.Informational
				};

				alert.AddButton ("Ok");

				alert.RunModal();
				return;
			}

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

			var client = new WebClientEx ();
			var values = new NameValueCollection
			{
				{ "username", txtUserName.StringValue },
				{ "password", txtPassWord.StringValue },
			};

			// Authenticate
			client.UploadValues("https://mac-torrents.me/login.php", values);
			string tempResponse = client.DownloadString ("https://mac-torrents.me/upload.php");

			string searchFor = "<input type=\"hidden\" name=\"auth\" value=\"";
			int indexSearchFor = tempResponse.IndexOf (searchFor) + searchFor.Length;
			string authString = tempResponse.Substring (indexSearchFor, 32);

			// Upload torrent
			Dictionary<string, object> paramsDic = new Dictionary<string, object> ();
			paramsDic.Add ("submit", "true");
			paramsDic.Add ("auth", authString);
			paramsDic.Add ("type", releaseType);
			paramsDic.Add ("title", txtTitleCode.StringValue);
			paramsDic.Add ("tags", txtTagsCode.StringValue);
			paramsDic.Add ("image", txtImageCode.StringValue);
			paramsDic.Add ("desc", txtDescriptionCode.Value);
			paramsDic.Add ("file_input", fp);

			if (cmbComp.StringValue == "10.6") {
				paramsDic.Add ("macos[]", new string[] {"1", "2", "3", "4"});
			}
			if (cmbComp.StringValue == "10.7")
			{
				paramsDic.Add ("macos[]", new string[] {"2", "3", "4"});
			}
			if (cmbComp.StringValue == "10.8")
			{
				paramsDic.Add ("macos[]", new string[] {"3", "4"});
			}			
			if (cmbComp.StringValue == "10.9")
			{
				paramsDic.Add ("macos[]", "4");
			}
			//Upload torrent
			FormUpload.MultipartFormDataPost (
				"https://mac-torrents.me/upload.php",
				"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_2) " +
					"AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.149 Safari/537.36",
				paramsDic,
				client.CookieContainer);

			try
			{
				tempResponse = client.DownloadString ("https://mac-torrents.me/logout.php?auth=" + authString);
			} catch {
			}

			txtNfoFilename.StringValue = "";
			txtDescription.Value = "";
			txtDescriptionCode.Value = "";
			txtTagsCode.StringValue = "";
			txtImageCode.StringValue = "";

			var alertEnd = new NSAlert {
				MessageText = "Done!",
				AlertStyle = NSAlertStyle.Informational
			};

			alertEnd.AddButton ("Ok");

			alertEnd.RunModal();
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
			txtTagsCode.StringValue = first.primaryGenreName.Replace(' ',',');
			txtImageCode.StringValue = first.artworkUrl100;
			txtDescription.Value = first.description;

			_releaseInfo.releaseWhatsNew = first.releaseNotes;
			_releaseInfo.releaseUrl = first.trackViewUrl;
			_releaseInfo.releaseRating = first.averageUserRating;
			_releaseInfo.releaseRatingNb = first.userRatingCount;
			//rebuildDescCode ();
		}

		public void rebuildDescCode()
		{
			if (_releaseInfo == null)
				return;

			//MacTorrents Code
			string typeOfCrack = string.Empty;
			if (_releaseInfo.releaseIsCracked)
				typeOfCrack = "Crack";
			if (_releaseInfo.releaseIsKeygen)
				typeOfCrack = "Keygen";
			if (_releaseInfo.releaseIsRetail)
				typeOfCrack = "Retail";

			string appStoreRating = string.Empty;
			if (_releaseInfo.releaseRatingNb != "0") {
				appStoreRating = (string.IsNullOrEmpty (_releaseInfo.releaseRating) ? "0" : _releaseInfo.releaseRating) +
					" (" + (string.IsNullOrEmpty (_releaseInfo.releaseRatingNb) ? "0" : _releaseInfo.releaseRatingNb) + " users)";
			} else {
				appStoreRating = "Unknown";
			}

			string finalCode = "[b]Name:[/b] " + _releaseInfo.releaseName + "\n";
			finalCode += "[b]Version:[/b] - " + _releaseInfo.releaseVersion + "\n\n";
			finalCode += "[b]Mac Platform:[/b] Intel\n";
			finalCode += "[b]Includes:[/b] " + typeOfCrack + "*\n\n";
			finalCode += "[b]OS version:[/b] " + cmbComp.StringValue + " or higher\n\n";
			finalCode += "[b]Link for more information:[/b] " + _releaseInfo.releaseUrl + "\n";
			finalCode += "[b]Rating:[/b] " + appStoreRating + "\n\n";
			finalCode += "[b]Info:[/b]\n\n" + txtDescription.Value + "\n\n";
			finalCode += "[b]What's New:[/b] \n" + 
				(string.IsNullOrEmpty(_releaseInfo.releaseWhatsNew) ? "Unknown or First version" : _releaseInfo.releaseWhatsNew) + "\n\n";
			finalCode += "[b]*" + typeOfCrack + " courtesy of " + _releaseInfo.releaseGroup + "[/b]\n";
			finalCode += "[b]Original Scene Files:[/b] Yes\n\n";

			if (!string.IsNullOrEmpty (txtNfoFilename.StringValue)) {
				using (StreamReader sr = new StreamReader (txtNfoFilename.StringValue, Encoding.GetEncoding(28591))) {
					string line = sr.ReadToEnd ();

					finalCode += "[b]NFO:[/b]\n" +
						"[hide][pre]" +
					line +
						"[/pre][/hide]\n\n";
				}
			}

			finalCode += "[b]Unzip/Unrar HowTo:[/b]\n" +
				"[hide][pre]" +
				"Option A:\n" +
				"- Download and install my app\n" +
				"  https://mac-torrents.me/forums.php?action=viewthread&threadid=64\n" +
				"- Make a shortcut to it in the launchpad\n" +
				"- Drag&drop any Zip or Rar files on the shortcut\n" +
				"- The app will extract all the files in a new folder\n" +
				"- Open the new folder\n" +
				"- Repeat the process until everything has been extracted.\n\n" + 
				"Option B:\n" +
				"- Open the terminal (CLI)\n" +
				"- Install Brew **DO THIS ONLY THE FIRST TIME**\n" +
				"  ruby -e \"$(curl -fsSL https://raw.github.com/Homebrew/homebrew/go/install)\"\n" +
				"- Install Unrar **DO THIS ONLY THE FIRST TIME**\n" +
				"  brew install unrar\n" +
				"- Change directory to where you downloaded the torrent\n" +
				"  ie. cd /Users/Jeff/Downloads/Torrent/Bob.Came.In.Pieces.v1.5.MacOSX-WaLMaRT\n" +
				"- To unzip files, use this:\n" +
				"  unzip -u -o \\*.zip\n" +
				"- To unrar files, use this:\n" +
				"  unrar e -y first_rar_file.rar\n" +
				"- Repeat the process until everything has been extracted.\n" +
				"[/pre][/hide]\n\n" +
				"[b]Install HowTo:[/b]\n" +
				"[hide][pre]" +
				"Most of my uploads are divided into three categories:\n" +
				"1. Retail/Patched\n" +
				"- Once everything has been extracted, simply drag&drop the app in your Applications folder\n" +
				"- Done!\n" +
				"2. Keygen\n" +
				"- Just like 1., except that mostly on the first run, the application will ask you for a\n" +
				"  registration key.\n" +
				"- Use the provided keygen to generate an activation key\n" +
				"- Usually copy/paste that key in the application and you're done.\n" +
				"3. Crack\n" +
				"- Just like 1., but sometimes the crack will need to be applied manually\n" +
				"- Copy/paste the cracked file(s) in the application folder and overwrite existing files.\n" +
				"- Done!\n" +
				"[/pre][/hide]\n\n" +
				"As always, if you need any help, just PM me!";

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
			_releaseInfo.releaseIsKeygen = false;
			if (fullSceneName.IndexOf ("Keyfilemaker") != -1 || 
				fullSceneName.IndexOf ("Keygen") != -1 || fullSceneName.IndexOf ("Keymaker") != -1)
				_releaseInfo.releaseIsKeygen = true;

			int whereIsMacosx = fullSceneName.ToUpper().IndexOf ("MACOSX");
			_releaseInfo.releaseName = string.Empty;
			_releaseInfo.releaseVersion = string.Empty;
			bool parsingVersion = false;

			for (int i = 0; i < whereIsMacosx - 1; i++) {
				if (fullSceneName.ToUpper() [i] == 'V' && char.IsNumber(fullSceneName [i + 1]))
					parsingVersion = true;

				if (parsingVersion) {
					_releaseInfo.releaseVersion += fullSceneName [i];
				} else {
					_releaseInfo.releaseName += fullSceneName [i];
				}
			}

			if (string.IsNullOrEmpty (_releaseInfo.releaseVersion))
				_releaseInfo.releaseVersion = "v1.0";

			_releaseInfo.releaseName = _releaseInfo.releaseName.Trim ();
			_releaseInfo.releaseVersion = _releaseInfo.releaseVersion.Substring(1).Trim().Replace(' ','.');
				
			string[] fullSceneNameArray = fullSceneName.Split (' ');
			_releaseInfo.releaseGroup = fullSceneNameArray [fullSceneNameArray.Length - 1];

			txtSearchTerms.StringValue = _releaseInfo.releaseName;
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
				" [Intel:"+ crackType +"] ["+ _releaseInfo.releaseVersion +"]";
		}
	}
}

