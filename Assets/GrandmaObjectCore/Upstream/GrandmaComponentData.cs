using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public enum GCDModifierType
    {
        Addition,
        Multiplication
    }

    public interface IGrandmaModifiable { }

    [Serializable]
    [CreateAssetMenu(menuName = "Core/Grandma Component Data")]
    public class GrandmaComponentData : ScriptableObject, IGrandmaModifiable
    {
        [HideInInspector]
        public string componentID;
        [HideInInspector]
        public string associatedObjID;
        [HideInInspector]
        public string dataClassName;

        public virtual bool IsValid
        {
            get
            {
                return true;
            }
        }

        void Awake()
        {
            this.dataClassName = this.GetType().ToString();
            this.componentID = Guid.NewGuid().ToString();
        }

        #region Modifiers
        public GrandmaComponentData Modified
        {
            get
            {
                return DeepModify(this.Clone()) as GrandmaComponentData;
            }
        }

        private object DeepModify(object val)
        {
            Debug.Log("DeepModify: " + val);

            foreach (var field in val.GetType().GetFields())
            {
                object fieldVal = field.GetValue(val);

                if (fieldVal.GetType().GetInterfaces().Contains(typeof(IGrandmaModifiable)))
                {
                    Debug.Log("DeepModify: GrandmaMod Field " + field.Name);

                    field.SetValue(val, DeepModify(fieldVal));
                }
                else if (ConvertToFloat(fieldVal).HasValue)
                {
                    float fVal = ConvertToFloat(fieldVal).Value;

                    field.SetValue(val, ModifyValue(field, fVal));
                }
            }

            return val;
        }

        //Helper for Modified
        private float ModifyValue(FieldInfo fieldInfo, float val)
        {
            foreach(var m in modifiers)
            {
                float? modVal = ConvertToFloat(GetFieldValue(m.data, fieldInfo));

                if(modVal.HasValue == false)
                {
                    continue;
                }

                switch (m.type)
                {
                    case GCDModifierType.Addition:
                        val += modVal.Value;
                        break;
                    case GCDModifierType.Multiplication:
                        val *= modVal.Value;
                        break;
                }
            }

            return val;
        }

        private float? ConvertToFloat(object val)
        {
            float? fVal = null;

            try
            {
                fVal = (float)val;
            }
            catch { }

            return fVal;
        }

        private object GetFieldValue(object obj, FieldInfo fieldInfo)
        {
            foreach (var field in obj.GetType().GetFields())
            {
                if(field == fieldInfo)
                {
                    return field.GetValue(obj);
                }

                var fieldVal = field.GetValue(obj);

                if (fieldVal.GetType().GetInterfaces().Contains(typeof(IGrandmaModifiable)))
                {
                    return GetFieldValue(fieldVal, fieldInfo);
                }
            }

            return null;
        }

        private class Modifier
        {
            public string name;
            public GrandmaComponentData data;
            public GCDModifierType type;
            public float priority;
        }

        private List<Modifier> modifiers = new List<Modifier>();

        public void Modify(string name, GrandmaComponentData modifier, float priority, GCDModifierType type)
        {
            Debug.Log("GrandmaComponentData: Modifying with " + modifier);

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

        public void RemoveModifier(GrandmaComponentData modifier)
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
            return JsonUtility.ToJson(this);//JsonConvert.SerializeObject(this);
        }

        public static GrandmaComponentData DeserialiseJSON(string json, Type type)
        {
            GrandmaComponentData newData = CreateInstance(type) as GrandmaComponentData;

            JsonUtility.FromJsonOverwrite(json, newData);

            return newData;
        }
        #endregion

        public override string ToString()
        {
            return SerializeJSON();
        }
    }
}

