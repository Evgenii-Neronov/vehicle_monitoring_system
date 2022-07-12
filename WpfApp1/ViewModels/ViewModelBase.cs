using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp1.Annotations;
using WpfApp1.Services;

namespace WpfApp1.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged, IServerConnectionStatus
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void InitConnectionEvents(object connectionStatusObject)
        {
            if (connectionStatusObject is IServerConnectionStatus connectionStatus)
            {
                connectionStatus.OnConnectionStatusChanged += (status) => { OnConnectionStatusChanged?.Invoke(status); };
            }
        }
        public event ConnectionStatus? OnConnectionStatusChanged;
    }
}
