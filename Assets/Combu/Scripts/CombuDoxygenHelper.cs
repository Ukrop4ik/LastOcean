/*
 * IGNORE THIS FILE.
 * This file is only here for internal usage, specifically it contains special commands for Doxygen to automatically generate the API documentation.
 * You can ignore it, anyway consider that you can run Doxygen (http://www.doxygen.org) and specify this folder as source to create the documentation directly from the scripts.
 */

/*! \mainpage API Reference
	\section sec_main_intro Introduction
	<a href="http://skaredcreations.com/wp/products/combu/" target="_blank">Combu</a> is a full featured solution
	to add online storage management for your player login system, highscores, friends, inventory and more
	for your games using a web server and MySQL database. It is shipped with client source code for Unity3D.

	This documentation is for <strong>version 2.x</strong> only. The documentation for <strong>version 1.x</strong> is shipped as PDF(s) within the package purchased.

	The 2nd generation of <strong>Combu</strong> marks a great step towards the optimal organization
	both for the ease of use and for a better integration within Unity system.
	The new version of Combu implements the <a href="http://docs.unity3d.com/ScriptReference/Social.html" target="_blank">Social API</a> of Unity,
	this way most of the standard code will work the same way as any other
	<a href="http://docs.unity3d.com/ScriptReference/SocialPlatforms.ISocialPlatform.html" target="_blank">ISocialPlatform</a>.

	\section sec_main_install Installation
	<ol>
		<li>
			<strong>Purchase Combu</strong>
			<br/>You can purchase Combu from <strong>Skared Creations</strong> <a href="http://skaredcreations.com/wp/downloads/combu/" target="_blank">website</a>
			or Unity3D <a href="http://u3d.as/4tg" target="_blank">Asset Store</a>.
		</li>
		<li>
			<strong>Setup the web server</strong>
			<br/>You can use a local web server before going live to production. Read the <a href="http://skaredcreations.com/wp/support/forum/combu-1/faq/#p269" target="_blank">FAQ</a>
			to learn how to setup the web server.
		</li>
		<li>
			<strong>Import Combu package</strong>
			<br/>Create an empty scene and import the Combu unitypackage (or from Asset Store window).
		</li>
		<li>
			<strong>Configure Combu Manager</strong>
 			<br/>Drag the prefab <em>Combu/Prefabs/CombuManager</em> in the scene and correct settings of the <strong>CombuManager</strong> component in order to connect to your web server.
		</li>
	</ol>
	The first time you will access the admin console (at: http://yourserver/combu_folder_path/admin) the system will create
	a new admin account with username and password <strong><em>admin</em></strong> and you should be automatically logged. As soon as you access
	the admin console for the first time on your live server you're strongly suggested to change the password of user <strong>admin</strong>
	in the section <strong>Admins</strong> or delete it and create a new user with a different username and password.

	To get started visit <a href="pages.html">Related Pages</a>.

	\section sec_main_doc Documentation
	You can download the API documentation as offline <a href="http://skaredcreations.com/api/combu/refman.pdf" target="_blank">PDF</a>
	or navigate online at <a href="http://skaredcreations.com/api/combu">http://skaredcreations.com/api/combu</a>

	You can also use <a href="http://www.doxygen.org" target="_blank">Doxygen</a> to build the HTML/PDF documentation directly from the Unity scripts of Combu.

	\section sec_main_assetstore Customers from Asset Store
	If you purchased Combu on <strong>Unity Asset Store</strong>, now you can redeem the download on this website too at no cost by providing the Invoice No.
	<a href="http://skaredcreations.com/wp/redeem-asset-store-invoice/" target="_blank">Click here</a> to redeem your invoice now.
 */

/*! \page page_doc Off-line documentation
	\tableofcontents
	\brief Download this documentation as <a href="http://skaredcreations.com/api/combu/refman.pdf" target="_blank">PDF</a>
 */

/*! \page page_server Web server setup
	\tableofcontents
	\brief In this section you will learn how to setup a web server on your local machine and install Combu.
	
	\section sec_server_production Live server
	<strong>Combu</strong> will work correctly in almost every hosting provider, since the production servers are usually configured correctly.
	So in production environment you'll only need to create the database on MySQL (and execute the file <em>combu_db.sql</em> into it)
	and edit the file <em>lib/config.php</em> in your <strong>Combu</strong> folder (that is the constants <em>GAME_DB_SERVER</em>,
	<em>GAME_DB_NAME</em>, <em>GAME_DB_USER</em> and <em>GAME_DB_PASS</em>), as explained in the <a href="http://youtu.be/PsZyYopzi40" target="_blank">tutorial</a> video.

	\section sec_server_local Local machine
	Before using a live server you may want to test your game earlier and work locally on your machine, in this case
	you have few alternatives (Microsoft IIS, Apache, XAMPP, etc) and what's to get only depends on your expertise and skills.
	<ol>
		<li>
			For the sake of this sample we will assume you have installed <a href="https://www.apachefriends.org/" target="_blank"><strong>XAMPP</strong></a>,
			it's a well-known free package that contains both the web server Apache and the MySQL server engine (it exists few similar packages, like LAMPP, they're almost the same for this example)
		</li>
		<li>
			Edit the file <strong>php.ini</strong> (if you're using XAMPP then it's usually located in the folder <em>xamppfiles/etc</em>) and assign the value <em>On</em> to the variable <em>short_open_tag</em>
			(the line should look like: <strong>short_open_tag=On</strong>)
		</li>
		<li>
			<strong>MySQL</strong> must be configured with case-sensitivity for table names, it's usually already configured on live servers by hosting providers;
			if you're running on your local <strong>Windows</strong> machine then edit the file <strong>my.ini</strong> (you'll find it in one of the folders inside XAMPP installation)
			and add a new line with <strong>lower_case_table_names=2</strong> just below the line that contains <strong>[mysqld]</strong>
			(if you haven't the section <em>[mysqld]</em> then add both lines at the end of the file)
		</li>
		<li>
			Start both HTTP and MySQL services (if you're using XAMPP, it can be done through the XAMPP Control Panel else from Windows Services)
		</li>
		<li>
			Create an empty database with name <strong>combu</strong>, select/open it and execute the file <strong>combu_db.sql</strong> in phpMyAdmin
			(in XAMPP it's usually installed at <em>http://localhost/phpmyadmin</em>) or <a href="http://www.mysql.com/products/workbench/" target="_blank">MySQL Workbench</a>
		</li>
		<li>
			Uncompress the file <strong>combu_web.zip</strong> into your local web server root (if you're using XAMPP then it's usually located in the folder <em>xamppfiles/htdocs</em>);
			if the database created in the previous step is not named <em>combu</em> then you'll need to edit the file <strong>lib/config.php</strong> and modify the value of the constant <em>GAME_DB_NAME</em>
		</li>
	</ol>

	\section sec_server_config Server and Client configuration
	Now that you learned how to setup your web server and have installed <strong>Combu</strong> server, you can configure the system at your needs. <a href="page_config.html">Click here</a> to get started.
*/

/*! \page page_config Server and Client configuration
	\tableofcontents
	\brief In this section you will learn how to setup your server and client.

	\section sec_config_server Server configuration
	The server setup is stored in the file <strong>lib/config.php</strong> as PHP defines (if you are new to PHP, "define" is the equivalent of "const" in C#):
	<ul>
		<li><strong>URL_ROOT</strong>: it is the absolute URL path to the root of Combu (e.g.: <em>/combu/</em>)</li>
		<li><strong style="text-decoration: line-through;">DEFAULT_TIMEZONE</strong>: this property has been <strong>deprecated</strong> in 2.1.10 (timestamps are saved in UTC)<!--it is used to force a default timezone in your webservices and webadmin (used for both PHP and database dates);
			if it is not defned or it is commented (as it is by default) then it will be used the default timezone confgured on your server system, else it must be a valid timezone supported by PHP
			(check the list of supported timezones: <a target="_blank" href="http://www.php.net/manual/en/timezones.php">http://www.php.net/manual/en/timezones.php</a>)</li>
		<li><strong>SECRET_KEY</strong>: it is used to secure your webservices from spoofing attacks by validating each request received, it's strongly suggested to set a secret key;
			you can generate a random string from <a target="_blank" href="http://www.random.org/passwords">http://www.random.org/passwords</a> (we suggest 12 or 24 length)--></li>
		<li><strong>GAME_DB_SERVER</strong>: it is the hostname or IP address of MySQL server</li>
		<li><strong>GAME_DB_NAME</strong>: it is name of MySQL database</li>
		<li><strong>GAME_DB_USER</strong>: it is the user name of MySQL connection</li>
		<li><strong>GAME_DB_PASS</strong>: it is the user password of MySQL connection</li>
		<li><strong>REGISTER_EMAIL_REQUIRED</strong>: it will require a valid email address upon new user creation</li>
		<li><strong>REGISTER_EMAIL_MULTIPLE</strong>: it allows to use the same email address for multiple accounts</li>
		<li><strong>REGISTER_EMAIL_ACTIVATION</strong>: it will create an activation code during user creation and sends it by email, then will require to activate the account before being able to login</li>
		<li><strong>REGISTER_EMAIL_SUBJECT</strong>: the subject text of the user registration email</li>
		<li><strong>REGISTER_EMAIL_MESSAGE</strong>: the full path to the html/text fle that contains the text of the user registration email; the message text can contain the following special words:
			<ul>
				<li><strong>{ACTIVATION_URL}</strong>: it will be replaced with the URL to activate the account (if <strong>REGISTER_EMAIL_ACTIVATION</strong> is <em>TRUE</em>)</li>
				<li><strong>{USERNAME}</strong>: it will be replaced with the chosen username</li>
			</ul></li>
		<li><strong>REGISTER_EMAIL_HTML</strong>: it establishes if the user registration email is in HTML or text</li>
		<li><strong>FRIENDS_REQUIRE_ACCEPT</strong>: if set to TRUE then the friend add action will require the destination user to accept or decline the request before it appears in the friends list</li>
		<li><strong>ONLINE_SECONDS</strong>: time interval in seconds to consider a user online from last action registered</li>
		<li><strong>CLEAR_PLAYER_SESSIONS</strong>: if set to TRUE then every time a player logs in, the system will delete older login sessions to maintain the sessions table cleaned and as smaller as possible</li>
		<li><strong>EMAIL_SENDER_ADDRESS</strong>: the sender address of outgoing mail</li>
		<li><strong>EMAIL_SENDER_NAME</strong>: the sender name of outgoing mail</li>
		<li><strong>NEWSLETTER_SENDER_ADDRESS</strong>: the sender address of outgoing newsletters</li>
		<li><strong>NEWSLETTER_SENDER_NAME</strong>: the sender name of outgoing newsletters</li>
		<li><strong>DEFAULT_LIST_LIMIT</strong>: it sets the default number of results fetched for pagination</li>
		<li><strong>LOG_FILEPATH</strong>: it is the physical path to the log fle used by the class AppLog</li>
		<li><strong>LOG_MAXFILESIZE</strong>: it is the maximum size in bytes of the log fle</li>
		<li><strong>URL_UPLOAD</strong>: root folder for uploads to be used as URL</li>
		<li><strong>UPLOAD</strong>: physical root folder for uploads</li>
		<li><strong>GUEST_PREFIX</strong>: the prefix string attached to the id for guest accounts (ex.: "Guest-")</li>
		<li><strong>EXPIRE_CLIENT_SESSION</strong>: expire account sessions (must be a valid parameter for DateInterval constructor: <a href="http://php.net/manual/en/class.dateinterval.php" target="_blank">http://php.net/manual/en/class.dateinterval.php</a>)</li>
	</ul>

	\section sec_config_client Client configuration
	In the inspector of <strong>CombuManager</strong> script (the prefab is just a GameObject with <em>CombuManager</em> script attached) you will find the following properties:
	<ul>
		<li><strong>Dont Destroy On Load</strong>: if checked, this GameObject will be alive for the whole lifetime of your game/app; if you want to login in one scene (for example main menu) and last until the game quits then you should enable this flag</li>
		<li><strong>Set As Default Social Platform</strong>: since Combu 2.x implements the Unity <strong>ISocialPlatform</strong> interface, if you enable this flag the Combu will be set as current platform on Unity and you will be able to use it also through <strong>Social.Active</strong></li>
		<li><strong>Secret Key</strong>: it is used to secure your webservices from spoofng attacks by signing each request; if you haven't a secure SSL connection on your server then we suggest to use a secret key,
			it must be the same as you defne on your webservice confg.php (please read the Server Documentation to defne one on your server)</li>
		<li><strong>Url Root Production</strong>: is the URL to the folder where you installed the web services on your production server (e.g.: <em>http://www.yourserver.com/combu/</em>)</li>
		<li><strong>Url Root Stage</strong>: is the URL to the folder where you installed the web services on your stage/development server, usually local machine (e.g.: <em>http://localhost/combu/</em>)</li>
		<li><strong>Use Stage</strong>: if checked, the web services calls will be directed to the stage server instead of production</li>
		<li><strong>Log Debug Info</strong>: if checked, all the calls to webservices will add the result text in the console window</li>
		<li><strong>Ping Interval Seconds</strong>: it's the interval in seconds to send auto-ping to server in order to maintain the online state of local user; to disable auto-ping set this to <em>0</em></li>
		<li><strong>Online Seconds</strong>: it's the time in seconds from last action registered to be considered as Online (last action and online state are cached in User class, you will need to reload over time if you need a precise info)</li>
		<li><strong>Playing Seconds</strong>: it's the time in seconds from last action to be considered as Playing</li>
		<li><strong style="text-decoration: line-through;">Timezone</strong>: this property has been <strong>deprecated</strong> in 2.1.10 (timestamps are saved in UTC)<!--it can be used to force the webservices to use a specifc timezone when dealing with the dates, for example to store
			a leaderboard score or achievement progress with the date referred to the timezone of the user; if it is empty then the webservices will use
			the default timezone confgured on the server, else it must be a valid timezone supported by PHP
			(checkout the list of supported timezone values: <a target="_blank" href="http://www.php.net/manual/en/timezones.php">http://www.php.net/manual/en/timezones.php</a>)--></li>
		<li><strong>Achievement UI Object</strong>: the GameObject that handles the user interface of Achievements</li>
		<li><strong>Achievement UI Function</strong>: the name of method to call on <strong>Achievement UI Object</strong> as result to <strong>Social.ShowAchievementsUI</strong></li>
		<li><strong>Leaderboard UI Object</strong>: the GameObject that handles the user interface of Leaderboard</li>
		<li><strong>Leaderboard UI Function</strong>: the name of method to call on <strong>Leaderboard UI Object</strong> as result to <strong>Social.ShowLeaderboardUI</strong></li>
	</ul>
*/

/*! \page page_users Authentication and Users
	\tableofcontents
	\brief In this section you will learn how to authenticate the local user, create a new user and load your users.

	\section sec_users_login Authentication
	To authenticate the local user you need to call <strong>CombuManager.instance.platform.Authenticate</strong>:
\code{.cs}
CombuManager.platform.Authenticate( "username", "password", (bool success, string error) => {
	if (success)
		Debug.Log("Login success: ID " + CombuManager.localUser.id);
	else
		Debug.Log("Login failed: " + error);
});
\endcode

	\subsection subsec_users_login_session Authenticate a saved session
	If you want to auto-login a previously saved session then you need to call <strong>AuthenticateSession</strong> on a <strong>User</strong> object:
\code{.cs}
string sessionToken = PlayerPrefs.GetString("SessionToken", "");
long userId = 0;
if (!long.TryParse (PlayerPrefs.GetString ("UserId", "0"), out userId) || userId < 0)
	userId = 0;
User.AuthenticateSession (userId, sessionToken, (bool success, string error) => {
	Debug.Log ("AuthenticateSession - Success=" + success + " > Error=" + error);
});
\endcode

	\section sec_users_register Registration
	To create a new user you need to create a new instance of <strong>User</strong> class, set at least <em>username</em> and <em>password</em> and then call <strong>Update</strong> on the instance:
\code{.cs}
User newUser = new User();
newUser.userName = "username";
newUser.password = "password";
newUser.Update( (bool success, string error) => {
	// NB: registration does not make the user logged
	if (success)
		Debug.Log("Save success: ID " + newUser.id);
	else
		Debug.Log("Save failed: " + error);
});
\endcode

	\subsection subsec_users_register_guest Create a guest account
	If you want to create a guest account then you need to call <strong>CreateGuest</strong> on a <strong>User</strong> object:
\code{.cs}
var user = new User ();
user.CreateGuest ((bool success, string error) => {
	Debug.Log ("CreateGuest - Success=" + success + " > Error=" + error);
	if (success) {
		// Store the id and sessionToken
		PlayerPrefs.SetString ("UserId", CombuManager.localUser.id);
		PlayerPrefs.SetString ("SessionToken", CombuManager.localUser.sessionToken);
		PlayerPrefs.Save ();
	}
});
\endcode

	\section sec_users_loadusers Load users
	To load the users data you can call <strong>CombuManager.instance.platform.LoadUsers()</strong>,
	or one of the <strong>User.Load()</strong> overloads
	(with <em>User.Load</em> form you will not need to cast back from <strong>IUserProfile</strong> to <strong>User</strong>):
\code{.cs}
// Load a user by Id
User.Load ( 123, ( User user ) => {
	Debug.Log("Success: " + (user == null ? "false" : "true"));
});
// Load a user by userName
User.Load ( "user1", ( User user ) => {
	Debug.Log("Success: " + (user == null ? "false" : "true"));
});
// Load users by Id
User.Load ( new long[] { 123, 456 }, ( User[] users ) => {
	Debug.Log("Loaded: " + users.Length);
});
// Search users by Username, Email, CustomData
// Filter players with custom data "Level" between 5 and 10
SearchCustomData[] searchData = new SearchCustomData[] {
	new SearchCustomData("Level", eSearchOperator.GreaterOrEquals, "5"),
	new SearchCustomData("Level", eSearchOperator.LowerOrEquals, "10")
};
User.Load("part-of-username", "email@server.com", searchData, 1, 1, (User[] users, int resultsCount, int pagesCount) => {
	Debug.Log("Loaded: " + users.Length);
});
\endcode
	You can also load a list of random users (excluding localUser):
\code{.cs}
// Filter players with custom data "Level" between 5 and 10
SearchCustomData[] searchData = new SearchCustomData[] {
	new SearchCustomData("Level", eSearchOperator.GreaterOrEquals, "5"),
	new SearchCustomData("Level", eSearchOperator.LowerOrEquals, "10")
};
User.Random(searchData, 3, (User[] users) => {
	foreach (User user in users)
	{
		if (user.lastSeen == null)
			Debug.Log(user.userName + " Never seen");
		else
		{
			System.DateTime seen = (System.DateTime)user.lastSeen;
			Debug.Log(user.userName + " Last seen: " + seen.ToLongDateString() + " at " + seen.ToLongTimeString() + " - Online state: " + user.state);
		}
	}
});
\endcode

	\section sec_users_customdata Custom Data
	You can store any type of data in the <strong>Hashtable</strong> <em>customData</em> of <strong>Profile</strong> class,
	for example you could store the user's virtual currency, level, experience in a RPG game:
\code{.cs}
CombuManager.localUser.customData["Coins"] = 100;
CombuManager.localUser.Update( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_users_onlinestate Online state
	The Profile class implements the <strong>IUserProfile</strong> interface, so it also provides the <em>state</em> property
	to get the online state of your users. Of course remember that we are in an asynchronous environment, so your players are not really connected in real-time
	and if you need to rely on the online state then you will need to implement your own polling system to refresh your lists from time to time.

	To mantain the online state CombuManager uses the settings <em>pingIntervalSeconds</em>, <em>onlineSeconds</em> and <em>playingSeconds</em>.
	Besides the ping function that is called every <em>pingIntervalSeconds</em> seconds (set 0 to disable, anyway we'd recommend to have the interval not too small,
	a value of 30 should be fine else you may suffer of high traffic), every action served by the webservices updates the "last action" date/time of a user.

	\section sec_users_class Create your User class
	Since you're able to extend the basic <strong>User</strong> class with the <em>customData</em> property,
	the best way to work with the system is to create your own class for users by inheriting from <strong>User</strong>.

	This way you can create your own account properties, that for sure are much more readable than <em>customData["myProperty"]</em>
	(especially if you need non-string values, it would be lot of explicit casts or Parse!).

	Remember to:
	<ul>
		<li>set their values in <em>customData</em>, so they will be passed to <strong>Update</strong> and saved to server</li>
		<li>override <strong>FromHashtable</strong> to fill the internal variables from the <em>customData</em> received from server</li>
	</ul>
\code{.cs}
public class CombuDemoUser : Combu.User
{
	string _myProperty1 = "";
	int _myProperty2 = 0;

	public string myProperty1
	{
		get { return _myProperty1; }
		set { _myProperty1 = value; customData["myProperty1"] = _myProperty1; }
	}
	
	public int myProperty2
	{
		get { return _myProperty2; }
		set { _myProperty2 = value; customData["myProperty2"] = _myProperty2; }
	}

	public CombuDemoUser()
	{
		myProperty1 = "";
		myProperty2 = 0;
	}

	public override void FromHashtable (Hashtable hash)
	{
		// Set User class properties
		base.FromHashtable (hash);

		// Set our own custom properties that we store in customData
		if (customData.ContainsKey("myProperty1"))
			_myProperty1 = customData["myProperty1"].ToString();
		if (customData.ContainsKey("myProperty2"))
			_myProperty2 = int.Parse(customData["myProperty2"].ToString());
	}
}
\endcode
	To use the new class in your code, you will need to pass the referenced type to the user-wise methods:
\code{.cs}
// Authenticate user
CombuManager.platform.Authenticate <CombuDemoUser> ( "username", "password", (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Create new user
CombuDemoUser newUser = new CombuDemoUser();
newUser.userName = "username";
newUser.password = "password";
newUser.myProperty1 = "Value";
newUser.myProperty2 = 100;
newUser.Update( (bool success, string error) => {
	// NB: registration does not make the user logged
	if (success)
		Debug.Log("Save success: ID " + newUser.id);
	else
		Debug.Log("Save failed: " + error);
});
\endcode
*/

/*! \page page_settings Server Settings
	\tableofcontents
	\brief In this section you will learn how to create server settings and access them from client.

	\section sec_settings_server Server
	You can create server settings in the administration console, in the section <strong>Settings</strong>.

	\section sec_settings_client Client
	The client automatically loads the settings in <strong>CombuManager.instance.serverInfo.settings</strong>:
\code{.cs}
while (!CombuManager.isInitialized)
	yield return null;
string mySetting = CombuManager.instance.serverInfo.settings["mySetting"].ToString();
\endcode
*/

/*! \page page_extplatforms Linking external platforms
	\tableofcontents
	\brief In this section you will learn how to authenticate and link the local user to an external platform like GameCenter, Facebook etc.

	\section sec_extplatforms_login Authentication
	To authenticate the local user with a platform Id you need to call <strong>CombuManager.localUser.AuthenticatePlatform</strong>.
	If the platform key+id exists then it will return the registered account, else it will create a new account with username <strong>PlatformName_PlatformId</strong> (including the underscore symbol).
\code{.cs}
// After you have logged in with Facebook SDK (http://u3d.as/5j1)
CombuManager.localUser.AuthenticatePlatform("Facebook", FB.UserId, (bool success, string error) => {
	if (success)
		Debug.Log("Login success: ID " + CombuManager.localUser.id);
	else
		Debug.Log("Login failed: " + error);
});
\endcode

	\section sec_extplatforms_linkplatform Link a platform to the local user
	If the local user is already logged, you can link a platform Id to the account with <strong>CombuManager.localUser.LinkPlatform</strong>:
\code{.cs}
CombuManager.localUser.LinkPlatform("YourPlatformName", "YourPlatformId", (bool success, string error) => {
	if (success)
		Debug.Log("Link success");
	else
		Debug.Log("Link failed: " + error);
});
\endcode

	\section sec_extplatforms_linkaccount Transfer the external platforms
	Sometimes may happen that you need to move the external platforms of the local user to another account,
	in this case you can use <strong>CombuManager.localUser.LinkAccount</strong> (the platforms key+id of the local user account will be transferred to the new account,
	the account and all its data/scores/etc deleted, and the new account will be assigned to the local user):
\code{.cs}
CombuManager.localUser.LinkAccount("other_username", "other_password", (bool success, string error) => {
	if (success)
		Debug.Log("Transfer success");
	else
		Debug.Log("Transfer failed: " + error);
});
\endcode
*/

/*! \page page_contacts Managing Contacts
	\tableofcontents
	\brief In this section you will learn how to retrieve the contacts lists of the local user and how to add/remove users to the friends/ignore lists.

	\section sec_contacts_list Loading Contacts
	To retrieve the list of contacts from the local user you need to call the <em>LoadFriends</em> method on <strong>CombuManager.localUser</strong>:
\code{.cs}
CombuManager.localUser.LoadFriends( eContactType.Friend, (bool success) => {
	if (success)
		Debug.Log("Success: " + CombuManager.localUser.friends.Length);
	else
		Debug.Log("Failed");
});
\endcode

	\section sec_contacts_add Adding Contacts
	To add another user to a contact list of the local user you need to call <em>AddContact</em>:
\code{.cs}
// Add by User/Profile object
CombuManager.localUser.AddContact(otherUser, eContactType.Friend, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Add by username
CombuManager.localUser.AddContact("username", eContactType.Friend, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_contacts_remove Removing Contacts
	To remove a user from the contact lists of the local user you need to call <em>RemoveContact</em>:
\code{.cs}
// Remove by User/Profile object
CombuManager.localUser.RemoveContact(otherUser, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Remove by username
CombuManager.localUser.RemoveContact("username", (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode
*/

/*! \page page_files Managing Files
	\tableofcontents
	\brief In this section you will learn how to retrieve the files lists of the local user or anyway accessible by him and how to add/remove files.

	\section sec_files_list Loading Files
	To retrieve a list of files you need to call <strong>UserFile.Load</strong>:
\code{.cs}
bool includeShared = true;
int pageNumber = 1;
int countPerPage = 10;
UserFile.Load(CombuManager.localUser.id, includeShared, pageNumber, countPerPage, (UserFile[] files, int resultsCount, int pagesCount, string error) => {
	if (string.IsNullOrEmpty(error))
		Debug.Log("Files loaded: " + files.Length);
	else
		Debug.Log(error);
});
\endcode

	\section sec_files_add Adding Files
	To add a file associated to the local user you need to create a new instance of <strong>UserFile</strong>, set <em>sharing</em> property and call <strong>Update</strong>:
\code{.cs}
byte[] screenshot = CombuManager.instance.CaptureScreenShot();
UserFile newFile = new UserFile();
newFile.sharing = UserFile.eShareType.Everybody;
newFile.customData ["Prop"] = "Value";
newFile.Update(screenshot, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_files_view Viewing Files
	To increase the <strong>View</strong> count of a file you need to call <strong>UserFile.View</strong>, or call the method <strong>View</strong> on a <strong>UserFile</strong> instance:
\code{.cs}
// View by File ID
UserFile.View(123, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// View by UserFile object
myFile.View( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_files_like Liking Files
	To increase the <strong>Like</strong> count of a file you need to call <strong>UserFile.Like</strong>, or call the method <strong>Like</strong> on a <strong>UserFile</strong> instance:
\code{.cs}
// Like by File ID
UserFile.Like(123, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Like by UserFile object
myFile.Like( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_files_remove Removing Files
	To remove a file owned by the local user you need to call <strong>UserFile.Delete</strong>, or call the method <strong>Delete</strong> on a <strong>UserFile</strong> instance:
\code{.cs}
// Remove by File ID
UserFile.Delete(123, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Remove from a UserFile object
myFile.Delete( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode
*/

/*! \page page_inventory Managing Inventory
	\tableofcontents
	\brief In this section you will learn how to manage the inventory of the local user.

	\section sec_inventory_list Loading Inventory
	To retrieve the list of items in the inventory of a user you need to call the <strong>Inventory.Load</strong>:
\code{.cs}
// Load the inventory of user ID '123'
Inventory.Load( "123", (Inventory[] items, string error) => {
	if (success)
		Debug.Log("Success: " + items.Length);
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_inventory_add Adding and Editing Items
	To add a new item in the inventory of the local user you need to create a new <strong>Inventory</strong> instance, set <em>name</em>, <em>quantity</em> and <em>customData</em> and then call <strong>Update</strong>:
\code{.cs}
// Add a new item
Inventory newItem = new Inventory();
newItem.name = "My item";
newItem.quantity = 1;
newItem.customData["Durability"] = 100;
newItem.Update( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Edit an item previously loaded
myItem.quantity--;
myItem.Update( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_inventory_remove Removing Items
	To remove an item from the inventory of the local user you need to call <strong>Inventory.Delete</strong>:
\code{.cs}
// Remove by inventory ID '123'
Inventory.Delete(123, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Remove by Inventory object
myItem.Delete( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode
*/

/*! \page page_mail Managing Messages
	\tableofcontents
	\brief In this section you will learn how to manage the in-game inbox of the local user.

	\section sec_mail_list Loading Messages
	To retrieve the list of messages of a user you need to call the <strong>Message.Load</strong>:
\code{.cs}
// Load the received messages from the page 1 with 3 results per page
Mail.Load(eMailList.Received, 1, 3, (Mail[] messages, int count, int pagesCount, string error) => {
	if (string.IsNullOrEmpty(error))
		Debug.Log("Success: " + messages.Length);
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_mail_add Sending Messages
	To send a new message to a user you need to call one of the overloads of <strong>Mail.Send</strong>:
\code{.cs}
// Send a private message to a user by Id
Mail.Send(123, "Subject", "Message body", false, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Send a private message to a user by Username
Mail.Send("user1", "Subject", "Message body", false, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Send a private message to multiple users by Id
Mail.Send( new long[] { 123, 456 }, "Subject", "Message body", false, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Send a private message to multiple users by Username
Mail.Send( new string[] { "user1", "user2" }, "Subject", "Message body", false, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\subsection subsec_mail_sendgroup Sending to UserGroup
	If you want to send a message to a UserGroup then you need to call <strong>Mail.SendMailToGroup</strong>:
\code{.cs}
// Send a message to the UserGroup ID '123'
Mail.SendMailToGroup(123, "Subject", "Message body", false, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_mail_read Marking Messages as Read
	To mark a message as <strong>Read</strong> you need to call <strong>Mail.Read</strong>, or call the method <strong>Read</strong> on a <strong>Mail</strong> instance:
\code{.cs}
// Mark as Read by Mail ID
Mail.Read( new long[] { 123, 456 }, new long[0], (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Mark as Read by Group ID
Mail.Read( new long[0], new long[] { 123, 456 }, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Mark as Read by Mail object
myMail.Read( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

\section sec_mail_unread Marking Messages as Unread
To mark a message as <strong>Unread</strong> you need to call <strong>Mail.Unread</strong>, or call the method <strong>Unread</strong> on a <strong>Mail</strong> instance:
\code{.cs}
// Mark as Unread by Mail ID
Mail.Unread( 123, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Mark as Unread by Mail object
myMail.Unread( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_mail_remove Removing Items
	To delete a message you need to call <strong>Message.Delete</strong>, or call the method <strong>Delete</strong> on a <strong>Mail</strong> instance:
\code{.cs}
// Remove by Message ID '123'
Mail.Delete(123, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Remove by Mail object
myMail.Delete( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_mail_conv Loading Conversations
	To load the list of discussions of the local user with others you can call <strong>Mail.LoadConversations</strong>,
	it will fill an <strong>ArrayList</strong> with objects of type <strong>User</strong> or <strong>UserGroup</strong> (based on the <em>idGroup</em> associated to the <strong>Mail</strong> object):
\code{.cs}
Mail.LoadConversations( (ArrayList conversations, int count, string error) => {
	if (string.IsNullOrEmpty(error))
		Debug.Log("Success: " + conversations.Count);
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_mail_count Counting Messages
	To count the messages in the inbox of the local user you can call <strong>Mail.Count</strong>:
\code{.cs}
// Count the messages sent from users ID
Mail.Count( new long[] { 123, 456 }, new long[0], (MailCount[] counts, string error) => {
	if (string.IsNullOrEmpty(error))
		Debug.Log("Success: " + counts.Length);
	else
		Debug.Log("Failed: " + error);
});
// Count the messages sent from groups ID
Mail.Count( new long[0], new long[] { 123, 456 }, (MailCount[] counts, string error) => {
	if (string.IsNullOrEmpty(error))
		Debug.Log("Success: " + counts.Length);
	else
		Debug.Log("Failed: " + error);
});
\endcode
*/

/*! \page page_groups Managing User Groups
	\tableofcontents
	\brief In this section you will learn how to manage the user groups.

	\section sec_groups_create Create a new group
	You can create a new group with <strong>UserGroup.Save</strong> (call this also to edit a group that was loaded):
\code{.cs}
UserGroup group = new UserGroup();
group.name = "My Group";
// Add some users
group.users = new User[] { {userName="OtherUser1"}, {userName="OtherUser2"} };
group.Save((bool success, string error) => {
	Debug.Log("Group saved: " + success);
});
\endcode

	\section sec_groups_load Load groups
	To load groups you have 3 choices:
	<ul>
		<li>load groups owned by local user
\code{.cs}
UserGroup.Load(CombuManager.localUser.idLong, (UserGroup[] groups, string error) => {
	Debug.Log(groups.Length);
});
\endcode
</li>
		<li>load groups in which local user is a member (owners are also members)
\code{.cs}
UserGroup.LoadMembership(CombuManager.localUser.idLong, (UserGroup[] groups, string error) => {
	Debug.Log(groups.Length);
});
\endcode
</li>

	\section sec_groups_join Join a group
	You can join a group with <strong>UserGroup.Join</strong>:
\code{.cs}
// Join local user
group.Join((bool success, string error) => {
	Debug.Log("Group joined: " + success);
});
// Join a list of users
group.Join(new string[] {"user1"}, (bool success, string error) => {
	Debug.Log("Group joined: " + success);
});
\endcode

	\section sec_groups_leave Leave a group
	You can leave a group with <strong>UserGroup.Leave</strong>:
\code{.cs}
// Leave local user
group.Leave((bool success, string error) => {
	Debug.Log("Group joined: " + success);
});
// Leave a list of users
group.Leave(new string[] {"user1"}, (bool success, string error) => {
	Debug.Log("Group joined: " + success);
});
\endcode
*/

/*! \page page_news Managing News
	\tableofcontents
	\brief In this section you will learn how to retrieve the in-game news.

	\section sec_news_list Loading News
	To retrieve the list of latest news you need to call <strong>News.Load</strong>:
\code{.cs}
// Load the page 1 with 10 results per page
News.Load(1, 10, (News[] news, int resultsCount, int pagesCount, string error) => {
	if (string.IsNullOrEmpty(error))
		Debug.Log("Success: " + news.Length);
	else
		Debug.Log("Failed: " + error);
});
\endcode
*/

/*! \page page_leaderboard Managing Leaderboards
	\tableofcontents
	\brief In this section you will learn how to access the leaderboards data and report a score.

	\section sec_leaderboard_list Loading Scores
	To retrieve the scores list of a leaderboard you can call <strong>CombuManager.platform.LoadScores</strong> (by default loads the first page with 10 results), or you can instantiate
	a new <strong>Leaderboard</strong> object (created with <strong>CombuManager.platform.CreateLeaderboard</strong>), set <em>timeScope</em>, <em>customTimescope</em> and <em>range</em> and then call <strong>LoadScores</strong>:
\code{.cs}
// Load the scores of the leaderboard ID '123' from the page 2 with 10 results per page
CombuManager.platform.LoadScores(123, 2, 10, (IScore[] scores) => {
	Debug.Log("Scores loaded: " + scores.Length);
});
// Load the scores of the leaderboard ID '123' from the page 1 with 10 results per page
Leaderboard board = CombuManager.platform.CreateLeaderboard();
board.id = "123";
board.timeScope = UnityEngine.SocialPlatforms.TimeScope.AllTime;
board.range = new UnityEngine.SocialPlatforms.Range(1, 10);
board.LoadScores((bool loaded) => {
	// Now you can access board.scores and board.localUserScore
});
\endcode

	\section sec_leaderboard_add Reporting Scores
	To report a new score of the local user you need to call <strong>CombuManager.platform.ReportScore</strong>, or you can call the method <strong>ReportScore</strong> on a <strong>Score</strong> instance:
\code{.cs}
// Report a score of 1000 to the leaderboard ID '123'
CombuManager.platform.ReportScore(1000, "123", (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Report a score of 1000 on a Score object,
// (previously loaded with LoadScores or a new with leaderboardID set)
myScore.value = 1000;
myScore.ReportScore( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_leaderboard_remove Loading Leaderboards data
	To load the leaderboards data like the title, description etc, you need to call <strong>Leaderboard.Load</strong>:
\code{.cs}
// Load the leaderboard ID '123'
Leaderboard.Load("123", (Leaderboard leaderboard, string error) => {
	if (success)
		Debug.Log("Success: " + leaderboard.title);
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_leaderboard_userscore Loading the score of a user
	To retrieve the best score of a user you need to call <strong>Leaderboard.LoadScoresByUser</strong>:
\code{.cs}
// Load the best score of leaderboard ID '123' for the User object with 10 results per page (to be attendible 'page', the limit must be the same as used with LoadScores)
Leaderboard.LoadScoresByUser("123", user, eLeaderboardInterval.Week, 10, (Score score, int page, string error) => {
	if (success)
		Debug.Log("Success: " + score.value + " at page " + page);
	else
		Debug.Log("Failed: " + error);
});
\endcode
*/

/*! \page page_achievement Managing Achievements
	\tableofcontents
	\brief In this section you will learn how to load the achievements and report a progress.

	\section sec_achievement_list Loading Achievements descriptions
	To retrieve the list of achievements you can call <strong>CombuManager.platform.LoadAchievementDescriptions</strong>
	or <strong>CombuManager.platform.LoadAchievements</strong> (the latter form is preferred because you will not need to cast back from <em>IAchievement</em> to <em>Achievement</em>):
\code{.cs}
// Load the achievement descriptions
CombuManager.platform.LoadAchievements( (Achievement[] achievements) => {
	Debug.Log("Achievements loaded: " + achievements.Length);
});
\endcode

	\section sec_achievement_add Reporting Progress
	To report a new progress of the local user you need to call <strong>CombuManager.platform.ReportProgress</strong>, or you can call the method <strong>ReportProgress</strong> on a <strong>Score</strong> instance:
\code{.cs}
// Report a score of 1000 to the achievement ID '123'
CombuManager.platform.ReportProgress(1000, "123", (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Report a progress of 30% on an Achievement object,
// (previously loaded with LoadAchievements or created with <strong>CombuManager.platform.CreateAchievement</strong> and with <em>id</em> set)
myAchievement.percentCompleted = 0.3;
myAchievement.ReportProgress( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode
*/

/*! \page page_tournaments Managing Tournaments
	\tableofcontents
	\brief In this section you will learn how to retrieve the tournaments list of the local user and how to add/remove tournaments.

	\section sec_tournaments_list Loading Tournaments
	To retrieve the list of tournaments accessible by the local user you need to call <strong>Tournament.Load</strong>:
\code{.cs}
// Load the active tournaments, no filter by customData
Tournament.Load(false, null, (Tournament[] tournaments) => {
	Debug.Log("Tournaments loaded: " + tournaments.Length);
});
\endcode

	\subsection subsec_tournaments_load Loading by Tournament ID
	You can also load a single Tournament by its ID:
\code{.cs}
// Load a Tournament by ID
Tournament.Load(123, (Tournament tournament) => {
	if (tournament != null)
		Debug.Log("Success: " + tournament.title);
	else
		Debug.Log("Failed");
});
\endcode

	\subsection subsec_tournaments_matches Matches of the Tournament
	Once that the Tournament has been loaded, the property <em>matches</em> gives you access to its matches and their extra data.

	\section sec_tournaments_add Quick Tournament
	To create a quick tournament with other users user you need to call <strong>Tournament.QuickTournament</strong>:
\code{.cs}
// Load 2 random users
User.Random( null, 2, (User[] users) => {
	Tournament t = Tournament.QuickTournament(users);
	if (t.matches.Count == 0)
	{
		Debug.Log("Something going wrong, no matches created");
	}
	else
	{
		t.title = "My Tournament 1";
		t.customData["Key1"] = "Value";
		
		t.Save((bool success, string error) => {
			
			Debug.Log("Success: " + success + " - Matches: " + t.matches.Count);
			
		});
	}
});
\endcode

	\subsection subsec_tournaments_customize Create your own Tournament
	You can take a look at the code in <strong>Tournament.QuickTournament</strong> to see how to create a <strong>Tournament</strong>
	and use the code as base to create your own type of tournaments.

	\section sec_tournaments_remove Removing Tournaments
	To delete a tournament you need to call <strong>Tournament.Delete</strong>, or call the method <strong>Delete</strong> on a <strong>Tournament</strong> instance:
\code{.cs}
// Remove by Tournament ID
Tournament.Delete(123, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Remove by Tournament object
myTournament.Delete( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode
*/

/*! \page page_matches Managing Matches
	\tableofcontents
	\brief In this section you will learn how to retrieve the matches list of the local user and how to add/remove matches.

	\section sec_matches_list Loading Matches
	To retrieve the list of matches of the local user you need to call <strong>Match.Load</strong>:
\code{.cs}
// Load the active matches, no Tournament ID, no filter by title
Match.Load(0, true, string.Empty, (Matches[] matches) => {
	Debug.Log("Matches loaded: " + matches.Length);
});
\endcode

	\subsection subsec_matches_load Loading by Match ID
	You can also load a single Matches by its ID:
\code{.cs}
// Load a Matches by ID
Match.Load(123, (Match match) => {
	if (match != null)
		Debug.Log("Success: " + match.title);
	else
		Debug.Log("Failed");
});
\endcode

	\subsection subsec_matches_rounds Rounds of the Match
	Once that the Match has been loaded, the property <em>rounds</em> gives you access to its rounds and scores:
	the collection <em>users</em> contains the <strong>MatchAccount</strong> objects (relationship between Match and Account),
	the collection <em>scores</em> contains the <strong>MatchRound</strong> objects (relationship between the Match-Account association and a round).

	\section sec_matches_add Quick Match
	To create a quick match with another user you need to call <strong>Match.QuickMatch</strong>:
\code{.cs}
// Load 2 random users (not only friends), no filter by customData, 1 round
Match.QuickMatch(false, null, 1, (Match match) => {
	if (match != null)
		Debug.Log("Success: " + match.title);
	else
		Debug.Log("Failed");
});
\endcode

	\subsection subsec_matches_customize Create your own Match
	You can take a look at the code in <strong>Match.QuickMatch</strong> to see how to create a <strong>Match</strong>
	and use the code as base to create your own matches.

	\section sec_matches_score Sending Score
	To send a score for the next round you need to call the method <strong>Score</strong> on a <strong>Match</strong> instance:
\code{.cs}
// Send a score of 1000
myMatch.Score(1000, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode

	\section sec_matches_remove Removing Matches
	To delete a match you need to call <strong>Match.Delete</strong>, or call the method <strong>Delete</strong> on a <strong>Match</strong> instance:
\code{.cs}
// Remove by Match ID
Match.Delete(123, (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
// Remove by Match object
myMatch.Delete( (bool success, string error) => {
	if (success)
		Debug.Log("Success");
	else
		Debug.Log("Failed: " + error);
});
\endcode
*/
