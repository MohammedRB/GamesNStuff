// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerGameKit.Characters
{
    /// <summary>An <see cref="AnimancerComponent"/> which also manages a <see cref="SpriteRenderer"/> and hit boxes.</summary>
    /// <remarks>
    /// Documentation: <see href="https://kybernetik.com.au/platformer/docs/characters/animancer">Animancer Component</see>
    /// </remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit.Characters/CharacterAnimancerComponent
    /// 
    [AddComponentMenu(Character.MenuPrefix + "Character Animancer Component")]
    [HelpURL(Character.APIDocumentation + nameof(CharacterAnimancerComponent))]
    public sealed class CharacterAnimancerComponent : AnimancerComponent
    {
        /************************************************************************************************************************/

        [SerializeField]
        private SpriteRenderer _Renderer;
        public SpriteRenderer Renderer => _Renderer;

        [SerializeField]
        private Character _Character;
        public Character Character => _Character;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only] Ensures that all fields have valid values and finds missing components nearby.</summary>
        private void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Renderer);
            gameObject.GetComponentInParentOrChildren(ref _Character);
        }
#endif

        /************************************************************************************************************************/

#if UNITY_ASSERTIONS
        /// <summary>[Assert-Only]
        /// Ensures that fading isn't used on this character since it doesn't work for <see cref="Sprite"/> animations.
        /// </summary>
        private void Awake()
        {
            DontAllowFade.Assert(this);
        }
#endif

        /************************************************************************************************************************/

        /// <summary>Is the <see cref="Renderer"/> facing to the left?</summary>
        public bool FacingLeft
        {
            get => _Renderer.flipX;
            set => _Renderer.flipX = value;
        }

        /// <summary>The horizontal direction the <see cref="Renderer"/> is facing.</summary>
        /// <remarks><c>1</c> for the right or <c>-1</c> for the left.</remarks>
        public float FacingX
        {
            get => _Renderer.flipX ? -1f : 1f;
            set
            {
                if (value != 0)
                    _Renderer.flipX = value < 0;
            }
        }

        /// <summary>The direction the <see cref="Renderer"/> is facing.</summary>
        public Vector2 Facing
        {
            get => new Vector2(FacingX, 0);
            set => FacingX = value.x;
        }

        /// <summary>
        /// Updates the <see cref="Facing"/> if the current state <see cref="States.CharacterState.CanTurn"/>.
        /// </summary>
        private void Update()
        {
            if (Character.StateMachine.CurrentState.CanTurn)
                Facing = Character.MovementDirection;
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Returns the <see cref="CharacterAnimancerComponent"/> associated with the
        /// <see cref="AnimancerEvent.CurrentState"/>.
        /// </summary>
        public static CharacterAnimancerComponent GetCurrent() => Get(AnimancerEvent.CurrentState);

        /// <summary>Returns the <see cref="CharacterAnimancerComponent"/> associated with the `node`.</summary>
        public static CharacterAnimancerComponent Get(AnimancerNode node) => Get(node.Root);

        /// <summary>Returns the <see cref="CharacterAnimancerComponent"/> associated with the `animancer`.</summary>
        public static CharacterAnimancerComponent Get(AnimancerPlayable animancer) => animancer.Component as CharacterAnimancerComponent;

        /************************************************************************************************************************/
        #region Hit Boxes
        /************************************************************************************************************************/

        private Dictionary<HitData, HitTrigger> _ActiveHits;
        private HashSet<Hit.ITarget> _IgnoreHits;

        /************************************************************************************************************************/

        /// <summary>Activates a <see cref="HitTrigger"/> for the `data`.</summary>
        public void AddHitBox(HitData data)
        {
            if (_IgnoreHits == null)
            {
                ObjectPool.Acquire(out _ActiveHits);
                ObjectPool.Acquire(out _IgnoreHits);
            }

            _ActiveHits.Add(data, HitTrigger.Activate(Character, data, FacingLeft, _IgnoreHits));
        }

        /// <summary>Deactivates a <see cref="HitTrigger"/> for the `data`.</summary>
        public void RemoveHitBox(HitData data)
        {
            if (_ActiveHits.TryGetValue(data, out var trigger))
            {
                trigger.Deactivate();
                _ActiveHits.Remove(data);
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Clears all currently active <see cref="HitTrigger"/>s and the list of objects hit by the current attack.
        /// </summary>
        public void EndHitSequence()
        {
            if (_IgnoreHits == null)
                return;

            ClearHitBoxes();
            ObjectPool.Release(ref _ActiveHits);
            ObjectPool.Release(ref _IgnoreHits);
        }

        /// <summary>Clears all currently active <see cref="HitTrigger"/>s.</summary>
        public void ClearHitBoxes()
        {
            if (_ActiveHits != null)
            {
                foreach (var trigger in _ActiveHits.Values)
                    trigger.Deactivate();
                _ActiveHits.Clear();
            }
        }

        /************************************************************************************************************************/

        /// <summary>Calls <see cref="EndHitSequence"/>.</summary>
        protected override void OnDisable()
        {
            EndHitSequence();
            base.OnDisable();
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}