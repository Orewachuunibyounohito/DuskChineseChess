using System.Collections.Generic;
using ChineseChess.Chesses;
using ChineseChess.SO;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace ChineseChess
{
    public class Congratulation : MonoBehaviour
    {
        private const string WINNER_TEXT_TEMPLATE = "Winner {0}";
        private const string BAR_TEXT_PATH = "WinnerBar/Bar/Label";
        private static GameObject congratulationParticlePrefab;

        [SerializeField]
        private TextMeshProUGUI barText;
        [ShowInInspector, ReadOnly]
        private Queue<ParticleSystem> particleQueue;

        private void Awake(){
            congratulationParticlePrefab = Resources.Load<PrefabsSettings>(ResourcesPath.PREFABS_SETTINGS)
                                                    .Particle;
            particleQueue = new();
            barText = transform.Find(BAR_TEXT_PATH).GetComponent<TextMeshProUGUI>();
            if(TryGetComponent<ColorTransfer>(out var transfer)){
                transfer.SetTarget(barText);
                transfer.ExecuteTransfer();
            }
        }

        public void SetWinnerName(string winner) => SetBarText(string.Format(WINNER_TEXT_TEMPLATE, winner));
        private void SetBarText(string text) => barText.SetText(text);

        public void GenerateParticle() => 
            particleQueue.Enqueue(Instantiate(congratulationParticlePrefab, transform).GetComponent<ParticleSystem>());
        public void Stop() => particleQueue.Dequeue().Stop();
    }
}
