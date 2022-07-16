// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer;
using Animancer.Units;
using UnityEngine;

namespace PlatformerGameKit
{
    /// <summary>Applies some <see cref="CameraShake"/> when taking damage.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/other/camera-shake">Camera Shake</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/CameraShakeWhenHit
    /// 
    [AddComponentMenu(Strings.MenuPrefix + "Camera Shake When Hit")]
    [HelpURL(Strings.APIDocumentation + "/" + nameof(CameraShakeWhenHit))]
    public sealed class CameraShakeWhenHit : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private Health _Health;
        public Health Health => _Health;

        [SerializeField, Multiplier]
        [Tooltip("The shake magnitude to apply regardless of damage taken")]
        private float _BaseMagnitude = 0.3f;
        public ref float BaseMagnitude => ref _BaseMagnitude;

        [SerializeField, Multiplier]
        [Tooltip("The additional shake magnitude which is multiplied by the damage taken")]
        private float _ScalingMagnitude = 0.02f;
        public ref float ScalingMagnitude => ref _ScalingMagnitude;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only] Ensures that all fields have valid values and finds missing components nearby.</summary>
        private void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Health);
            PlatformerUtilities.NotNegative(ref _BaseMagnitude);
            PlatformerUtilities.NotNegative(ref _ScalingMagnitude);
        }
#endif

        /************************************************************************************************************************/

        private void Awake()
        {
            _Health.OnHitReceived += (hit) =>
            {
                CameraShake.Instance.Magnitude += _BaseMagnitude + hit.damage * _ScalingMagnitude;
            };
        }

        /************************************************************************************************************************/
    }
}