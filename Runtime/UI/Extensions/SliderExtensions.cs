using SorceressSpell.LibrarIoh.Math;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace SorceressSpell.LibrarIoh.Unity.Unity2D.Pixel.UI
{
    public static class SliderExtensions
    {
        #region Methods

        public static void SetValuePixel(this Slider slider, float value)
        {
            slider.value = MathOperations.RoundToNearestMultiplier(MathOperations.Clamp(value, 0f, 1f), (1 / ((RectTransform)slider.fillRect.parent).rect.width));
        }

        #endregion Methods
    }
}
