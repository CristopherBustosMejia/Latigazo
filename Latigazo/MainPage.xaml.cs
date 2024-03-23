using Plugin.Maui.Audio;

#pragma warning disable CS8622

namespace Latigazo
{
    public partial class MainPage : ContentPage
    {
        private const double sensibility = 2.5;
        private bool isMoved = false;

        public MainPage()
        {
            InitializeComponent();
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Accelerometer.Start(SensorSpeed.UI);
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var position = e.Reading;

            var acceleration = Math.Sqrt(
                position.Acceleration.X * position.Acceleration.X +
                position.Acceleration.Y * position.Acceleration.Y +
                position.Acceleration.Z * position.Acceleration.Z
            );

            if (acceleration >= sensibility && !isMoved)
            {
                isMoved = true;
                playSound();
            }
        }

        private async void playSound()
        {
            String fileName = "whiplash.wav";
            try
            {
                IAudioPlayer player = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(fileName));
                player.Play();
                await Task.Delay(500);
                isMoved = false;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }

}
