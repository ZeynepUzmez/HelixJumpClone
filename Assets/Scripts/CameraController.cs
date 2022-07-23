using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public BallController target;
    private float offset; // Kamera ve top arasındaki ilk mesafeyi korumak icin

    private void Awake()
    {
        offset = transform.position.y - target.transform.position.y;//kameranın ysi - topun ysi
    }

    void Update () {
        //  Kamerayı yumuşak bir şekilde hedef yüksekliğe hareket ettirmek icin 
        Vector3 curPos = transform.position;
        curPos.y = target.transform.position.y + offset;
        transform.position = curPos;
    }
}
