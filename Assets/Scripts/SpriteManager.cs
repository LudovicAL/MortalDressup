using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance = null;     //Allows other scripts to call functions from SpriteManager.
    public Sprite spriteSource;                       //Drag a reference to the sprite source which will display the sprite
    public Sprite[] sprites;


    void Start()
    {
        loadSprites();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Sprite[] loadSprites()
    {

        Sprite[] sprites = new Sprite[4];
        //Load an Sprite (Assets/Resources/Sprites/sprite01.png)

        sprites[0] = Resources.Load<Sprite>("Sprites/BAM");
        sprites[1] = Resources.Load<Sprite>("Sprites/BOOM");
        sprites[2] = Resources.Load<Sprite>("Sprites/POW");
        sprites[3] = Resources.Load<Sprite>("Sprites/WHAM");
        
        return sprites;
    }

    public void displayRandomSprite(params Sprite[] sprites)
    {
        int randomIndex = Random.Range(0, sprites.Length);

        //Set the sprite to the sprite at our randomly chosen index.
        spriteSource = sprites[randomIndex];
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