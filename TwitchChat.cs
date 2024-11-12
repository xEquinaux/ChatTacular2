using System;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using Chat_tacular;

namespace ChatTacular2
{
    public class ProgramStartup
    {
        public static void Run()
        {
            new Login().ShowDialog();
        }
    }

    public class Bot
    {
        public static Bot Instance;
        public TwitchClient client;
	
        public Bot()
        {
            Instance = this;
                                                                             
            ConnectionCredentials credentials = new ConnectionCredentials(Chat_tacular.App.username, Chat_tacular.App.auth);
	        var clientOptions = new ClientOptions
                {
                    MessagesAllowedInPeriod = 750,
                    ThrottlingPeriod = TimeSpan.FromSeconds(30)
                };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, Chat_tacular.App.channel);

            client.OnLog += Main.Instance.Client_OnLog;
            client.OnJoinedChannel += Main.Instance.Client_OnJoinedChannel;
            client.OnMessageReceived += Main.Instance.Client_OnMessageReceived;
            client.OnWhisperReceived += Main.Instance.Client_OnWhisperReceived;
            client.OnNewSubscriber += Main.Instance.Client_OnNewSubscriber;
            client.OnConnected += Main.Instance.Client_OnConnected;

            client.Connect();
        }
    }
}