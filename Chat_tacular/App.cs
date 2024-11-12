using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Threading;

namespace Chat_tacular;

public class App : Application
{
	//internal static Settings Settings;

	//internal static ImageBase Chat;

	internal static string auth;

	internal static string username;

	internal static string channel;

	internal const string ClientID = "l2hfsdik4txfm297h4yguxqhad9x21";

	internal const string Twitch0 = "https://api.twitch.tv/helix/";

	internal static string ProgramOAuth => Login.ProgramOAuth;

	internal static string RefAPI(IDictionary<string, string> urlVars, IDictionary<string, string> header, string URL = "https://api.twitch.tv/helix/", string suffix = "users")
	{
		string empty = string.Empty;
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(EncodeURL(urlVars, URL, suffix));
		for (int i = 0; i < header.Count; i++)
		{
			string text = header.Keys.ToArray()[i];
			httpWebRequest.Headers[text] = header[text];
		}
		try
		{
			using StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream());
			return streamReader.ReadToEnd();
		}
		catch
		{
			return string.Empty;
		}
	}

	internal static string GetUserData(string username, string programOAuth, string clientId = "l2hfsdik4txfm297h4yguxqhad9x21")
	{
		return RefAPI(new Dictionary<string, string> { { "login", username } }, new Dictionary<string, string>
		{
			{ "Client-Id", clientId },
			{
				"Authorization",
				"Bearer " + programOAuth.Replace("oauth:", "")
			}
		});
	}

	internal static string GetProgramOAuth(string username, string programOAuth, string clientId = "l2hfsdik4txfm297h4yguxqhad9x21")
	{
		return RefAPI(new Dictionary<string, string> { { "login", username } }, new Dictionary<string, string>
		{
			{ "Client-Id", clientId },
			{
				"Authorization",
				"Bearer " + programOAuth.Replace("oauth:", "")
			}
		});
	}

	internal static string GetBroadcasterEmotes(string userId, string userOAuth, string clientId = "l2hfsdik4txfm297h4yguxqhad9x21")
	{
		return RefAPI(new Dictionary<string, string> { { "broadcaster_id", userId } }, new Dictionary<string, string>
		{
			{ "Client-Id", clientId },
			{
				"Authorization",
				"Bearer " + userOAuth.Replace("oauth:", "")
			}
		}, "https://api.twitch.tv/helix/", "chat/emotes");
	}

	internal static string GetGlobalEmotes(string userId, string userOAuth, string clientId = "l2hfsdik4txfm297h4yguxqhad9x21")
	{
		return RefAPI(null, new Dictionary<string, string>
		{
			{ "Client-Id", clientId },
			{
				"Authorization",
				"Bearer " + userOAuth.Replace("oauth:", "")
			}
		}, "https://api.twitch.tv/helix/", "chat/emotes/global");
	}

	internal static string GetChannelEmotes(string userId, string userOAuth, string clientId = "l2hfsdik4txfm297h4yguxqhad9x21")
	{
		return RefAPI(new Dictionary<string, string> { { "broadcaster_id", userId } }, new Dictionary<string, string>
		{
			{ "Client-Id", clientId },
			{
				"Authorization",
				"Bearer " + userOAuth.Replace("oauth:", "")
			}
		}, "https://api.twitch.tv/helix/", "emotes");
	}

	internal static string EncodeURL(IDictionary<string, string> values, string URL, string suffix)
	{
		string text = URL + suffix;
		if (values != null)
		{
			for (int i = 0; i < values.Count; i++)
			{
				string text2 = values.Keys.ToArray()[i];
				text += string.Format("{0}{1}={2}", (i > 0) ? "&" : "?", text2, values[text2]);
			}
		}
		return text;
	}

	public App()
	{
		base.DispatcherUnhandledException += App_DispatcherUnhandledException;
	}

	private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
		//Logger.LogErrors(e?.Exception?.ToString(), MessageType.Error);
		if (MessageBox.Show($"Internal error. Close program? y/n\n\n{e?.Exception?.Message}", "Error", MessageBoxButton.YesNo, MessageBoxImage.Hand) == MessageBoxResult.Yes)
		{
			Application.Current?.Shutdown();
		}
		e.Handled = true;
	}
}
