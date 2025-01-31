using System.Collections.Generic;
using System.Linq;
using DataModels;
using Unity.VisualScripting;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ComboList", menuName = "Lists/ComboList", order = 1)]
    public class ComboList : ScriptableObject
    {
        public List<Combo> combos = new List<Combo>();
        
        [System.Serializable]
        public class Combo 
        {
          public List<EDiceSide> values = new List<EDiceSide>();
        }

        public bool MatchesCombo(List<EDiceSide> toList)
        {
            foreach (var combo in combos)
            {
                if (toList.Count < combo.values.Count) continue;
                var values = new List<EDiceSide>(combo.values);
                for(var i = values.Count - 1; i >= 0; i--)
                {
                    if (toList.Any(v => v == values[i]))
                    {
                        values.RemoveAt(i);
                    }
                    
                    if (values.Count == 0)
                    {
                        Debug.Log("Combo");
                        return true;
                    }
                }
                    
            }
            return false;
        }
    }
}