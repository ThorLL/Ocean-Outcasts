using _Project.Scripts.Configuration;
using _Project.Scripts.Logic.Submarine;
using _Project.Scripts.Logic.Submarine.Room;
using _Project.Scripts.Model;
using _Project.Scripts.UI.Submarine.ProgressBar;
using _Project.Scripts.Logic.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Submarine
{
    public class ShipResourcesUI : MonoBehaviour
    {
        // TODO Somewhere over the rainbow someone is screaming about this
        [SerializeField] private ShipRoomManagerBehaviour shipRoomManagerBehaviour;
        
        [SerializeField] private EngineIntegrityBar engineIntegrityBar;
        [SerializeField] private NavigationBar navigationBar;
        [SerializeField] private OxygenBar oxygenBar;
        [SerializeField] private AmmunitionCounter ammunitionCounter;
        [SerializeField] private ThreatAlert.ThreatAlert threatAlert;
        
        [SerializeField] private float lowOxygenThreshold;
        [SerializeField] private float lowEngineIntegrityThreshold;
        
        [SerializeField] private Image altimeterImage;
        [SerializeField] private TextMeshProUGUI altimeterImageText;
        [SerializeField] private TextMeshProUGUI altimeterText;
        
        private GameParameters _parameters;
        
        private void Awake()
        {
            ShipBehaviour.OnGenericUIToBeUpdated += OnGenericUIToBeUpdated;
            EncounterTrigger.OnSetThreatAlertStatus += OnShowThreatAlert;
            _parameters = GameParameters.Instance;
        }
        
        private void OnShowThreatAlert(object sender, bool show)
        {
            threatAlert.ShowAlert(show);
        }
        
        private void OnGenericUIToBeUpdated(object sender, GameData gameData)
        {
            engineIntegrityBar.UpdateVisual(gameData.Ship.EngineIntegrity);
            navigationBar.UpdateVisual(gameData.Ship.Navigation, gameData.Ship.Speed); // TODO Speed
            oxygenBar.UpdateVisual(gameData.Ship.Oxygen);
            ammunitionCounter.UpdateVisual(gameData.Ship.Ammunition);

            oxygenBar.ShowWarningIcon(gameData.Ship.Oxygen < lowOxygenThreshold);
            engineIntegrityBar.ShowWarningIcon(gameData.Ship.EngineIntegrity < lowEngineIntegrityThreshold);

            var isThereSomeoneInWeaponsRoom = shipRoomManagerBehaviour.GetCrewMembers(ShipRoomName.Weapons).Count > 0;
            ammunitionCounter.ShowIconIncreasingAmmunition(isThereSomeoneInWeaponsRoom);
            
            var zone = ZoneUtil.GetZoneFromDepth(_parameters.descendZones, gameData.Ship.Altimeter);
            
            altimeterText.text = $"{Mathf.RoundToInt(gameData.Ship.Altimeter)}m";
            
            altimeterImageText.text = zone.zoneName;
            altimeterImage.sprite = zone.sprite;
        }
        
    }
}