using UnityEngine;

public class Arrow : MonoBehaviour {
    [SerializeField] private Player player;
    void Update() {
        Vector3 lookMouse = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        float angle = Mathf.Atan2(lookMouse.y, lookMouse.x) * Mathf.Rad2Deg;
        transform.Rotate(0, 0, angle - 90);

        if(Input.GetMouseButtonDown(0)) {
            gameObject.SetActive(false);
            player.SetDirection(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            GameController.GetInstance().finishedAims++;
            Debug.Log("definiu!");
        }
    }
}
