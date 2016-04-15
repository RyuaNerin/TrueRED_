﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hammock.Streaming;
using TrueRED.Framework;
using TrueRED.Modules;
using Tweetinvi;

namespace TrueRED
{
	class Program
	{
		static void Main( string[] args )
		{
			#region Initialize Program

			Log.Init( );
			StringSetsManager.LoadStringSets( "Stringsets" );

			var setting = new INIParser( Path.Combine( Directory.GetCurrentDirectory( ), "setting.ini" ) );
			string consumerKey = setting.GetValue( "Authenticate", "ConsumerKey" );
			string consumerSecret = setting.GetValue( "Authenticate", "CconsumerSecret" );
			string accessToken = setting.GetValue( "Authenticate", "AccessToken" );
			string accessSecret = setting.GetValue( "Authenticate", "AccessSecret" );
			Auth.SetUserCredentials( consumerKey, consumerSecret, accessToken, accessSecret );
			var user = User.GetAuthenticatedUser( );
			Log.Debug( "UserCredentials :", string.Format( "{0}({1}) [{2}]", user.Name, user.ScreenName, user.Id ) );

			long ownerID = 0;
			try
			{
				ownerID = long.Parse(setting.GetValue("AppInfo", "OwnerID"));
			}
			catch ( FormatException e )
			{
				ownerID = 0;
            }
			var owner = User.GetUserFromId( ownerID );
			#endregion

			#region Initialize Modules

			var modules = new Dictionary<string, Modules.Module>();
			var iniModules = new List<IUseSetting>();
			var streamModules = new List<IStreamListener>();
			var timetasks = new List<ITimeTask>();

			var YoruHello = new Modules.ReactorModule(user,  "YoruHelloReactor", new TimeSet(20), new TimeSet(29));
			modules.Add( "YoruHello", YoruHello );
			streamModules.Add( YoruHello );

			var AsaHello = new Modules.ReactorModule(user,  "AsaHelloReactor", new TimeSet(5), new TimeSet(12));
			modules.Add( "AsaHello", AsaHello );
			streamModules.Add( AsaHello );

			var TimeTweet = new Modules.SchedulerModule(user, "TimeTweet" );
			modules.Add( "TimeTweet", TimeTweet );
			timetasks.Add( TimeTweet );

			var AutoFollow = new Modules.ReflectorModule(user);
			modules.Add( "AutoFollow", AutoFollow );
			streamModules.Add( AutoFollow );

			var Switcher = new Modules.ControllerModule(user, owner, modules);
			modules.Add( "Switcher", Switcher );
			streamModules.Add( Switcher );
			
			#endregion

			foreach ( var item in timetasks )
			{
				Task.Factory.StartNew( ( ) => item.Run( ) );
			}

			foreach ( var item in iniModules )
			{
				item.OpenSettings( );
			}
			
			CreateStream( streamModules );

			new Display.AppConsole( ).ShowDialog( );

			foreach ( var item in iniModules )
			{
				item.SaveSettings( );
			}
		}

		static void CreateStream( List<IStreamListener> modules )
		{
			if ( modules.Count == 0 ) return;

			var userStream = Tweetinvi.Stream.CreateUserStream();
			foreach ( var module in modules )
			{
				userStream.TweetCreatedByAnyone += module.TweetCreateByAnyone;
				userStream.MessageSent += module.MessageSent;
				userStream.MessageReceived += module.MessageReceived;
				userStream.TweetFavouritedByAnyone += module.TweetFavouritedByAnyone;
				userStream.TweetUnFavouritedByAnyone += module.TweetUnFavouritedByAnyone;
				userStream.ListCreated += module.ListCreated;
				userStream.ListUpdated += module.ListUpdated;
				userStream.ListDestroyed += module.ListDestroyed;
				userStream.BlockedUser += module.BlockedUser;
				userStream.UnBlockedUser += module.UnBlockedUser;
				userStream.FollowedUser += module.FollowedUser;
				userStream.FollowedByUser += module.FollowedByUser;
				userStream.UnFollowedUser += module.UnFollowedUser;
				userStream.AuthenticatedUserProfileUpdated += module.AuthenticatedUserProfileUpdated;
				userStream.FriendIdsReceived += module.FriendIdsReceived;
				userStream.AccessRevoked += module.AccessRevoked;
			}
			userStream.StartStream( );

		}
	}
}
