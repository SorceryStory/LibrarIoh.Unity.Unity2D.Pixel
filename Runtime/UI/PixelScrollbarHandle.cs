using SorceressSpell.LibrarIoh.Math;
using UnityEngine;
using UnityEngine.UI;

namespace SorceressSpell.LibrarIoh.Unity.Unity2D.Pixel.UI
{
    public class PixelScrollbarHandle : MonoBehaviour
    {
        #region Fields

        public int MinSize = 16;

        private RectTransform _rectTransform;
        private Scrollbar _scrollbar;

        #endregion Fields

        #region Methods

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _scrollbar = GetComponentInParent<Scrollbar>();
        }

        private void LateUpdate()
        {
            bool vertical = (_scrollbar.direction == Scrollbar.Direction.TopToBottom || _scrollbar.direction == Scrollbar.Direction.BottomToTop);

            RectTransform parentRectTransform = (RectTransform)_rectTransform.parent;

            float contentSize = (vertical ? parentRectTransform.rect.height : parentRectTransform.rect.width);
            float pixelUnit = 1 / contentSize;

            _rectTransform.anchorMin = new Vector2(
                vertical ? _rectTransform.anchorMin.x : MathOperations.RoundToNearestMultiplier(_rectTransform.anchorMin.x, pixelUnit),
                vertical ? MathOperations.RoundToNearestMultiplier(_rectTransform.anchorMin.y, pixelUnit) : _rectTransform.anchorMin.y
                );
            _rectTransform.anchorMax = new Vector2(
                vertical ? _rectTransform.anchorMax.x : MathOperations.RoundToNearestMultiplier(_rectTransform.anchorMax.x, pixelUnit),
                vertical ? MathOperations.RoundToNearestMultiplier(_rectTransform.anchorMax.y, pixelUnit) : _rectTransform.anchorMax.y
                );

            // Forcing Min Size
            int currentSize = (int)((vertical ? (_rectTransform.anchorMax.y - _rectTransform.anchorMin.y) : (_rectTransform.anchorMax.x - _rectTransform.anchorMin.x)) / pixelUnit);

            // Always add towards highest empty space
            if (currentSize < MinSize)
            {
                float spaceMin = vertical ? _rectTransform.anchorMin.y : _rectTransform.anchorMin.x;
                float spaceMax = 1 - (vertical ? _rectTransform.anchorMax.y : _rectTransform.anchorMax.x);

                float anchorSizeOfHandle = MinSize * pixelUnit;

                if (spaceMax > spaceMin)
                {
                    _rectTransform.anchorMax = new Vector2(
                        vertical ? _rectTransform.anchorMax.x : _rectTransform.anchorMin.x + anchorSizeOfHandle,
                        vertical ? _rectTransform.anchorMin.y + anchorSizeOfHandle : _rectTransform.anchorMax.y
                   );
                }
                else
                {
                    _rectTransform.anchorMin = new Vector2(
                        vertical ? _rectTransform.anchorMin.x : _rectTransform.anchorMax.x - anchorSizeOfHandle,
                        vertical ? _rectTransform.anchorMax.y - anchorSizeOfHandle : _rectTransform.anchorMin.y
                   );
                }
            }
        }

        #endregion Methods
    }
}
