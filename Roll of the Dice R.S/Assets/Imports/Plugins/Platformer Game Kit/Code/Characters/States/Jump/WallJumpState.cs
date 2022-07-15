// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer;
using Animancer.Units;
using UnityEngine;

namespace PlatformerGameKit.Characters.States
{
    /// <summary>A <see cref="HoldJumpState"/> which allows the character to jump off walls.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/characters/states/jump/wall">Wall Jump</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.Characters.States/WallJumpState
    /// 
    [AddComponentMenu(MenuPrefix + "Wall Jump State")]
    [HelpURL(APIDocumentation + nameof(WallJumpState))]
    public class WallJumpState : HoldJumpState
    {
        /************************************************************************************************************************/

        [SerializeField, Meters]
        [Tooltip("The wall detection range")]
        private float _DetectionDistance = 0.2f;
        public float DetectionDistance => _DetectionDistance;

        [SerializeField, Multiplier]
        [Tooltip("The amount of horizontal force applied (relative to the vertical force)")]
        private float _HorizontalMultiplier = 1;
        public float HorizontalMultiplier => _HorizontalMultiplier;

        /************************************************************************************************************************/

        private Vector2 _WallNormal;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <inheritdoc/>
        protected override void OnValidate()
        {
            base.OnValidate();
            PlatformerUtilities.NotNegative(ref _DetectionDistance);
            PlatformerUtilities.NotNegative(ref _HorizontalMultiplier);
        }
#endif

        /************************************************************************************************************************/

        public override bool CanEnterState
        {
            get
            {
                if (Character.Body.IsGrounded ||
                    _DetectionDistance <= 0)
                    return false;

                // Check in the direction you are facing first.

                var direction = new Vector2(Character.Animancer.FacingX, 0);
                if (CheckForWallJump(direction))
                    return true;

                // If that failed, check the opposite direction.

                if (CheckForWallJump(-direction))
                    return true;

                return false;
            }
        }

        /************************************************************************************************************************/

        private bool CheckForWallJump(Vector2 direction)
        {
            var bounds = Character.Body.Collider.bounds;
            var layers = Character.Body.TerrainFilter.layerMask;
            var count = Physics2D.BoxCastNonAlloc(
                bounds.center, bounds.size, Character.Body.Rotation, direction,
                PlatformerUtilities.OneRaycastHit, _DetectionDistance, layers);

            if (count > 0)
            {
                _WallNormal = -direction;
                return true;
            }
            else return false;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Sets the <see cref="CharacterBody2D.IsGrounded"/> to be <c>true</c> momentarily so that
        /// <see cref="AirJumpState"/> resets its jump counter.
        /// </summary>
        public override void OnEnterState()
        {
            Character.Body.IsGrounded = true;
            base.OnEnterState();
            Character.Body.IsGrounded = false;
            _WallNormal = default;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Prevents <see cref="LandState"/> from being triggered by setting the
        /// <see cref="CharacterBody2D.IsGrounded"/> because at that point the <see cref="JumpState.Animation"/> hasn't
        /// started playing yet.
        /// </summary>
        public override bool CanExitState => Animation.State.IsPlaying;

        /************************************************************************************************************************/

        public override Vector2 CalculateJumpVelocity()
        {
            AnimancerUtilities.Assert(_HorizontalMultiplier == 0 || _WallNormal != default,
                $"{nameof(WallJumpState)} can't calculate the jump velocity without the wall normal." +
                $" This likely means it was forced to enter without checking {nameof(CanEnterState)}");

            if (Character.MovementDirection.x == 0)
                Character.Animancer.Facing = _WallNormal;

            var speed = CalculateJumpSpeed(Height);

            var velocity = Character.Body.Velocity;
            velocity.x = 0;
            velocity.y *= Inertia;
            velocity.y += speed;
            velocity += _WallNormal * (speed * _HorizontalMultiplier);
            return velocity;
        }

        /************************************************************************************************************************/
    }
}