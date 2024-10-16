using Sirenix.OdinInspector;
using UnityEngine;

namespace ChineseChess.SO
{
    [CreateAssetMenu(menuName = "Chinese Chess/Settings/Prefabs", fileName = "New Settings")]
    public class PrefabsSettings : ScriptableObject
    {
        private const int DEFAULT_LABEL_WIDTH = 100;
        private const string GAMEPLAY_GROUP = "Gameplay";
        private const string CHESS_GROUP = GAMEPLAY_GROUP + "/Chess";
        private const string FOG_GROUP = GAMEPLAY_GROUP + "/Fog";
        private const string MENU_UI_GROUP = GAMEPLAY_GROUP + "/MenuUI";
        private const string ENDING_GROUP = GAMEPLAY_GROUP + "/Ending";

        [FoldoutGroup(GAMEPLAY_GROUP)]
        [FoldoutGroup(CHESS_GROUP), LabelWidth(DEFAULT_LABEL_WIDTH)]
        public GameObject Chess;
        [FoldoutGroup(CHESS_GROUP), LabelWidth(DEFAULT_LABEL_WIDTH)]
        public GameObject PreviewChess;
        [FoldoutGroup(CHESS_GROUP), LabelWidth(DEFAULT_LABEL_WIDTH)]
        public GameObject PreviewRampage_x;
        [FoldoutGroup(CHESS_GROUP), LabelWidth(DEFAULT_LABEL_WIDTH)]
        public GameObject PreviewRampage_y;
        [FoldoutGroup(FOG_GROUP), LabelWidth(DEFAULT_LABEL_WIDTH)]
        public GameObject Fog;
        [FoldoutGroup(MENU_UI_GROUP), LabelWidth(DEFAULT_LABEL_WIDTH)]
        public GameObject BehaviourMenu;
        [FoldoutGroup(ENDING_GROUP), LabelWidth(DEFAULT_LABEL_WIDTH)]
        public GameObject Ending;
        [FoldoutGroup(ENDING_GROUP), LabelWidth(DEFAULT_LABEL_WIDTH)]
        public GameObject Particle;
    }
}
