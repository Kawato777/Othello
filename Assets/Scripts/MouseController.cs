using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���N���b�N
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Ray�𐶐�
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) // Ray�𓊎�
            {
                gameController.PutStone(hit);
            }
        }
    }
}
