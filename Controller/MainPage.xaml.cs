using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace Controller
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            AddText("Старт метода работы...");
            AddText("Запущен!...");
            AddText("Запуск лонг пулл...");
            var longpoll = new LongPoll();

            AddText("Создание и старт потока...");

            var thread = new Thread(longpoll.Start);
            thread.Start();

            AddText("Поток запущен...");


            longpoll.ErrorEvent += NewError;
            longpoll.NewMessageEvent += NewMessage;
            longpoll.DebugEvent += AddText;

        }

        public void Running()
        {
           
        }

        public void NewMessage(Models.Message message)
        {
            var textBlock = new TextBlock();
            string text = $"[{message.Time}] => ({message.PeerId}) {message.Text}";
            textBlock.Text = text;
            panel.Children.Add(textBlock);
        }

        public void AddText(string Text)
        {
            var textBlock = new TextBlock();
            textBlock.Text = Text;
            panel.Children.Add(textBlock);
        }

        public void NewError(Exception e)
        {
            var textBlock = new TextBlock();
            string text = $"ОШИБКА: {e.Message}";
            textBlock.Text = text;
            panel.Children.Add(textBlock);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
