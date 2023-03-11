using SorceressSpell.LibrarIoh.Math;
using UnityEngine;
using UnityEngine.UI;

namespace SorceressSpell.LibrarIoh.Unity.Unity2D.Pixel.UI
{
    public class PixelScrollbar : MonoBehaviour
    {
        #region Fields

        public int ScrollUnits = 1;

        private Scrollbar _scrollbar;
        private ScrollRect _scrollRect;

        #endregion Fields

        #region Methods

        private void Awake()
        {
            _scrollRect = GetComponentInParent<ScrollRect>();
            _scrollbar = GetComponent<Scrollbar>();
        }

        private void LateUpdate()
        {
            UpdateScrollbarValue();
        }

        private void UpdateScrollbarValue()
        {
            bool vertical = _scrollbar.direction == Scrollbar.Direction.TopToBottom || _scrollbar.direction == Scrollbar.Direction.BottomToTop;

            RectTransform parentRectTransform = (RectTransform)_scrollRect.content.parent;


            float contentViewportSize = vertical ? parentRectTransform.rect.height : parentRectTransform.rect.width;
            float contentFullSize = vertical ? _scrollRect.content.rect.height : _scrollRect.content.rect.width;

            if (contentViewportSize % ScrollUnits != 0 || contentFullSize % ScrollUnits != 0)
            {
                // ScrollUnits won't be able to fit the content satisfactorily. Aborting.
                return;
            }

            // Value
            float scrollValueUnit = ScrollUnits / ((contentFullSize - contentViewportSize));
            _scrollbar.value = MathOperations.RoundToNearestMultiplier(MathOperations.Clamp(_scrollbar.value, 0f, 1f), scrollValueUnit);
        }

        #endregion Methods
    }
}
