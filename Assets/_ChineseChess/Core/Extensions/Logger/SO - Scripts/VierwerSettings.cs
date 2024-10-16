using System.Collections.Generic;
using ChuuniExtension.Loggers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ChineseChess.SO
{
    [CreateAssetMenu(menuName = "Chinese Chess/Settings/Viewer", fileName = "New Settings")]
    public class ViewerSettings : ScriptableObject
    {
        public List<ChessViewerFieldName> ToggledField;
    }
}
