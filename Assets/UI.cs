using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Transform livePos;
    public Text liveText;
    public Transform collectablePos;
    public Text collectableText;
    public CharacterMovement player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        liveText.transform.position = livePos.position;
        liveText.transform.rotation = livePos.rotation;
        liveText.text = "x " + player.Lives;

        collectableText.transform.position = collectablePos.position;
        collectableText.transform.rotation = collectablePos.rotation;
        collectableText.text = "x " + player.Collected;
    }
}
