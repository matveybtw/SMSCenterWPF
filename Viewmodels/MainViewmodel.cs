using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SMSC;
using WPFbase;
namespace PorjectSMSCWPF.Viewmodels
{
    public class SentMessage : IMessage
    {
        string Text;
        string phone;
        string id;
        public string Message { get => Text; set => Text = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Id { get => id; set => id = value; }
        public string MessageType { get; set; }
        DateTime time { get; set; }
        public string TimeSent { get => time.ToString("dd.MM.yyyy HH:mm:ss"); set => time = DateTime.Parse(value); }

    }
    public class MainViewmodel : OnPropertyChangedHandler
    {
        public List<string> TypesOfMessage { get; set; } = new List<string>()
        {
            "SMS",
            "Viber"
        };
        public ObservableCollection<SentMessage> SentMessages { get; set; } = new ObservableCollection<SentMessage>();
        public ChangingItem<string> chosentype { get; set; } = new ChangingItem<string>();
        public ChangingItem<string> Login { get; set; } = new ChangingItem<string>();
        public ChangingItem<string> Password { get; set; } = new ChangingItem<string>();
        public ChangingItem<string> Phone { get; set; } = new ChangingItem<string>();
        public ChangingItem<string> Text { get; set; } = new ChangingItem<string>();
        public MainViewmodel()
        {
            chosentype.Item = "";
            Login.Item = "";
            Password.Item = "";
            Phone.Item = "";
            Text.Item = "";
        }
        public ICommand SendMessage => new RelayCommand(o =>
        {
            SMSCenter smsc = new SMSCenter(Login.Item, Password.Item);
            if (chosentype.Item == "SMS")
            {
                SMSMessage sm = new SMSMessage()
                {
                    Message = Text.Item,
                    Phone = Phone.Item
                };
                smsc.SendSms(ref sm);
                SentMessages.Add(new SentMessage()
                {
                    Id = sm.Id,
                    Message = sm.Message,
                    MessageType = "SMS",
                    Phone = sm.Phone,
                    TimeSent = DateTime.Now.ToString()
                });
                OnPropertyChanged(nameof(SentMessages));
                string Status = "Статус:" + smsc.GetStatus(sm).ToString() + "\nНомер получателя: " + sm.Phone + "\nТекст сообщения: " + sm.Message + "\nId сообщения: " + sm.Id + "\nТип сообщения: " + "SMS" + "Время отправки: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
                MessageBox.Show(Status, "Sent message status", MessageBoxButton.OK, MessageBoxImage.None);
                chosentype.Item = "";
                Login.Item = "";
                Password.Item = "";
                Phone.Item = "";
                Text.Item = "";
            }
            else if (chosentype.Item == "Viber")
            {
                ViberMessage vm = new ViberMessage()
                {
                    Message = Text.Item,
                    Phone = Phone.Item
                };
                smsc.SendViber(ref vm);
                SentMessages.Add(new SentMessage()
                {
                    Id = vm.Id,
                    Message = vm.Message,
                    MessageType = "Viber",
                    Phone = vm.Phone,
                    TimeSent = DateTime.Now.ToString()

                });
                OnPropertyChanged(nameof(SentMessages));
                string Status = "Статус:" + smsc.GetStatus(vm).ToString() + "\nНомер получателя: " + vm.Phone + "\nТекст сообщения: " + vm.Message + "\nId сообщения: " + vm.Id + "\nТип сообщения: " + "Viber" + "\nДата и время отправки: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
                MessageBox.Show(Status, "Sent message status", MessageBoxButton.OK, MessageBoxImage.None);

            }
        }, o => chosentype.Item.Length > 0 && Login.Item.Length > 0 && Password.Item.Length > 0 && Phone.Item.Length > 0 && Text.Item.Length > 0 && Regex.IsMatch(Phone.Item, @"^\+38\(0+\d{2}\)+\d{7}"));

    }
}