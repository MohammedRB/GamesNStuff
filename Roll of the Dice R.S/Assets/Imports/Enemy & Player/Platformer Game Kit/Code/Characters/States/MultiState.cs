// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer;
using UnityEngine;

namespace PlatformerGameKit.Characters.States
{
    /// <summary>A <see cref="CharacterState"/> which redirects to other states.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/characters/states/multi-state">Multi-State</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.Characters.States/MultiState
    /// 
    [AddComponentMenu(MenuPrefix + "Multi State")]
    [HelpURL(APIDocumentation + nameof(MultiState))]
    [DefaultExecutionOrder(DefaultExecutionOrder)]
    public class MultiState : CharacterState
    {
        /************************************************************************************************************************/

        /// <summary>Run before other states in case <see cref="FixedUpdate"/> changes the state.</summary>
        public const int DefaultExecutionOrder = -1000;

        /************************************************************************************************************************/

        [SerializeField]
        [Tooltip("While in one of the States, should it try to enter them again in order every " + nameof(FixedUpdate) + "?")]
        private bool _AutoInternalTransitions;
        public bool AutoInternalTransitions => _AutoInternalTransitions;

        [SerializeField]
        [Tooltip("The other states that this one will try to enter in order")]
        private CharacterState[] _States;
        public CharacterState[] States => _States;

        private CharacterState _CurrentState;

        /************************************************************************************************************************/

        public override bool CanEnterState => Character.StateMachine.CanSetState(_States);

        public override bool CanExitState => true;

        /************************************************************************************************************************/

        public override void OnEnterState()
        {
            if (Character.StateMachine.TrySetState(_States))
            {
                if (_AutoInternalTransitions)
                {
                    _CurrentState = Character.StateMachine.CurrentState;
                    enabled = true;
                }
            }
            else
            {
                var text = ObjectPool.AcquireStringBuilder()
                    .AppendLine($"{nameof(MultiState)} failed to enter any of its {nameof(States)}:");

                for (int i = 0; i < _States.Length; i++)
                {
                    text.Append("    [")
                        .Append(i)
                        .Append("] ")
                        .AppendLine(_States[i].ToString());
                }

                Debug.LogError(text.ReleaseToString(), this);
            }
        }

        public override void OnExitState() { }

        /************************************************************************************************************************/

        protected virtual void FixedUpdate()
        {
            if (_CurrentState != Character.StateMachine.CurrentState)
            {
                enabled = false;
                return;
            }

            var newState = Character.StateMachine.CanSetState(_States);
            if (_CurrentState != newState && newState != null)
            {
                _CurrentState = newState;
                Character.StateMachine.ForceSetState(newState);
            }
        }

        /************************************************************************************************************************/
    }
}