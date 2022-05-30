using System.Timers;
using Timer = System.Timers.Timer;

namespace BlazorEcommerce.Client.Services.ToastService
{
    public class ToastService : IDisposable
    {
        public event Action<string, ToastLevel> OnShow;

        public event Action OnHide;

        private Timer Countdown;

        public void ShowToast(string message, ToastLevel level, int time = 5000)
        {
            OnShow?.Invoke(message, level);
            StartCountdown(time);
        }

        private void StartCountdown(int time = 5000)
        {
            SetCountdown(time);

            if (Countdown.Enabled)
            {
                Countdown.Stop();
                Countdown.Start();
            }
            else
            {
                Countdown.Start();
            }
        }

        private void SetCountdown(int time = 5000)
        {
            if (Countdown == null)
            {
                Countdown = new Timer(time);
                Countdown.Elapsed += HideToast;
                Countdown.AutoReset = false;
            }
        }

        private void HideToast(object source, ElapsedEventArgs args)
        {
            OnHide?.Invoke();
        }

        public void Dispose()
        {
            Countdown?.Dispose();
        }
    }
}
