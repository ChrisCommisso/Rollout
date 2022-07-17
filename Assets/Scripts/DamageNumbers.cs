using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumbers : MonoBehaviour
{
    public static DamageNumbers Instance;
    public List<GameObject> numbers;
    public List<float> numberTimers;
    public Canvas numberCanvas;
    public GameObject numberPool;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < numbers.Count; i++)
        {
            if(numberTimers[i]<0)
            {
                Destroy(numbers[i]);
                numbers.RemoveAt(i);
                numberTimers.RemoveAt(i);
            }
            else
            {
                numberTimers[i] -= Time.deltaTime;
                numbers[i].transform.localScale = numbers[i].transform.localScale * 0.95f;
            }

        }
    }

    public void CreateNumber(Vector3 WorldPos, float LifeTime)
    {
        GameObject newNumber = Instantiate(numberPool, CameraController.Instance.mainCamera.WorldToScreenPoint(WorldPos), Quaternion.identity, numberCanvas.transform);
        numbers.Add(newNumber);
        numberTimers.Add(LifeTime);

    }
}
