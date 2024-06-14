using Unity.Collections;
using Unity.Entities;

namespace Baracuda.Native
{
    public struct EntityNameData : IComponentData
    {
        public FixedString64Bytes Name;
    }
}