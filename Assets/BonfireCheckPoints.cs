using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireCheckPoints : MonoBehaviour
{
    public Bonfire[] bonfires;
    public CharacterMovement player;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < bonfires.Length; i++)
        {
            bonfires[i].CheckPointID = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
      //  if (player.transform.position.y < 0.25f)
        //{
           // Vector3 bonfirePos = GetBonfireById(player.CheckPointID).transform.position;
           // player.transform.position = new Vector3(bonfirePos.x, bonfirePos.y + 0.5f, bonfirePos.z);
       // }
       
    }

    Bonfire GetBonfireById(int id)
    {
        foreach (var bonfire in bonfires)
        {
            if (bonfire.CheckPointID == id)
                return bonfire;
        }
        return null;
    }
}
