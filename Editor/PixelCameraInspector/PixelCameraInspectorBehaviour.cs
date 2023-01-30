using SorceressSpell.LibrarIoh.Math;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SorceressSpell.LibrarIoh.Unity.Unity2D.Pixel.Editor
{
    [CustomEditor(typeof(PixelCamera))]
    internal class PixelCameraInspectorBehaviour : UnityEditor.Editor
    {
        #region Fields

        public VisualTreeAsset UXML;
        private Toggle _cropXToggle;
        private Toggle _cropYToggle;
        private Toggle _forceHighestPixelSizeToggle;
        private PixelCamera _pixelCamera;
        private IntegerField _pixelSizeField;
        private FloatField _pixelsPerUnitField;
        private HelpBox _possibleRenderArtifactsMessageHelpBox;
        private Vector2IntField _targetResolutionField;
        private VisualElement _targetSpecificResolutionDependentVisualElement;
        private Toggle _targetSpecificResolutionToggle;

        #endregion Fields

        #region Methods

        public override VisualElement CreateInspectorGUI()
        {
            // Create a new VisualElement to be the root of our inspector UI
            VisualElement inspectorVisualElement = new VisualElement();

            // Load from default reference
            UXML.CloneTree(inspectorVisualElement);

            // Pixel
            _pixelSizeField = inspectorVisualElement.Q<IntegerField>("PixelSize");
            _pixelSizeField.RegisterCallback<ChangeEvent<int>>(OnPixelSizeChanged);

            UpdatePixelSizeElement();

            _pixelsPerUnitField = inspectorVisualElement.Q<FloatField>("PixelsPerUnit");
            _pixelsPerUnitField.RegisterCallback<ChangeEvent<float>>(OnPixelsPerUnitChanged);

            // Crop
            _cropXToggle = inspectorVisualElement.Q<Toggle>("CropX");
            _cropXToggle.RegisterCallback<ChangeEvent<bool>>(OnCropXChanged);

            _cropYToggle = inspectorVisualElement.Q<Toggle>("CropY");
            _cropYToggle.RegisterCallback<ChangeEvent<bool>>(OnCropYChanged);

            // Specific Resolution
            _targetSpecificResolutionToggle = inspectorVisualElement.Q<Toggle>("TargetSpecificResolution");
            _targetSpecificResolutionToggle.RegisterCallback<ChangeEvent<bool>>(OnTargetSpecificResolutionChanged);

            _targetSpecificResolutionDependentVisualElement = inspectorVisualElement.Q<VisualElement>("TargetSpecificResolutionDependent");
            UpdateTargetSpecificResolutionDependentElement();

            _targetResolutionField = inspectorVisualElement.Q<Vector2IntField>("TargetResolution");
            _targetResolutionField.RegisterCallback<ChangeEvent<Vector2Int>>(OnTargetResolutionChanged);

            _forceHighestPixelSizeToggle = inspectorVisualElement.Q<Toggle>("ForceHighestPixelSize");
            _forceHighestPixelSizeToggle.RegisterCallback<ChangeEvent<bool>>(OnForceHighestPixelSizeChanged);

            // asdsadada
            _possibleRenderArtifactsMessageHelpBox = inspectorVisualElement.Q<HelpBox>("PossibleRenderArtifactsMessage");

            UpdatePossibleRenderArtifactsMessageElement();

            // Return the finished inspector UI
            return inspectorVisualElement;
        }

        private void OnCropXChanged(ChangeEvent<bool> evt)
        {
            _pixelCamera.SetRecalculate();
        }

        private void OnCropYChanged(ChangeEvent<bool> evt)
        {
            _pixelCamera.SetRecalculate();
        }

        private void OnDisable()
        {
            _pixelCamera.OnRenderQuadSizeChanged -= OnPixelCameraRenderQuadSizeChanged;
        }

        private void OnEnable()
        {
            _pixelCamera = (PixelCamera)target;

            _pixelCamera.OnRenderQuadSizeChanged += OnPixelCameraRenderQuadSizeChanged;
        }

        private void OnForceHighestPixelSizeChanged(ChangeEvent<bool> evt)
        {
            //_pixelSizeField.SetEnabled(!evt.newValue);
            //_pixelSizeSlider.SetEnabled(!(_pixelCamera.TargetSpecificResolution && _pixelCamera.ForceHighestPixelSize));

            //_pixelCamera.ForceHighestPixelSize = evt.newValue;

            _pixelCamera.SetRecalculate();

            UpdatePixelSizeElement();
        }

        private void OnPixelCameraRenderQuadSizeChanged(Vector2Int newRenderSize)
        {
            UpdatePossibleRenderArtifactsMessageElement();
        }

        private void OnPixelSizeChanged(ChangeEvent<int> evt)
        {
            _pixelSizeField.value = MathOperations.Clamp(_pixelSizeField.value, 1, _pixelCamera.PixelSizeMax);

            _pixelCamera.SetRecalculate();
        }

        private void OnPixelsPerUnitChanged(ChangeEvent<float> evt)
        {
            _pixelCamera.SetRecalculate();
        }

        private void OnTargetResolutionChanged(ChangeEvent<Vector2Int> evt)
        {
            _pixelCamera.SetRecalculate();
        }

        private void OnTargetSpecificResolutionChanged(ChangeEvent<bool> evt)
        {
            _pixelCamera.SetRecalculate();

            UpdatePixelSizeElement();
            UpdateTargetSpecificResolutionDependentElement();
        }

        private void UpdatePixelSizeElement()
        {
            _pixelSizeField.SetEnabled(!(_pixelCamera.TargetSpecificResolution && _pixelCamera.ForceHighestPixelSize));
        }

        private void UpdatePossibleRenderArtifactsMessageElement()
        {
            bool possibleRenderArtifactsX = (Screen.width % _pixelCamera.PixelSize != 0 && !_pixelCamera.CropX);
            bool possibleRenderArtifactsY = (Screen.height % _pixelCamera.PixelSize != 0 && !_pixelCamera.CropY);

            if (possibleRenderArtifactsX || possibleRenderArtifactsY)
            {
                _possibleRenderArtifactsMessageHelpBox.style.display = DisplayStyle.Flex;

                _possibleRenderArtifactsMessageHelpBox.messageType = HelpBoxMessageType.Warning;

                if (possibleRenderArtifactsX && possibleRenderArtifactsY)
                {
                    _possibleRenderArtifactsMessageHelpBox.text = string.Format(PixelCameraInspectorStrings.TextHelpBoxPossibleRenderArtifactsBothAxis, Screen.width, Screen.height, _pixelCamera.PixelSize);
                }
                else if (possibleRenderArtifactsX)
                {
                    _possibleRenderArtifactsMessageHelpBox.text = string.Format(PixelCameraInspectorStrings.TextHelpBoxPossibleRenderArtifactsSingleAxis, PixelCameraInspectorStrings.AxisWidth, Screen.width, _pixelCamera.PixelSize, "X");
                }
                else if (possibleRenderArtifactsY)
                {
                    _possibleRenderArtifactsMessageHelpBox.text = string.Format(PixelCameraInspectorStrings.TextHelpBoxPossibleRenderArtifactsSingleAxis, PixelCameraInspectorStrings.AxisHeight, Screen.height, _pixelCamera.PixelSize, "Y");
                }
            }
            else
            {
                _possibleRenderArtifactsMessageHelpBox.style.display = DisplayStyle.None;
            }
        }

        private void UpdateTargetSpecificResolutionDependentElement()
        {
            _targetSpecificResolutionDependentVisualElement.style.display = _pixelCamera.TargetSpecificResolution ? DisplayStyle.Flex : DisplayStyle.None;
        }

        #endregion Methods
    }
}
