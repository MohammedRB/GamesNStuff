// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using PlatformerGameKit.Characters;
using System;

namespace PlatformerGameKit.BehaviourTrees
{
    /// <summary>A <see cref="LeafNode"/> which inverts the <see cref="Character.MovementDirectionX"/>.</summary>
    /// <remarks>
    /// Documentation:
    /// <see href="https://kybernetik.com.au/platformer/docs/characters/brains/behaviour/specific#leaves">
    /// Behaviour Tree Brains - Leaves</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.BehaviourTrees/TurnAround
    /// 
    [Serializable]
    public sealed class TurnAround : LeafNode
    {
        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override Result Execute()
        {
            var character = Context<Character>.Current;
            character.MovementDirectionX = -character.MovementDirectionX;
            return Result.Pass;
        }

        /************************************************************************************************************************/
    }
}