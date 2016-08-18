using System.Collections.Generic;
using UNotifier;

namespace Freelance
{
    public class FreelanceViewModel : ViewModel
    {
        public NotifyTaskCompletion<List<FreelanceOffer>> Offers { get; private set; }

        override public void RefreshData()
        {
            Offers = new NotifyTaskCompletion<List<FreelanceOffer>>(FreelanceOffer.GetOffers());
        }
    }
}
