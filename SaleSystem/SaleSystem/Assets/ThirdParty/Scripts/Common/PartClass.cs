namespace MyTools
{
    public class PartClass<T>
    {
        protected T _parent;

        public virtual void Init(T parent)
        {
            _parent = parent;
        }
    }
}