using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualChart3D.Common
{
    interface ITimer
    {
        void Start();

        void Stop();

        void DisplayTime();
    }

    class CustomTimer : ITimer
    {
        System.Diagnostics.Stopwatch sw;

        private const string CalculatedTimeTitle = "Затраченное время";
        private const int SymbolsAfterComma = 3;
        private const string BadTimerStart = "Ошибка. Повторный запуск уже ведущего отсчет таймера.";
        private const string BadTimerEnd = "Ошибка. Попытка остановить еще не запущенный отчет таймера.";
        private const string TimeElapsedFormat = "Подсчет длился {0} сек.";
        private bool _isCountDown;

        public CustomTimer()
        {
            sw = new System.Diagnostics.Stopwatch();
            _isCountDown = false;
        }

        public void DisplayTime()
        {
            double time = sw.Elapsed.TotalSeconds;
            time = Math.Round(time, SymbolsAfterComma);

            if (time == 0)
            {
                time = Math.Pow(10, -SymbolsAfterComma);
            }

            Utils.ShowWarningMessage(String.Format(TimeElapsedFormat, time.ToString()), CalculatedTimeTitle);
        }

        public void Start()
        {
            if (_isCountDown)
            {
                throw new InvalidOperationException(BadTimerStart);
            }

            sw.Reset();
            sw.Start();
            _isCountDown = true;
        }

        public void Stop()
        {
            if (!_isCountDown)
            {
                throw new InvalidOperationException(BadTimerEnd);
            }

            sw.Stop();
            DisplayTime();
            _isCountDown = false;
        }
    }
}
