using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CodingDojo4.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        private Client clients;
        private bool connect = false;

        public string Name { get; set; }
        public string Nachricht { get; set; }
        public ObservableCollection<string> EmpfangeneNachrichten { get; set; }
        public RelayCommand ConnectBtnClick { get; set; }
        public RelayCommand SendBtnClick { get; set; }

        public MainViewModel()
        {
            Nachricht = "";
            EmpfangeneNachrichten = new ObservableCollection<string>();

            ConnectBtnClick = new RelayCommand(
                () =>
                {
                    connect = true;
                    clients = new Client("127.0.0.1", 10100, new Action<string>(NeuEmpfangeneNachrichten), ClientDisconnected);

                },
            () =>
            {
                return (!connect);
            });
            SendBtnClick = new RelayCommand(
      () =>
      {
          clients.Send(Name + ": " + Nachricht);
          EmpfangeneNachrichten.Add("YOU: " + Nachricht);
      }, () => { return (connect && Nachricht.Length >= 1); });
        }

        private void NeuEmpfangeneNachrichten(string obj)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                EmpfangeneNachrichten.Add(Nachricht);
            });
        }

        private void ClientDisconnected()
        {
            connect = false;
            CommandManager.InvalidateRequerySuggested();
        }
    }
}