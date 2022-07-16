// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using System;

namespace PlatformerGameKit.BehaviourTrees
{
    /// <summary>An <see cref="IBehaviourNode"/> which executes some logic.</summary>
    /// <remarks>
    /// Documentation:
    /// <see href="https://kybernetik.com.au/platformer/docs/characters/brains/behaviour/general#leaves">
    /// Behaviour Tree Brains - Leaves</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.BehaviourTrees/LeafNode
    /// 
    [Serializable]
    public abstract class LeafNode : IBehaviourNode
    {
        /************************************************************************************************************************/

        /// <inheritdoc/>
        public abstract Result Execute();

        /************************************************************************************************************************/

        /// <inheritdoc/>
        int IBehaviourNode.ChildCount => 0;

        /// <inheritdoc/>
        IBehaviourNode IBehaviourNode.GetChild(int index)
            => throw new NotSupportedException($"A {nameof(LeafNode)} doesn't have any children.");

        /************************************************************************************************************************/
    }
}