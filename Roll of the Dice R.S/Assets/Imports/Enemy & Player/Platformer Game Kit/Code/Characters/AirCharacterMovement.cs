// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using UnityEngine;

namespace PlatformerGameKit.Characters
{
    /// <summary>Moves a flying <see cref="Character"/>.</summary>
    /// <remarks>
    /// Documentation:
    /// <see href="https://kybernetik.com.au/platformer/docs/characters/physics#air-character-movement">
    /// Physics - Air Character Movement</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.Characters/AirCharacterMovement
    /// 
    [AddComponentMenu(Character.MenuPrefix + "Air Character Movement")]
    [HelpURL(Character.APIDocumentation + nameof(AirCharacterMovement))]
    public sealed class AirCharacterMovement : CharacterMovement
    {
        /************************************************************************************************************************/

        [SerializeField, MetersPerSecond] private float _HorizontalSpeed = 8;
        [SerializeField, MetersPerSecond] private float _AscentSpeed = 4;
        [SerializeField, MetersPerSecond] private float _DescentSpeed = 6;
        [SerializeField, Seconds] private float _Smoothing = 0.1f;

        private Vector2 _SmoothingVelocity;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <inheritdoc/>
        protected override void OnValidate()
        {
            base.OnValidate();
            PlatformerUtilities.NotNegative(ref _HorizontalSpeed);
            PlatformerUtilities.NotNegative(ref _AscentSpeed);
            PlatformerUtilities.NotNegative(ref _DescentSpeed);
            PlatformerUtilities.NotNegative(ref _Smoothing);
        }
#endif

        /************************************************************************************************************************/

        protected override Vector2 UpdateVelocity(Vector2 velocity)
        {
            var currentState = Character.StateMachine.CurrentState;
            var brainMovement = Character.MovementDirection;
            var speedMultiplier = currentState.MovementSpeedMultiplier;

            Vector2 targetVelocity;
            if (speedMultiplier == 0)
            {
                targetVelocity = default;
            }
            else
            {
                targetVelocity.x = _HorizontalSpeed;
                targetVelocity.y = brainMovement.y > 0 ? _AscentSpeed : _DescentSpeed;
                targetVelocity = Vector2.Scale(targetVelocity, brainMovement) * speedMultiplier;
            }

            return PlatformerUtilities.SmoothDamp(velocity, targetVelocity, ref _SmoothingVelocity, _Smoothing);
        }

        /************************************************************************************************************************/
    }
}