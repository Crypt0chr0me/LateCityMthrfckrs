﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobShadowScript : MonoBehaviour
{

    [Header("Settings")]
    public Transform _parent;//prefab
    public Transform _parentSpawned;
    public Vector3 _parentOffset = new Vector3(0f, 0.01f, 0f);
    public LayerMask _layerMask;
    public float TimeStep = 0.5f;
    public int MaxSteps = 10;
    public float InterpFactor = 3f;
    public float AngularInterpFactor = 3f;

    bool _interp = false;
    Vector3 _lastPos = new Vector3(0,0,0);
    Vector3 _lastUp = new Vector3(0, 1, 0);
    BR_Business biz;

    void Start()
    {
        if(_parent!= null)
        {
            _parentSpawned = Instantiate(_parent, transform.position, transform.rotation);
            _parentSpawned.gameObject.SetActive(true);
            _parentSpawned.SetParent(transform);
            _parent.gameObject.SetActive(false);
            biz = GetComponent<BR_Business>();
        }
        
    }
    
    void Update()
    {
        if (_parentSpawned != null && _parentSpawned.gameObject.activeSelf && biz)
        {
            Vector3 cPos = transform.position;
            Vector3 cVel = GetComponent<Rigidbody>().velocity;
            RaycastHit castHit = new RaycastHit();
            bool hit = false;
            for(int i = 0; i < MaxSteps && !hit; i++)
            {
                hit = Physics.Raycast(cPos, cVel, out castHit, TimeStep * cVel.magnitude, _layerMask);
                Debug.DrawLine(cPos, cPos + TimeStep * cVel);
                cPos += TimeStep * cVel;
                cVel += TimeStep * biz.gravityMagnitude * biz.gravityDirection;
                if (cVel.z > biz.maxFallSpeed)
                    cVel = new Vector3(cVel.x, cVel.y, biz.maxFallSpeed);
            }

            if (hit)
            {
                if (!_interp)
                {
                    _parentSpawned.position = castHit.point + _parentOffset;    // Set Position
                    _parentSpawned.up = castHit.normal;                         // Rotate to same angle as ground

                    _lastPos = _parentSpawned.position;
                    _lastUp = _parentSpawned.up;
                    _interp = true;
                }
                else
                {
                    Vector3 targetUp = castHit.normal;
                    Vector3 targetPos = castHit.point + _parentOffset.y * castHit.normal;

                    _parentSpawned.position = Vector3.Lerp(_lastPos, targetPos, Time.deltaTime * InterpFactor);
                    _parentSpawned.up = Vector3.RotateTowards(_lastUp, targetUp, Time.deltaTime * AngularInterpFactor, 0.0f);

                    _lastPos = _parentSpawned.position;
                    _lastUp = _parentSpawned.up;
                }

                
            }
            else
            {
                _interp = false;
                _parentSpawned.position = transform.position - new Vector3(0, -1000, 0);
            }

            //Ray ray = new Ray(transform.position, -Vector3.up);
            //RaycastHit hitInfo;

            //if (Physics.Raycast(ray, out hitInfo, 100f, _layerMask))
            //{
            //    //Debug.Log(hitInfo.transform.gameObject);
            //    // Position
            //    _parentSpawned.position = hitInfo.point + _parentOffset;

            //    // Rotate to same angle as ground
            //    _parentSpawned.up = hitInfo.normal;
            //}
            //else
            //{
            //    // If raycast not hitting (air beneath feet), position it far away
            //    _parentSpawned.position = new Vector3(0f, 1000f, 0f);
            //}
        }
        
    }

}

