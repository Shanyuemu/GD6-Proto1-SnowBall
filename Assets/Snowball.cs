using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    [SerializeField] SpriteRenderer layer1;
    [SerializeField] SpriteRenderer layer2;
    [SerializeField] SpriteRenderer layer3;

    [Space(10)]

    [SerializeField] SpawnObjects gameLogic;

    int score = 0;
    int size = 1;

    // Start is called before the first frame update
    void Start()
    {
        setSize(3);
    }

    public void setSize(int s)
    {
        if(s == size) return; //no change
        size = Mathf.Clamp(s, 0, 3);

        layer1.enabled = (size > 0);
        layer2.enabled = (size > 1);
        layer3.enabled = (size > 2);

        if(size <= 0) gameOver();
    }

    public void grow()
    {
        setSize(size + 1);
        score++;
    }

    public void shrink()
    {
        setSize(size - 1);
    }

    public void gameOver()
    {
        if(size != 0) setSize(0);
        if(gameLogic != null) gameLogic.gameOver();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        string t = col.gameObject.tag;
        Debug.Log("Snowball trigger: " + t);

        if(t == "Snow")
        {
            Destroy(col.gameObject);
            grow();
        }
        else if (t == "Rock")
        {
            Destroy(col.gameObject);
            shrink();
        }
        else if (t == "Wall")
            gameOver();
    }
}
