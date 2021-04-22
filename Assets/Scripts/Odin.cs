using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Odin : MonoBehaviour
{
    public int numTillNextPart;

    private int numCollectibles = 0;
    private int currentChildIndex = 0;
    private static int numberOfParts = 9;

    public void AddPart()
    {
        if (currentChildIndex < numberOfParts)
        {
            numCollectibles += 1;

            if (numCollectibles >= numTillNextPart)
            {
                transform.GetChild(currentChildIndex).gameObject.SetActive(true);
                currentChildIndex += 1;
                numCollectibles = 0;
            }
        }
    }
}
