using UnityEngine;

namespace SorceressSpell.LibrarIoh.Unity.Unity2D.Pixel
{
    public static class Vector2Extensions
    {
        #region Methods

        public static Vector2 RoundToPixelByPixelsPerUnit(this Vector2 position, float pixelsPerUnit)
        {
            float pixelUnitSize = PixelOperations.CalculatePixelSize(pixelsPerUnit);
            return RoundToPixelByPixelSize(position, pixelUnitSize);
        }

        public static Vector2 RoundToPixelByPixelSize(this Vector2 position, float pixelSize)
        {
            return new Vector2(
                PixelOperations.CalculatePixelPositionByPixelSize(position.x, pixelSize),
                PixelOperations.CalculatePixelPositionByPixelSize(position.y, pixelSize)
            );
        }

        #endregion Methods
    }
}
