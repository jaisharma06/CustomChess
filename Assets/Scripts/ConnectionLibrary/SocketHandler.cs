using BestHTTP.SocketIO3;
using System;
using UnityEngine;

namespace CustomChess.ConnectionLibrary
{
    public class SocketHandler
    {
        public SocketData socketData {get; private set;} = new SocketData();

        public SocketHandler(string socketUrl)
        {
            socketData.socketManager = new SocketManager(new Uri(socketUrl), SetSocketOptions());

            socketData.socket?.On(SocketIOEventTypes.Connect, OnConnect);
            socketData.socket?.On(SocketIOEventTypes.Disconnect, OnDisconnect);
            socketData.socket?.On<Error>(SocketIOEventTypes.Error, OnError);
        }

        private SocketOptions SetSocketOptions()
        {
            SocketOptions socketOptions = new SocketOptions()
            {
                ConnectWith = BestHTTP.SocketIO3.Transports.TransportTypes.WebSocket,
                Reconnection = true,
                ReconnectionAttempts = int.MaxValue,
                ReconnectionDelay = TimeSpan.FromSeconds(1),
                ReconnectionDelayMax = TimeSpan.FromSeconds(1),
                RandomizationFactor = 0,
                Timeout = TimeSpan.FromSeconds(7),
                AutoConnect = true,
                QueryParamsOnlyForHandshake = true
            };

            return socketOptions;
        }

        private void OnConnect()
        {
            Debug.Log("Socket Connected");
        }

        private void OnDisconnect()
        {
            Debug.Log("Socket Disconnected");
        }

        private void OnError(Error error)
        {
            Debug.Log($"Socket Error: {error}");
        }
    }

    public class SocketData
    {
        public bool isSocketConnected;
        public SocketManager socketManager;
        public Socket socket { get => (socketManager != null) ? socketManager.Socket : null; }
    }
}