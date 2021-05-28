﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BER_ERHI_c223901b45f74af0a160b6a254574b90
{
    public class BeadsController : MonoBehaviour
    {
        private List<GameObject> childSpheres = new List<GameObject>();
        private List<OneBeadController> childBeadsControllers = new List<OneBeadController>();
        private Vector3 center = new Vector3(0, -5, -2);
        private int countToStartTick = 0;
        private float prevMoveInterval = 0.0f;
        private bool tickCourutineIsRunning = false;
        

        private float radiusY = 1.0f;
        private float radiusX = 1.0f;
        private float radiusZ = 1.0f;
        private int length = 12;
        private float moveInterval = 0.3f;

        public GameObject spherePrefab;

        private void Awake()
        {
            GameManager.Instance.OnGameStarted.AddListener(InitBeads);
        }

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.settings.OnCircleLengthChanged.AddListener(HandleCircleLengthChange);            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Tick();
            }

            if(countToStartTick > 1)
            {
                if(!Global.Equal(moveInterval, 0.0f))
                {
                    prevMoveInterval = moveInterval;
                    moveInterval = 0.0f;
                }
            }
            else if(countToStartTick > 0)
            {
                if(Global.Equal(moveInterval, 0.0f) && !Global.Equal(prevMoveInterval, 0.0f))
                {
                    moveInterval = prevMoveInterval;
                }                
            }

            if (!tickCourutineIsRunning && countToStartTick > 0)
            {
                StartCoroutine(MoveBeadsForOneTick());
            }

        }

        private void InitBeads()
        {
            length = GameManager.Instance.settings.lengthOfCircle;
            checkRadiuses();
            calculateCenter();
            childBeadsControllers.Clear();
            
            for (int i = 0; i < length; i++)
            {
                Vector3 positionOfSphere = getPositionForSphere(i);
                GameObject sphere;

                if (childSpheres.Count <= i)
                {
                    sphere = Instantiate(spherePrefab, positionOfSphere, spherePrefab.transform.rotation, transform);
                    childSpheres.Add(sphere);
                }
                else
                {
                    sphere = childSpheres[i];
                    sphere.SetActive(true);
                    sphere.transform.Translate(positionOfSphere - sphere.transform.position);
                }
                OneBeadController childBeadController = sphere.GetComponent<OneBeadController>();
                childBeadsControllers.Add(childBeadController);
                childBeadController.Init(this, i, i);
            }

            if(childSpheres.Count > length)
            {
                for(int i = length; i < childSpheres.Count; i++)
                {
                    childSpheres[i].SetActive(false);
                }
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
            countToStartTick++;
        }

        private IEnumerator MoveBeadsForOneTick() {

            tickCourutineIsRunning = true;
            
            childBeadsControllers[0].StartWalkToPosition(length);

            if(!Global.Equal(moveInterval, 0.0f))
            {
                yield return new WaitForSeconds(moveInterval);
            }            

            for(int i=1; i<childBeadsControllers.Count; i++)
            {
                childBeadsControllers[i].StartWalkToPosition(i - 1);
                if (!Global.Equal(moveInterval, 0.0f))
                {
                    yield return new WaitForSeconds(moveInterval);
                }
            }

            childBeadsControllers.Add(childBeadsControllers[0]);
            childBeadsControllers.RemoveAt(0);
            childBeadsControllers[childBeadsControllers.Count-1].StartWalkToPosition(length-1);

            if(countToStartTick > 0)
            {
                countToStartTick--;
            }

            tickCourutineIsRunning = false;

            yield return null;
        }

        public Vector3 getPositionOnStepOfMovingTo(int numberOfPosition, int fromPosition, float partOfDistance)
        {
            float angleOfPosition = getAngleForPosition(numberOfPosition);
            float angleOfPrevPosition = getAngleForPosition(fromPosition);

            while (angleOfPosition < angleOfPrevPosition)
            {
                angleOfPosition += 2 * Mathf.PI;
            }

            float currentAngle = (angleOfPosition - angleOfPrevPosition) * partOfDistance +
                angleOfPrevPosition;

            return getPositionByAngle(currentAngle);

        }

        private void HandleCircleLengthChange()
        {
            ResizeBeads();            
        }

        private void ResizeBeads()
        {
            int lengthBefore = length;
            length = GameManager.Instance.settings.lengthOfCircle;
            checkRadiuses();
            calculateCenter();

            if (GameManager.Instance.settings.lengthOfCircle > lengthBefore)
            {
                AddNewBeads(lengthBefore);
            }
            else
            {
                HideSomeBeads();
            }
        }

        private void AddNewBeads(int lengthBefore)
        {
            for(int i = lengthBefore; i < length; i++)
            {
                GameObject sphere = null;
                if(childSpheres.Count <= i)
                {
                    sphere = CreateAndInitNewBead(i);
                }
                else
                {
                    sphere = childSpheres[i];
                    sphere.SetActive(true);
                    sphere.transform.Translate(getPositionForSphere(i) - sphere.transform.position);
                    OneBeadController childController = sphere.GetComponent<OneBeadController>();
                    childController.Init(this, i, i);
                    childBeadsControllers.Add(childController);
                }
            }
        }

        private GameObject CreateAndInitNewBead(int number)
        {
            GameObject sphere = Instantiate(spherePrefab, getPositionForSphere(number), spherePrefab.transform.rotation, transform);
            sphere.SetActive(true);
            childSpheres.Add(sphere);
            OneBeadController childController = sphere.GetComponent<OneBeadController>();
            childController.Init(this, number, number);
            childBeadsControllers.Add(childController);
            return sphere;
        }

        private void HideSomeBeads()
        {
            for(int i = length; i < childSpheres.Count; i++)
            {
                //TODO: find gameobject by childBeadsControllers
            }
        }

    }
}