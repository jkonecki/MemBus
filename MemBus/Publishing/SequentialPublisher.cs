namespace MemBus.Publishing
{
    /// <summary>
    /// This is the most simple publisher that works like event handlers: All subscriptions are called in sequence.
    /// If any subscription throws an exception the chain is broken. This will also be used when calling the awaitable Publish
    /// method
    /// </summary>
    #if WINRT
    public class SequentialPublisher : IPublishPipelineMember, IAsyncPublishPipelineMember
    #else
    public class SequentialPublisher : IPublishPipelineMember
    #endif
    {
        public void LookAt(PublishToken token)
        {
            foreach (var s in token.Subscriptions)
                s.Push(token.Message);
        }

        #if WINRT
        public async System.Threading.Tasks.Task LookAtAsync(PublishToken token)
        {
            LookAt(token);
        }
        #endif
    }
}