﻿using MemBus;
using MemBus.Tests.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemBus.Tests.Help
{
    public class FakePublisher : IPublisher, IDisposable
    {
        public void Publish(object message)
        {
            Message = message;
        }

        #if WINRT
        public Task PublishAsync(object message)
        {
            return new Task(()=> Message = message);
        }
        #endif

        public object Message { get; set; }

        public void VerifyMessageIsOfType<T>()
        {
            Message.ShouldNotBeNull();
            Message.ShouldBeOfType<T>();
        }


        public IDisposable Publish<M>(IObservable<M> observable)
        {
            return this;
        }

        void IDisposable.Dispose() { }
    }
}
