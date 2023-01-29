using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public const float t_interval = 0.125f;  //move every t_interval seconds

    float t_counter = 0;

    public static float[] d_ypos = {2.9f, 1.03f, -0.83f, -2.71f, -4.57f};
    
    int row = 2;    //middle
    float x_pos = -7.841f;

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
        else 
            return;

        //execute

        float vertical = Input.GetAxis("Vertical");
        float e = 0.1f;
        
        if(vertical > e && row > 0) row--;
        if(vertical < -e && row < 4) row++;
    
        row = Mathf.Clamp(row, 0, 4);
        transform.position = new Vector3(x_pos, d_ypos[row], 0);

    }
}
