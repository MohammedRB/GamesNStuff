// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using Animancer;
using System;
using UnityEngine;

namespace PlatformerGameKit.BehaviourTrees
{
    /// <summary>
    /// An <see cref="IBehaviourNode"/> which calls <see cref="IBehaviourNode.Execute"/> on an array of
    /// <see cref="Children"/>.
    /// </summary>
    /// <remarks>
    /// Documentation:
    /// <see href="https://kybernetik.com.au/platformer/docs/characters/brains/behaviour/general#groups">
    /// Behaviour Tree Brains - Groups</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.BehaviourTrees/GroupNode
    /// 
    [Serializable]
    public abstract class GroupNode : IBehaviourNode, IPolymorphicReset
    {
        /************************************************************************************************************************/

        [SerializeReference]
        [Tooltip("The other nodes on which " + nameof(Execute) + " is called to determine the " + nameof(Result) + " of this node")]
        private IBehaviourNode[] _Children;

        /// <summary>
        /// The other nodes on which <see cref="IBehaviourNode.Execute"/> is called to determine the
        /// <see cref="Result"/> of this node.
        /// </summary>
        public ref IBehaviourNode[] Children => ref _Children;

        /************************************************************************************************************************/

        /// <inheritdoc/>
        void IPolymorphicReset.Reset()
        {
            _Children = new IBehaviourNode[2];
        }

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public abstract Result Execute();

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public int ChildCount => _Children.Length;

        /// <inheritdoc/>
        public IBehaviourNode GetChild(int index) => _Children[index];

        /************************************************************************************************************************/
    }
}