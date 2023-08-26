using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName ="Scriptable object/Item")]
public class Item : ScriptableObject
{
    public Sprite image;
    public ActionType type;
    public int cost;
    public float duration;
    public int extraHealth;

    public enum ActionType
    {
        health,
        attackDamage,
        attackSpeed
    }
}
