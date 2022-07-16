// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer;
using UnityEngine;

namespace PlatformerGameKit.Characters.States
{
    /// <summary>A <see cref="CharacterState"/> that plays an idle animation.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/characters/states/idle/idle">Idle</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.Characters.States/IdleState
    /// 
    [AddComponentMenu(MenuPrefix + "Idle State")]
    [HelpURL(APIDocumentation + nameof(IdleState))]
    public class IdleState : CharacterState
    {
        /************************************************************************************************************************/

        [SerializeField] private ClipTransition _Animation;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <inheritdoc/>
        protected override void OnValidate()
        {
            base.OnValidate();
            _Animation?.Clip.EditModeSampleAnimation(Character);
        }
#endif

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