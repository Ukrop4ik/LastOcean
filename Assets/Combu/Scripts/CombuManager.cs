
using UnityEngine;
using UnityEngine.SocialPlatforms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
#if UNITY_WSA
using UnityEngine.Windows;
#endif
using Combu;

namespace Combu
{
	#region Generic enums

	/// <summary>
	/// Contact type.
	/// </summary>
	public enum eContactType : int
	{
		Friend = 0,
		Request,
		Ignore
	}

	/// <summary>
	/// Mail list.
	/// </summary>
	public enum eMailList : int
	{
		Received = 0,
		Sent,
		Both
	}

	/// <summary>
	/// Custom search operator.
	/// </summary>
	public enum eSearchOperator
	{
		Equals,
		Disequals,
		Like,
		Greater,
		GreaterOrEquals,
		Lower,
		LowerOrEquals
	}
	
	/// <summary>
	/// Leaderboard interval.
	/// </summary>
	public enum eLeaderboardInterval : int
	{
		Total = 0,
		Month,
		Week,
		Today
	}
	
	/// <summary>
	/// Leaderboard time scope.
	/// </summary>
	public enum eLeaderboardTimeScope : int
	{
		None,
		Month
	}

	#endregion

	#region Generic classes

	/// <summary>
	/// Search custom data.
	/// </summary>
	[Serializable]
	public class SearchCustomData
	{
		public string key;
		public eSearchOperator op;
		public string value;
		
		public SearchCustomData(string key, eSearchOperator op, string value)
		{
			this.key = key;
			this.op = op;
			this.value = value;
		}
	}

	/// <summary>
	/// Combu server info.
	/// </summary>
	public class CombuServerInfo
	{
		public string version = string.Empty;
		public DateTime time = DateTime.MinValue;
		public Hashtable settings = new Hashtable();

		public CombuServerInfo()
		{
		}
		public CombuServerInfo (Hashtable data)
		{
			if (data.ContainsKey("Version") && data["Version"] != null)
				version = data["Version"].ToString();
			if (data.ContainsKey("Time") && data["Time"] != null)
				DateTime.TryParseExact(data["Time"].ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out time);
			if (data.ContainsKey("Settings") && data["Settings"] != null)
				settings = data["Settings"].ToString().hashtableFromJson();
		}
		public override string ToString ()
		{
			string versionCompare;
			switch (version.CompareTo(CombuManager.COMBU_VERSION))
			{
			case -1:
				versionCompare = "lower: " + CombuManager.COMBU_VERSION;
				break;
			case 1:
				versionCompare = "greater: " + CombuManager.COMBU_VERSION;
				break;
			default:
				versionCompare = "match";
				break;
			}
			return string.Format ("[Combu Server Info] Version: {0} | Time: {1}",
			                      version + " (" + versionCompare + ")",
			                      time.ToString("yyyy-MM-dd HH:mm"));
		}
	}

	#endregion

	/// <summary>
	/// Combu Manager class, it follows the Singleton pattern design (accessible through the 'instance' static property),
	/// it means that you shouldn't have more than one instance of this component in your scene.
	/// </summary>
	public class CombuManager : MonoBehaviour
	{
		/// <summary>
		/// Current API version.
		/// </summary>
		public const string COMBU_VERSION = "2.1.10";

		/// <summary>
		/// The singleton instance of CombuManager
		/// </summary>
		protected static CombuManager _instance;
		/// <summary>
		/// The singleton instance of CombuPlatform.
		/// </summary>
		protected static CombuPlatform _platform;

		/// <summary>
		/// Gets the current singleton instance.
		/// </summary>
		/// <value>The instance.</value>
		public static CombuManager instance { get { return _instance; } }
		/// <summary>
		/// Gets the Combu ISocialPlatform implementation.
		/// </summary>
		/// <value>The platform.</value>
		public static CombuPlatform platform { get { return _platform; } }
		/// <summary>
		/// Gets the local user.
		/// </summary>
		/// <value>The local user.</value>
		public static User localUser { get { return (_platform == null ? null : (User)_platform.localUser); } }
		/// <summary>
		/// Gets a value indicating whether the Singleton instance of <see cref="Combu.CombuManager"/> is initialized.
		/// </summary>
		/// <value><c>true</c> if is initialized; otherwise, <c>false</c>.</value>
		public static bool isInitialized { get { return (_instance != null && _instance._serverInfo != null); } }

		/// <summary>
		/// Should call DontDestroyOnLoad on the CombuManager gameObject?
		/// Recommended: set to true
		/// </summary>
		public bool dontDestroyOnLoad = true;
		/// <summary>
		/// Should set Combu as the active social platform? The previous platform is accessible from defaultSocialPlatform
		/// </summary>
		public bool setAsDefaultSocialPlatform;
		/// <summary>
		/// The secret key: it must match the define <strong>SECRET_KEY</strong> configured on the web.
		/// </summary>
		public string secretKey;
		/// <summary>
		/// The URL root for the production environment.
		/// </summary>
		public string urlRootProduction;
		/// <summary>
		/// The URL root for the stage environment.
		/// </summary>
		public string urlRootStage;
		/// <summary>
		/// If <em>true</em> sets the stage as current environment (default: false for production).
		/// </summary>
		public bool useStage;
		/// <summary>
		/// Print debug info in the console log.
		/// </summary>
		public bool logDebugInfo;
		/// <summary>
		/// The ping interval in seconds (set 0 to disable automatic pings).
		/// Ping is currently used to mantain the online state of the local user
		/// and is automatically called only is the local user is authenticated.
		/// </summary>
		public float pingIntervalSeconds = 30f;
		/// <summary>
		/// The max seconds from now to a user's <strong>lastSeen</strong> to consider the online state.
		/// </summary>
		public int onlineSeconds = 120;
		/// <summary>
		/// The max seconds from now to a user's <strong>lastSeen</strong> to consider the playing state.
		/// </summary>
		public int playingSeconds = 120;
		/// <summary>
		/// Can be used to filter the current system date timezone (the value must be valid in PHP: http://www.php.net/manual/en/timezones.php).
		/// </summary>
		[Obsolete("Timestamps are saved in UTC")]
		public string timezone;

		/// <summary>
		/// The achievement user interface object for CombuPlatform.ShowAchievementsUI().
		/// </summary>
		public GameObject achievementUIObject;
		/// <summary>
		/// The achievement user interface function for CombuPlatform.ShowAchievementsUI().
		/// </summary>
		public string achievementUIFunction;
		/// <summary>
		/// The leaderboard user interface object for CombuPlatform.ShowLeaderboardUI().
		/// </summary>
		public GameObject leaderboardUIObject;
		/// <summary>
		/// The leaderboard user interface function for CombuPlatform.ShowLeaderboardUI().
		/// </summary>
		public string leaderboardUIFunction;
		
		protected ISocialPlatform _defaultSocialPlatform;
		protected CombuServerInfo _serverInfo;
		protected bool downloading, cancelling;

		/// <summary>
		/// Gets the default social platform defined (this is set before Combu is set as activate, eventually).
		/// </summary>
		/// <value>The default social platform.</value>
		public ISocialPlatform defaultSocialPlatform { get { return _defaultSocialPlatform; } }
		/// <summary>
		/// Gets a value indicating whether this <see cref="Combu.CombuManager"/> is downloading from a webservice.
		/// </summary>
		/// <value><c>true</c> if is downloading; otherwise, <c>false</c>.</value>
		public bool isDownloading { get { return downloading; } }
		/// <summary>
		/// Gets a value indicating whether this <see cref="Combu.CombuManager"/> is cancelling a webservice request.
		/// </summary>
		/// <value><c>true</c> if is cancelling; otherwise, <c>false</c>.</value>
		public bool isCancelling { get { return cancelling; } }
		/// <summary>
		/// Gets a value indicating whether <see cref="Combu.CombuManager.localUser"/> is authenticated.
		/// </summary>
		/// <value><c>true</c> if is authenticated; otherwise, <c>false</c>.</value>
		public bool isAuthenticated { get { return (localUser != null && localUser.authenticated); } }
		/// <summary>
		/// Gets the server info.
		/// </summary>
		/// <value>The server info.</value>
		public CombuServerInfo serverInfo { get { return _serverInfo; } }

		protected virtual void Awake ()
		{
			// Ensure we have one only instance
			if (_instance != null)
			{
				Destroy(this);
			}
			else
			{
				if (dontDestroyOnLoad)
					DontDestroyOnLoad(gameObject);
				_instance = this;
				downloading = false;
				cancelling = false;
				_defaultSocialPlatform = Social.Active;
				_platform = new CombuPlatform();
				if (setAsDefaultSocialPlatform)
					Social.Active = _platform;
			}
		}

		void Start ()
		{
			if (!urlRootProduction.EndsWith("/"))
				urlRootProduction += "/";
			if (!urlRootStage.EndsWith("/"))
				urlRootStage += "/";

			// Get the server info
			GetServerInfo((bool success, CombuServerInfo loadedInfo) => {
				if (success)
				{
					_serverInfo = loadedInfo;
					Debug.Log(_serverInfo.ToString());
				}
				else
				{
					_serverInfo = new CombuServerInfo();
					Debug.LogError("Failed to get Combu server info");
				}
			});
		}

		void OnEnable ()
		{
			if (pingIntervalSeconds > 0)
				InvokeRepeating("AutoPing", 0, pingIntervalSeconds);
		}

		void OnDisable ()
		{
			if (pingIntervalSeconds > 0)
				CancelInvoke("AutoPing");
		}
		
		#region Utilities

		/// <summary>
		/// This is InvokeRepeating on enable and automatically pings the server. If the user was authenticated and the ping fails, the local user is automatically disconnected.
		/// </summary>
		void AutoPing ()
		{
			//Ping(true,  null);
			Ping(true, (bool success) => {
				if (!success && localUser.authenticated)
					_platform.SetLocalUser(new User());
			});
		}

		/// <summary>
		/// Calls a webservice.
		/// </summary>
		/// <param name='url'>
		/// URL.
		/// </param>
		/// <param name='form'>
		/// Form.
		/// </param>
		/// <param name='onComplete'>
		/// On complete callback.
		/// </param>
		public void CallWebservice (string url, WWWForm form, System.Action<string, string> onComplete)
		{
//			if (!string.IsNullOrEmpty(timezone))
//				form.AddField("Timezone", timezone);
			StartCoroutine(DownloadUrl(url, form, onComplete));
		}
		
		/// <summary>
		/// Cancels the current request (next frame).
		/// </summary>
		public void CancelRequest ()
		{
			if (downloading && !cancelling)
				cancelling = true;
		}
		
		/// <summary>
		/// Cancels all the current requests (immediately).
		/// </summary>
//		public void CancelAllRequests ()
//		{
//			StopAllCoroutines();
//		}
		
		/// <summary>
		/// Downloads the content of an URL with the specified form.
		/// </summary>
		/// <returns>The URL.</returns>
		/// <param name="url">URL.</param>
		/// <param name="form">Form.</param>
		/// <param name="onComplete">On complete.</param>
		protected IEnumerator DownloadUrl (string url, WWWForm form, Action<string, string> onComplete)
		{
			// Set the flag to know that it's downloading
			downloading = true;
			// Add secure checksum to the form
			SecureRequest(form);
			if (logDebugInfo)
				Debug.Log("Sending: " + url + "?" + System.Text.ASCIIEncoding.ASCII.GetString(form.data));
			// Call the webservice
			WWW www = new WWW(url, form);
			bool cancelled = false;
			while (true)
			{
				if (www.isDone)
					break;
				if (cancelling)
				{
					cancelled = true;
					break;
				}
				yield return null;
			}
			downloading = false;
			cancelling = false;
			string error = (cancelled ? "Request cancelled" : www.error);
			string text = (string.IsNullOrEmpty(error) ? www.text : "");
			if (logDebugInfo)
				Debug.Log("TEXT: " + text + "\nERROR: " + error);
			if (onComplete != null)
				onComplete(text, error);
		}
		
		/// <summary>
		/// Creates a new form to be passed to a webservice.
		/// </summary>
		/// <returns>
		/// The form.
		/// </returns>
		public WWWForm CreateForm (User user = null)
		{
			if (user == null) {
				user = localUser;
			}
			WWWForm form = new WWWForm();
			if (user.authenticated || (user.idLong > 0 && !string.IsNullOrEmpty (user.sessionToken))) {
				form.AddField ("UID", user.id);
				form.AddField ("UGUID", user.sessionToken);
			}
			return form;
		}
		
		/// <summary>
		/// Secure the WWWForm by signing the request.
		/// </summary>
		/// <param name='form'>
		/// Form.
		/// </param>
		protected void SecureRequest (WWWForm form)
		{
			// Exit if secret key is not defined
			if (string.IsNullOrEmpty(secretKey))
				return;
			// Get the current timestamp
			string data = System.DateTime.Now.Ticks.ToString();
			// Create the signature by merging and encrypting the timestamp, the user session token and the secret key
			string signature = EncryptSHA1(data + (localUser == null ? "" : localUser.sessionToken) + secretKey);
			// Add the signature parameters to the request
			form.AddField("sig_time", data);
			form.AddField("sig_crc", signature);
		}
		
		/// <summary>
		/// Gets the absolute URL from a relative.
		/// </summary>
		/// <returns>
		/// The URL.
		/// </returns>
		/// <param name='relativeUrl'>
		/// Relative URL.
		/// </param>
		public string GetUrl (string relativeUrl)
		{
			Uri uri = new Uri( new Uri(useStage ? urlRootStage : urlRootProduction) , relativeUrl);
			return uri.ToString();
		}

		/// <summary>
		/// Captures the screen shot.
		/// </summary>
		/// <returns>The screen shot.</returns>
		/// <param name="thumbnailHeight">Thumbnail height.</param>
		/// <param name="excludeCams">Exclude cams.</param>
		public static byte[] CaptureScreenshot(int thumbnailHeight = 720, List<Camera> excludeCams = null)
		{
			if (excludeCams == null)
				excludeCams = new List<Camera>();
			
			float ratio = (float)Screen.width / (float)Screen.height;
			int thumbnailWidth = Mathf.RoundToInt((float)thumbnailHeight * ratio);
			
			byte[] result;
			
			RenderTexture rt = new RenderTexture(thumbnailWidth, thumbnailHeight, 24);
			Texture2D screenShot = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
			
			List<Camera> allCameras = new List<Camera>();
			allCameras.AddRange(FindObjectsOfType<Camera>());
			allCameras.RemoveAll( cam => !cam.enabled );
			allCameras.Sort(CompareCameraDepth);
			
			for (int i = 0; i < allCameras.Count; ++i)
			{
				Camera cam = allCameras[i];
				if (excludeCams.Contains(cam))
					continue;
				
				cam.targetTexture = rt;
				cam.Render();
				RenderTexture.active = rt;
				screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
				screenShot.Apply();
				
				cam.targetTexture = null;
				RenderTexture.active = null;
			}
			
			result = screenShot.EncodeToPNG();
			
			DestroyImmediate(rt);
			DestroyImmediate(screenShot);
			
			return result;
		}

		/// <summary>
		/// Compares the camera depth (sort the cameras in CaptureScreenshot).
		/// </summary>
		/// <returns>The camera depth.</returns>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		static int CompareCameraDepth (Camera a, Camera b)
		{
			return a.depth.CompareTo(b.depth);
		}

		/// <summary>
		/// Encrypts a string in MD5.
		/// </summary>
		/// <returns>The M d5.</returns>
		/// <param name="inputString">Input string.</param>
		public static string EncryptMD5 (string inputString)
		{
			System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();
			if (!string.IsNullOrEmpty (inputString))
			{
#if UNITY_WSA
                byte[] result = Crypto.ComputeMD5Hash(System.Text.ASCIIEncoding.ASCII.GetBytes(inputString));
#else
				System.Security.Cryptography.MD5 encrypt = new System.Security.Cryptography.MD5CryptoServiceProvider ();
				//compute hash from the bytes of text
				encrypt.ComputeHash (System.Text.ASCIIEncoding.ASCII.GetBytes (inputString));
				//get hash result after compute it
				byte[] result = encrypt.Hash;
#endif
				for (int i = 0; i < result.Length; i++) {
					//change it into 2 hexadecimal digits for each byte
					strBuilder.Append (result [i].ToString ("x2"));
				}
			}
			return strBuilder.ToString();
		}

		/// <summary>
		/// Encrypts a string in SHA1.
		/// </summary>
		/// <returns>The SH a1.</returns>
		/// <param name="inputString">Input string.</param>
		public static string EncryptSHA1 (string inputString)
		{
			System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();
			if (!string.IsNullOrEmpty (inputString))
			{
#if UNITY_WSA
                byte[] result = Crypto.ComputeSHA1Hash (System.Text.ASCIIEncoding.ASCII.GetBytes(inputString));
#else
                System.Security.Cryptography.SHA1 encrypt = new System.Security.Cryptography.SHA1CryptoServiceProvider ();
				//compute hash from the bytes of text
				encrypt.ComputeHash (System.Text.ASCIIEncoding.ASCII.GetBytes (inputString));
				//get hash result after compute it
				byte[] result = encrypt.Hash;
#endif
                for (int i = 0; i < result.Length; i++) {
					//change it into 2 hexadecimal digits
					//for each byte
					strBuilder.Append (result [i].ToString ("x2"));
				}
			}
			return strBuilder.ToString();
		}
		
#endregion

		/// <summary>
		/// Gets the server info.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void GetServerInfo (Action<bool, CombuServerInfo> callback)
		{
			WWWForm form = CreateForm();
			form.AddField("action", "info");
			CallWebservice(GetUrl("server.php"), form, (string text, string error) => {
				bool success = false;
				Hashtable data = new Hashtable();
				if (string.IsNullOrEmpty(error) && !string.IsNullOrEmpty(text))
				{
					data = text.hashtableFromJson();
					if (data != null)
						success = true;
				}
				CombuServerInfo info = null;
				if (success)
					info = new CombuServerInfo(data);
				if (callback != null)
					callback(success, info);
			});
		}

		/// <summary>
		/// Ping the server.
		/// </summary>
		/// <param name="onlyIfAuthenticated">If set to <c>true</c> then it runs only if local user is authenticated.</param>
		/// <param name="callback">Callback.</param>
		public void Ping (bool onlyIfAuthenticated = true, Action<bool> callback = null)
		{
			// Skip if CombuManager is not initialized
			if (!isInitialized)
				return;
			// If requested, do pings only if local user is authenticated
			if (onlyIfAuthenticated && !isAuthenticated)
			{
				if (callback != null)
					callback(false);
				return;
			}
			WWWForm form = CreateForm();
			form.AddField("action", "ping");
			CallWebservice(GetUrl("server.php"), form, (string text, string error) => {
				bool success = false;
				Hashtable data = new Hashtable();
				if (string.IsNullOrEmpty(error) && !string.IsNullOrEmpty(text))
				{
					data = text.hashtableFromJson();
					if (data != null)
					{
						success = bool.Parse(data["success"].ToString());
					}
				}
				if (callback != null)
					callback(success);
			});
		}
	}
}
