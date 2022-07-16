// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace PlatformerGameKit.Characters.States
{
    /// <summary>A <see cref="HoldJumpState"/> which allows the character to jump a limited number of times in the air.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/characters/states/jump/air">Air Jump</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.Characters.States/AirJumpState
    /// 
    [AddComponentMenu(MenuPrefix + "Air Jump State")]
    [HelpURL(APIDocumentation + nameof(AirJumpState))]
    public class AirJumpState : HoldJumpState
    {
        /************************************************************************************************************************/

        [SerializeField]
        [Tooltip("The number of times the character can jump in the air without touching the ground")]
        private int _AirJumps;
        public int AirJumps => _AirJumps;

        /************************************************************************************************************************/

        private int _AirJumpsUsed;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <inheritdoc/>
        protected override void OnValidate()
        {
            base.OnValidate();
            PlatformerUtilities.NotNegative(ref _AirJumps);
        }
#endif

        /************************************************************************************************************************/

        protected override void Awake()
        {
            base.Awake();

            Character.Body.OnGroundedChanged += (isGrounded) =>
            {
                if (isGrounded)
                    _AirJumpsUsed = 0;
            };
        }

        /************************************************************************************************************************/

        public override bool CanEnterState => !Character.Body.IsGrounded && _AirJumpsUsed < _AirJumps;

        /************************************************************************************************************************/

        public override void OnEnterState()
        {
            base.OnEnterState();
            _AirJumpsUsed++;
        }

        /************************************************************************************************************************/
    }
}