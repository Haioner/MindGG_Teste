using System.Collections.Generic;
using UnityEngine;

public enum rarityEnum { Junior, Middle, Senior };

[System.Serializable]
public class Rarity
{
    public rarityEnum raritys;
    public Vector2 timePitch;
}

[CreateAssetMenu(menuName ="Employees/ManagerRarity")]
public class ManagerRaritySO : ScriptableObject
{
    public List<Rarity> RarityList = new List<Rarity>();

}
