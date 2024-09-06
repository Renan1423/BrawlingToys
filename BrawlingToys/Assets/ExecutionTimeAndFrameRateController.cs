using UnityEngine;

public class ExecutionTimeAndFrameRateController : MonoBehaviour {
    void Update() {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (Input.GetKey(KeyCode.LeftShift)) {
            if (Input.GetKeyDown(KeyCode.F1))
                Application.targetFrameRate = 15;
            if (Input.GetKeyDown(KeyCode.F2))
                Application.targetFrameRate = 30;
            if (Input.GetKeyDown(KeyCode.F3))
                Application.targetFrameRate = 60;
            if (Input.GetKeyDown(KeyCode.F4))
                Application.targetFrameRate = 900;
        }

        if (Input.GetKey(KeyCode.LeftControl)) {
            if (Input.GetKeyDown(KeyCode.F1))
                Time.timeScale = 1;
            if (Input.GetKeyDown(KeyCode.F2))
                Time.timeScale = 1.5f;
            if (Input.GetKeyDown(KeyCode.F3))
                Time.timeScale = 2f;
            if (Input.GetKeyDown(KeyCode.F4))
                Time.timeScale = 0.5f;
        }
#endif
    }
}
