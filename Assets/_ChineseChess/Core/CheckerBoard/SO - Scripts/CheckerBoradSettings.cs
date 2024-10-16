using System;
using System.Collections;
using System.Collections.Generic;
using ChineseChess.Chesses;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ChineseChess.SO
{
    [CreateAssetMenu(menuName = "Chinese Chess/Settings/CheckerBoard", fileName = "New CheckerBoard Settings")]
    public class CheckerBoradSettings : ScriptableObject
    {
        [Title("Model")]
        [ReadOnly]
        public int Width = 9;
        [ReadOnly]
        public int Length = 10;
        [ReadOnly]
        public int RiverWidth = 1;

        [Title("View")]
        public AroundOffset AroundOffsets;
        public float PositionToViewCoefficient;

        [Serializable]
        public class AroundOffset : IEnumerable<OffsetData>
        {
            public List<OffsetData> offsets;

            public IEnumerator<OffsetData> GetEnumerator() => offsets.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => offsets.GetEnumerator();
        }
        [Serializable]
        public class OffsetData
        {
            public DirectionType direction;
            public Vector2Int vector;
        }
    }
}
