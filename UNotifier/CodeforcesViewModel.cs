using System;
using System.Collections.Generic;
using System.Windows.Threading;
using UNotifier;

namespace Codeforces
{
    public class CodeforcesViewModel
    {
        private static TimeSpan refreshTime = new TimeSpan(0, 30, 0);

        public NotifyTaskCompletion<List<Contest>> Contests { get; private set; }

        public CodeforcesViewModel()
        {
            Contests = new NotifyTaskCompletion<List<Contest>>(Contest.GetContests());
            DispatcherTimer timer = new DispatcherTimer();

            timer.Tick += new EventHandler(timerTick);
            timer.Interval = refreshTime;
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e)
        {
            Contests = new NotifyTaskCompletion<List<Contest>>(Contest.GetContests());
        }
    }
}
