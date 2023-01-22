using SorceressSpell.LibrarIoh.Math;
using UnityEngine;

namespace SorceressSpell.LibrarIoh.Unity.Unity2D.Pixel
{
    public static class Vector3Extensions
    {
        #region Methods

        public static Vector3 RoundToPixelByPixelsPerUnit(this Vector3 position, float pixelsPerUnit)
        {
            float pixelUnitSize = PixelOperations.CalculatePixelSize(pixelsPerUnit);
            return RoundToPixelByPixelSize(position, pixelUnitSize);
        }

        public static Vector3 RoundToPixelByPixelSize(this Vector3 position, float pixelSize)
        {
            return new Vector3(
                MathOperations.RoundToNearestMultiplier(position.x, pixelSize),
                MathOperations.RoundToNearestMultiplier(position.y, pixelSize),
                MathOperations.RoundToNearestMultiplier(position.z, pixelSize)
            );
        }

        #endregion Methods
    }
}
