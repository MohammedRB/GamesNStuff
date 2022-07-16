// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using System;
using UnityEngine;

namespace PlatformerGameKit.BehaviourTrees
{
    /// <summary>
    /// An <see cref="IBehaviourNode"/> which calls <see cref="IBehaviourNode.Execute"/> on a <see cref="Child"/> node
    /// and modifies its <see cref="Result"/>.
    /// </summary>
    /// <remarks>
    /// Documentation:
    /// <see href="https://kybernetik.com.au/platformer/docs/characters/brains/behaviour/general#modifiers">
    /// Behaviour Tree Brains - Modifiers</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.BehaviourTrees/ModifierNode
    /// 
    [Serializable]
    public abstract class ModifierNode : IBehaviourNode
    {
        /************************************************************************************************************************/

        [SerializeReference]
        [Tooltip("The other node on which " + nameof(Execute) + " is called to determine the " + nameof(Result) + " of this node")]
        private IBehaviourNode _Child;

        /// <summary>
        /// The other node on which <see cref="IBehaviourNode.Execute"/> is called to determine the
        /// <see cref="Result"/> of this node.
        /// </summary>
        public ref IBehaviourNode Child => ref _Child;

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public virtual Result Execute()
        {
            if (_Child != null)
                return _Child.Execute();
            else
                return Result.Fail;
        }

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public int ChildCount => 1;

        /// <inheritdoc/>
        public IBehaviourNode GetChild(int index) => _Child;

        /************************************************************************************************************************/
    }
}