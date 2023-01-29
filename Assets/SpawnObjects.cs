using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnObjects : MonoBehaviour
{
    public enum Types {None = 0, Snow = 1, Rock = 2, Wall = 3};

    [SerializeField] GameObject snow;
    [SerializeField] GameObject rock;
    [SerializeField] GameObject wall;

    GameObject g_obj;
    ScrollingObject so;

    float t_counter = 0;
    public static bool game_running = true;

    Types[] prev_col = {Types.None, Types.None, Types.None, Types.None, Types.None};
    Types[] current_col = {Types.None, Types.None, Types.None, Types.None, Types.None};

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        t_counter += Time.deltaTime;
        if(t_counter >= ScrollingObject.t_interval)
        {
            t_counter = 0;
            spawn();
        }
    }

    void spawn()
    {
        Types t = Types.None;
        float r = 0.0f;

        int snow_count = 0;
        int rock_count = 0;     //0-(3 - wall_count)
        int wall_count = 0;     //0-(4 - rock_count)
 
        for(int i=0; i<5; i++)  //for all 5 rows...
        {
            float min = 0.0f;                   //probability (wall : rock : snow : nothing -> 2:3:5:10)

            if (wall_count + rock_count > 3) 
                min = 0.5f;                     //no more than 4 rocks and walls combined
            else if (rock_count > 2) 
                min = 0.75f;                    //no more than 3 rocks total
            else if ((snow_count + wall_count + rock_count == 4) || prev_col[i] != Types.None) 
                min = 1.0f;                     //at least one empty space, and an empty space after each object

            r = Random.Range(min, 2.0f);
            if(r < 0.2f)        //0.0f <= r < = 0.2f
                t = Types.Wall;
            else if(r < 0.5f)   //0.2f <= r < = 0.5f
                t = Types.Rock;
            else if(r < 1.0f)   //0.5f <= r < = 1.0f
                t = Types.Snow;
            else                //1.0f <= r < = 2.0f
                t = Types.None;
            
            current_col[i] = t;

            switch (t)
            {
                case Types.None:
                    //nothing
                break;
                case Types.Snow: //snow
                    snow_count++;
                    g_obj = Instantiate(snow);
                break;
                case Types.Rock: //rock
                    rock_count++;
                    g_obj = Instantiate(rock);
                break;
                case Types.Wall: //wall
                    wall_count++;
                    g_obj = Instantiate(wall);
                break;
            }
            
            if(t != Types.None && g_obj != null)
            {
                g_obj.transform.parent = this.transform;
                so = g_obj.GetComponent<ScrollingObject>();
                if(so != null) so.spawn(i);
                else Debug.Log("scrolling object missing");
            }
            prev_col[i] = current_col[i];

        }

        //Debug.Log("Spawn Column: " + current_col[0] + ", " + current_col[1] + ", " + current_col[2] + ", " + current_col[3] + ", " + current_col[4]);
        //Debug.Log("Spawn! snow: " + snow_count + ", rock: " + rock_count + ", wall: " + wall_count + "\n;");
    }

    public void gameOver()
    {
        game_running = false;
        //game over stuff....
        gameObject.SetActive(false);
    }

}
