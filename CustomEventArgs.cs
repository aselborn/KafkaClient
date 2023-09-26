namespace kafkaclient
{
    public class CustomEventArgs<T> : EventArgs
    {
        public T Entity { get; }

        public CustomEventArgs(T entity)
        {
            Entity = entity;
        }
    }
}