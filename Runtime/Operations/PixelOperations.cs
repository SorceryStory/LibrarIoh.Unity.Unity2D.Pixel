using SorceressSpell.LibrarIoh.Math;

namespace SorceressSpell.LibrarIoh.Unity.Unity2D.Pixel
{
    public static class PixelOperations
    {
        #region Methods

        public static float CalculatePixelPositionByPixelSize(float position, float pixelUnitSize)
        {
            return MathOperations.RoundToNearestMultiplier(position, pixelUnitSize);
        }

        public static float CalculatePixelPositionByPixelsPerUnit(float position, float pixelsPerUnit)
        {
            float pixelUnitSize = CalculatePixelSize(pixelsPerUnit);
            return CalculatePixelPositionByPixelSize(position, pixelUnitSize);
        }

        public static float CalculatePixelSize(float pixelsPerUnit)
        {
            return 1f / pixelsPerUnit;
        }

        #endregion Methods
    }
}
