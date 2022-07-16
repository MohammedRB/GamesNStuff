// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer;
using PlatformerGameKit.BehaviourTrees;
using UnityEngine;

namespace PlatformerGameKit.Characters.Brains
{
    /// <summary>A <see cref="CharacterBrain"/> that uses a behaviour tree.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/characters/brains/behaviour">Behaviour Tree Brain</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.Characters.Brains/BehaviourTreeBrain
    /// 
    [AddComponentMenu(MenuPrefix + "Behaviour Tree Brain")]
    [HelpURL(APIDocumentation + nameof(BehaviourTreeBrain))]
    public class BehaviourTreeBrain : CharacterBrain
    {
        /************************************************************************************************************************/

        [SerializeReference] private IBehaviourNode _OnAwake;
        [SerializeReference] private IBehaviourNode _OnFixedUpdate;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only] Initializes default values for the fields.</summary>
        protected virtual void Reset()
        {
            _OnAwake = new SetMovementForward();
            _OnFixedUpdate = new Selector();
            ((IPolymorphicReset)_OnFixedUpdate).Reset();
        }
#endif

        /************************************************************************************************************************/

        /// <summary>Executes the <see cref="_OnAwake"/> behaviour tree.</summary>
        protected virtual void Awake()
        {
            using (new Context<Character>(Character))
                _OnAwake?.Execute();
        }

        /************************************************************************************************************************/

        /// <summary>Executes the <see cref="_OnFixedUpdate"/> behaviour tree.</summary>
        protected virtual void FixedUpdate()
        {
            using (new Context<Character>(Character))
                _OnFixedUpdate?.Execute();
        }

        /************************************************************************************************************************/
    }
}