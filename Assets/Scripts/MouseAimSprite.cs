using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAimSprite : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
    }
    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
    }
}
