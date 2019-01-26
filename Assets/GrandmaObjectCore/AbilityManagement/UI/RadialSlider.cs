using UnityEngine;
using UnityEngine.UI;

namespace Grandma
{
    public class RadialSlider : MonoBehaviour
    {
        [SerializeField]
        private Image image;
        [SerializeField]
        private Text text;

        public void SetValue(float value)
        {
            image.fillAmount = value;

            if (text != null)
            {
                text.text = MathUtility.TruncateToSignificantDigits(value, 1).ToString();
            }
        }
    }
}
