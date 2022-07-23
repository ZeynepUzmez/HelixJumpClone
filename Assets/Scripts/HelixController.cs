using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixController : MonoBehaviour {

    private Vector3 startRotation;
    private Vector2 lastTapPos;//en son dokunulan yer
    private float helixDistance;
    public Transform topTransform;//en ust nokta
    public Transform goalTransform;//en alt nokta
    public GameObject helixLevelPrefab;
    public List<Stage> allStages = new List<Stage>();
    private List<GameObject> spawnedLevels = new List<GameObject>();

    // Balangıc kısmı icin
    void Awake () {
        //cubugun uzunlugunu bulma
        startRotation = transform.localEulerAngles;
        helixDistance = topTransform.localPosition.y - (goalTransform.localPosition.y + .1f);
        LoadStage(0);
    }
	
	// Update is called once per frame
	void Update () {
        // Tıklama (veya parmakla basma) kullanarak sarmalı döndürmek ve sürüklemek icin
        if (Input.GetMouseButton(0))
        {

            Vector2 curTapPos = Input.mousePosition;

            if (lastTapPos == Vector2.zero)
                lastTapPos = curTapPos;

            float delta = lastTapPos.x - curTapPos.x;
            lastTapPos = curTapPos;

            transform.Rotate(Vector3.up * delta);
        }

        if (Input.GetMouseButtonUp(0))
        {
            lastTapPos = Vector2.zero;
        }
    }

    public void LoadStage(int stageNumber)
    {
        // Doğru parca almak icin (hata buradan kaynakli olabilir)
        Stage stage = allStages[Mathf.Clamp(stageNumber, 0, allStages.Count - 1)]; 

        if (stage == null)
        {
            Debug.LogError("No stage " + stageNumber + " found in allStages list (HelixController). All stages assigned in list?");
            return;
        }

        // Yeni bir arka plan oluşturma
        Camera.main.backgroundColor = allStages[stageNumber].stageBackgroundColor;
        FindObjectOfType<BallController>().GetComponent<Renderer>().material.color = allStages[stageNumber].stageBallColor;

        // Helixsin rotasyonunu sıfırlama
        transform.localEulerAngles = startRotation;

        // Eski seviye varsa onu yok etmek icin
        foreach (GameObject go in spawnedLevels)
            Destroy(go);

        // Yeni seviyeler yazmak icin
        float levelDistance = helixDistance / stage.levels.Count;
        float spawnPosY = topTransform.localPosition.y;

        for (int i = 0; i < stage.levels.Count; i++)
        {
            spawnPosY -= levelDistance;
            GameObject level = Instantiate(helixLevelPrefab, transform);
            Debug.Log("Spawned Level");
            level.transform.localPosition = new Vector3(0, spawnPosY, 0);
            spawnedLevels.Add(level);

            // Bazı parcalari disable etme
            int partsToDisable = 12 - stage.levels[i].partCount;
            List<GameObject> disabledParts = new List<GameObject>();

            Debug.Log("Should disable " + partsToDisable);

            while (disabledParts.Count < partsToDisable)
            {
                //random parcaları secmek icin
                GameObject randomPart = level.transform.GetChild(Random.Range(0, level.transform.childCount)).gameObject;
                if (!disabledParts.Contains(randomPart))
                {
                    randomPart.SetActive(false);
                    disabledParts.Add(randomPart);
                    Debug.Log("Disabled Part");
                }
            }

            // Ölüm parçalarını ölüm parçası olarak işaretlemek için
            List<GameObject> leftParts = new List<GameObject>();

            foreach (Transform t in level.transform)
            {
                t.GetComponent<Renderer>().material.color = allStages[stageNumber].stageLevelPartColor; // Parçaların rengini değiştirme

                if (t.gameObject.activeInHierarchy)
                    leftParts.Add(t.gameObject);
            }

            Debug.Log(leftParts.Count + " parts left");

            List<GameObject> deathParts = new List<GameObject>();

            Debug.Log("Should mark " + stage.levels[i].deathPartCount + " death parts");

            while (deathParts.Count < stage.levels[i].deathPartCount)
            {
                GameObject randomPart = leftParts[(Random.Range(0, leftParts.Count))];

                if (!deathParts.Contains(randomPart))
                {
                    randomPart.gameObject.AddComponent<DeathPart>();
                    deathParts.Add(randomPart);
                    Debug.Log("Set death part");
                }
            }


        }
    }
    }
