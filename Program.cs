using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Diagnostics;
using System.Windows.Forms;
using System.Windows.Input;
using FoundationR;
using FoundationR.Lib;
using FoundationR.Rew;
using FoundationR.Loader;
using FoundationR.Ext;
using FoundationR.Headers;
using Microsoft.Win32;
using Chat_tacular;
using TwitchLib.Client.Events;
using TwitchLib.Communication.Interfaces;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Extensions;
using System.DirectoryServices.ActiveDirectory;

namespace ChatTacular2
{
	internal class Program
	{
		static int StartX => 0;
		static int StartY => 0;
		static int Width => 640;
		static int Height => 480;
		static int BitsPerPixel => 32;
		static string Title = "Foundation_GameTemplate";

		[STAThread]
		static void Main()
		{
			new Login().ShowDialog();
			Main m = null;
			Thread t = new Thread(() => { (m = new Main()).Run(SurfaceType.WindowHandle_Loop, new Surface(StartX, StartY, Width, Height, Title, BitsPerPixel)); });
			t.SetApartmentState(ApartmentState.STA);
			t.Start();
			Bot bot = new Bot();
			while (Console.ReadLine() != "exit") ;
			t.Abort();
			Environment.Exit(0);
		}
	}
	public class Main : Foundation
	{
		Point mouse;
		RECT window_frame;
		REW pane;
		REW tile;
		REW cans;
		REW solidColor;
		Form form;
		IList<Keys> keyboard = new List<Keys>();
		bool handled = false;
		string chatLog = string.Empty;

		public static Main Instance;

		internal Main()
		{
			Instance = this;
		}

		public override void RegisterHooks(Form form)
		{
			Foundation.UpdateEvent += Update;
			Foundation.ResizeEvent += Resize;
			Foundation.InputEvent += Input;
			Foundation.DrawEvent += Draw;
			Foundation.InitializeEvent += Initialize;
			Foundation.LoadResourcesEvent += LoadResources;
			Foundation.MainMenuEvent += MainMenu;
			Foundation.PreDrawEvent += PreDraw;
			Foundation.ViewportEvent += Viewport;
			Foundation.ExitEvent += Exit;
			this.form = form;
		}

		protected bool Exit(ExitArgs e)
		{
			return false;
		}

		public override void ClearInput()
		{
			keyboard.Clear();
			handled = false;
		}

		protected void Input(InputArgs e)
		{
			try
			{ 
				form.Invoke(new Action(() =>
				{
					var _mouse = form.PointToClient(System.Windows.Forms.Cursor.Position);
					int x = _mouse.X;
					int y = _mouse.Y;
					this.mouse = new Point(x + 8, y + 31);
				}));
			}
			catch
			{
				return;
			}
		}

		protected void Viewport(ViewportArgs e)
		{
		}

		protected void PreDraw(PreDrawArgs e)
		{
		}

		protected void MainMenu(DrawingArgs e)
		{
		}

		protected void LoadResources()
		{
			Asset.LoadFromFile(@".\Textures\bluepane.rew", out pane);
			Asset.LoadFromFile(@".\Textures\background.rew", out tile);
			Asset.LoadFromFile(@".\Textures\cans.rew", out cans);
		}

		protected void Initialize(InitializeArgs e)
		{
		}

		protected void Draw(DrawingArgs e)
		{
			e.rewBatch.Draw(cans, RewBatch.Viewport.X, RewBatch.Viewport.X);
			//e.rewBatch.Draw(pane, 0, 0);
			if (mouse.X + 50 >= 640 || mouse.Y + 50 >= 480 || mouse.X <= 0 || mouse.Y <= 0)
				goto COLORS;
			e.rewBatch.Draw(tile.GetPixels(), mouse.X, mouse.Y, 50, 50);
		COLORS:
			e.rewBatch.Draw(REW.Create(50, 50, Color.White, Ext.GetFormat(4)), 0, 0);
			e.rewBatch.Draw(REW.Create(50, 50, Color.Red, Ext.GetFormat(4)), 50, 0);
			e.rewBatch.Draw(REW.Create(50, 50, Color.Green, Ext.GetFormat(4)), 100, 0);
			e.rewBatch.Draw(REW.Create(50, 50, Color.Blue, Ext.GetFormat(4)), 150, 0);
			e.rewBatch.Draw(REW.Create(50, 50, Color.Gray, Ext.GetFormat(4)), 200, 0);
			e.rewBatch.Draw(REW.Create(50, 50, Color.Black, Ext.GetFormat(4)), 250, 0);
			e.rewBatch.Draw(REW.Create(50, 50, Color.Purple, Ext.GetFormat(4)), 640, 50);
			e.rewBatch.DrawString("Arial", chatLog, 50, 50, 500, 500, Color.White);
		}

		protected void Update(UpdateArgs e)
		{
		}

		protected bool Resize(ResizeArgs e)
		{
			return false;
		}

		private new bool KeyDown(Key k)
		{
			return Keyboard.PrimaryDevice.IsKeyDown(k);
		}
		private new bool KeyUp(Key k)
		{
			return Keyboard.PrimaryDevice.IsKeyUp(k);
		}

		private void WriteLine(string message)
		{
			chatLog += message + '\n';
		}

		public void Client_OnLog(object sender, OnLogArgs e)
        {
            //WriteLine($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        }
  
        public void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            WriteLine($"Connected to {e.AutoJoinChannel}");
        }
  
        public void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
			WriteLine("Hey guys! I am a bot connected via TwitchLib!");
            Bot.Instance.client.SendMessage(e.Channel, "Hey guys! I am a bot connected via TwitchLib!");
        }

        public void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
		{
			if (e.ChatMessage.Message.Contains("badword"))
                Bot.Instance.client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromMinutes(30), "Bad word! 30 minute timeout!");
        }
        
        public void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
			if (e.WhisperMessage.Username == "my_friend")
                Bot.Instance.client.SendWhisper(e.WhisperMessage.Username, "Hey! Whispers are so cool!!");
        }
        
        public void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            if (e.Subscriber.SubscriptionPlan == SubscriptionPlan.Prime)
                Bot.Instance.client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points! So kind of you to use your Twitch Prime on this channel!");
			else
                Bot.Instance.client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points!");
        }
	}
}
