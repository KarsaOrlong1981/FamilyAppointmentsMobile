using System.Text;

namespace FamilyAppointmentsMobile.Helpers 
{
    public static class DispatcherHelper
    {
        private static IDispatcher _dispatcher;

        public static void Initialize()
        {
            if (_dispatcher == null)
            {
                _dispatcher = Application.Current?.Dispatcher ?? throw new InvalidOperationException("Dispatcher is not available.");
            }
        }

        public static void Reset()
        {
            _dispatcher = null;
        }

        private static void CheckDispatcher()
        {
            if (_dispatcher == null)
            {
                var message = new StringBuilder("The DispatcherHelper is not initialized.");
                message.AppendLine();
                message.Append("Call DispatcherHelper.Initialize() at App.xaml.cs OnStart()");
                throw new InvalidOperationException(message.ToString());
            }
        }

        public static void CheckBeginInvokeOnUI(Action action)
        {
            if (action == null)
            {
                return;
            }

            CheckDispatcher();

            if (_dispatcher.IsDispatchRequired)
            {
                _dispatcher.Dispatch(action);
            }
            else
            {
                action();
            }
        }

        public static async Task RunAsync(Action action)
        {
            CheckDispatcher();

            if (_dispatcher.IsDispatchRequired)
            {
                await _dispatcher.DispatchAsync(action);
            }
            else
            {
                action();
            }
        }
    }

}
