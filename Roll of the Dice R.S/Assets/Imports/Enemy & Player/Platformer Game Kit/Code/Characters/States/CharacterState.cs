// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer;
using Animancer.FSM;
using UnityEngine;

namespace PlatformerGameKit.Characters.States
{
    /// <summary>
    /// Base class for the various states a <see cref="Platformer.Character"/> can be in and actions they can perform.
    /// </summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/characters/states">States</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.Characters.States/CharacterState
    /// 
    [HelpURL(APIDocumentation + nameof(CharacterState))]
    public abstract class CharacterState : StateBehaviour, IOwnedState<CharacterState>
    {
        /************************************************************************************************************************/

        /// <summary>The menu prefix for <see cref="AddComponentMenu"/>.</summary>
        public const string MenuPrefix = Character.MenuPrefix + "States/";

        /// <summary>The URL of the website where the <see cref="States"/> API documentation is hosted.</summary>
        public const string APIDocumentation = Strings.APIDocumentation + "." + nameof(Characters) + "." + nameof(States) + "/";

        /************************************************************************************************************************/

        [SerializeField]
        private Character _Character;

        /// <summary>The <see cref="Examples.PlatformerCharacter"/> that owns this state.</summary>
        public Character Character => _Character;

        /// <summary>Sets the <see cref="Character"/>.</summary>
        /// <remarks>
        /// This is not a property setter because you shouldn't be casually changing the owner of a state. Usually this
        /// would only be used when adding a state to a character using a script.
        /// </remarks>
        public void SetCharacter(Character character) => _Character = character;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only] Ensures that all fields have valid values and finds missing components nearby.</summary>
        protected override void OnValidate()
        {
            base.OnValidate();
            gameObject.GetComponentInParentOrChildren(ref _Character);
        }
#endif

        /************************************************************************************************************************/

        public virtual float MovementSpeedMultiplier => 0;

        public virtual bool CanTurn => MovementSpeedMultiplier != 0;

        /************************************************************************************************************************/

        public StateMachine<CharacterState> OwnerStateMachine => _Character.StateMachine;

        /************************************************************************************************************************/
    }
}