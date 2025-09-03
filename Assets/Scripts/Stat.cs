using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Stat
{
    [SerializeField] private int baseValue;

    public List<int> modifiers;
    public int GetValue()
    {
        int finalValue = baseValue;

        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }

        return finalValue;
    }

    public void AddModifier(int _modifer)
    {
        modifiers.Add(_modifer);
    }

    public void RemoveModifier(int _modifer)
    {
        modifiers.Remove(_modifer);
    }
}
