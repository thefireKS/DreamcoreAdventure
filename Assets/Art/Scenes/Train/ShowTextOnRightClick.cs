using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTextOnRightClick : MonoBehaviour
{
    private bool isTextVisible = false;
    [SerializeField] private GameObject Canvas;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                isTextVisible = !isTextVisible;

                if (isTextVisible)
                {
                    Canvas.SetActive(true);
                    Time.timeScale = 0;
                }

                else
                {
                    Canvas.SetActive(false);
                    Time.timeScale = 1;
                }
            }
        }
    }
}