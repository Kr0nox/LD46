using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour {
    
    public Sprite sprite;
    public GameObject groundItem;
    public bool equipted;
    public bool grounded;
    public Transform controller;
    protected GameUIController ui;
    public float fill = 0;

    public static bool hasShownText = false;
    public bool hasFill;
    public AudioSource packSound;
    
    private void Start() {
        groundItem.GetComponent<SpriteRenderer>().sprite = sprite;
        grounded = true;
        transform.SetParent(controller);
        ui = controller.GetComponent<GameController>().ui;
    }

    public void AppearOnGround(Vector2 pos) {
        transform.SetParent(controller);
        transform.position = pos;
        groundItem.SetActive(true);
        grounded = true;
        equipted = false;
        OnDrop();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (grounded && other.CompareTag("Player")) {
            if (!hasShownText) {
                other.gameObject.GetComponent<PlayerController>().ui.SetText("Press 'E' to pick up tool.", KeyCode.E);
                hasShownText = true;
            }
            StartCoroutine(WaitForPickUp(other.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        StopAllCoroutines();
    }

    IEnumerator WaitForPickUp(GameObject player) {
        while (!Input.GetKeyDown(KeyCode.E))
            yield return null;
        player.GetComponent<PlayerController>().PicKUp(this);
        transform.SetParent(player.transform);
        transform.localPosition = new Vector2(0, 0); 
        groundItem.SetActive(false);
        grounded = false;
        ui.ChangeItem(sprite);
        ui.SetItemFill(fill,hasFill);
        packSound.Play();
        OnPickUp();
    }
    
    public virtual void ActionWhileDown() {
        if (!equipted)
            return;
    }

    public virtual void ActionOnPress() {
        if (!equipted)
            return;
    }

    protected virtual void OnPickUp() {
        
    }

    protected virtual void OnDrop() {
        
    }

    public virtual void Upgrade(int number) {
        
    }

    public virtual void ActionRelease() {
        
    }
    
}
