using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

    public Rigidbody2D TriggerBody;


    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnCollisionEnter2D(Collision2D other) {

        // Debug.Log(string.Format("We've collided with {0}, and you have died {1}", other.transform.name, "A Vicious Death"));

        //If we hit the FG death!
        if (other.transform.name == "Tilemap-FG" && !GameManager.instance.IsDead) {
            GameManager.instance.PlayClip("hit");

            GameManager.instance.Dead();
            GameManager.instance.PlayClip("dead", group:"effects");

        }
        else {
            Debug.Log(string.Format("We've collided with {0}", other.transform.name));
        }
    }


    public void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(string.Format("We've collided with object {0}", other.transform.name));

        if (other.transform.GetComponent<Coin>() ) {
            GameManager.instance.CollectCoin(other.gameObject);
        }

        if (other.transform.GetComponent<Door>() ) {
            GameManager.instance.Dead(true);
        }


    }


}
