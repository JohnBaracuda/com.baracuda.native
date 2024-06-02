using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Baracuda.DOTS
{
    public static class TypeConversionExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 ToFloat3(this Color color)
        {
            return new float3(color.r, color.g, color.b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 ToFloat4(this Color color)
        {
            return new float4(color.r, color.g, color.b, color.a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ToColor(this float3 color)
        {
            return new Color(color.x, color.y, color.z, 1);
        }
    }
}