using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Itens : ScriptableObject
{
    public Sprite icon;
    public string name;
    public float value;

    [System.Serializable]
    public enum type
    {
        Potion,
        Elixir,
        Crystal
    }

    public type itemType;

    [System.Serializable]
    public enum slotType
    {
        Helmet,
        Armor,
        Shield
        
    }

    public slotType slotsType;

    public void getAction()
    {
        switch (itemType)
        {
            case type.Potion:
                Debug.Log("Health + " + value);
                break;

            case type.Elixir:
                Debug.Log("Health + " + value);
                break;

            case type.Crystal:
                Debug.Log("Health + " + value);
                break;

        }
    }
}
