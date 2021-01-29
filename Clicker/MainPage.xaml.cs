using System;
using Windows.Storage;
using Windows.ApplicationModel;
using System.IO;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Clicker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public StorageFolder SavePath = Package.Current.InstalledLocation;
        public int Clicks, Second, AddClicksPerClick = 1, AddClicksPerSecond = 0;
        public int ClicksPerClickPrice = 50, ClicksPerSecondPrice = 50;
        public int ColorNow = 0;
        public bool GameIsPaused, SaveIsOn, ConsoleIsVisible, TipsIsVisible;
        public string[] ConsoleInputText;

        enum ColorTypes { white, red, orange, yellow, green, lightblue, blue, purple}

        public MainPage()
        {
            this.InitializeComponent();
            /*
            if (File.Exists(SavePath.Path + @"\gamesave.sav"))
            {
                SaveIsOn = true;
                LoadGame();
            } */
            Time();
            ElementSoundPlayer.State = ElementSoundPlayerState.On;
        }

        public void SaveGame()
        {
            if(File.Exists(SavePath.Path + @"\gamesave.sav"))
            {
                File.Delete(SavePath.Path + @"\gamesave.sav");
            }
            BinaryWriter binaryWriter = new BinaryWriter(File.Open(SavePath.Path + @"\gamesave.sav", FileMode.Create));
            binaryWriter.Write(Clicks);
            binaryWriter.Write(AddClicksPerClick);
            binaryWriter.Write(AddClicksPerSecond);
            binaryWriter.Write(ClicksPerClickPrice);
            binaryWriter.Write(ClicksPerSecondPrice);
            binaryWriter.Write(ColorNow);
            binaryWriter.Dispose();
        }

        public void LoadGame()
        {
            if(File.Exists(SavePath.Path))
            {
                try
                {
                    BinaryReader binaryReader = new BinaryReader(File.OpenRead(SavePath.Path + @"\gamesave.sav"));
                    Clicks = binaryReader.ReadInt32();
                    AddClicksPerClick = binaryReader.ReadInt32();
                    AddClicksPerSecond = binaryReader.ReadInt32();
                    ClicksPerClickPrice = binaryReader.ReadInt32();
                    ClicksPerSecondPrice = binaryReader.ReadInt32();
                    ColorNow = binaryReader.ReadInt32();
                    binaryReader.Dispose();
                }
                catch
                {

                }
            }
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GameIsPaused == false)
            {
                GameIsPaused = true;
                PauseBtn.Content = "⏩";
            }
            else
            {
                GameIsPaused = false;
                PauseBtn.Content = "⏸";
            }
            PlayFocus();

        }

        private void ColorChangingBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangeColor();
            PlayFocus();

        }

        private void HelpBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TipsIsVisible == true)
            {
                HelpTeTi.IsOpen = false;
                PauseTeTi.IsOpen = false;
                ChangeColorTeTi.IsOpen = false;
                ClickTeTi.IsOpen = false;
                ClickUpgradeTeTi.IsOpen = false;
                SecondUpgradeTeTi.IsOpen = false;
                TipsIsVisible = false;
            }
            else
            {
                HelpTeTi.IsOpen = true;
                PauseTeTi.IsOpen = true;
                ChangeColorTeTi.IsOpen = true;
                ClickTeTi.IsOpen = true;
                ClickUpgradeTeTi.IsOpen = true;
                SecondUpgradeTeTi.IsOpen = true;
                TipsIsVisible = true;
            }
            if (ConsoleIsVisible == true)
            {
                ConsoleTextBox.Visibility = Visibility.Collapsed;
                ConsoleEnterBtn.Visibility = Visibility.Collapsed;
                ConsoleIsVisible = false;
            }
            else
            {
                ConsoleTextBox.Visibility = Visibility.Visible;
                ConsoleEnterBtn.Visibility = Visibility.Visible;
                ConsoleIsVisible = true;
            }
            PlayFocus();

        }

        private void ConsoleEnterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ConsoleTextBox.Text != "")
            {
                try
                {
                    ConsoleInputText = ConsoleTextBox.Text.Split(' ');
                    if(ConsoleInputText[0] == "Clicks")
                    {
                        Clicks = Convert.ToInt32(ConsoleInputText[1]);
                    }
                    if (ConsoleInputText[0] == "Second")
                    {
                        Second = Convert.ToInt32(ConsoleInputText[1]);
                    }
                    if (ConsoleInputText[0] == "AddClicksPerClick")
                    {
                        AddClicksPerClick = Convert.ToInt32(ConsoleInputText[1]);
                    }
                    if (ConsoleInputText[0] == "AddClicksPerSecond")
                    {
                        AddClicksPerSecond = Convert.ToInt32(ConsoleInputText[1]);
                    }
                    if (ConsoleInputText[0] == "ClicksPerClickPrice")
                    {
                        ClicksPerClickPrice = Convert.ToInt32(ConsoleInputText[1]);
                    }
                    if (ConsoleInputText[0] == "ClicksPerSecondPrice")
                    {
                        ClicksPerSecondPrice = Convert.ToInt32(ConsoleInputText[1]);
                    }
                    if (ConsoleInputText[0] == "Help")
                    {
                        ConsoleTextBox.Text = "Clicks (int) /Second (int) /AddClicksPerClick (int) /AddClicksPerSecond (int) /ClicksPerClickPrice (int) /ClicksPerSecondPrice (int)";
                    }
                    if (ConsoleInputText[0] == "Info")
                    {
                        ConsoleTextBox.Text = "Clicks/Second/AddClicksPerClick/AddClicksPerSecond/ClicksPerClickPrice/ClicksPerSecondPrice";
                    }
                }
                catch
                {

                }
            }
        }

        /*
         * public int Clicks, Second, AddClicksPerClick = 1, AddClicksPerSecond = 0;
         * public int ClicksPerClickPrice = 50, ClicksPerSecondPrice = 50;
         * public int ColorNow = 0;
         * public bool GameIsPaused, SaveIsOn;
         */

        public async void Time()
        {
            while (true)
            {
                if(GameIsPaused == false)
                {
                    Second++;
                    Clicks += AddClicksPerSecond;
                    ClickBtn.Content = Clicks;
                    //SaveGame();
                    await Task.Delay(1000);
                }
                else
                {
                    await Task.Delay(25);
                }
            }
        }

        public void ChangeColor()
        {
            switch (ColorNow)
            {
                //🔴🟠🟡🟢🔵🟣⚪🟥🟧🟨🟩🟦🟪⬜
                case 0:
                    //red
                    ThisGrid.Background = new SolidColorBrush(Color.FromArgb(255, 236, 28, 36));
                    ColorNow = 1;
                    ColorChangingBtn.Content = "🟠";
                    break;
                case 1:
                    //orange
                    ThisGrid.Background = new SolidColorBrush(Color.FromArgb(255, 255, 127, 39));
                    ColorNow = 2;
                    ColorChangingBtn.Content = "🟡";
                    break;
                case 2:
                    //yellow
                    ThisGrid.Background = new SolidColorBrush(Color.FromArgb(255, 255, 242, 0));
                    ColorNow = 3;
                    ColorChangingBtn.Content = "🟢";
                    break;
                case 3:
                    //green
                    ThisGrid.Background = new SolidColorBrush(Color.FromArgb(255, 14, 209, 69));
                    ColorNow = 4;
                    ColorChangingBtn.Content = "🔵";
                    break;
                case 4:
                    //lightblue
                    ThisGrid.Background = new SolidColorBrush(Color.FromArgb(255, 0, 168, 243));
                    ColorNow = 5;
                    ColorChangingBtn.Content = "🔵";
                    break;
                case 5:
                    //blue
                    ThisGrid.Background = new SolidColorBrush(Color.FromArgb(255, 63, 72, 204));
                    ColorNow = 6;
                    ColorChangingBtn.Content = "🟣";
                    break;
                case 6:
                    //purple
                    ThisGrid.Background = new SolidColorBrush(Color.FromArgb(255, 146, 61, 186));
                    ColorNow = 7;
                    ColorChangingBtn.Content = "⚪";
                    break;
                case 7:
                    //white
                    ThisGrid.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    ColorNow = 0;
                    ColorChangingBtn.Content = "🔴";
                    break;
            }
        }

        private void ClickBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GameIsPaused == false)
            {
                Clicks += AddClicksPerClick;
                ClickBtn.Content = Clicks;
            }
            PlayFocus();

        }

        private void ClickUpgradeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GameIsPaused == false && Clicks >= ClicksPerClickPrice)
            {
                AddClicksPerClick++;
                Clicks -= ClicksPerClickPrice;
                ClicksPerClickPrice += (ClicksPerClickPrice / 2);
                ClickBtn.Content = Clicks;
                ClickUpgradeBtn.Content = "Buy per click upgrade.\nPrice: " + ClicksPerClickPrice;
            }
            PlayFocus();
        }

        private void SecondUpgradeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GameIsPaused == false && Clicks >= ClicksPerSecondPrice)
            {
                AddClicksPerSecond++;
                Clicks -= ClicksPerSecondPrice;
                ClicksPerSecondPrice += (ClicksPerSecondPrice / 2);
                ClickBtn.Content = Clicks;
                SecondUpgradeBtn.Content = "Buy per second upgrade.\nPrice: " + ClicksPerSecondPrice;
            }
            PlayFocus();

        }

        void PlayFocus()
        {
            ElementSoundPlayer.Play(ElementSoundKind.Focus);
        }
    }
}
