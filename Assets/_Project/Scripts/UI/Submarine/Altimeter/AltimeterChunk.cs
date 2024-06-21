using System;
using _Project.Scripts.Logic.Submarine;
using _Project.Scripts.Model;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Submarine.Altimeter
{
    public class AltimeterChunk : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI depthText;
        private int _depth;

        public void SetDepth(int depth)
        {
            _depth = depth;
            depthText.text = depth + "m";
        }
        
        void Awake()
        {
            ShipBehaviour.OnGenericUIToBeUpdated += OnGenericUIToBeUpdated;
        }

        private void OnGenericUIToBeUpdated(object sender, GameData e)
        {
            var depthDiff = MathF.Abs(_depth - e.Ship.Altimeter);
            depthText.alpha = 0.6f-(depthDiff/3000);
        }
    }
}
