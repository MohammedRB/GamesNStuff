// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using PlatformerGameKit.Characters;
using PlatformerGameKit.Characters.States;
using System;
using UnityEngine;

namespace PlatformerGameKit.BehaviourTrees
{
    /// <summary>
    /// A <see cref="LeafNode"/> which calls <see cref="Animancer.FSM.StateMachine{TState}.TrySetState(TState)"/>.
    /// </summary>
    /// <remarks>
    /// Documentation:
    /// <see href="https://kybernetik.com.au/platformer/docs/characters/brains/behaviour/specific#leaves">
    /// Behaviour Tree Brains - Leaves</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.BehaviourTrees/TrySetState
    /// 
    [Serializable]
    public sealed class TrySetState : LeafNode
    {
        /************************************************************************************************************************/

        [SerializeField]
        [Tooltip("The state for the character to attempt to enter")]
        private CharacterState _State;

        /// <summary>The state for the character to attempt to enter.</summary>
        public ref CharacterState State => ref _State;

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override Result Execute()
        {
            var character = Context<Character>.Current;
            return character.StateMachine.TrySetState(_State).ToResult();
        }

        /************************************************************************************************************************/
    }
}