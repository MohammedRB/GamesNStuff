// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using System;

namespace PlatformerGameKit.BehaviourTrees
{
    /// <summary>
    /// An <see cref="IBehaviourNode"/> that checks a boolean <see cref="Condition"/> without an option for
    /// <see cref="Result.Pending"/>.
    /// </summary>
    /// <remarks>
    /// Documentation:
    /// <see href="https://kybernetik.com.au/platformer/docs/characters/brains/behaviour/specific#conditions">
    /// Behaviour Tree Brains - Conditions</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.BehaviourTrees/ConditionNode
    /// 
    [Serializable]
    public abstract class ConditionNode : IBehaviourNode
    {
        /************************************************************************************************************************/

        /// <summary>Accesses the <see cref="Condition"/> and calls <see cref="BehaviourTreeUtilities.ToResult"/>.</summary>
        public Result Execute() => Condition.ToResult();

        /// <summary>
        /// Called by <see cref="Execute"/> to run this node's main logic. <code>true</code> returns
        /// <see cref="Result.Pass"/> and <code>false</code> returns <see cref="Result.Fail"/>.
        /// </summary>
        public abstract bool Condition { get; }

        /************************************************************************************************************************/

        /// <inheritdoc/>
        int IBehaviourNode.ChildCount => 0;

        /// <inheritdoc/>
        IBehaviourNode IBehaviourNode.GetChild(int index)
            => throw new NotSupportedException($"A {nameof(ConditionNode)} doesn't have any children.");

        /************************************************************************************************************************/
    }
}