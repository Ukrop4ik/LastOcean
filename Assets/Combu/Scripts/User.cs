using UnityEngine;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Combu
{
	/// <summary>
	/// User class implementing the Unity built-in Social interfaces (specialized IUserProfile, ILocalUser).
	/// </summary>
	[System.Serializable]
	public class User : Profile, ILocalUser
	{
		static System.Exception ExceptionCombuNotInitialized = new System.Exception("Combu Manager not initialized");
		static System.Exception ExceptionOnlyLocalUser = new System.Exception("This method works only on localUser");
		
		public string password;
		
		Profile[] _friends = new Profile[0];
		Profile[] _ignored = new Profile[0];
		Profile[] _requests = new Profile[0];
		bool _authenticated = false;
		
		public User ()
		{
		}
		public User (bool authenticated)
		{
			_authenticated = authenticated;
		}
		public User (string jsonString)
		{
			FromJson(jsonString);
		}
		public User (Hashtable hash)
		{
			FromHashtable(hash);
		}

		public virtual void FromUser (User source)
		{
			Hashtable data = new Hashtable();
			data.Add("Id", source.id);
			data.Add("Username", source.userName);
			data.Add("GUID", source.sessionToken);
			if (source.lastSeen != null)
				data.Add("LastSeen", ((System.DateTime)source.lastSeen).ToString("yyyy-MM-dd HH:mm:ss"));
			data.Add("Email", source.email);
			data.Add("CustomData", source.customData );
			FromHashtable(data);
		}

		#region ILocalUser implementation
		
		/// <summary>
		/// Authenticate the user.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public virtual void Authenticate (System.Action<bool> callback)
		{
			Authenticate(password, (bool success, string error) => {
				if (callback != null)
					callback(success);
			});
		}
		/// <summary>
		/// Authenticate the user with the specified password.
		/// </summary>
		/// <param name="password">Password.</param>
		/// <param name="callback">Callback.</param>
		/// <typeparam name="T">Type for User.</typeparam>
		public virtual void Authenticate (string password, System.Action<bool, string> callback)
		{
			if (!CombuManager.isInitialized)
				throw ExceptionCombuNotInitialized;
			WWWForm form = new WWWForm();
			form.AddField ("action", "login");
			form.AddField ("Username", userName);
			form.AddField ("Password", CombuManager.EncryptMD5 (password));
			DoAuthenticate (form, callback);
		}
		public static void AuthenticateSession (long userId, string token, System.Action<bool, string> callback)
		{
			if (userId > 0 && !string.IsNullOrEmpty (token)) {
				// Set the id and sessionToken for CombuManager.SecureRequest
				User user = new User();
				user._id = userId;
				user._sessionToken = token;

				WWWForm form = CombuManager.instance.CreateForm (user);
				form.AddField ("action", "login_session");
				user.DoAuthenticate (form, callback);
			} else {
				if (callback != null)
					callback (false, "Invalid account or session");
			}
		}
		public virtual void CreateGuest (System.Action<bool, string> callback)
		{
			WWWForm form = new WWWForm ();
			form.AddField ("action", "create_guest");
			DoAuthenticate (form, callback);
		}
		void DoAuthenticate(WWWForm form, System.Action<bool, string> callback)
		{
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("users.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (success && result.ContainsKey("message") && result["message"] != null)
						{
							_authenticated = true;
							FromJson(result["message"].ToString());
							CombuManager.platform.SetLocalUser(this);
						}
						else if (!success && result.ContainsKey("message"))
						{
							error = result["message"].ToString();
						}
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}
		
		/// <summary>
		/// Loads the friends of the current logged user.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public virtual void LoadFriends (System.Action<bool> callback)
		{
			LoadFriends<User>(eContactType.Friend, callback);
		}
		/// <summary>
		/// Loads the friends of the current logged user.
		/// </summary>
		/// <param name="contactType">Contact type.</param>
		/// <param name="callback">Callback.</param>
		public virtual void LoadFriends (eContactType contactType, System.Action<bool> callback)
		{
			LoadFriends<User>(contactType, callback);
		}
		/// <summary>
		/// Loads the friends of the current logged user.
		/// </summary>
		/// <param name="contactType">Contact type.</param>
		/// <param name="callback">Callback.</param>
		/// <typeparam name="T">Type for User.</typeparam>
		public virtual void LoadFriends<T> (eContactType contactType, System.Action<bool> callback) where T: User, new()
		{
			if (!CombuManager.isInitialized)
				throw ExceptionCombuNotInitialized;
			switch (contactType)
			{
			case eContactType.Friend:
				_friends = new Profile[0];
				break;
			case eContactType.Ignore:
				_ignored = new Profile[0];
				break;
			case eContactType.Request:
				_requests = new Profile[0];
				break;
			}
			WWWForm form = CombuManager.instance.CreateForm();
			form.AddField("action", "list");
			form.AddField("Id", id);
			form.AddField("State", ((int)contactType).ToString());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("contacts.php"), form, (string text, string error) => {
				List<T> users = new List<T>();
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null && result.ContainsKey("results"))
					{
						ArrayList list = (ArrayList)result["results"];
						if (list != null)
						{
							foreach (Hashtable data in list)
							{
								// Create a new user object from the result
								T friend = new T();
								friend.FromHashtable(data);
								// Add to the list
								users.Add(friend);
							}
						}
					}
				}
				switch (contactType)
				{
				case eContactType.Friend:
					_friends = users.ToArray();
					break;
				case eContactType.Ignore:
					_ignored = users.ToArray();
					break;
				case eContactType.Request:
					_requests = users.ToArray();
					break;
				}
				if (callback != null)
					callback(string.IsNullOrEmpty(error));
			});
		}
		
		public IUserProfile[] friends { get { return _friends; } }
		public IUserProfile[] ignored { get { return _ignored; } }
		public IUserProfile[] requests { get { return _requests; } }
		public bool authenticated { get { return _authenticated; } }
		public virtual bool underage { get { return false; } }
		
		#endregion
		
		/// <summary>
		/// Update or Create this user to server, whether id is positive and greater than zero.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public virtual void Update (System.Action<bool, string> callback)
		{
			if (!CombuManager.isInitialized)
				throw ExceptionCombuNotInitialized;
			WWWForm form = new WWWForm();
			if (long.Parse(id) > 0)
			{
				form = CombuManager.instance.CreateForm();
				// Update existing account
				form.AddField("action", "update");
				form.AddField("Id", id);
			}
			else
			{
				// Register new account
				form.AddField("action", "create");
				form.AddField("Password", CombuManager.EncryptMD5(password));
			}
			form.AddField("Username", userName);
			if (!string.IsNullOrEmpty(email))
				form.AddField("Email", email);
			form.AddField("CustomData", customData.toJson());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("users.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (success && result.ContainsKey("message") && result["message"] != null)
							FromJson(result["message"].ToString());
						else if (!success && result.ContainsKey("message") && result["message"] != null)
							error = result["message"].ToString();
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}
		
		/// <summary>
		/// Delete this instance from the server.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public virtual void Delete (System.Action<bool, string> callback)
		{
			Delete(userName, password, callback);
		}
		
		/// <summary>
		/// Delete a user from the server.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <param name="callback">Callback.</param>
		public static void Delete (string username, string password, System.Action<bool, string> callback)
		{
			if (!CombuManager.isInitialized)
				throw ExceptionCombuNotInitialized;
			WWWForm form = CombuManager.instance.CreateForm();
			form.AddField("action", "delete");
			form.AddField("Username", username);
			form.AddField("Password", CombuManager.EncryptMD5(password));
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("users.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (!success && result.ContainsKey("message"))
							error = result["message"].ToString();
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}

		/// <summary>
		/// Reload the specified users from server (by Id).
		/// </summary>
		/// <param name="updateUsers">List of users.</param>
		/// <param name="callback">Callback.</param>
		public static void Load (User[] updateUsers, System.Action<bool> callback)
		{
			if (updateUsers == null || updateUsers.Length == 0)
			{
				if (callback != null)
					callback(true);
				return;
			}
			List<long> ids = new List<long>();
			foreach (var user in updateUsers)
				ids.Add(user.idLong);
			Load(ids.ToArray(), (User[] users) => {
				bool success = (users != null && users.Length > 0);
				if (success)
				{
					foreach (var user in users)
					{
						foreach (var me in updateUsers)
						{
							if (user.idLong.Equals(me.idLong))
								me.FromUser(user);
						}
					}
				}
				if (callback != null)
					callback(success);
			});
		}
		
		/// <summary>
		/// Load of the current user from server.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void Load (System.Action<bool> callback)
		{
			Load( new long[] { _id }, (User[] users) => {
				bool success = (users.Length > 0);
				if (success)
					FromUser(users[0]);
				if (callback != null)
					callback(success);
			});
		}
		
		/// <summary>
		/// Loads a user by Id.
		/// </summary>
		/// <param name="userId">User Id.</param>
		/// <param name="callback">Callback.</param>
		public static void Load (long userId, System.Action<User> callback)
		{
			Load(new long[] { userId }, (User[] users) => {
				if (callback != null)
					callback( users.Length > 0 ? users[0] : null );
			});
		}
		
		/// <summary>
		/// Loads a user by userName.
		/// </summary>
		/// <param name="userName">User Name.</param>
		/// <param name="callback">Callback.</param>
		public static void Load (string userName, System.Action<User> callback)
		{
			Load(new string[] { userName }, (User[] users) => {
				if (callback != null)
					callback( users.Length > 0 ? users[0] : null );
			});
		}
		
		/// <summary>
		/// Loads the users by Id.
		/// </summary>
		/// <param name="userIds">User Ids.</param>
		/// <param name="callback">Callback.</param>
		/// <param name="updateUser">If passed its data will be replaced with the server result.</param>
		public static void Load (long[] userIds, System.Action<User[]> callback)
		{
			if (!CombuManager.isInitialized)
				throw ExceptionCombuNotInitialized;
			string[] strUserIds = new string[userIds.Length];
			for (int i = 0; i < userIds.Length; ++i)
			{
				strUserIds[i] = userIds[i].ToString();
			}
			WWWForm form = CombuManager.instance.CreateForm();
			form.AddField("action", "load");
			form.AddField("Ids", string.Join(",", strUserIds));
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("users.php"), form, (string text, string error) => {
				
				List<User> profiles = new List<User>();
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						bool success = false;
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (success)
						{
							ArrayList profilesList = result["message"].ToString().arrayListFromJson();
							if (profilesList != null)
							{
								foreach (Hashtable profileData in profilesList)
								{
									User user = new User(profileData);
									profiles.Add(user);
								}
							}
						}
						else if (result.ContainsKey("message") && result["message"] != null)
						{
							error = result["message"].ToString();
						}
					}
				}
				
				if (callback != null)
					callback(profiles.ToArray());
			});
		}
		
		/// <summary>
		/// Loads the users by userName.
		/// </summary>
		/// <param name="userNames">User Names.</param>
		/// <param name="callback">Callback.</param>
		public static void Load (string[] userNames, System.Action<User[]> callback)
		{
			if (!CombuManager.isInitialized)
				throw ExceptionCombuNotInitialized;
			WWWForm form = CombuManager.instance.CreateForm();
			form.AddField("action", "load");
			form.AddField("Usernames", string.Join(",", userNames));
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("users.php"), form, (string text, string error) => {
				
				List<User> profiles = new List<User>();
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						bool success = false;
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (success)
						{
							ArrayList profilesList = result["message"].ToString().arrayListFromJson();
							if (profilesList != null)
							{
								foreach (Hashtable profileData in profilesList)
								{
									User user = new User(profileData);
									profiles.Add(user);
								}
							}
						}
						else if (result.ContainsKey("message") && result["message"] != null)
						{
							error = result["message"].ToString();
						}
					}
				}
				
				if (callback != null)
					callback(profiles.ToArray());
			});
		}
		
		/// <summary>
		/// Loads the users by searching for the specified parameters.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="email">Email.</param>
		/// <param name="customData">Custom data.</param>
		/// <param name="pageNumber">Page number.</param>
		/// <param name="limit">Limit.</param>
		/// <param name="callback">Callback.</param>
		public static void Load<T> (string username, string email, SearchCustomData[] customData, bool isOnline, int pageNumber, int limit, System.Action<T[], int, int> callback) where T: User, new()
		{
			if (!CombuManager.isInitialized)
				throw ExceptionCombuNotInitialized;
			
			// Build the JSON string for CustomData filtering
			string searchCustomData = "";
			if (customData != null)
			{
				ArrayList list = new ArrayList();
				foreach (SearchCustomData data in customData)
				{
					if (string.IsNullOrEmpty(data.key))
						continue;
					Hashtable search = new Hashtable();
					search.Add("key", data.key);
					search.Add("value", data.value);
					switch (data.op) {
					case eSearchOperator.Equals:
						search.Add("op", "=");
						break;
					case eSearchOperator.Disequals:
						search.Add("op", "!");
						break;
					case eSearchOperator.Like:
						search.Add("op", "%");
						break;
					case eSearchOperator.Greater:
						search.Add("op", ">");
						break;
					case eSearchOperator.GreaterOrEquals:
						search.Add("op", ">=");
						break;
					case eSearchOperator.Lower:
						search.Add("op", "<");
						break;
					case eSearchOperator.LowerOrEquals:
						search.Add("op", "<=");
						break;
					}
					list.Add(search);
				}
				if (list.Count > 0)
					searchCustomData = list.toJson();
			}
			
			WWWForm form = CombuManager.instance.CreateForm();
			form.AddField("action", "search");
			form.AddField("Limit", limit.ToString());
			form.AddField("Page", pageNumber.ToString());
			
			if (isOnline)
				form.AddField("Online", "1");
			
			if (!string.IsNullOrEmpty(username))
				form.AddField("Username", username);
			
			if (!string.IsNullOrEmpty(email))
				form.AddField("Email", email);
			
			if (!string.IsNullOrEmpty(searchCustomData))
				form.AddField("CustomData", searchCustomData);
			
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("users.php"), form, (string text, string error) => {
				
				List<T> profiles = new List<T>();
				int count = 0, pagesCount = 0;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null && result.ContainsKey("total"))
					{
						count = int.Parse(result["total"].ToString());
						pagesCount = int.Parse(result["pages"].ToString());
						ArrayList profilesList = (ArrayList)result["results"];
						if (profilesList != null)
						{
							foreach (Hashtable profileData in profilesList)
							{
								T user = new T();
								user.FromHashtable(profileData);
								profiles.Add(user);
							}
						}
					}
				}
				
				if (callback != null)
					callback(profiles.ToArray(), count, pagesCount);
			});
		}
		
		/// <summary>
		/// Loads a specified count of random users.
		/// </summary>
		/// <param name="customData">Custom data.</param>
		/// <param name="count">Count.</param>
		/// <param name="callback">Callback.</param>
		public static void Random<T> (SearchCustomData[] customData, int count, System.Action<T[]> callback) where T: User, new()
		{
			if (!CombuManager.isInitialized)
				throw ExceptionCombuNotInitialized;
			
			// Build the JSON string for CustomData filtering
			string searchCustomData = "";
			if (customData != null)
			{
				ArrayList list = new ArrayList();
				foreach (SearchCustomData data in customData)
				{
					if (string.IsNullOrEmpty(data.key))
						continue;
					Hashtable search = new Hashtable();
					search.Add("key", data.key);
					search.Add("value", data.value);
					switch (data.op) {
					case eSearchOperator.Equals:
						search.Add("op", "=");
						break;
					case eSearchOperator.Disequals:
						search.Add("op", "!");
						break;
					case eSearchOperator.Like:
						search.Add("op", "%");
						break;
					case eSearchOperator.Greater:
						search.Add("op", ">");
						break;
					case eSearchOperator.GreaterOrEquals:
						search.Add("op", ">=");
						break;
					case eSearchOperator.Lower:
						search.Add("op", "<");
						break;
					case eSearchOperator.LowerOrEquals:
						search.Add("op", "<=");
						break;
					}
					list.Add(search);
				}
				if (list.Count > 0)
					searchCustomData = list.toJson();
			}
			
			WWWForm form = CombuManager.instance.CreateForm();
			form.AddField("action", "random");
			if (!string.IsNullOrEmpty(searchCustomData))
				form.AddField("CustomData", searchCustomData);
			form.AddField("Count", count.ToString());
			
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("users.php"), form, (string text, string error) => {
				
				List<T> profiles = new List<T>();
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null && result.ContainsKey("total"))
					{
						count = int.Parse(result["total"].ToString());
						ArrayList profilesList = (ArrayList)result["results"];
						if (profilesList != null)
						{
							foreach (Hashtable profileData in profilesList)
							{
								T user = new T();
								user.FromHashtable(profileData);
								profiles.Add(user);
							}
						}
					}
				}
				
				if (callback != null)
					callback(profiles.ToArray());
			});
		}
		
		/// <summary>
		/// Verify if it exists an account with the specified username and email.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="email">Email.</param>
		/// <param name="callback">Callback.</param>
		public static void Exists (string username, string email, System.Action<bool, string> callback)
		{
			if (!CombuManager.isInitialized)
				throw ExceptionCombuNotInitialized;
			WWWForm form = CombuManager.instance.CreateForm();
			form.AddField("action", "exists");
			if (!string.IsNullOrEmpty(username))
				form.AddField("Username", username);
			if (!string.IsNullOrEmpty(email))
				form.AddField("Email", email);
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("users.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (success)
						{
							success = bool.Parse(result["message"].ToString());
						}
						else if (result.ContainsKey("message") && result["message"] != null)
						{
							error = result["message"].ToString();
						}
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}
		
		/// <summary>
		/// Resets the password of this user.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public virtual void ResetPassword (System.Action<bool, string> callback)
		{
			ResetPassword(_id, string.Empty, callback);
		}
		
		/// <summary>
		/// Resets the password of a user by Id Account.
		/// </summary>
		/// <param name="idUser">Identifier user.</param>
		/// <param name="callback">Callback.</param>
		public static void ResetPassword (long idUser, System.Action<bool, string> callback)
		{
			ResetPassword(idUser, string.Empty, callback);
		}
		/// <summary>
		/// Resets the password of a user by Username.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="callback">Callback.</param>
		public static void ResetPassword (string username, System.Action<bool, string> callback)
		{
			ResetPassword(0, username, callback);
		}
		/// <summary>
		/// Resets the password of a user.
		/// </summary>
		/// <param name="idUser">Identifier user.</param>
		/// <param name="username">Username.</param>
		/// <param name="callback">Callback.</param>
		static void ResetPassword (long idUser, string username, System.Action<bool, string> callback)
		{
			if (!CombuManager.isInitialized)
				throw ExceptionCombuNotInitialized;
			WWWForm form = CombuManager.instance.CreateForm();
			form.AddField("action", "reset_pwd");
			if (idUser > 0)
				form.AddField("Id", idUser.ToString());
			else if (!string.IsNullOrEmpty(username))
				form.AddField("Username", username);
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("users.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (!success && result.ContainsKey("message") && result["message"] != null)
						{
							error = result["message"].ToString();
						}
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}
		
		/// <summary>
		/// Changes the password of this user.
		/// </summary>
		/// <param name="newPassword">New password.</param>
		/// <param name="callback">Callback.</param>
		public virtual void ChangePassword (string newPassword, System.Action<bool, string> callback)
		{
			ChangePassword(_id, string.Empty, string.Empty, newPassword, (bool success, string error) => {
				if (success)
					FromJson(error);
				if (callback != null)
					callback(success, error);
			});
		}
		
		/// <summary>
		/// Changes the password of a user.
		/// </summary>
		/// <param name="idUser">Identifier user.</param>
		/// <param name="username">Username.</param>
		/// <param name="resetCode">Reset code.</param>
		/// <param name="newPassword">New password.</param>
		/// <param name="callback">Callback.</param>
		public static void ChangePassword (long idUser, string username, string resetCode, string newPassword, System.Action<bool, string> callback)
		{
			if (!CombuManager.isInitialized)
				throw ExceptionCombuNotInitialized;
			WWWForm form = CombuManager.instance.CreateForm();
			form.AddField("action", "change_pwd");
			form.AddField("Password", CombuManager.EncryptMD5(newPassword));
			if (CombuManager.localUser.authenticated && CombuManager.localUser.idLong.Equals(idUser))
			{
				// We are changing the password of localUser, no extra config needed
			}
			else
			{
				if (idUser > 0)
					form.AddField("Id", idUser.ToString());
				else if (!string.IsNullOrEmpty(username))
					form.AddField("Username", username);
				form.AddField("Code", resetCode);
			}
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("users.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (result.ContainsKey("message") && result["message"] != null)
						{
							error = result["message"].ToString();
						}
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}
		
		/// <summary>
		/// Authenticates the user from an external platform (like Facebook, Game Center, GooglePlay etc).
		/// Note you are the only responsible for the external authentication, Combu only stores the info that you send.
		/// </summary>
		/// <param name="platformKey">Platform key.</param>
		/// <param name="platformId">Platform identifier.</param>
		/// <param name="callback">Callback.</param>
		public virtual void AuthenticatePlatform (string platformKey, string platformId, System.Action<bool, string> callback)
		{
			AuthenticatePlatform<User>(platformKey, platformId, callback);
		}
		public virtual void AuthenticatePlatform<T> (string platformKey, string platformId, System.Action<bool, string> callback) where T: User, new()
		{
			if (!CombuManager.isInitialized)
				throw ExceptionOnlyLocalUser;
			WWWForm form = new WWWForm();
			form.AddField("action", "login_platform");
			form.AddField("PlatformKey", platformKey);
			form.AddField("PlatformId", platformId);
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("users.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (success && result.ContainsKey("message") && result["message"] != null)
						{
							_authenticated = true;
							FromJson(result["message"].ToString());
							T user = new T();
							user.FromJson(result["message"].ToString());
							user._authenticated = true;
							CombuManager.platform.SetLocalUser(user);
						}
						else if (!success && result.ContainsKey("message"))
						{
							error = result["message"].ToString();
						}
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}
		
		/// <summary>
		/// Links the currently logged account to another: all the platforms Ids of the current user
		/// will be transferred to the new account and the current account will be deleted.
		/// </summary>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <param name="callback">Callback.</param>
		public virtual void LinkAccount (string username, string password, System.Action<bool, string> callback)
		{
			if (!CombuManager.isInitialized)
				throw ExceptionCombuNotInitialized;
			if (!_id.Equals(CombuManager.localUser.idLong))
				throw ExceptionOnlyLocalUser;
			WWWForm form = CombuManager.instance.CreateForm();
			form.AddField("action", "link_account");
			form.AddField("Username", username);
			form.AddField("Password", CombuManager.EncryptMD5(password));
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("users.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (success && result.ContainsKey("message") && result["message"] != null)
						{
							_authenticated = true;
							FromJson(result["message"].ToString());
							CombuManager.platform.SetLocalUser(this);
						}
						else if (!success && result.ContainsKey("message"))
						{
							error = result["message"].ToString();
						}
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}
		
		/// <summary>
		/// Links a new platform Id to the logged account.
		/// </summary>
		/// <param name="platformKey">Platform key.</param>
		/// <param name="platformId">Platform identifier.</param>
		/// <param name="callback">Callback.</param>
		public virtual void LinkPlatform (string platformKey, string platformId, System.Action<bool, string> callback)
		{
			if (!CombuManager.isInitialized)
				throw ExceptionCombuNotInitialized;
			if (!_id.Equals(CombuManager.localUser.idLong))
				throw ExceptionOnlyLocalUser;
			WWWForm form = CombuManager.instance.CreateForm();
			form.AddField("action", "link_platform");
			form.AddField("PlatformKey", platformKey);
			form.AddField("PlatformId", platformId);
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("users.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (success && result.ContainsKey("message") && result["message"] != null)
						{
							_authenticated = true;
							FromJson(result["message"].ToString());
							CombuManager.platform.SetLocalUser(this);
						}
						else if (!success && result.ContainsKey("message"))
						{
							error = result["message"].ToString();
						}
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}
		
		/// <summary>
		/// Adds the contact.
		/// </summary>
		/// <param name="otherUsername">Other username.</param>
		/// <param name="contactType">Contact type.</param>
		/// <param name="callback">Callback.</param>
		public void AddContact (string otherUsername, eContactType contactType, System.Action<bool, string> callback)
		{
			AddContact( string.Empty, otherUsername, contactType, callback );
		}
		/// <summary>
		/// Adds the contact.
		/// </summary>
		/// <param name="otherUser">Other user.</param>
		/// <param name="contactType">Contact type.</param>
		/// <param name="callback">Callback.</param>
		public void AddContact (Profile otherUser, eContactType contactType, System.Action<bool, string> callback)
		{
			AddContact( otherUser == null ? string.Empty : otherUser.id, string.Empty, contactType, callback );
		}
		/// <summary>
		/// Adds the contact.
		/// </summary>
		/// <param name="idUser">Identifier user.</param>
		/// <param name="username">Username.</param>
		/// <param name="contactType">Contact type.</param>
		/// <param name="callback">Callback.</param>
		void AddContact (string idUser, string username, eContactType contactType, System.Action<bool, string> callback)
		{
			if (!CombuManager.isInitialized)
				throw ExceptionCombuNotInitialized;
			// Use only on localUser
			if (CombuManager.localUser == null || !CombuManager.localUser.id.Equals(id))
				throw ExceptionOnlyLocalUser;
			WWWForm form = CombuManager.instance.CreateForm();
			form.AddField("action", "save");
			if (!string.IsNullOrEmpty(idUser))
			{
				form.AddField("Id", id);
			}
			else if (!string.IsNullOrEmpty(username))
			{
				form.AddField("Username", username);
			}
			else
			{
				if (callback != null)
					callback(false, "No user");
				return;
			}
			form.AddField("State", ((int)contactType).ToString());
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("contacts.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (!success && result.ContainsKey("message") && result["message"] != null)
						{
							error = result["message"].ToString();
						}
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}
		
		/// <summary>
		/// Removes the contact.
		/// </summary>
		/// <param name="otherUsername">Other username.</param>
		/// <param name="callback">Callback.</param>
		public void RemoveContact (string otherUsername, System.Action<bool, string> callback)
		{
			RemoveContact( string.Empty, otherUsername, callback );
		}
		/// <summary>
		/// Removes the contact.
		/// </summary>
		/// <param name="otherUser">Other user.</param>
		/// <param name="callback">Callback.</param>
		public void RemoveContact (Profile otherUser, System.Action<bool, string> callback)
		{
			RemoveContact( otherUser == null ? string.Empty : otherUser.id, string.Empty, callback );
		}
		/// <summary>
		/// Removes the contact.
		/// </summary>
		/// <param name="idUser">Identifier user.</param>
		/// <param name="otherUsername">Other username.</param>
		/// <param name="callback">Callback.</param>
		void RemoveContact (string idUser, string otherUsername, System.Action<bool, string> callback)
		{
			if (!CombuManager.isInitialized)
				throw ExceptionCombuNotInitialized;
			// Use only on localUser
			if (CombuManager.localUser == null || !CombuManager.localUser.id.Equals(id))
				throw ExceptionOnlyLocalUser;
			WWWForm form = CombuManager.instance.CreateForm();
			form.AddField("action", "delete");
			if (!string.IsNullOrEmpty(idUser))
			{
				form.AddField("Id", id);
			}
			else if (!string.IsNullOrEmpty(otherUsername))
			{
				form.AddField("Username", otherUsername);
			}
			else
			{
				if (callback != null)
					callback(false, "No user");
				return;
			}
			CombuManager.instance.CallWebservice(CombuManager.instance.GetUrl("contacts.php"), form, (string text, string error) => {
				bool success = false;
				if (string.IsNullOrEmpty(error))
				{
					Hashtable result = text.hashtableFromJson();
					if (result != null)
					{
						if (result.ContainsKey("success"))
							bool.TryParse(result["success"].ToString(), out success);
						if (!success && result.ContainsKey("message") && result["message"] != null)
						{
							error = result["message"].ToString();
						}
					}
				}
				if (callback != null)
					callback(success, error);
			});
		}

        public void Authenticate(Action<bool, string> callback)
        {
            throw new NotImplementedException();
        }
    }
}
