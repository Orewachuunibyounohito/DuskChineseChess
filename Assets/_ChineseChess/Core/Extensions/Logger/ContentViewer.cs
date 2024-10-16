using System;
using System.Collections.Generic;
using ChineseChess.Chesses;
using ChineseChess.SO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ChuuniExtension.Loggers
{
    public class ContentViewer : MonoBehaviour
    {
        [ReadOnly, ShowIf("@Fields.Count > 0")]
        public List<ContentField> Fields;

        protected void Awake(){
            var toggledFields = Resources.Load<ViewerSettings>(ResourcesPath.VIEWER_SETTINGS).ToggledField;
            Fields = new List<ContentField>();
            foreach(var fieldName in toggledFields){
                AddField($"{fieldName}", "");
            }
        }

        public void AddField(ContentField field) => AddField(field.Label, field.Value);
        public void AddField(string label, string value){
            if(Fields.Exists((field) => field.Label == label)){ return ; }
            var newField = new ContentField{ Label = label, Value = value };
            Fields.Add(newField);
        }

        public void UpdateField(string label, string value){
            Fields.Find((field)=> field.Label == label).Value = value;
        }
        
        [Serializable]
        public class ContentField : IEquatable<ContentField>
        {
            [HideLabel, HorizontalGroup("Content")]
            public string Label;
            [HideLabel, HorizontalGroup("Content")]
            public string Value;
            // public string InvokerName;

            public bool Equals(ContentField other) => Label == other.Label;
        }
    }

}