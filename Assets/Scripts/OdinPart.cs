using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OdinPart : MonoBehaviour
{
    private GameObject odinStatue;
    private Vector3 odinDirection;
    private Transform child;
    public float moveToStatueSpeed;
    public float rotationSpeed;

    private bool moveToOdin;
    private GameObject flyToHand;
    private CharacterMovement player;

    /*
     * Yes, this is expensive, but passing by reference would be tedious and time consuming
     */
    private void Start()
    {
        child = transform.GetChild(0);
        odinStatue = GameObject.FindGameObjectWithTag("Odin");
        odinDirection = (odinStatue.transform.position - transform.position).normalized;
        flyToHand = GameObject.FindGameObjectWithTag("UICollectable");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
    }

    public void InstantiateOdinPart()
    {
        child = transform.GetChild(0);
        odinStatue = GameObject.FindGameObjectWithTag("Odin");
    }

    private void Update()
    {
        if (moveToOdin)
        {
            Vector3 flyDir = (flyToHand.transform.position - transform.position).normalized;
            transform.Translate(flyDir * moveToStatueSpeed * Time.deltaTime);
        }

        child.RotateAround(transform.position, transform.up, rotationSpeed * Time.deltaTime);
    }

    public void SetMoveToOdin(bool moveToOdin)
    {
        this.moveToOdin = moveToOdin;
        odinStatue.GetComponent<Odin>().AddPart();
        odinDirection = (odinStatue.transform.position - transform.position).normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          //  transform.parent.GetComponent<AudioSource>().Play();
            moveToOdin = true;
            odinStatue.GetComponent<Odin>().AddPart();
          //  StartCoroutine(DestroyObject(0.75f));
        }
        if (other.CompareTag("UICollectable"))
        {
            player.Collected += 1;
            if (player.Collected % 50 == 0)
                player.Lives += 1;
            Destroy(gameObject);
        }
    }
    

    IEnumerator DestroyObject(float time)
    {
        yield return new WaitForSeconds(time);

        gameObject.SetActive(false);
    }
}
