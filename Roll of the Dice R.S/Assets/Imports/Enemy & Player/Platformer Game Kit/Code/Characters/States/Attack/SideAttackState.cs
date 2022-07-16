// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace PlatformerGameKit.Characters.States
{
    /// <summary>An <see cref="AttackState"/> with separate animations for left and right.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/characters/states/attack/side">Side Attack</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.Characters.States/SideAttackState
    /// 
    [AddComponentMenu(MenuPrefix + "Side Attack State")]
    [HelpURL(APIDocumentation + nameof(SideAttackState))]
    public sealed class SideAttackState : AttackState
    {
        /************************************************************************************************************************/

        [SerializeField] private AttackTransition _LeftAnimation;
        [SerializeField] private AttackTransition _RightAnimation;

        /************************************************************************************************************************/

        private void Awake()
        {
            _LeftAnimation.Events.OnEnd += Character.StateMachine.ForceSetDefaultState;
            _RightAnimation.Events.OnEnd += Character.StateMachine.ForceSetDefaultState;
        }

        /************************************************************************************************************************/

        public override void OnEnterState()
        {
            base.OnEnterState();
            var animation = Character.MovementDirectionX < 0 ? _LeftAnimation : _RightAnimation;
            Character.Animancer.Play(animation);
        }

        /************************************************************************************************************************/
    }
}