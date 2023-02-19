using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyRotator : MonoBehaviour {
    [SerializeField] float rotationSpeed = 0.01f;
    [SerializeField] float distanceToGround = 3f;
    Quaternion startingRotation;
    
    // Start is called before the first frame update
    void Start() {
        startingRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update() {
        RotateBody();
    }

    void RotateBody() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, distanceToGround);
        Quaternion targetRotation;
        if (hit.collider != null) {
            // rotate upper body to match the normal of the surface beneath it
            targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.localRotation;            
        }
        else {
            // if we're not on the ground, just rotate to face the local default direction
            targetRotation = startingRotation;
        }

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotationSpeed);
    }

    private void OnDrawGizmos() {
        // Gizmos.color = Color.red;
        // Gizmos.DrawRay(transform.position, Vector3.down * distanceToGround);
        // Gizmos.color = Color.green;
        // Gizmos.DrawRay(transform.position, transform.up * distanceToGround);
    }
}