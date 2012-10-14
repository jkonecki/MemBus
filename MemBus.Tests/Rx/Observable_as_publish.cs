﻿using System;

using System.Text;
using MemBus.Configurators;
using MemBus.Tests.Help;
using MemBus.Tests.Frame;
using System.Reactive.Linq;
using MemBus.Messages;
using MessageA = Membus.Reactive.Tests.Help.MessageA;

#if WINRT
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TestFixtureSetUp = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestInitializeAttribute;
#else
using NUnit.Framework;

#endif

namespace MemBus.Tests.Rx
{
    [TestFixture]
    public class Observable_as_publish
    {
        private class DumbObservable<M> : IObservable<M>, IDisposable
        {
            private IObserver<M> _observer;

            public IDisposable Subscribe(IObserver<M> observer)
            {
                _observer = observer;
                return this;
            }

            public void Message(M msg)
            {
                if (_observer != null)
                  _observer.OnNext(msg);
            }

            public void Throw(Exception x)
            {
                _observer.OnError(x);
            }

            public void End()
            {
                _observer.OnCompleted();
            }

            public void Dispose()
            {
                _observer.OnCompleted();
                _observer = null;
            }
        }

        readonly IBus bus = BusSetup.StartWith<Conservative>().Construct();
        private DumbObservable<string> _dumbo;

        [TestFixtureSetUp]
        public void Setup()
        {
            _dumbo = new DumbObservable<string>();
            bus.Publish(_dumbo);
        }

        [Test]
        public void Messages_emanating_from_observable_are_received()
        {
            var messageReceived = false;
            using (bus.Subscribe((string s) => messageReceived = true))
            {
                _dumbo.Message("Hi");
            }
            messageReceived.ShouldBeTrue();
        }

        [Test]
        public void exception_is_mapped_to_correct_message()
        {
            var messageReceived = false;
            using (bus.Subscribe((ExceptionOccurred s) => messageReceived = true))
            {
                _dumbo.Throw(new ArgumentException());
            }
            messageReceived.ShouldBeTrue();
        }

        [Test]
        public void end_is_mapped_to_correct_message()
        {
            var messageReceived = false;
            using (bus.Subscribe((MessageStreamCompleted<string> s) => messageReceived = true))
            {
                _dumbo.End();
            }
            messageReceived.ShouldBeTrue();
        }

    }
}
