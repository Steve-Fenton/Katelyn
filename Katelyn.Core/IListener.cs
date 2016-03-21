using System;
using System.Net.Http;

namespace Katelyn.Core
{
    public interface IListener
    {
        void OnError(string address, Exception exception);

        void OnSuccess(string address);

        void OnEnd();
    }
}