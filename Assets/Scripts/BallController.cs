using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    public Rigidbody rb;
    public float impulseForce = 1.4f; //kuvvet

    private Vector3 startPos;//baslangic noktasi
    public int perfectPass = 0;
    private bool ignoreNextCollision;
    public bool isSuperSpeedActive;

    private void Awake()
    {
        startPos = transform.position;
    }



    private void OnCollisionEnter(Collision other)
    {
        if (ignoreNextCollision)
            return;
        if (isSuperSpeedActive)
        {
            if (!other.transform.GetComponent<Goal>())
            {
               
                Destroy(other.transform.parent.gameObject);

            }

        }
        // Süper hız etkin değilse ve bir ölüm parçası vurulursa -> oyunu yeniden başlatın
        else
        {
            DeathPart deathPart = other.transform.GetComponent<DeathPart>();
            if (deathPart)
                deathPart.HittedDeathPart();
        }

        rb.velocity = Vector3.zero; // Daha büyük bir mesafe düştükten sonra topun daha yükseğe zıplamasını önlemek için hızı kaldırmak

        rb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);



        // İki parcaya carpip iki kat ziplamamasi icin
        ignoreNextCollision = true;
        Invoke("AllowCollision", .1f);

        // Super hız
        perfectPass = 0;
        isSuperSpeedActive = false;
    }

    private void Update()
    {
        // super hızı aktive etmek
        if (perfectPass >= 4 && !isSuperSpeedActive)
        {
            isSuperSpeedActive = true;
            rb.AddForce(Vector3.down * 2.0f, ForceMode.Impulse);
        }
    }

    public void ResetBall()
    {
        transform.position = startPos;
    }

    private void AllowCollision()
    {
        ignoreNextCollision = false;
    }


}
//super hiz goale carpinca onu da parcaliyor (HATA)