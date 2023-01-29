using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static float[] d_ypos = {2.9f, 1.03f, -0.83f, -2.71f, -4.57f};
    
    public const float t_interval = 0.125f;  //move every t_interval seconds

    float t_counter = 0;
    float e = 0.1f;         //error
    
    int row = 2;    //middle
    float x_pos = -7.841f;

    bool down_key = false;
    bool up_key = false;

    [SerializeField] Snowball snowball;

    void Start()
    {
        transform.position = new Vector3(x_pos, d_ypos[row], 0);        
    }

    // Update is called once per frame
    void Update()
    {
        t_counter += Time.deltaTime;
        
        if(t_counter >= t_interval)
            t_counter = 0;

    //----

        float vertical = Input.GetAxis("Vertical");
        
        if(vertical > e) 
        {
            if(down_key) return;
            down_key = true;
            if(row > 0) row--;
        }
        else 
            down_key = false;

        if(vertical < -e) 
        {
            if(up_key) return;
            up_key = true;
            if(row < 4) row++;
        }
        else 
            up_key = false;

        row = Mathf.Clamp(row, 0, 4);
        transform.position = new Vector3(x_pos, d_ypos[row], 0);

    }

    public int getRow()
    {
        return row;
    }

    public void collision(GameObject g)
    {
        snowball.collision(g);
    }
}
