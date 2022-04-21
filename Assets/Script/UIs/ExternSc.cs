using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExternSc : MonoBehaviour
{
  public void OnClickMyButton()
  {
    PopupSystem.Instance.OpenPopUp(
      "first",
      "second",
      () =>
      {
        Debug.Log("Onclick Okay");
      },
      () =>
      {
        Debug.Log("Onclick No");
      });
  } 
}
