using UnityEngine;

[System.Serializable]  // This makes it show up in the inspector.
public class PowerUp
{
    public GameObject prefab;  // The prefab for the power-up item
    public ItemRarity rarity;  // The rarity of the power-up
}
