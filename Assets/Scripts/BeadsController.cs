using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeadsController : MonoBehaviour
{
    private int curTick = 0;
    private List<GameObject> childSpheres = new List<GameObject>();
    private List<OneBeadController> childBeadsControllers = new List<OneBeadController>();
    private Vector3 center = new Vector3(0, -5, -2);
    
    public float radiusY = 5.0f;
    public float radiusX = 2.0f;
    public float radiusZ = 2.0f;
    public int length = 12;
    
    public GameObject spherePrefab;

    // Start is called before the first frame update
    void Start()
    {
        InitBeads();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Tick();
        }
    }

    private void InitBeads()
    {
        checkRadiuses();
        calculateCenter();

        for(int i=0; i < length; i++)
        {
            Vector3 positionOfSphere = getPositionForSphere(i);
            GameObject sphere = Instantiate(spherePrefab, positionOfSphere, spherePrefab.transform.rotation, transform);
            childSpheres.Add(sphere);
            OneBeadController childBeadController = sphere.GetComponent<OneBeadController>();
            childBeadsControllers.Add(childBeadController);
            childBeadController.Init(this, i, i);
        }
    }

    private void checkRadiuses()
    {
        float minRadius = ((length + 2) * spherePrefab.GetComponent<SphereCollider>().radius * 2.001f) / (2 * Mathf.PI);

        radiusX = checkAndGetRadius(radiusX, minRadius);
        radiusY = checkAndGetRadius(radiusY, minRadius);
        radiusZ = checkAndGetRadius(radiusZ, minRadius);
    }

    private float checkAndGetRadius(float radiusForCheck, float minRadius)
    {
        if (radiusForCheck < minRadius)
        {
            return minRadius;
        }
        return radiusForCheck;
    }

    private void calculateCenter()
    {
        float x = 0;
        float y = -radiusY;
        float z = radiusZ;

        center = new Vector3(x, y, z);
    }

    public Vector3 getPositionForSphere(int numberOfPosition)
    {
        float angleOfPosition = getAngleForPosition(numberOfPosition);
        return getPositionByAngle(angleOfPosition);        
    }

    public Vector3 getPositionByAngle(float angleOfPosition)
    {
        float x = radiusX * Mathf.Cos(angleOfPosition) + center.x;
        float y = radiusY * Mathf.Sin(angleOfPosition) + center.y;
        float z = radiusZ * Mathf.Cos(angleOfPosition) + center.z;

        return new Vector3(x, y, z);
    }

    public float getAngleForPosition(int numberOfPosition)
    {
        return (Mathf.PI / 2 - numberOfPosition * (2 * Mathf.PI / (length + 2)));
    }

    private void Tick()
    {
        //childSpheres.Insert
        childBeadsControllers[0].StartWalkToPosition(length);
    }

    public Vector3 getPositionOnStepOfMovingTo(int numberOfPosition, int fromPosition, float partOfDistance)
    {
        float angleOfPosition = getAngleForPosition(numberOfPosition);
        float angleOfPrevPosition = getAngleForPosition(fromPosition);

        while(angleOfPosition < angleOfPrevPosition)
        {
            angleOfPosition += 2 * Mathf.PI;
        }

        float currentAngle = (angleOfPosition - angleOfPrevPosition) * partOfDistance +
            angleOfPrevPosition;

        return getPositionByAngle(currentAngle);

    }

}
