using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    public const float t_interval = 0.5f;  //move every t_interval seconds
    
    public const int col_count = 12;
    public const int row_count = 5;

    public static float[] d_xpos = { 8.493f, 6.83f, 5.16f, 3.51f, 1.84f, 0.17f, -1.5f, -3.16f, -4.83f, -6.1f, -6.1f, -9.05f };        //column -> y position (right to left)
    public static float[] d_ypos = { 3.028f, 1.172f, -0.704f, -2.563f, -4.418f };      //rows -> x position (top to bottom)
    
    float t_counter = 0;

    int row = 0;    //0-4, initialized by spawn
    int col = 0;    //far right

    SpriteRenderer sr;

    [SerializeField] Vector3 offset;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if(sr != null) sr.enabled = true;
    }

    public void spawn(int r)
    {
        if(r >= row_count || r < 0) Debug.Log("invalid row: " + r);
        else row = r;

        transform.position = new Vector3(d_xpos[col] + offset.x, d_ypos[row] + offset.y, 0);
        if(sr != null) sr.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        t_counter += Time.deltaTime;
        if(t_counter >= t_interval)
        {
            t_counter = 0;
            move();
        }
    }

    void move() //move left / scroll
    {
        col++;
        
        if(col == 9 || col == 10) sr.enabled = false;
        else if(col == (col_count - 1)) sr.enabled = true;

        if(col >= col_count)
            Destroy(gameObject);    //off screen - out of sight out of mind
        else
            transform.position = new Vector3(d_xpos[col % col_count] + offset.x, transform.position.y + offset.y, 0);  // move to next position
    }

}
