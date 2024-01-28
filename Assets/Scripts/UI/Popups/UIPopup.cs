using UnityEngine;

namespace PitchPerfect.UI
{
    public class UIPopup : MonoBehaviour
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
        
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
