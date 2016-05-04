﻿using System;
using System.Collections.Generic;
using TrueRED.Display;
using TrueRED.Framework;
using Tweetinvi;
using Tweetinvi.Core.Events.EventArguments;

namespace TrueRED.Modules
{
	public class WeatherModule : Module, IStreamListener
	{
		public override string ModuleName
		{
			get
			{
				return "Weather";
			}
		}

		public override string ModuleDescription
		{
			get
			{
				return "Tweet current weather";
			}
		}

		public WeatherModule( ) : base( string.Empty )
		{

		}
		public WeatherModule( string name ) : base( name )
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
			var tweet = args.Tweet;

			if ( !IsRunning ) return;
			if ( tweet.CreatedBy.Id == User.Id ) return;
			if ( tweet.IsRetweet == true ) return;
			if ( tweet.InReplyToUserId != User.Id ) return;

			if ( tweet.Text.Contains( "날씨" ) )
			{
				Log.Print( this.Name, string.Format( "Catch tweet [{0}({1}) : {2}]", tweet.CreatedBy.Name, tweet.CreatedBy.ScreenName, tweet.Text ) );
				// GPS로 위치 찾기
				if ( tweet.Place != null )
				{
					var place = tweet.Place.BoundingBox.Coordinates[0];
					Framework.HttpRepeaters.Weather.getWeather( place.Latitude, place.Longitude,
						delegate ( Framework.HttpRepeaters.Weather.WeatherResult response )
						{
							var result = Tweet.PublishTweetInReplyTo( string.Format( "@{0} 바깥 날씨는 {1}이고, 바깥 온도는 {2}℃야", tweet.CreatedBy.ScreenName, response.weatherKr, response.tempreture ), tweet.Id );
							Log.Print( this.Name, string.Format( "Send tweet [{0}]", result.Text ) );
						} );
				}
				// 문자열로 위치 찾기
				else
				{
					var result = Tweet.PublishTweetInReplyTo( string.Format( "@{0} 미안해. 현재 GPS정보 없이는 날씨를 알려줄 수 없어", tweet.CreatedBy.ScreenName), tweet.Id );
					Log.Print( this.Name, string.Format( "Send tweet [{0}]", result.Text ) );
				}
			}
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

		public override void OpenSettings( INIParser parser )
		{

		}

		public override void SaveSettings( INIParser parser )
		{
			parser.SetValue( "Module", "IsRunning", IsRunning );
			parser.SetValue( "Module", "Type", this.GetType( ).FullName );
			parser.SetValue( "Module", "Name", Name );
		}

		public override void Release( )
		{
			throw new NotImplementedException( );
		}

		public override Module CreateModule( object[] @params )
		{
			var module = new WeatherModule((string)@params[0]);
			return module;
		}

		public override List<ModuleFaceCategory> GetModuleFace( )
		{
			List<ModuleFaceCategory> face = new List<Display.ModuleFaceCategory>();

			var category1 = new ModuleFaceCategory("Module" );
			category1.Add( ModuleFaceCategory.ModuleFaceTypes.String, "모듈 이름" );
			face.Add( category1 );

			return face;
		}
	}
}
