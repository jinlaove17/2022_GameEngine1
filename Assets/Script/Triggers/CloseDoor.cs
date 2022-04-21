using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    public GameObject[] doors = null;

    private bool isActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive)
        {
            if (other.CompareTag("Player"))
            {
                isActive = true;

                StartCoroutine(Closing());
            }
        }
    }

    IEnumerator Closing()
    {
        bool[] isClosed = new bool[doors.Length];

        while (true)
        {
            for (int i = 0; i < doors.Length; ++i)
            {
                if (!isClosed[i])
                {
                    if (doors[i].transform.position.y <= 0.0f)
                    {
                        Vector3 newPosition = doors[i].transform.position;

                        newPosition.y = 0.0f;
                        doors[i].transform.position = newPosition;
                        isClosed[i] = true;
                        continue;
                    }

                    doors[i].transform.Translate(6.0f * Time.deltaTime * -Vector3.up);
                }
            }

            bool clear = true;

            for (int j = 0; j < doors.Length; ++j)
            {
                if (!isClosed[j])
                {
                    clear = false;
                    break;
                }
            }

            if (clear)
            {
                break;
            }

            yield return null;
        }

        transform.gameObject.SetActive(false);

        GameManager.Instance.PrepareNextStage();
        print("현재 스테이지 레벨: " + GameManager.Instance.Stage);
    }
}
