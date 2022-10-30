using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] BoardController boardController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ¶NbN
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Rayð¶¬
            if (Physics.Raycast(ray, out RaycastHit hit)) // RayðË
            {
                boardController.PutStone(hit);
            }
        }
    }
}
