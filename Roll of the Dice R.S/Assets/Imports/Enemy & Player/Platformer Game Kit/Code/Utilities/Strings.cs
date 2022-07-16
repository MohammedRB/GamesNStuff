// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member.

using UnityEngine;

namespace PlatformerGameKit
{
    /// <summary>Various string constants used throughout the <see cref="PlatformerGameKit"/>.</summary>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/Strings
    /// 
    public static class Strings
    {
        /************************************************************************************************************************/

        /// <summary>The name of this product.</summary>
        public const string ProductName = "Platformer Game Kit";

        /// <summary>The standard prefix for <see cref="AddComponentMenu"/>.</summary>
        public const string MenuPrefix = ProductName + "/";

        /// <summary>The URL of the website where the Platformer Game Kit documentation is hosted.</summary>
        public const string Documentation = "https://kybernetik.com.au/platformer";

        /// <summary>The URL of the website where the Platformer Game Kit API documentation is hosted.</summary>
        public const string APIDocumentation = Documentation + "/api/" + nameof(PlatformerGameKit);

        public const string Docs = Documentation + "/docs/";

        /// <summary>The URL of the website where the Platformer Game Kit example documentation is hosted.</summary>
        public const string Examples = Docs + "scenes";

        public const string ChangeLogPrefix = Docs + "changes";

        public const string Forum = "https://forum.unity.com/threads/1148402";

        public const string Issues = "https://github.com/KybernetikGames/platformer/issues";

        /// <summary>The email address which handles support for the Platformer Game Kit.</summary>
        public const string DeveloperEmail = "mail@kybernetik.com.au";

#if UNITY_EDITOR
        /// <summary>[Editor-Only] A common [<see cref="TooltipAttribute.tooltip"/>] for Debug Line Duration fields.</summary>
        public const string DebugLineDurationTooltip =
            "[Editor-Only] Determines how long scene view debug lines are shown for this object.";
#endif

        /************************************************************************************************************************/
    }
}