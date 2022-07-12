using System;
using WpfApp1.Models;

namespace WpfApp1.Services
{

    public delegate void ConnectionStatus(ConnectingStatus status);

    public interface IServerConnectionStatus
    {

        public event ConnectionStatus OnConnectionStatusChanged;
    }
}
