using JetBrains.Annotations;

namespace Baracuda.Native.Collections
{
    public interface INativePredicate<T> where T : unmanaged
    {
        [Pure]
        public bool Evaluate(in T element);
    }
}