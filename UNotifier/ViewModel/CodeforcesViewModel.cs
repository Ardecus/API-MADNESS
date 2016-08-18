using System.Collections.Generic;
using UNotifier;

namespace Codeforces
{
    public class CodeforcesViewModel : ViewModel
    {
        public NotifyTaskCompletion<List<CodeforcesContest>> Contests { get; private set; }

        override public void RefreshData()
        {
            Contests = new NotifyTaskCompletion<List<CodeforcesContest>>(CodeforcesContest.GetContests());
        }
    }
}
