using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance = null;     //Allows other scripts to call functions from SpriteManager.                    //Drag a reference to the sprite source which will display the sprite
    public Sprite[] sprites;
    public Sprite[] characters;


    void Start()
    {
        loadSprites();
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

        Sprite[] characters = new Sprite[12];
        //Load an Sprite (Assets/Resources/Sprites/sprite01.png)

        characters[0] = Resources.Load<Sprite>("Sprites/character_0");
        characters[1] = Resources.Load<Sprite>("Sprites/character_1");
        characters[2] = Resources.Load<Sprite>("Sprites/character_2");
        characters[3] = Resources.Load<Sprite>("Sprites/character_3");
        characters[4] = Resources.Load<Sprite>("Sprites/character_4");
        characters[5] = Resources.Load<Sprite>("Sprites/character_5");
        characters[6] = Resources.Load<Sprite>("Sprites/character_6");
        characters[7] = Resources.Load<Sprite>("Sprites/character_7");
        characters[8] = Resources.Load<Sprite>("Sprites/character_8");
        characters[9] = Resources.Load<Sprite>("Sprites/character_9");
        characters[10] = Resources.Load<Sprite>("Sprites/character_10");
        characters[11] = Resources.Load<Sprite>("Sprites/character_11");

        this.characters = characters;
    }

    public Sprite getRandomCharacter(params Sprite[] characters)
    {
        int randomIndex = Random.Range(0, characters.Length);
        Debug.Log(characters.Length);
        Debug.Log("Random" + randomIndex);
        //Set the sprite to the sprite at our randomly chosen index.
        return characters[randomIndex];
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