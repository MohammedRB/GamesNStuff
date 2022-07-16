// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using Animancer;
using Animancer.Units;
using UnityEngine;

namespace PlatformerGameKit
{
    /// <summary>A camera movement manager for a 2D platformer.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/other/camera-man">Camera Man</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/CameraMan
    /// 
    [AddComponentMenu(Strings.MenuPrefix + "Camera Man")]
    [HelpURL(Strings.APIDocumentation + "/" + nameof(CameraMan))]
    [ExecuteAlways]
    public sealed class CameraMan : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        [Tooltip("The camera this script is controlling")]
        private Camera _Camera;
        public ref Camera Camera => ref _Camera;

        [SerializeField]
        [Tooltip("The object this script is tracking")]
        private Transform _Target;
        public ref Transform Target => ref _Target;

        [SerializeField]
        [Tooltip("[Optional] The Rigidbody2D of the Target")]
        private Rigidbody2D _TargetRigidbody;
        public ref Rigidbody2D TargetRigidbody => ref _TargetRigidbody;

        [SerializeField]
        [Tooltip("The position offset from the target to the camera")]
        private Vector3 _Offset;
        public ref Vector3 Offset => ref _Offset;

        [SerializeField, Multiplier]
        [Tooltip("Determines how much the distance from the camera to where it is supposed to be affects its velocity")]
        private float _OffsetFactor = 1;
        public ref float OffsetFactor => ref _OffsetFactor;

        [SerializeField, Multiplier]
        [Tooltip("Determines how much the velocity of the Target Rigidbody affects the velocity of the camera")]
        private float _VelocityFactor = 1;
        public ref float VelocityFactor => ref _VelocityFactor;

        [SerializeField, MetersPerSecondPerSecond]
        [Tooltip("The flat rate at which the camera accelerates")]
        private float _LinearAcceleration = 1;
        public ref float LinearAcceleration => ref _LinearAcceleration;

        [SerializeField, Multiplier]
        [Tooltip("The additional rate at which the camera accelerates in proportion to how far away its velocity is from the desired value")]
        private float _DynamicAcceleration = 1;
        public ref float DynamicAcceleration => ref _DynamicAcceleration;

        /// <summary>The <see cref="Component.transform"/> cached since it is used frequently.</summary>
        private Transform _Transform;

        /// <summary>The current speed and direction this camera is moving.</summary>
        private Vector3 _Velocity;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only] Ensures that all fields have valid values and finds missing components nearby.</summary>
        private void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Camera);

            if ((_Target == null || _TargetRigidbody == null) && !UnityEditor.EditorUtility.IsPersistent(this))
            {
                var target = GameObject.FindGameObjectWithTag("Player");
                if (target != null)
                {
                    if (_Target == null)
                        _Target = target.transform;

                    target.GetComponentInParentOrChildren(ref _TargetRigidbody);
                }
            }

            PlatformerUtilities.NotNegative(ref _VelocityFactor);
            PlatformerUtilities.NotNegative(ref _OffsetFactor);
            PlatformerUtilities.NotNegative(ref _LinearAcceleration);
            PlatformerUtilities.NotNegative(ref _DynamicAcceleration);

            if (_Target != null)
            {
                if (_Offset == default)
                    _Offset = transform.position - _Target.position;
                else
                    transform.position = _Target.position + _Offset;
            }
        }
#endif

        /************************************************************************************************************************/

        private void Awake()
        {
            _Transform = transform;
        }

        /************************************************************************************************************************/

        private void LateUpdate()
        {
            if (_Target == null)
                return;

            var position = _Transform.position;
            var destination = _Target.position + _Offset;

#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (UnityEditor.Selection.Contains(gameObject))
                {
                    _Offset = position - _Target.position;
                }
                else
                {
                    _Transform.position = destination;
                }

                return;
            }
#endif

            var hasRigidbody = _TargetRigidbody != null;
            var velocity = hasRigidbody ? _TargetRigidbody.velocity : default;

            var screenPosition = _Camera.WorldToScreenPoint(destination);
            if (screenPosition.x < 0 ||
                screenPosition.y < 0 ||
                screenPosition.x > Screen.width ||
                screenPosition.y > Screen.height)
            {
                _Transform.position = destination;
                _Velocity = velocity;
                return;
            }

            var targetVelocity = (destination - position) * _OffsetFactor;

            if (_VelocityFactor > 0)
                targetVelocity += (Vector3)(velocity * _VelocityFactor);

            var deltaTime = Time.deltaTime;

            _Velocity = Vector3.MoveTowards(_Velocity, targetVelocity, _LinearAcceleration * deltaTime);
            _Velocity = Vector3.LerpUnclamped(_Velocity, targetVelocity, _DynamicAcceleration * deltaTime);

            position += _Velocity * deltaTime;

            _Transform.position = position;
        }

        /************************************************************************************************************************/
    }
}