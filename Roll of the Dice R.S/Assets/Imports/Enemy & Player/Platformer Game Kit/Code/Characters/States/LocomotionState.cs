// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer;
using UnityEngine;

namespace PlatformerGameKit.Characters.States
{
    /// <summary>A <see cref="CharacterState"/> that plays an idle animation.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/characters/states/idle/locomotion">Locomotion</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.Characters.States/LocomotionState
    /// 
    [AddComponentMenu(MenuPrefix + "Locomotion State")]
    [HelpURL(APIDocumentation + nameof(LocomotionState))]
    public class LocomotionState : CharacterState
    {
        /************************************************************************************************************************/

        [SerializeField]
        private ClipTransition _Idle;
        public ClipTransition Idle => _Idle;

        [SerializeField]
        private ClipTransition _Walk;
        public ClipTransition Walk => _Walk;

        [SerializeField]
        private ClipTransition _Run;
        public ClipTransition Run => _Run;

        [SerializeField]
        private ClipTransition _Fall;
        public ClipTransition Fall => _Fall;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <inheritdoc/>
        protected override void OnValidate()
        {
            base.OnValidate();
            _Idle?.Clip.EditModeSampleAnimation(Character);
        }
#endif

        /************************************************************************************************************************/

        public ClipTransition CurrentAnimation
        {
            get
            {
                // If airbourne -> Fall.
                if (!Character.Body.IsGrounded && _Fall.IsValid)
                    return _Fall;

                // If trying to move -> Run or Walk.
                if (Character.MovementDirection.x != 0)
                    return Character.Run && _Run.IsValid ? _Run : _Walk;

                // Otherwise Idle.
                return _Idle;
            }
        }

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override void OnEnterState()
        {
            base.OnEnterState();
            Character.Animancer.Play(CurrentAnimation);
        }

        /************************************************************************************************************************/

        protected virtual void Update()
        {
            Character.Animancer.Play(CurrentAnimation);
        }

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override float MovementSpeedMultiplier => 1;

        /************************************************************************************************************************/
    }
}