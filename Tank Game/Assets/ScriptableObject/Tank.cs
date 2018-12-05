using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{

    public string tankName = "Default";
    public int startingHP = 100;
    public int power = 3;
    public int speed = 3;
    public int armor = 3;
    public int FiringSpeed = 3;

    //The display prefab to be instantiated to represent this tank in the menu and in-game
    public GameObject displayPrefab;

    public Ability[] tankAbilities;

    //How many stars are displayed per stat for this tank in the customization screen.
    //[HeaderAttribute("Selection Star Rating")]
    public int startingHPRating { get; set; }
    public int powerRating { get; set; }
    public int speedRating { get; set; }
    public int armourRating { get; set; }
    public int firingSpeedRating { get; set; }

    public void getTankDetails()
    {

    }

}
