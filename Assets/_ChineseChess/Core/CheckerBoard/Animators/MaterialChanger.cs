using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class MaterialChanger : MonoBehaviour{
    [InlineEditor]
    public Material Material;
    public bool UseChangeColor;
    public bool UseChangeMetallic;

    [Title("Color Config")]
    [ShowIfGroup("UseChangeColor")]
    public Color DestinaitionColor;
    [ShowIfGroup("UseChangeColor")]
    public bool ColorLoop;

    [Title("Metallic Config")]
    [ShowIfGroup("UseChangeMetallic")]
    [Range(0, 1)]
    public float DestinaitionMetallic = 0;
    [ShowIfGroup("UseChangeMetallic")]
    public bool MetallicLoop;

    private Color originalColor;
    private float originalMetallic;

    private float MaterialMetallic{
        get => Material.GetFloat("_Metallic");
        set => Material.SetFloat("_Metallic", value);
    }

    private void OnDisable(){
        Material.color = originalColor;
        Material.SetFloat("_Metallic", originalMetallic);
    }

    void Start()
    {   
        // foreach(var propertyType in Enum.GetValues(typeof(MaterialPropertyType)).Cast<MaterialPropertyType>()){
        //     Debug.Log($"Type: {propertyType}\n{string.Join(", ", Material.GetPropertyNames(propertyType))}");
        // }

        originalColor = Material.color;
        originalMetallic = MaterialMetallic;

        if(UseChangeColor)   { StartCoroutine(ChangeColorTask(originalColor, DestinaitionColor)); }
        if(UseChangeMetallic){ StartCoroutine(ChangeMetallicTask(originalMetallic, DestinaitionMetallic)); }
    }

    private IEnumerator ChangeColorTask(Color start, Color destination){
        var interval = 0.05f;
        var delta = (destination - start) * interval;
        var duration = 1f;
        var timer = 0f;
        while(timer < duration){
            yield return new WaitForSeconds(interval);
            Material.color += delta;
            timer += interval;
        }
        if(ColorLoop){
            StartCoroutine(ChangeColorTask(destination, start));
        }
    }

    private IEnumerator ChangeMetallicTask(float start, float destination){
        var interval = 0.05f;
        var delta = (destination - start) * interval;
        var duration = 1f;
        var timer = 0f;
        while(timer < duration){
            yield return new WaitForSeconds(interval);
            MaterialMetallic += delta;
            timer += interval;
        }
        if(ColorLoop){
            StartCoroutine(ChangeMetallicTask(destination, start));
        }
    }
}
