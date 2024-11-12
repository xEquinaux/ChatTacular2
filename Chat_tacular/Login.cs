using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Chat_tacular.Properties;

namespace Chat_tacular;

public partial class Login : Window, IComponentConnector
{
	private bool closing = true;

	private bool AuthFlag
	{
		get
		{
			return Chat_tacular.Properties.Settings.Default.authFlag;
		}
		set
		{
			Chat_tacular.Properties.Settings.Default.authFlag = value;
		}
	}

	private string AuthToken
	{
		get
		{
			return Chat_tacular.Properties.Settings.Default.authCode;
		}
		set
		{
			Chat_tacular.Properties.Settings.Default.authCode = value;
		}
	}

	internal static string ProgramOAuth
	{
		get
		{
			return Chat_tacular.Properties.Settings.Default.programAuth;
		}
		set
		{
			Chat_tacular.Properties.Settings.Default.programAuth = value;
		}
	}

	internal static System.Windows.Media.Color ChatBGColor
	{
		get
		{
			System.Drawing.Color chatBgColor = Chat_tacular.Properties.Settings.Default.chatBgColor;
			return System.Windows.Media.Color.FromRgb(chatBgColor.R, chatBgColor.G, chatBgColor.B);
		}
		set
		{
			Chat_tacular.Properties.Settings.Default.chatBgColor = System.Drawing.Color.FromArgb(255, value.R, value.G, value.B);
		}
	}

	internal static System.Windows.Point ChatDimensions
	{
		get
		{
			System.Drawing.Point chatSize = Chat_tacular.Properties.Settings.Default.chatSize;
			return new System.Windows.Point(chatSize.X, chatSize.Y);
		}
		set
		{
			Chat_tacular.Properties.Settings.Default.chatSize = new System.Drawing.Point((int)value.X, (int)value.Y);
		}
	}

	internal static string ChatFont
	{
		get
		{
			string chatFontName = Chat_tacular.Properties.Settings.Default.chatFontName;
			if (chatFontName == null)
			{
				return "Calibri";
			}
			return chatFontName;
		}
		set
		{
			Chat_tacular.Properties.Settings.Default.chatFontName = value;
		}
	}

	internal static int ChatFontSize
	{
		get
		{
			return Chat_tacular.Properties.Settings.Default.chatFontSize;
		}
		set
		{
			Chat_tacular.Properties.Settings.Default.chatFontSize = value;
		}
	}

	internal static string BgChatFontColor
	{
		get
		{
			return Chat_tacular.Properties.Settings.Default.bgFontColor;
		}
		set
		{
			Chat_tacular.Properties.Settings.Default.bgFontColor = value;
		}
	}

	internal static string FgChatFontColor
	{
		get
		{
			return Chat_tacular.Properties.Settings.Default.fgFontColor;
		}
		set
		{
			Chat_tacular.Properties.Settings.Default.fgFontColor = value;
		}
	}

	public Login()
	{
		InitializeComponent();
		if (AuthFlag)
		{
			App.auth = AuthToken.Replace(" ", "").Replace("oauth:", "");
			box_oauth.Text = "oauth:" + App.auth;
		}
		box_oauth.Text = AuthToken;
		box_channel.Text = Chat_tacular.Properties.Settings.Default.loginChannel;
		box_username.Text = Chat_tacular.Properties.Settings.Default.loginName;
	}

	private void button_verify_Click(object sender, RoutedEventArgs e)
	{
		Chat_tacular.Properties.Settings.Default.Reset();
		Process process = LoadURL();
		while (!process.HasExited)
		{
			Thread.Sleep(1000);
		}
		AuthFlag = true;
		Chat_tacular.Properties.Settings.Default.Save();
	}

	private void button_exit_Click(object sender, RoutedEventArgs e)
	{
		closing = false;
		App.auth = box_oauth.Text;
		App.username = box_username.Text;
		App.channel = box_channel.Text;
		AuthToken = App.auth;
		if (App.auth.Length > 0 && App.username.Length > 3 && App.channel.Length > 3)
		{
			Chat_tacular.Properties.Settings.Default.Save();
			Close();
		}
	}

	private void Window_Closing(object sender, CancelEventArgs e)
	{
		string auth = App.auth;
		if (auth == null || auth.Length >= 30)
		{
			string username = App.username;
			if (username == null || username.Length >= 4)
			{
				string channel = App.channel;
				if (channel == null || channel.Length >= 4)
				{
					return;
				}
			}
		}
		Application.Current.Shutdown();
	}

	private HttpWebResponse GetUrl(IDictionary<string, string> values, string URL)
	{
		HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(App.EncodeURL(values, URL, ""));
		obj.Method = "GET";
		obj.KeepAlive = true;
		obj.AllowAutoRedirect = true;
		obj.Accept = "*/*";
		return (HttpWebResponse)obj.GetResponse();
	}

	private Process LoadURL(string URL = "https://id.twitch.tv/oauth2/authorize")
	{
		return Process.Start(GetUrl(new Dictionary<string, string>
		{
			{ "client_id", "l2hfsdik4txfm297h4yguxqhad9x21" },
			{ "redirect_uri", "https://dev.circleprefect.com/auth.php" },
			{ "response_type", "token" },
			{ "scope", "chat:edit chat:read" }
		}, URL).ResponseUri.AbsoluteUri);
	}

	private void box_oauth_TextChanged(object sender, TextChangedEventArgs e)
	{
		button_exit.IsEnabled = box_oauth.Text.Contains("oauth:") && box_oauth.Text.Length == 36;
	}

	private void Window_Closed(object sender, EventArgs e)
	{
		if (closing)
		{
			Application.Current.Shutdown();
		}
	}
}
