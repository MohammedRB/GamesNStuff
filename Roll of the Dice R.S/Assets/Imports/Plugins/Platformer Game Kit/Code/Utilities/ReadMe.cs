// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#if UNITY_EDITOR

using UnityEngine;

namespace PlatformerGameKit
{
    /// <summary>[Editor-Only] A welcome screen for the <see cref="PlatformerGameKit"/>.</summary>
    // [CreateAssetMenu(menuName = Strings.MenuPrefix + "Read Me", order = Strings.AssetMenuOrder)]
    [HelpURL(Strings.APIDocumentation + "/" + nameof(ReadMe))]
    public class ReadMe : Animancer.Editor.ReadMe
    {
        /************************************************************************************************************************/

        /// <summary>The release ID of the current version.</summary>
        /// <example><list type="bullet">
        /// <item>[1] = v1.0: 2021-07-??.</item>
        /// </list></example>
        protected override int ReleaseNumber => 1;

        /// <inheritdoc/>
        protected override string ReleaseNumberPrefKey => nameof(PlatformerGameKit) + "." + nameof(ReleaseNumber);

        /// <inheritdoc/>
        protected override string ProductName => Strings.ProductName;

        /// <inheritdoc/>
        protected override string VersionName => "v1.0";

        /// <inheritdoc/>
        protected override string DocumentationURL => Strings.Documentation;

        /// <inheritdoc/>
        protected override string ChangeLogURL => Strings.ChangeLogPrefix;// + "/v1-0";

        /// <inheritdoc/>
        protected override string ExampleURL => Strings.Examples;

        /// <inheritdoc/>
        protected override string ForumURL => Strings.Forum;

        /// <inheritdoc/>
        protected override string IssuesURL => Strings.Issues;

        /// <inheritdoc/>
        protected override string DeveloperEmail => Strings.DeveloperEmail;

        /************************************************************************************************************************/
    }
}

#endif