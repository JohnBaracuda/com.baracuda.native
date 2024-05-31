using JetBrains.Annotations;

namespace Baracuda.DOTS.Collections
{
    public interface INativePredicate<T> where T : unmanaged
    {
        [Pure]
        public bool Evaluate(ref T element);
    }
}