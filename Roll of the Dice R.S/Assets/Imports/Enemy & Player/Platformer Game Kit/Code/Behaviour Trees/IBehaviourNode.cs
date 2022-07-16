// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using Animancer;
using System.Collections.Generic;

namespace PlatformerGameKit.BehaviourTrees
{
    /// <summary>A node in a Behaviour Tree.</summary>
    /// <remarks>
    /// Documentation:
    /// <see href="https://kybernetik.com.au/platformer/docs/characters/brains/behaviour/introduction#core-concept">
    /// Behaviour Tree Brains - Core Concept</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.BehaviourTrees/IBehaviourNode
    /// 
    public interface IBehaviourNode : IPolymorphic
    {
        /************************************************************************************************************************/

        /// <summary>Runs this node's main logic.</summary>
        Result Execute();

        /************************************************************************************************************************/

        /// <summary>The number of children this node has.</summary>
        int ChildCount { get; }

        /// <summary>Gets the child node at the specified `index`.</summary>
        IBehaviourNode GetChild(int index);

        /************************************************************************************************************************/
    }

    /************************************************************************************************************************/

    /// <summary>Utility methods for <see cref="BehaviourTrees"/>.</summary>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.BehaviourTrees/BehaviourTreeUtilities
    /// 
    public static partial class BehaviourTreeUtilities
    {
        /************************************************************************************************************************/

        /// <summary>Returns a list containing the `node` and all of its children (recursively).</summary>
        /// <remarks>The list is retrieved using <see cref="ObjectPool.AcquireList{T}"/>.</remarks>
        public static List<IBehaviourNode> GetChildren(this IBehaviourNode node)
        {
            var list = ObjectPool.AcquireList<IBehaviourNode>();
            node.GetChildren(list);
            return list;
        }

        /************************************************************************************************************************/

        /// <summary>Adds the `node` and all of its children to the `list` (recursively).</summary>
        public static void GetChildren(this IBehaviourNode node, List<IBehaviourNode> list)
        {
            if (node == null)
                return;

            list.Add(node);

            var childCount = node.ChildCount;
            for (int i = 0; i < childCount; i++)
                node.GetChild(i).GetChildren(list);
        }

        /************************************************************************************************************************/
    }
}