using Unity.Entities;

namespace Baracuda.Native
{
    public static class WorldUtility
    {
        public const WorldSystemFilterFlags ServerClientThinFlags = WorldSystemFilterFlags.ServerSimulation | WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation;

        public static void DestroyGameWorlds()
        {
            for (var index = World.All.Count - 1; index >= 0; index--)
            {
                var world = World.All[index];
                if (world.Flags == WorldFlags.Game)
                {
                    world.Dispose();
                }
            }
        }

        public static void DestroyGameClientWorlds()
        {
            for (var index = World.All.Count - 1; index >= 0; index--)
            {
                var world = World.All[index];
                if ((world.Flags & WorldFlags.GameClient) != 0)
                {
                    world.Dispose();
                }
            }
        }

        public static void DestroyGameThinClientWorlds()
        {
            for (var index = World.All.Count - 1; index >= 0; index--)
            {
                var world = World.All[index];
                if ((world.Flags & WorldFlags.GameThinClient) != 0)
                {
                    world.Dispose();
                }
            }
        }

        public static void DestroyGameServerWorlds()
        {
            for (var index = World.All.Count - 1; index >= 0; index--)
            {
                var world = World.All[index];
                if ((world.Flags & WorldFlags.GameServer) != 0)
                {
                    world.Dispose();
                }
            }
        }
    }
}