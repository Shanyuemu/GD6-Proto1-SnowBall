using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    [SerializeField] SpriteRenderer layer1;
    [SerializeField] SpriteRenderer layer2;
    [SerializeField] SpriteRenderer layer3;

    [Space(5)]

    [SerializeField] SpriteRenderer foxSprite;

    [Space(10)]

    [SerializeField] SpawnObjects gameLogic;
    [SerializeField] SoundPlayer soundPlayer;

    [Space(10)]

    [SerializeField] int flashTimes = 2;
    [SerializeField] float flashRate = 0.25f;

    bool damageCoolDown = false;

    int score = 0;
    int size = 1;

    // Start is called before the first frame update
    void Awake()
    {
        setSize(2);
    }

    void Update()
    {
        if(gameLogic != null)
        {
            if(!gameLogic.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Space))
            {
                gameLogic.gameObject.SetActive(true);
                gameLogic.restart();

                foxSprite.gameObject.SetActive(true);

                setSize(2);
                score = 0;
            }
        }
    }

    public void setSize(int s)
    {
        if(s == size) return; //no change
        size = Mathf.Clamp(s, 0, 3);
/* 
        layer1.enabled = (size > 0);
        layer2.enabled = (size > 1);
        layer3.enabled = (size > 2);
 */
        layer1.enabled = (size == 1);
        layer2.enabled = (size == 2);
        layer3.enabled = (size == 3);
        
        if(size <= 0) gameOver();
    }

    public void grow()
    {
        if(soundPlayer != null) soundPlayer.positive();
        setSize(size + 1);
        score++;
    }

    public void shrink()
    {
        if(soundPlayer != null) soundPlayer.damage();
        setSize(size - 1);
        StartCoroutine("playerFlash");
    }

    public void gameOver()
    {
        if(size != 0) setSize(0);
        StartCoroutine("playerFlash");
        if(gameLogic != null) gameLogic.gameOver();
    }

    public void victory()
    {
        if(gameLogic != null) gameLogic.gameOver(true);
    }

    public void collision(GameObject col)
    {
        if(size <= 0) return;

        string t = col.tag;
        //Debug.Log("Snowball collision: " + t);

        if(t == "Snow")
        {
            Destroy(col);
            grow();
        }
        else
        {
            if(damageCoolDown) return;
            if (t == "Rock")
            {
                Destroy(col);
                shrink();
            }
            else if (t == "Wall")
                gameOver();
        }
    }

    IEnumerator playerFlash()
    {
        damageCoolDown = true;

        bool l1 = layer1.enabled;
        bool l2 = layer2.enabled;
        bool l3 = layer3.enabled;

        for(int i=0; i<flashTimes; i++)
        {
            foxSprite.enabled = false;
            layer1.enabled = false;
            layer2.enabled = false;
            layer3.enabled = false;
            yield return new WaitForSeconds(flashRate);
        
            foxSprite.enabled = true;
            layer1.enabled = (size > 0);
            layer2.enabled = (size > 1);
            layer3.enabled = (size > 2);
            yield return new WaitForSeconds(flashRate);
        }

        damageCoolDown = false;

        if(size == 0) 
            foxSprite.gameObject.SetActive(false);
    }

    public bool gameRunning()
    {
        return (gameLogic.gameObject.activeSelf) ? gameLogic.gameRunning() : false;
    }

}
