using System;
using System.Collections.Generic;
using ChineseChess.Chesses;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ChineseChess.SO
{
    [CreateAssetMenu(menuName = "Chinese Chess/Settings/Materials", fileName = "New Materials")]
    public class MaterialsSettings : ScriptableObject
    {
        public Material Red;
        public Material Black;
        [Title("Chess")]
        public List<ChessMaterialContent> chessMaterials;
    }

    [Serializable]
    public class ChessMaterialContent
    {
        [HideLabel]
        public ChessType chess;
        public Material red;
        public Material black;
    }
}
