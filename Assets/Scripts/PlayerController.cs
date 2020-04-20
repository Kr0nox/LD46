using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    
    private Vector2 movement = new Vector2(); 
    private Rigidbody2D rb;
    public float speed = 1;
    public GameUIController ui;
    public Tool equipted;
    public Collider2D col;
    public Animator anim;
    public AudioSource packSound;
    
    /* Facing:
    0 => Up
    1 => Left
    2 => Down
    3 => Right
    */
    private int dir;
    private bool hasPosesion = false;
    
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * movement.normalized);
    }

    void Update() {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(KeyCode.Mouse0)) {
            if (equipted != null) {
                equipted.ActionWhileDown();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (equipted != null) {
                equipted.ActionOnPress();
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && equipted != null && hasPosesion) {
            bool touched = false;
            foreach (GameObject o in GameObject.FindGameObjectsWithTag("Item")) {
                if (col.IsTouching(o.GetComponent<Collider2D>())) {
                    touched = true;
                    break;
                }
            }

            if (!touched) {
                equipted.AppearOnGround(transform.position);
                equipted = null;
                ui.ChangeItem(null);
                packSound.Play();
            }
        }

        hasPosesion = equipted != null;

        if (Input.GetKeyUp(KeyCode.Mouse0)) {
            if(equipted != null)
                equipted.ActionRelease();
        }

        int newDir = -1;
        if (movement.x > 0) {
            newDir = 3;
        } else if (movement.x < 0) {
            newDir = 1;
        } else if (movement.y > 0) {
            newDir = 0;
        } else if (movement.y < 0) {
            newDir = 2;
        }

        if (newDir == -1) {
            
        } else if (newDir != dir) {
            dir = newDir;
            anim.SetInteger("direction", dir);
            anim.SetTrigger("changeDir");
        }
        
        anim.SetBool("walk", movement.magnitude != 0);
        
    }

    public void PicKUp(Tool tool) {
        if (equipted != null) {
            equipted.AppearOnGround(transform.position);
        }
        equipted = tool;
        equipted.equipted = true;
    }
}
