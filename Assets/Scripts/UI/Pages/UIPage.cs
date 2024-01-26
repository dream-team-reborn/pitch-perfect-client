using UnityEngine;

namespace PitchPerfect.UI
{
    public class UIPage : MonoBehaviour
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