using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class InteractiveGolem : MonoBehaviour
{
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        float xDir = Input.GetAxis("Horizontal");
        float yDir = Input.GetAxis("Vertical");

        if (xDir < 0) {
            transform.localScale = new UnityEngine.Vector3(-0.35f, 0.35f, 0.35f);
        } else {
            transform.localScale = new UnityEngine.Vector3(0.35f, 0.35f, 0.35f);
        }

        UnityEngine.Vector3 dir = new UnityEngine.Vector3(xDir, yDir, 0);
        dir.Normalize();
        animator.SetFloat("Speed", dir.magnitude);

        transform.Translate(Time.deltaTime * dir);
    }
}
