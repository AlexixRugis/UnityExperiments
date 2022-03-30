using UnityEngine;

namespace ARTech.Nav3D
{
    public static class VectorOps
    {
        public static Vector3 RoundToGrid(Vector3 position, float gridSize)
        {
            return ((Vector3)Vector3Int.RoundToInt(position / gridSize)) * gridSize;
        }  
    }
}