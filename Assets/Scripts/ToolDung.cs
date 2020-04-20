using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolDung : Tool {
    public Collider2D[] fields;
    public PlayerController player;
    public static bool firstDung = true;

    private void Awake() {
        controller = GameObject.FindGameObjectsWithTag("Controller")[0].transform;
        player = FindObjectOfType<PlayerController>();
        packSound = controller.GetComponent<AudioSource>();
        GameObject[] g = GameObject.FindGameObjectsWithTag("Field");
        fields = new Collider2D[g.Length];
        for (int i = 0; i < g.Length; i++) {
            fields[i] = g[i].GetComponent<Collider2D>();
        }
    }

    public override void ActionOnPress() {
        foreach (Collider2D field in fields) {
            if (field.bounds.Contains(transform.position)) {
                field.gameObject.GetComponent<Field>().GetDung();
                player.equipted = null;
                ui.ChangeItem(null);
                Destroy(gameObject);
            }
        }
    }

    protected override void OnPickUp() {
        if (firstDung) {
            ui.SetText("This is fertilizer. Use it with the left mouse button on a field to raise its nutrient.", KeyCode.Mouse0);
            firstDung = false;
        }
    }
}
