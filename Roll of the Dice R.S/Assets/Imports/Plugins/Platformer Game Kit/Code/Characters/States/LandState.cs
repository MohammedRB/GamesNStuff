// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer;
using Animancer.Units;
using UnityEngine;

namespace PlatformerGameKit.Characters.States
{
    /// <summary>A <see cref="CharacterState"/> that plays a landing animation.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/characters/states/idle/land">Land</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.Characters.States/LandState
    /// 
    [AddComponentMenu(MenuPrefix + "Land State")]
    [HelpURL(APIDocumentation + nameof(LandState))]
    public class LandState : CharacterState
    {
        /************************************************************************************************************************/

        [SerializeField]
        private ClipTransition _Animation;

        [SerializeField, MetersPerSecond]
        [Tooltip("This state will only activate if the character is moving at least this fast downwards when they land")]
        private float _RequiredDownSpeed = 7;

        [SerializeField, Range(0, 1)]
        [Tooltip("The character's speed is multiplied by this value while in this state")]
        private float _MovementSpeedMultiplier = 1;

        /// <inheritdoc/>
        public override float MovementSpeedMultiplier => _MovementSpeedMultiplier;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <inheritdoc/>
        protected override void OnValidate()
        {
            base.OnValidate();
            PlatformerUtilities.NotNegative(ref _RequiredDownSpeed);
            PlatformerUtilities.Clamp(ref _MovementSpeedMultiplier, 0, 1);
        }
#endif

        /************************************************************************************************************************/

        private void Awake()
        {
            _Animation.Events.OnEnd += Character.StateMachine.ForceSetDefaultState;

            Character.Body.OnGroundedChanged += OnGroundedChanged;
        }

        /************************************************************************************************************************/

        private void OnGroundedChanged(bool isGrounded)
        {
            if (isGrounded &&
                Context<ContactPoint2D>.Current.relativeVelocity.y >= _RequiredDownSpeed)
                Character.StateMachine.TrySetState(this);
        }

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override void OnEnterState()
        {
            base.OnEnterState();
            Character.Animancer.Play(_Animation);
        }

        /************************************************************************************************************************/
    }
}