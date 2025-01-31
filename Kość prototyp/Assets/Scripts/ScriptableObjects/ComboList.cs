﻿using System.Collections.Generic;
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
            public string name;
            public int rank = 1;
          public List<EDiceSide> values = new List<EDiceSide>();
        }
        /// <summary>
        /// Checks all possible combos, finds highest ranked, first match
        /// </summary>
        /// <returns>1 if reaches maxRank, 0 if reaches minRank, -1 if fails</returns>
        public int MatchesCombo(List<EDiceSide> toList, int minRank, int critRank)
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
                        if(combo.rank < minRank)
                        {
                            continue;
                        }
                        if(combo.rank >= critRank)
                        {
                            return 1;
                        }
                        return 0;
                    }
                }
                    
            }
            return -1;
        }
    }
}