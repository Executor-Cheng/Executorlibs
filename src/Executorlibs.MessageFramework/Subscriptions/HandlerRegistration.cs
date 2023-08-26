using System;

namespace Executorlibs.MessageFramework.Subscriptions
{
    public readonly struct HandlerRegistration : IDisposable
    {
        private readonly IDisposable[]? _registrations;

        public HandlerRegistration(IDisposable[] registrations)
        {
            _registrations = registrations;
        }

        public void Dispose()
        {
            if (_registrations is IDisposable[] registrations)
            {
                foreach (var registration in registrations)
                {
                    registration.Dispose();
                }
            }
        }
    }
}
