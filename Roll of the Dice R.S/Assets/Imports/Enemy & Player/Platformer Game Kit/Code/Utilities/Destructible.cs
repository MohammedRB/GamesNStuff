// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace PlatformerGameKit
{
    /// <summary>A <see cref="Hit.ITarget"/> which destroys the <see cref="GameObject"/> when hit.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/combat/hits">Hits</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/Destructible
    /// 
    [AddComponentMenu(Strings.MenuPrefix + "Destructible")]
    [HelpURL(Strings.APIDocumentation + "/" + nameof(Destructible))]
    public sealed class Destructible : MonoBehaviour, Hit.ITarget
    {
        /************************************************************************************************************************/

        /// <inheritdoc/>
        public bool CanBeHit(ref Hit hit) => true;

        /// <inheritdoc/>
        public void ReceiveHit(ref Hit hit) => Destroy(gameObject);

        /************************************************************************************************************************/
    }
}