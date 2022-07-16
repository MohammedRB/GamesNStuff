// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using System;

namespace PlatformerGameKit.BehaviourTrees
{
    /// <summary>
    /// A <see cref="ModifierNode"/> which executes its <see cref="ModifierNode.Child"/> and returns the opposite of
    /// its <see cref="Result"/> (using <see cref="BehaviourTreeUtilities.Invert"/>).
    /// </summary>
    /// <remarks>
    /// Documentation:
    /// <see href="https://kybernetik.com.au/platformer/docs/characters/brains/behaviour/general#modifiers">
    /// Behaviour Tree Brains - Modifiers</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.BehaviourTrees/Invert
    /// 
    [Serializable]
    public sealed class Invert : ModifierNode
    {
        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override Result Execute()
            => Child != null ? Child.Execute().Invert() : Result.Pass;

        /************************************************************************************************************************/
    }
}