using Unity.Entities;
using UnityEngine;

namespace Baracuda.Native
{
    public class NameAuthoring : MonoBehaviour
    {
        [SerializeField] private string entityName;

        public class NameBaker : Baker<NameAuthoring>
        {
            public override void Bake(NameAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new EntityNameData
                {
                    Name = authoring.entityName
                });
            }
        }
    }
}