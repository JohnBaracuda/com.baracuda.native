using Unity.Entities;

namespace Baracuda.Native
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct EntityNamingSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndInitializationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<EntityNameData>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI
                .GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (autoEntityName, entity) in SystemAPI
                         .Query<RefRO<EntityNameData>>()
                         .WithEntityAccess())
            {
                ecb.SetName(entity, autoEntityName.ValueRO.Name);
                ecb.RemoveComponent<EntityNameData>(entity);
            }
        }
    }
}