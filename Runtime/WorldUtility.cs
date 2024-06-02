using Unity.Entities;

namespace Baracuda.DOTS
{
    public static class WorldUtility
    {
        public static void DestroyLocalSimulationWorld()
        {
            for (var index = World.All.Count - 1; index >= 0; index--)
            {
                var world = World.All[index];
                if (world.Flags == WorldFlags.Game)
                {
                    world.Dispose();
                    break;
                }
            }
        }

        public static void DestroyClientWorlds()
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

        public static void DestroyThinClientWorlds()
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

        public static void DestroyServerWorlds()
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