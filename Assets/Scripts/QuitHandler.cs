using UnityEngine;

public class QuitHandler : MonoBehaviour {
    void Update() {
        if (Input.GetButton("Quit")) {
            Application.Quit();
        }
    }
}