using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
{
    public enum GCDModifierType
    {
        Addition,
        Multiplication
    }

    [Serializable]
    public class GrandmaComponentData : ScriptableObject
    {
        [HideInInspector]
        public string associatedObjID;
        [HideInInspector]
        public string dataClassName;

        public virtual bool IsValid
        {
            get
            {
                Init();
                return Type.GetType(dataClassName)?.IsSubclassOf(typeof(GrandmaComponentData)) == true;
            }
        }

        public void Init()
        {
            this.dataClassName = this.GetType().ToString();
        }

        #region Modifiers
        public GrandmaComponentData Modified
        {
            get
            {
                GrandmaComponentData modified = this.Clone();

                foreach(var field in this.GetType().GetFields())
                {
                    Debug.Log(field.GetValue(this));

                    float val = 0f;
                        
                    try
                    {
                        val = Convert.ToSingle(field.GetValue(this));
                    } catch
                    {
                        continue;
                    }

                    Debug.Log("GrandmaComponentData: Field value " + val + " " + field);
                   
                    if(val != null)
                    {
                        field.SetValue(modified, ModifyValue(field, val));   
                    }                 
                }

                return modified;
            }
        }

        //Helper for Modified
        private float ModifyValue(FieldInfo fieldInfo, float val)
        {
            foreach(var m in modifiers)
            {
                float modVal = Convert.ToSingle(fieldInfo.GetValue(m.data));

                if(modVal == null)
                {
                    continue;
                }

                switch (m.type)
                {
                    case GCDModifierType.Addition:
                        val += modVal;
                        break;
                    case GCDModifierType.Multiplication:
                        val *= modVal;
                        break;
                }
            }

            return val;
        }

        private class Modifier
        {
            public string name;
            public GrandmaComponentData data;
            public GCDModifierType type;
            public float priority;
        }

        private List<Modifier> modifiers = new List<Modifier>();

        public virtual void Modify(string name, GrandmaComponentData modifier, float priority, GCDModifierType type)
        {
            modifiers.Add(new Modifier()
            {
                name = name,
                data = modifier,
                priority = priority,
                type = type
            });

            //Lower priority acts first
            modifiers.OrderBy(x => x.priority);

            //OnModify?
        }

        public virtual void RemoveModifier(GrandmaComponentData modifier)
        {
            modifiers.RemoveAll(x => x.data == modifier);

            //OnModify?
        }
        #endregion

        public GrandmaComponentData Clone()
        {
            return DeserialiseJSON(SerializeJSON(), GetType());
        }

        #region Serialisation / Deserialisation
        public string SerializeJSON()
        {
            if(IsValid == false)
            {
                Init();
            }

            return JsonUtility.ToJson(this);//JsonConvert.SerializeObject(this);
        }

        public static GrandmaComponentData DeserialiseJSON(string json, Type type)
        {
            GrandmaComponentData newData = ScriptableObject.CreateInstance(type) as GrandmaComponentData;

            JsonUtility.FromJsonOverwrite(json, newData);

            return newData;
        }
        #endregion
    }
}

