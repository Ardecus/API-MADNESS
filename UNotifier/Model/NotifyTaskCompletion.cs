using System.ComponentModel;
using System.Threading.Tasks;

namespace UNotifier
{
    public sealed class NotifyTaskCompletion<TResult> : INotifyPropertyChanged
    {
        public Task<TResult> Task { get; private set; }
        public TResult Result
        {
            get
            {
                return (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : default(TResult);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public NotifyTaskCompletion(Task<TResult> task)
        {
            Task = task;
            if (!task.IsCompleted)
            {
                var _ = WatchTaskAsync(task);
            }
        }

        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
                Task<TResult> inner = Task<TResult>.Factory.StartNew(() => { return default(TResult); });
                task = inner;
            }

            var propertyChanged = PropertyChanged;
            if (propertyChanged != null && task.IsCompleted)
            {
                propertyChanged(this, new PropertyChangedEventArgs($"{nameof(Result)}"));
            }
        }
    }
}