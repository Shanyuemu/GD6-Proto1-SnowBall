using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    [SerializeField] GameObject snow;
    [SerializeField] GameObject rock;
    [SerializeField] GameObject wall;

    GameObject g_obj;
    ScrollingObject so;

    float t_counter = 0;

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
        int rock_count = 0;     //0-(3 - wall_count)
        int snow_count = 0;     //1-3
        int wall_count = 0;     //0-(4 - rock_count)
 
        for(int i=0; i<5; i++)  //for all 5 rows...
        {
            int g = (Random.Range(0, 4) < 2) ? 0 : Random.Range(1, 4);
            Debug.Log("random: " + g);

            switch (g)
            {
                case 1: //snow
                    snow_count++;
                    g_obj = Instantiate(snow);
                    so = g_obj.GetComponent<ScrollingObject>();
                    if(so != null) so.spawn(i);
                break;
                case 2: //rock
                    if((rock_count + wall_count) < 4 && rock_count < 2) //4 or more, then no new rock, no more than 3 rocks
                    {
                        rock_count++;
                        g_obj = Instantiate(rock);
                        so = g_obj.GetComponent<ScrollingObject>();
                        if(so != null) so.spawn(i);
                    }
                break;
                case 3: //wall
                    if((rock_count + wall_count) < 4)
                    {
                        wall_count++;
                        g_obj = Instantiate(wall);
                        so = g_obj.GetComponent<ScrollingObject>();
                        if(so != null) so.spawn(i);
                    }
                break;
                default:
                    g_obj = null;
                    so = null;
                break;
            }
        }
        Debug.Log("Spawn! snow: " + snow_count + ", rock: " + rock_count + ", wall: " + wall_count + "\n;");
    }

    public int random(int min, int max)
    {
        int r = Random.Range(min, max);
        return r;
    }
}
