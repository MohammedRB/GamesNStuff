// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer;
using UnityEngine;

namespace PlatformerGameKit.Characters.Brains
{
    /// <summary>Base class for components that decide what a <see cref="Characters.Character"/> wants to do.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/characters/brains">Brains</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.Characters.Brains/CharacterBrain
    /// 
    [AddComponentMenu(MenuPrefix + "Character Brain")]
    [HelpURL(APIDocumentation + nameof(CharacterBrain))]
    [DefaultExecutionOrder(DefaultExecutionOrder)]
    public abstract class CharacterBrain : MonoBehaviour
    {
        /************************************************************************************************************************/

        /// <summary>The menu prefix for <see cref="AddComponentMenu"/>.</summary>
        public const string MenuPrefix = Character.MenuPrefix + "Brains/";

        /// <summary>The URL of the website where the <see cref="Brains"/> API documentation is hosted.</summary>
        public const string APIDocumentation = Strings.APIDocumentation + "." + nameof(Characters) + "." + nameof(Brains) + "/";

        /// <summary>Run inputs before everything else.</summary>
        public const int DefaultExecutionOrder = -10000;

        /************************************************************************************************************************/

        [SerializeField]
        private Character _Character;

        /// <summary>The <see cref="Characters.Character"/> this brain is controlling.</summary>
        public ref Character Character => ref _Character;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only] Ensures that all fields have valid values and finds missing components nearby.</summary>
        protected virtual void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Character);
        }
#endif

        /************************************************************************************************************************/
    }
}