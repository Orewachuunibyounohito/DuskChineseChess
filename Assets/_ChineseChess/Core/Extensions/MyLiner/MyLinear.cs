using UnityEngine;

namespace ChuuniExtension
{
    public class MyLinear
    {
        public float CoefficientX;
        public float CoefficientY;
        public float Constant;

        public MyLinear(float xC, float yC, float c){
            CoefficientX = xC;
            CoefficientY = yC;
            Constant = c;
        }

        public MyLinear(Vector2Int original, Vector2Int destination){
            var vector = destination - original;
            CoefficientX = vector.y;
            CoefficientY = -vector.x;
            Constant = CoefficientX*original.x + CoefficientY*original.y;
        }

        public float FindY(float x) => CoefficientX*x - Constant;
        public float FindX(float y) => CoefficientY*y - Constant;
        public Vector2 FindPointByX(float x) => new Vector2(x, FindY(x));
        public Vector2 FindPointByY(float y) => new Vector2(FindX(y), y);
        public bool IsInLine(float x, float y) => (CoefficientX*x + CoefficientY*y) == Constant;
        public bool IsInLine(Vector2 point) => (CoefficientX*point.x + CoefficientY*point.y) == Constant;
    }
}