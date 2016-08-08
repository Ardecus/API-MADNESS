using System.Collections.Generic;

namespace Codeforces
{
    public class CodeforcesViewModel
    {
        public List<Contest> Items
        {
            get { return Contest.GetContests(); }
        }
    }
}
