// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0282 // There is no defined ordering between fields in multiple declarations of partial struct.

using Animancer;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PlatformerGameKit
{
    /// <summary>The details of a hit.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/combat/hits">Hits</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/Hit
    /// 
    public partial struct Hit
    {
        /************************************************************************************************************************/

        /// <summary>An object that can be <see cref="Hit"/>.</summary>
        public interface ITarget
        {
            /// <summary>Can this object be affected by the current <see cref="Hit"/> details?</summary>
            bool CanBeHit(ref Hit hit);

            /// <summary>Applies the effects of the current <see cref="Hit"/> to this object.</summary>
            void ReceiveHit(ref Hit hit);
        }

        /************************************************************************************************************************/

        /// <summary>The object being hit.</summary>
        public ITarget target;

        /// <summary>Objects that cannot be hit.</summary>
        public HashSet<ITarget> ignore;

        /************************************************************************************************************************/

        /// <inheritdoc/>
        public override string ToString()
        {
            var text = ObjectPool.AcquireStringBuilder()
                .Append($"{nameof(Hit)}({nameof(target)}='").Append(target).Append('\'');

            AppendDetails(text);

            text.Append($", {nameof(ignore)}=");
            if (ignore == null)
            {
                text.Append("null");
            }
            else
            {
                text.Append('[')
                    .Append(ignore.Count)
                    .Append("] {");

                var first = true;
                foreach (var ignore in ignore)
                {
                    if (first)
                        first = false;
                    else
                        text.Append(", ");

                    text.Append(ignore);
                }
                text.Append('}');
            }
            text.Append(')');
            return text.ReleaseToString();
        }

        /// <summary>Appends any additional details of the hit defined in other parts of this <c>partial</c> struct.</summary>
        partial void AppendDetails(StringBuilder text);

        /************************************************************************************************************************/

        /// <summary>Calls <see cref="Component.GetComponentInParent{T}"/>.</summary>
        public static ITarget GetTarget(Component component)
            => component.GetComponentInParent<ITarget>();

        /// <summary>Calls <see cref="GameObject.GetComponentInParent{T}"/>.</summary>
        public static ITarget GetTarget(GameObject gameObject)
            => gameObject.GetComponentInParent<ITarget>();

        /************************************************************************************************************************/

        /// <summary>Can the `component` be hit by the <see cref="Current"/>?</summary>
        public bool CanHit(Component component, bool resultIfNoTarget = false)
        {
            if (component != null)
            {
                var target = GetTarget(component);
                if (target != null)
                    return target.CanBeHit(ref this);
            }

            return resultIfNoTarget;
        }

        /// <summary>Can the `gameObject` be hit by the <see cref="Current"/>?</summary>
        public bool CanHit(GameObject gameObject, bool resultIfNoTarget = false)
        {
            if (gameObject != null)
            {
                var target = GetTarget(gameObject);
                if (target != null)
                    return target.CanBeHit(ref this);
            }

            return resultIfNoTarget;
        }

        /************************************************************************************************************************/

        /// <summary>Calls <see cref="ITarget.ReceiveHit"/> if the `target` can be hit by the <see cref="Current"/>.</summary>
        public bool TryHit(ITarget target, bool dontHitAgain = true)
        {
            if (target == null)
                return false;

            this.target = target;

            if ((ignore != null && ignore.Contains(target)) ||
                !target.CanBeHit(ref this) ||
                target == null)// Could have been changed or set to null by CanBeHit.
                return false;

            if (dontHitAgain)
                ignore?.Add(target);

            target.ReceiveHit(ref this);

            return true;
        }

        /************************************************************************************************************************/

        /// <summary>Calls <see cref="GetTarget(Component)"/> and <see cref="TryHit(ITarget, bool)"/>.</summary>
        public bool TryHitComponent(Component target, bool dontHitAgain = true)
            => TryHit(GetTarget(target), dontHitAgain);

        /************************************************************************************************************************/

        /// <summary>Calls <see cref="TryHitComponent"/> on each of the `targets`.</summary>
        public void TryHitComponents(Component[] targets, bool dontHitAgain = true)
            => TryHitComponents(targets, targets.Length, dontHitAgain);

        /// <summary>Calls <see cref="TryHitComponent"/> on each of the `targets`.</summary>
        public void TryHitComponents(Component[] targets, int count, bool dontHitAgain = true)
        {
            for (int i = 0; i < count; i++)
            {
                var copy = this;
                copy.TryHitComponent(targets[i], dontHitAgain);
            }
        }

        /************************************************************************************************************************/
    }
}