// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using Animancer.Units;
using UnityEngine;

namespace PlatformerGameKit
{
    /// <summary>A system for shaking the camera.</summary>
    /// <remarks>
    /// The techniques used in this script are based on a GDC talk:
    /// <see href="https://www.youtube.com/watch?v=tu-Qe66AvtY">
    /// Math for Game Programmers: Juicing Your Cameras With Math</see>.
    /// <para></para>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/other/camera-shake">Camera Shake</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/CameraShake
    /// 
    [AddComponentMenu(Strings.MenuPrefix + "Camera Shake")]
    [HelpURL(Strings.APIDocumentation + "/" + nameof(CameraShake))]
    public sealed class CameraShake : MonoBehaviour
    {
        /************************************************************************************************************************/

        /// <summary>The currently active <see cref="CameraShake"/> instance.</summary>
        public static CameraShake Instance { get; private set; }

        [SerializeField]
        [Tooltip("The maximum distance the shake can move the camera on each axis (in meters)")]
        private Vector3 _PositionStrength;
        public ref Vector3 PositionStrength => ref _PositionStrength;

        [SerializeField]
        [Tooltip("The maximum angle the shake can rotate the camera on each axis (in degrees)")]
        private Vector3 _RotationStrength;
        public ref Vector3 RotationStrength => ref _RotationStrength;

        [SerializeField, Multiplier]
        [Tooltip("The rate at which the shake values change")]
        private float _Frequency = 6;
        public ref float Frequency => ref _Frequency;

        [SerializeField, Units(" ^")]
        [Tooltip("The current magnitude is raised to this power to calculate the actual shake magnitude")]
        private float _MagnitudeExponent = 1;
        public ref float MagnitudeExponent => ref _MagnitudeExponent;

        [SerializeField, Units(" /s")]
        [Tooltip("The rate at which the magnitude decreases")]
        private float _Damping = 1.5f;
        public ref float Damping => ref _Damping;

        private Transform _Transform;
        private Vector3 _PositionOffset;
        private Vector3 _RotationOffset;

        private float _Magnitude;

        /// <summary>The current normalized strength of the camera shake (<c>0</c> = none, <c>1</c> = full).</summary>
        public float Magnitude
        {
            get => _Magnitude;
            set
            {
                _Magnitude = Mathf.Clamp01(value);
                enabled = value > 0;
            }
        }

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only] Ensures that all fields have valid values and finds missing components nearby.</summary>
        private void OnValidate()
        {
            PlatformerUtilities.NotNegative(ref _Frequency);
            _MagnitudeExponent = Mathf.Clamp(_MagnitudeExponent, 0.2f, 5f);
            PlatformerUtilities.NotNegative(ref _Damping);

            if (_Magnitude <= 0)
                enabled = false;
        }
#endif

        /************************************************************************************************************************/

        private void Awake()
        {
            Debug.Assert(Instance == null, $"Another {nameof(CameraShake)} instance already exists", this);
            Instance = this;
            _Transform = transform;
        }

        /************************************************************************************************************************/

        private void LateUpdate()
        {
            _Magnitude -= _Damping * Time.deltaTime;
            if (_Magnitude <= 0)
            {
                enabled = false;
                return;
            }

            _Transform.position -= _PositionOffset;
            _Transform.eulerAngles -= _RotationOffset;

            var magnitude = Mathf.Pow(_Magnitude, _MagnitudeExponent);

            var time = _Frequency * Time.timeSinceLevelLoad;

            _PositionOffset.x = Noise(_PositionStrength.x, magnitude, 0, time);
            _PositionOffset.y = Noise(_PositionStrength.y, magnitude, 1, time);
            _PositionOffset.z = Noise(_PositionStrength.z, magnitude, 2, time);
            _RotationOffset.x = Noise(_RotationStrength.x, magnitude, 3, time);
            _RotationOffset.y = Noise(_RotationStrength.y, magnitude, 4, time);
            _RotationOffset.z = Noise(_RotationStrength.z, magnitude, 5, time);

            _Transform.position += _PositionOffset;
            _Transform.eulerAngles += _RotationOffset;
        }

        /************************************************************************************************************************/

        private static float Noise(float baseStrength, float magnitude, float x, float y) =>
            baseStrength == 0 ?
            0 :// Avoid the actual Noise lookup for values we aren't using.
            baseStrength * magnitude * Noise(x, y);

        private static float Noise(float x, float y) => Mathf.PerlinNoise(x, y) * 2 - 1;

        /************************************************************************************************************************/

        private void OnDisable()
        {
            _Transform.position -= _PositionOffset;
            _Transform.eulerAngles -= _RotationOffset;

            _Magnitude = 0;
            _PositionOffset = _RotationOffset = default;
        }

        /************************************************************************************************************************/
    }
}