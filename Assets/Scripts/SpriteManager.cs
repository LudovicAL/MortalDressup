using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance = null;     //Allows other scripts to call functions from SpriteManager.                    //Drag a reference to the sprite source which will display the sprite
    public Sprite[] sprites;
    public Object[] characters;


    void Start()
    {
        loadSprites();
        loadCharacters();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void loadSprites()
    {
        Sprite[] sprites = new Sprite[4];

        sprites[0] = Resources.Load<Sprite>("Sprites/BAM");
        sprites[1] = Resources.Load<Sprite>("Sprites/BOOM");
        sprites[2] = Resources.Load<Sprite>("Sprites/POW");
        sprites[3] = Resources.Load<Sprite>("Sprites/WHAM");  

        this.sprites = sprites;
    }

    public void loadCharacters()
    {
        characters = Resources.LoadAll("Sprites/Characters");

        this.characters = characters;
    }

    public Sprite getRandomCharacter()
    {
        int randomIndex = Random.Range(1, characters.Length - 1);
        //Set the sprite to the sprite at our randomly chosen index.
        return (Sprite) characters[randomIndex];
    }

    public Sprite getRandomFightingSprite()
    {
        int randomIndex = Random.Range(0, sprites.Length);
        //Set the sprite to the sprite at our randomly chosen index.
        return sprites[randomIndex];
    }

    public void drawRandomFightingSprite(float x, float y) 
    {
        GameObject go = new GameObject();
        go.name = "PunchSprite";
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.transform.position = new Vector2(x, y);
        sr.sprite = getRandomFightingSprite();
        Destroy(go, 0.5f);
    }

    void Awake()
    {
        //Check if there is already an instance of SpriteManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SpriteManager.
            Destroy(gameObject);
        //Set SpriteManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }
}