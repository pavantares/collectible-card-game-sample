using UnityEngine;

namespace Pavantares.CCG.View
{
    public class ViewBase : MonoBehaviour
    {
        public virtual void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}
