using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Assertions;

namespace Baracuda.Native
{
    public static class EntityUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Entity GetSingletonEntityIgnoreEnabledComponents(this ref EntityQuery query,
            ref SystemState state)
        {
            var entities = query.ToEntityArray(state.WorldUpdateAllocator);
            Assert.IsTrue(entities.Length == 1, "The number of entities that match this query is not exactly one");
            return entities[0];
        }
    }
}