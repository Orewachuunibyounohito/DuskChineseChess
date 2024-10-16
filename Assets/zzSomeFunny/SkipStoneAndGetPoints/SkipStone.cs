using System.Collections.Generic;
using UnityEngine;

namespace SkipStoneAndGetPoints
{
    public class SkipStone
    {   
        // point, index
        // Dictionary<int, int> stoneDictionary = new();

        public Dictionary<int, int> SetupDictionary(params int[] points){
            Dictionary<int, int> stoneDictionary = new();
            for(int index = 0; index < points.Length; index++){
                stoneDictionary.Add(points[index], index);
            }
            return stoneDictionary;
        }

        public int RandomSkipAndCalculatePoints_UntilLastItemAndNonBack(params int[] points){
            Dictionary<int, int> stoneDictionary = SetupDictionary(points);
            Queue<int> path = new();
            int index = 0;
            int lastIndex = stoneDictionary.Count-1;
            // int nextIndex = index+1;
            for(int nextIndex = index+1; nextIndex < lastIndex; nextIndex = index+1){
                index = Random.Range(nextIndex, lastIndex);
                path.Enqueue(points[index]);
            }

            int result = 0;
            int previousIndex = 0;
            while(path.Count > 0){
                int currentPoint = path.Dequeue();
                int currentIndex = stoneDictionary[currentPoint];
                int steps = currentIndex - previousIndex;
                result += currentPoint * steps;
                
                previousIndex = currentIndex;
            }
            return result;
        }
    }
}