﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueRED.Framework;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces;

namespace TrueRED.Modules
{
	public class ReflectorModule : Module, IStreamListener, IUseSetting
	{
		public static string ModuleName { get; protected set; } = "Reflector";
		public static string ModuleDescription { get; protected set; } = "Auto mutal-follow";
		public static List<Display.ModuleFaceCategory> GetModuleFace()
		{
			List<Display.ModuleFaceCategory> face = new List<Display.ModuleFaceCategory>();
			
			return face;
		}
		public static ReflectorModule CreateModule( List<System.Windows.Forms.Control> InputForms )
		{
			return null;
		}

		public ReflectorModule( string name ) : base( name )
		{

		}

		void IStreamListener.AccessRevoked( object sender, AccessRevokedEventArgs args )
		{

		}

		void IStreamListener.AuthenticatedUserProfileUpdated( object sender, AuthenticatedUserUpdatedEventArgs args )
		{

		}

		void IStreamListener.BlockedUser( object sender, UserBlockedEventArgs args )
		{

		}

		void IStreamListener.FollowedByUser( object sender, UserFollowedEventArgs args )
		{
			if ( !IsRunning ) return;
			User.FollowUser( args.User );
			Log.Http( "Reflector worked", string.Format( "AutoFollowed {0}({1})", args.User.Name, args.User.ScreenName ) );
		}

		void IStreamListener.FollowedUser( object sender, UserFollowedEventArgs args )
		{
		}

		void IStreamListener.FriendIdsReceived( object sender, GenericEventArgs<IEnumerable<long>> args )
		{

		}

		void IStreamListener.ListCreated( object sender, ListEventArgs args )
		{

		}

		void IStreamListener.ListDestroyed( object sender, ListEventArgs args )
		{

		}

		void IStreamListener.ListUpdated( object sender, ListEventArgs args )
		{

		}

		void IStreamListener.MessageReceived( object sender, MessageEventArgs args )
		{

		}

		void IStreamListener.MessageSent( object sender, MessageEventArgs args )
		{

		}

		void IStreamListener.TweetCreateByAnyone( object sender, TweetReceivedEventArgs args )
		{
		}

		void IStreamListener.TweetFavouritedByAnyone( object sender, TweetFavouritedEventArgs args )
		{

		}

		void IStreamListener.TweetUnFavouritedByAnyone( object sender, TweetFavouritedEventArgs args )
		{

		}

		void IStreamListener.UnBlockedUser( object sender, UserBlockedEventArgs args )
		{

		}

		void IStreamListener.UnFollowedUser( object sender, UserFollowedEventArgs args )
		{

		}

		void IUseSetting.OpenSettings( INIParser parser )
		{
		}

		void IUseSetting.SaveSettings( INIParser parser )
		{
			parser.SetValue( "Module", "IsRunning", IsRunning );
			parser.SetValue( "Module", "Type", this.GetType( ).FullName );
			parser.SetValue( "Module", "Name", Name );
		}
	}
}
