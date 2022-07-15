// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer;
using Animancer.FSM;
using PlatformerGameKit.Characters.States;
using UnityEngine;

namespace PlatformerGameKit.Characters
{
    /// <summary>
    /// A centralised group of references to the common parts of a character and a <see cref="StateMachine"/> for their
    /// actions.
    /// </summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/characters/character">Character Component</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.Characters/Character
    /// 
    [AddComponentMenu(MenuPrefix + "Character")]
    [HelpURL(APIDocumentation + nameof(Character))]
    [SelectionBase]
    public class Character : MonoBehaviour
    {
        /************************************************************************************************************************/

        /// <summary>The menu prefix for <see cref="AddComponentMenu"/>.</summary>
        public const string MenuPrefix = Strings.MenuPrefix + "Characters/";

        /// <summary>The URL of the website where the <see cref="Characters"/> API documentation is hosted.</summary>
        public const string APIDocumentation = Strings.APIDocumentation + "." + nameof(Characters) + "/";

        /************************************************************************************************************************/

        [SerializeField]
        private CharacterAnimancerComponent _Animancer;
        public CharacterAnimancerComponent Animancer => _Animancer;

        [SerializeField]
        private CharacterBody2D _Body;
        public CharacterBody2D Body => _Body;

        [SerializeField]
        private Health _Health;
        public Health Health => _Health;

        [SerializeField]
        private CharacterState _Idle;
        public CharacterState Idle => _Idle;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only] Ensures that all fields have valid values and finds missing components nearby.</summary>
        protected virtual void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Animancer);
            gameObject.GetComponentInParentOrChildren(ref _Body);
            gameObject.GetComponentInParentOrChildren(ref _Health);
            gameObject.GetComponentInParentOrChildren(ref _Idle);
        }
#endif

        /************************************************************************************************************************/

        private Vector2 _MovementDirection;

        /// <summary>The direction this character wants to move.</summary>
        public Vector2 MovementDirection
        {
            get => _MovementDirection;
            set
            {
                _MovementDirection.x = Mathf.Clamp(value.x, -1, 1);
                _MovementDirection.y = Mathf.Clamp(value.y, -1, 1);
            }
        }

        /// <summary>The horizontal direction this character wants to move.</summary>
        public float MovementDirectionX
        {
            get => _MovementDirection.x;
            set => _MovementDirection.x = Mathf.Clamp(value, -1, 1);
        }

        /// <summary>The vertical direction this character wants to move.</summary>
        public float MovementDirectionY
        {
            get => _MovementDirection.y;
            set => _MovementDirection.y = Mathf.Clamp(value, -1, 1);
        }

        /************************************************************************************************************************/

        /// <summary>Does this character currently want to run?</summary>
        public bool Run { get; set; }

        /************************************************************************************************************************/

        /// <summary>The Finite State Machine that manages the actions of this character.</summary>
        public readonly StateMachine<CharacterState>.WithDefault
            StateMachine = new StateMachine<CharacterState>.WithDefault();

        /************************************************************************************************************************/

        /// <summary>Initializes this character.</summary>
        protected virtual void Awake()
        {
            StateMachine.DefaultState = _Idle;

#if UNITY_ASSERTIONS
            // Add your name to all children to make debugging easier.
            foreach (Transform child in transform)
                child.name = $"{child.name} ({name})";
#endif
        }

        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>[Editor-Only] Displays some non-serialized details at the bottom of the Inspector.</summary>
        /// <example>
        /// <see cref="https://kybernetik.com.au/inspector-gadgets/pro">Inspector Gadgets Pro</see> would allow this to
        /// be implemented much easier by simply placing
        /// <see cref="https://kybernetik.com.au/inspector-gadgets/docs/script-inspector/inspectable-attributes">
        /// Inspectable Attributes</see> on the fields and properties we want to display like so:
        /// <para></para><code>
        /// [Inspectable]
        /// public Vector2 MovementDirection ...
        /// 
        /// [Inspectable]
        /// public bool Run { get; set; }
        /// 
        /// [Inspectable]
        /// public CharacterState CurrentState
        /// {
        ///     get => StateMachine.CurrentState;
        ///     set => StateMachine.TrySetState(value);
        /// }
        /// </code>
        /// </example>
        [UnityEditor.CustomEditor(typeof(Character), true)]
        public class Editor : UnityEditor.Editor
        {
            /************************************************************************************************************************/

            /// <inheritdoc/>
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (!UnityEditor.EditorApplication.isPlaying)
                    return;

                var target = (Character)this.target;

                target.MovementDirection = UnityEditor.EditorGUILayout.Vector2Field("Movement Direction", target.MovementDirection);

                target.Run = UnityEditor.EditorGUILayout.Toggle("Run", target.Run);

                UnityEditor.EditorGUI.BeginChangeCheck();
                var state = UnityEditor.EditorGUILayout.ObjectField(
                    "Current State", target.StateMachine.CurrentState, typeof(CharacterState), true);
                if (UnityEditor.EditorGUI.EndChangeCheck())
                    target.StateMachine.TrySetState((CharacterState)state);
            }

            /************************************************************************************************************************/
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}