using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject door = null;

    private bool isActive = false;
    private const float maxHeight = 3.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive)
        {
            if (other.CompareTag("Player"))
            {
                isActive = true;

                StartCoroutine(Opening());
            }
        }
    }

    IEnumerator Opening()
    {
        while (true)
        {
            if (door.transform.position.y > maxHeight)
            {
                Vector3 newPosition = door.transform.position;

                newPosition.y = maxHeight;
                door.transform.position = newPosition;
                break;
            }

            door.transform.Translate(4.0f * Time.deltaTime * Vector3.up);

            yield return null;
        }

        transform.gameObject.SetActive(false);
    }
}
