// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using System;
using UnityEngine;

namespace PlatformerGameKit
{
    /// <summary>A method called when a value is changed.</summary>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/ValueChangeEvent_1
    /// 
    public delegate void ValueChangeEvent<T>(T oldValue, T newValue);

    /// <summary>Keeps track of the health of an object.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/combat/hits">Hits</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/Health
    /// 
    [AddComponentMenu(Strings.MenuPrefix + "Health")]
    [HelpURL(Strings.APIDocumentation + "/" + nameof(Health))]
    [DefaultExecutionOrder(DefaultExecutionOrder)]
    public sealed class Health : MonoBehaviour, ITeam, Hit.ITarget, ISerializationCallbackReceiver
    {
        /************************************************************************************************************************/

        /// <summary>Initialize the <see cref="CurrentHealth"/> earlier than anything else will use it.</summary>
        public const int DefaultExecutionOrder = -5000;

        /************************************************************************************************************************/

        [SerializeField]
        private Team _Team;

        /// <summary>[<see cref="ITeam"/>] The <see cref="Platformer.Team"/> this object is on.</summary>
        public Team Team => _Team;

        /************************************************************************************************************************/

        [SerializeField]
        private int _MaximumHealth;
        public int MaximumHealth
        {
            get => _MaximumHealth;
            private set
            {
                var oldValue = _MaximumHealth;
                _MaximumHealth = value;
                OnMaximumHealthChanged?.Invoke(oldValue, value);
            }
        }

        public event ValueChangeEvent<int> OnMaximumHealthChanged;

        /************************************************************************************************************************/

        /// <summary>Determines how <see cref="SetMaximumHealth"/> affects the <see cref="CurrentHealth"/>.</summary>
        public enum HealthChangeMode
        {
            /// <summary>
            /// Get the <see cref="CurrentHealth"/> as a percentage of the <see cref="MaximumHealth"/> before the
            /// change then set it to that same percentage afterwards.
            /// </summary>
            /// <example><code>
            /// Health is 50 / 100 (Current is 50% of Maximum)
            /// Set Maximum to 120
            /// Health is 60 / 120 (Current is 50% of Maximum)
            /// </code></example>
            Scale,

            /// <summary>Add the change directly to the <see cref="CurrentHealth"/>.</summary>
            /// <example><code>
            /// Health is 50 / 100
            /// Set Maximum to 120
            /// Health is 70 / 120 (Maximum was increased by 20 so Current also increased by 20)
            /// </code></example>
            Offset,

            /// <summary>
            /// Do nothing to the <see cref="CurrentHealth"/> except clamping it to stay below the
            /// <see cref="MaximumHealth"/> if the change is a decrease.</summary>
            /// <example><code>
            /// Health is 50 / 100
            /// Set Maximum to 120
            /// Health is 50 / 120 (Current was not modified)
            /// </code></example>
            Ignore,
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Sets the <see cref="MaximumHealth"/> and then adjusts the <see cref="CurrentHealth"/> based on the `mode`.
        /// </summary>
        public void SetMaximumHealth(int value, HealthChangeMode mode)
        {
            if (_MaximumHealth == value)
                return;

            switch (mode)
            {
                case HealthChangeMode.Scale:
                    var percentage = _CurrentHealth / _MaximumHealth;
                    MaximumHealth = value;
                    CurrentHealth = value * percentage;
                    break;

                case HealthChangeMode.Offset:
                    var offset = value - _MaximumHealth;
                    MaximumHealth = value;
                    CurrentHealth += offset;
                    break;

                case HealthChangeMode.Ignore:
                    MaximumHealth = value;
                    CurrentHealth = _CurrentHealth;
                    break;

                default:
                    throw new ArgumentException($"Unsupported {nameof(HealthChangeMode)}", nameof(mode));
            }
        }

        /************************************************************************************************************************/

        private int _CurrentHealth;
        public int CurrentHealth
        {
            get => _CurrentHealth;
            set
            {
                var oldValue = _CurrentHealth;
                _CurrentHealth = Mathf.Clamp(value, 0, _MaximumHealth);
                if (_CurrentHealth != oldValue)
                {
                    if (OnCurrentHealthChanged != null)
                        OnCurrentHealthChanged(oldValue, _CurrentHealth);
                    // If an event was registered, we expect it to handle death.
                    // Otherwise just destroy yourself at 0 health.
                    else if (_CurrentHealth <= 0)
                        Destroy(gameObject);
                }
            }
        }

        public event ValueChangeEvent<int> OnCurrentHealthChanged;

        /************************************************************************************************************************/

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _MaximumHealth = Math.Max(1, _MaximumHealth);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            CurrentHealth = _MaximumHealth;
        }

        /************************************************************************************************************************/

#if UNITY_ASSERTIONS
        private void Awake()
        {
            Debug.Assert(_MaximumHealth > 0, $"{nameof(MaximumHealth)} isn't positive.", this);
        }
#endif

        /************************************************************************************************************************/

        bool Hit.ITarget.CanBeHit(ref Hit hit) =>
            _CurrentHealth > 0 &&
            _Team.IsEnemy(hit.team);

        void Hit.ITarget.ReceiveHit(ref Hit hit)
        {
            CurrentHealth -= hit.damage;
            OnHitReceived?.Invoke(hit);
        }

        public event Action<Hit> OnHitReceived;

        /************************************************************************************************************************/
    }
}