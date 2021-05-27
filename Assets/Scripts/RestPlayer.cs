using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestPlayer : MonoBehaviour
{
    public GameObject player;
    public Transform resetPoint;
    public enum ResetType { RESET_POS, RESET_GAME };
    public ResetType type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        if (type == ResetType.RESET_POS)
            player.transform.position = new Vector3(0.4f, 1f, 0.85f);

        if (type == ResetType.RESET_GAME)
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("title screen");
    }
}
