using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GUI.Healthbar
{
    public class RingHealthbarControler : MonoBehaviour
    {
        [SerializeField]
        [Required]
        private Image fillImage;
        [SerializeField]
        [Required]
        private Slider slider;

        [SerializeField]
        private Color maxColor = Color.green;

        [SerializeField]
        private Color minColor = Color.red;
        public void SetValue(float value)
        {
            value = Mathf.Clamp01(value);
            slider.value = value;
            fillImage.color = Color.Lerp(maxColor, minColor, value);
        }
    }
}
