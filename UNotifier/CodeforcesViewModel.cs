using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

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
