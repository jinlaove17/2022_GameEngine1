using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject door;

    // 트리거 발동 시 출력할 UI 텍스트
    public string mainGuideText;
    public string subGuideText;

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

                GameManager.Instance.systemUI.mainGuideText.text = mainGuideText;
                GameManager.Instance.systemUI.subGuideText.text = subGuideText;
                GameManager.Instance.systemUI.ShowUI();
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
