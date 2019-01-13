using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New birdObject", menuName = "Bird Object", order = 51)]
public class birdObject : ScriptableObject {

    [SerializeField]
    private string birdName;
    [SerializeField]
    private string scientificName;
    [SerializeField]
    private string description;
    [SerializeField]
    private string biome;
    [SerializeField]
    private Sprite picture;
    
    public string BirdName
    {
        get
        {
            return birdName;
        }
    }

    public string ScientificName
    {
        get
        {
            return scientificName;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }
    }

    public string Biome
    {
        get
        {
            return biome;
        }
    }

    public Sprite Picture
    {
        get
        {
            return picture;
        }
    }

}
