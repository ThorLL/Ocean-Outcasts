using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Submarine.ThreatAlert
{
    public class ThreatAlert: MonoBehaviour
    {
        [SerializeField] private Image threatAlertImage;

        private void Awake()
        {
            ShowAlert(false);
        }

        public void ShowAlert(bool active)
        {
            threatAlertImage.gameObject.SetActive(active);;
        }
        
    }
}