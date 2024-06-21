using System.Collections.Generic;
using _Project.Scripts.Model;
using _Project.Scripts.UI.Encounter.Tiles;
using UnityEngine;

namespace _Project.Scripts.UI.Encounter.TileCreators
{
    public class CrewMemberTilesCreator : MonoBehaviour
    {
        [SerializeField] private CrewMemberTile crewMemberPrefab;

        public void CreateCrewMemberTiles(List<CrewMember> crewMembers)
        {
            
            crewMembers.ForEach(crewMember =>
            {
                var instance = Instantiate(crewMemberPrefab, gameObject.transform, false);
                EncounterUI.GetDestroyOnNextLoad().Add(instance.gameObject);
                instance.Init(crewMember);
            });
        }
    }
}