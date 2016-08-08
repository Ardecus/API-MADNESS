using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UNotifier
{
    [DataContract]
    public class JsonResponce<T>
    {
        [DataMember] public string status;
        [DataMember] public List<T> result;
    }
}
