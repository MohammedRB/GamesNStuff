// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using Animancer;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformerGameKit
{
    /// <summary>Displays the <see cref="Health.CurrentHealth"/> and <see cref="Health.MaximumHealth"/>.</summary>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/HealthDisplay
    /// 
    [AddComponentMenu(Strings.MenuPrefix + "Health Display")]
    [HelpURL(Strings.APIDocumentation + "/" + nameof(HealthDisplay))]
    public sealed class HealthDisplay : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private Health _Health;
        [SerializeField] private Text _Text;

        public int numOfHearts;

        public Image[] hearts;
        public Sprite fullHeart;
        public Sprite emptyHeart;

        /************************************************************************************************************************/

#if UNITY_EDITOR
        /// <summary>[Editor-Only] Ensures that all fields have valid values and finds missing components nearby.</summary>
        private void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Text);
        }
#endif

        /************************************************************************************************************************/

        private void Awake()
        {
            _Health.OnCurrentHealthChanged += (oldValue, newValue) => OnHealthChanged();
            _Health.OnMaximumHealthChanged += (oldValue, newValue) => OnHealthChanged();
            OnHealthChanged();
        }

        /************************************************************************************************************************/

        private void OnHealthChanged()
        {
            _Text.text = $"Health: {_Health.CurrentHealth} / {_Health.MaximumHealth}";
            
            /*if ((_Health.CurrentHealth/10) > numOfHearts){
            _Health.CurrentHealth = numOfHearts * 10;
            }*/

            for (int i = 0; i<hearts.Length; i++){
                if ((i*10) < _Health.CurrentHealth){
                    hearts[i].sprite = fullHeart;
                }
                else {
                    hearts[i].sprite = emptyHeart;
                }

                if ((i*10) > numOfHearts){
                    hearts[i].enabled = true;
                } else{
                    hearts[i].enabled = false;
                }
            }   
        }

        /************************************************************************************************************************/
    }
}