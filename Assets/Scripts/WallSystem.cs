using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSystem : MonoBehaviour
{
    private bool canThrowOver;
    private enum wallType
    {
        highWall,
        lowWall,
        door
        
    };
    [SerializeField]private wallType type;
    // Start is called before the first frame update
    void Start()
    {
        if(type == wallType.highWall)
        {
            canThrowOver = false;
        }
        else
        {
            canThrowOver = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool CanThrowOver()
    {
        return canThrowOver;
    }
}
