using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemBus.Messages;
using MemBus.Subscribing;

namespace MemBus.Support
{
    /// <summary>
    /// Extensions used internally by Membus
    /// </summary>
    public static class UsefulExtensions
    {
        /// <summary>
        /// Sends off <see cref="ExceptionOccurred"/> messages on a bus based on a list of faulted tasks
        /// </summary>
        public static void PublishExceptionMessages(this Task[] tasks, IBus bus)
        {
            for (int i = 0; i < tasks.Length; i++)
                if (tasks[i].IsFaulted)
                    bus.Publish(new ExceptionOccurred(tasks[i].Exception));
        }

        /// <summary>
        /// A single item as Enumerable
        /// </summary>
        public static IEnumerable<T> AsEnumerable<T>(this T item)
        {
            yield return item;
        }

        /// <summary>
        /// LINQ extension method to get a HashSet
        /// </summary>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items)
        {
            return new HashSet<T>(items);
        }

        /// <summary>
        /// LINQ extension method to get a HashSet with a Select Func
        /// </summary>
        public static HashSet<O> ToHashSet<T,O>(this IEnumerable<T> items, Func<T,O> select)
        {
            return new HashSet<O>(items.Select(select));
        }

        /// <summary>
        /// Perform an action with every item of an enumerable
        /// </summary>
        public static void Each<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var i in items)
                action(i);
        }

        /// <summary>
        /// string.Format as extension method
        /// </summary>
        public static string Fmt(this string @string, params object[] args)
        {
            if (@string == null) throw new ArgumentNullException("string");
            return string.Format(@string, args);
        }

        /// <summary>
        /// Return the value accessesd by selector or the default(O) if the input is null
        /// </summary>
        public static O IfNotNull<I,O>(this I input, Func<I,O> selector) where I : class
        {
            return input != null ? selector(input) : default(O);
        }

        internal static bool CheckDenyOrAllIsGood(this object obj)
        {
            return obj is IDenyShaper ? ((IDenyShaper)obj).Deny : false;
        }

        internal static IDisposable TryReturnDisposerOfSubscription(this ISubscription sub)
        {
            return sub is IDisposableSubscription ? ((IDisposableSubscription)sub).GetDisposer() : null;
        }

    }
}