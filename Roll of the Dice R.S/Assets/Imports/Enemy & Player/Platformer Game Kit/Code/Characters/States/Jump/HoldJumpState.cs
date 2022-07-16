// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer.Units;
using UnityEngine;
using static Animancer.Validate;

namespace PlatformerGameKit.Characters.States
{
    /// <summary>A <see cref="JumpState"/> which allows you to hold the button down to jump higher.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/characters/states/jump/hold">Hold Jump</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.Characters.States/HoldJumpState
    /// 
    [AddComponentMenu(MenuPrefix + "Hold Jump State")]
    [HelpURL(APIDocumentation + nameof(HoldJumpState))]
    public class HoldJumpState : JumpState
    {
        /************************************************************************************************************************/

        [SerializeField, MetersPerSecondPerSecond(Rule = Value.IsNotNegative)]
        [Tooltip("The continuous acceleration applied while holding the jump button")]
        private float _HoldAcceleration = 40;
        public float HoldAcceleration => _HoldAcceleration;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <inheritdoc/>
        protected override void OnValidate()
        {
            base.OnValidate();
            PlatformerUtilities.NotNegative(ref _HoldAcceleration);
        }
#endif

        /************************************************************************************************************************/

        protected virtual void FixedUpdate()
        {
            Character.Body.Velocity += new Vector2(0, _HoldAcceleration * Time.deltaTime);
        }

        /************************************************************************************************************************/
    }
}