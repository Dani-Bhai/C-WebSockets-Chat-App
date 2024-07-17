using ChattingApp.MVVM.Core;
using ChattingApp.MVVM.Model;
using ChattingApp.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChattingApp.MVVM.ViewModel
{
    class MainViewModel
    {

        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<string> Messages { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand SendMessageToServerCommand { get; set; }
        private Server _server;

        public string Username {  get; set; }
        public string Message { get; set; }


        public MainViewModel() {
            Users = new ObservableCollection<User>();
            Messages = new ObservableCollection<string>();
            _server = new Server();
            _server.connectedEvent += UserConnected; 
            _server.messageReceivedEvent += MessageReceived; 
            _server.userDisconnectEvent += UserDisconnected; 
            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(Username), o=> !string.IsNullOrEmpty(Username));
            
            SendMessageToServerCommand = new RelayCommand(o => _server.SendMessageToServer(Message), o=> !string.IsNullOrEmpty(Message));
        }

        private void MessageReceived()
        {
            var msg = _server._packetReader.ReadMessage();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
        }

        private void UserDisconnected()
        {
            var uid = _server._packetReader.ReadMessage();
            var user = Users.Where(x => x.UID == uid).FirstOrDefault();
            Application.Current.Dispatcher.Invoke(() => Users.Remove(user));
        }

        private void UserConnected()
        {
            var user = new User
            {
                Username = _server._packetReader.ReadMessage(),
                UID = _server._packetReader.ReadMessage(),
            };

            if (!Users.Any(x => x.UID == user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }
    }
}
