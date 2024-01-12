using UnityEngine;

public class NetController : MonoBehaviour
{
    public GameObject centerNet;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == RaftCollision.INTERACTABLEITEMTAG)
        {
            other.gameObject.transform.parent.SetParent(gameObject.transform);

            other.gameObject.transform.parent.GetComponent<ItemController>().InNat = true;

            other.gameObject.transform.position = centerNet.transform.position;
        }
    }
}
