using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Linq;

namespace CodingDojo4_Server.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        private Server server;
        private const int port = 8090;
        private const string ip = "127.0.0.1";
        private bool connect = false;

        public RelayCommand StartBtnClick { get; set; }
        public RelayCommand StopBtnClick { get; set; }
        public RelayCommand DropClientBtnClick { get; set; }
        public ObservableCollection<string> Users { get; set; }
        public ObservableCollection<string> Nachrichten { get; set; }

        public string SelectedUser { get; set; }

        public int NoOfReceivedMessages
        {
            get
            {
                return Nachrichten.Count;
            }
        }


        public MainViewModel()
        {
            Nachrichten = new ObservableCollection<string>();
            Users = new ObservableCollection<string>();

            StartBtnClick = new RelayCommand(
                () =>
                {
                    server = new Server(ip, port, UpdateGuiWithNewMessage);
                    server.StartAccepting();
                    connect = true;
                },
                () => { return !connect; });


            StopBtnClick = new RelayCommand(

                () =>
                {
                    server.StopAccepting();
                    connect = false;
                },
                                () => { return connect; });

            DropClientBtnClick = new RelayCommand(() =>
{
    server.DisconnectSpecificClient(SelectedUser);
    Users.Remove(SelectedUser);
},
    () => { return (SelectedUser != null); });


        }
            public void UpdateGuiWithNewMessage(string message)
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    string name = message.Split(':')[0];
                    if (!Users.Contains(name))
                    {
                        Users.Add(name);
                    }
                    Nachrichten.Add(message);
                    RaisePropertyChanged("NoOfReceivedMessages");
                });




            }
        }
    }
