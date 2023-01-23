using SorceressSpell.LibrarIoh.Math;
using UnityEngine;

namespace SorceressSpell.LibrarIoh.Unity.Unity2D.Pixel
{
    [RequireComponent(typeof(Transform))]
    [RequireComponent(typeof(Camera))]
    [DisallowMultipleComponent]
    [AddComponentMenu("LibrarIoh/Pixel/Pixel Camera")]
    [ExecuteAlways]
    public class PixelCamera : MonoBehaviour
    {
        #region Fields

        private Camera _camera;

        [SerializeField]
        private Color _clearColor = Color.black;

        [SerializeField]
        private bool _clearScreen = false;

        [SerializeField]
        private bool _cropX = false;

        [SerializeField]
        private bool _cropY = false;

        [SerializeField]
        private bool _forceHighestPixelSize = false;

        [SerializeField]
        private int _pixelSize = 1;

        [SerializeField]
        private int _pixelSizeMax = 1;

        [SerializeField]
        private float _pixelsPerUnit = 16;

        private bool _recalculate;

        [SerializeField]
        private Vector2Int _targetResolution = new Vector2Int(320, 240);

        [SerializeField]
        private bool _targetSpecificResolution = false;

        private Transform _transform;

        #endregion Fields

        #region Properties

        public Color ClearColor
        {
            set
            {
                _clearColor = value;
            }
            get
            {
                return _clearColor;
            }
        }

        public bool ClearScreen
        {
            set
            {
                _clearScreen = value;
            }
            get
            {
                return _clearScreen;
            }
        }

        public bool CropX
        {
            set
            {
                if (value != _cropX)
                {
                    _cropX = value;
                    _recalculate = true;
                }
            }
            get
            {
                return _cropX;
            }
        }

        public bool CropY
        {
            set
            {
                if (value != _cropY)
                {
                    _cropY = value;
                    _recalculate = true;
                }
            }
            get
            {
                return _cropY;
            }
        }

        public bool ForceHighestPixelSize
        {
            set
            {
                if (value != _forceHighestPixelSize)
                {
                    _forceHighestPixelSize = value;
                    _recalculate = true;
                }
            }
            get
            {
                return _forceHighestPixelSize;
            }
        }

        public int PixelSize
        {
            set
            {
                if (value != _pixelSize)
                {
                    _pixelSize = value;
                    _recalculate = true;
                }
            }
            get
            {
                return _pixelSize;
            }
        }

        public int PixelSizeMax
        {
            get
            {
                return _pixelSizeMax;
            }
        }

        public float PixelsPerUnit
        {
            set
            {
                if (value != _pixelsPerUnit)
                {
                    _pixelsPerUnit = value;
                    _recalculate = true;
                }
            }
            get
            {
                return _pixelsPerUnit;
            }
        }

        public Vector2Int TargetResolution
        {
            set
            {
                if (value != _targetResolution)
                {
                    _targetResolution = value;
                    _recalculate = true;
                }
            }
            get
            {
                return _targetResolution;
            }
        }

        public bool TargetSpecificResolution
        {
            set
            {
                if (value != _targetSpecificResolution)
                {
                    _targetSpecificResolution = value;
                    _recalculate = true;
                }
            }
            get
            {
                return _targetSpecificResolution;
            }
        }

        #endregion Properties

        #region Delegates

        public delegate void RenderQuadSizeChangedEventHandler(Vector2Int newRenderSize);

        #endregion Delegates

        #region Events

        public event RenderQuadSizeChangedEventHandler OnRenderQuadSizeChanged;

        #endregion Events

        #region Methods

        // Used by the custom inspector
        public void SetRecalculate()
        {
            _recalculate = true;
        }

        private void LateUpdate()
        {
            if (_recalculate)
            {
                RecalculateCameraParameters();
            }
        }

        private void OnDisable()
        {
            UnsetCamera();
        }

        private void OnEnable()
        {
            _transform = GetComponent<Transform>();
            _camera = GetComponent<Camera>();

            SetCamera();

            SetRecalculate();
        }

        private void OnPreCull()
        {
            SetWorldToCameraMatrix();
        }

        private void OnPreRender()
        {
            if (ClearScreen)
            {
                // According to: https://docs.unity3d.com/ScriptReference/GL.html GL drawing
                // commands execute immediately. That means if you call them in Update(), they will
                // be executed before the camera is rendered (and the camera will most likely clear
                // the screen, making the GL drawing not visible).
                GL.Clear(true, true, ClearColor, 0f);
            }
        }

        private void OnValidate()
        {
            if (PixelSize < 1)
            {
                PixelSize = 1;
            }

            if (TargetSpecificResolution && PixelSize > PixelSizeMax)
            {
                PixelSize = PixelSizeMax;
            }
        }

        private void RecalculateCameraParameters()
        {
            Vector2Int finalRenderSize;

            // Render Size and Pixel Size Recalculation
            if (_targetSpecificResolution)
            {
                int maxPixelSizeX = MathOperations.TruncateToInt((float)Screen.width / _targetResolution.x);
                int maxPixelSizeY = MathOperations.TruncateToInt((float)Screen.height / _targetResolution.y);

                _pixelSizeMax = MathOperations.Min(maxPixelSizeX, maxPixelSizeY);

                if (_pixelSizeMax > 0)
                {
                    finalRenderSize = new Vector2Int(_targetResolution.x, _targetResolution.y);
                    _pixelSize = _forceHighestPixelSize ? _pixelSizeMax : MathOperations.Clamp(_pixelSize, 1, _pixelSizeMax);
                }
                else
                {
                    finalRenderSize = new Vector2Int(Screen.width, Screen.height);
                    _pixelSize = 1;
                }
            }
            else
            {
                _pixelSizeMax = MathOperations.Min(Screen.width, Screen.height) / 2;

                finalRenderSize = new Vector2Int(
                    MathOperations.TruncateToInt((float)Screen.width / _pixelSize),
                    MathOperations.TruncateToInt((float)Screen.height / _pixelSize)
                    );
            }

            // This prevents pixels cut in half. To prevent this, you want the same ammount of
            // pixels to be rendered from the position of the camera.
            finalRenderSize = new Vector2Int(
                finalRenderSize.x % 2 != 0 ? finalRenderSize.x - 1 : finalRenderSize.x,
                finalRenderSize.y % 2 != 0 ? finalRenderSize.y - 1 : finalRenderSize.y
                );

            finalRenderSize *= _pixelSize;

            // Camera Size Recalculation
            _camera.orthographicSize = ((_cropY ? finalRenderSize.y : Screen.height) / 2.0f) / (_pixelsPerUnit * _pixelSize);

            // Camera Viewport Rect Recalculation
            if (_cropX || _cropY)
            {
                Vector2Int remainder = new Vector2Int(Screen.width, Screen.height) - finalRenderSize;
                Vector2Int bottomLeft = remainder / 2;
                //Vector2Int topright = remainder - bottomLeft;

                _camera.rect = new Rect(
                    _cropX ? (float)bottomLeft.x / Screen.width : 0f,
                    _cropY ? (float)bottomLeft.y / Screen.height : 0f,
                    _cropX ? (float)finalRenderSize.x / Screen.width : 1f,
                    _cropY ? (float)finalRenderSize.y / Screen.height : 1f
                    );
            }
            else
            {
                _camera.rect = new Rect(0f, 0f, 1f, 1f);
            }

            // Propagate new Render Size
            OnRenderQuadSizeChanged?.Invoke(finalRenderSize);

            _recalculate = false;
        }

        private void SetCamera()
        {
            _camera.clearFlags = CameraClearFlags.SolidColor;
            _camera.orthographic = true;
        }

        /// <summary>
        /// Sets the camera's worldToCameraMatrix by its offset to its pixel round position so it
        /// renders at an acceptable pixel position.
        /// </summary>
        private void SetWorldToCameraMatrix()
        {
            Vector3 rawCameraPosition = _transform.position;
            Vector3 roundedCameraPosition = rawCameraPosition.RoundToPixelByPixelsPerUnit(PixelsPerUnit);
            Vector3 offset = roundedCameraPosition - rawCameraPosition;
            offset.z = -offset.z;
            Matrix4x4 offsetMatrix = Matrix4x4.TRS(-offset, Quaternion.identity, new Vector3(1.0f, 1.0f, -1.0f));

            _camera.worldToCameraMatrix = offsetMatrix * _camera.transform.worldToLocalMatrix;
        }

        private void UnsetCamera()
        {
            _transform.position = _transform.position.RoundToPixelByPixelsPerUnit(PixelsPerUnit);

            _camera.rect = new Rect(0f, 0f, 1f, 1f);
            _camera.ResetWorldToCameraMatrix();
        }

        #endregion Methods
    }
}
