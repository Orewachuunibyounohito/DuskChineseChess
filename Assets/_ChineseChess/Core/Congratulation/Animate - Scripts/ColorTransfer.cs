using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ColorTransfer : MonoBehaviour
{
    private const float FPS_60_INTERVAL = 0.0166667f;

    public float Duration;
    public bool Repeat;

    [Space(3)]
    [Title("Debug Info")]
    [ReadOnly]
    public Color Original;
    [SerializeField, ReadOnly]
    private Color peekDestinationColor;
    [SerializeField,ReadOnly]
    private bool isRunning;
    [ShowInInspector, ReadOnly]
    private object Target;

    private void Start(){
        if(Target == null){ return ; }
        Original = (Color)Target.GetType().GetProperty("color").GetValue(Target);
        if(Original == null){
            Debug.LogError($"NotSupportedError: {Target} haven't \"color\" property.");
        }
    }

    public void SetTarget(object target){
        Target = target;
    }

    public void ExecuteTransfer(){
        if(isRunning){ return ; }
        StartCoroutine(TransferTask());
    }
    private IEnumerator TransferTask(){
        Original = (Color)Target.GetType().GetProperty("color").GetValue(Target);
        isRunning = true;
        float timer = 0;
        float interval = FPS_60_INTERVAL;
        Color destColor = GetRandomColor();
        peekDestinationColor = destColor;
        Color diffColor = destColor - Original;
        Color currentColor = Original;
        float speed = interval / Duration;
        while (timer < Duration){
            timer += interval;
            yield return new WaitForSeconds(interval);
            currentColor += diffColor * speed;
            Target.GetType().GetProperty("color").SetValue(Target, currentColor);
        }
        isRunning = false;
        if(Repeat){ ExecuteTransfer(); }
    }

    private Color GetRandomColor() =>
        new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
}
