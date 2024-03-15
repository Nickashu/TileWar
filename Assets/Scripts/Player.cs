using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour {
    public Tilemap myTileMap, enemyTileMap;
    public Tile myTile, enemyTile;
    public float movementSpeed;
    private Rigidbody2D rb;
    private Vector2 direction;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 dir) {
        direction = dir.normalized;
    }
    public void StartMovement() {
        rb.AddForce(direction * movementSpeed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("bg2")) {
            Debug.Log(collision.GetContacts(collision.contacts));
            Grid tileMapGrid = collision.gameObject.GetComponent<Tilemap>().layoutGrid;
            Vector3Int tilePos = tileMapGrid.WorldToCell(collision.GetContact(0).point);
            changeTile(tilePos);
        }
        rb.AddForce(collision.GetContact(0).normal.normalized * movementSpeed, ForceMode2D.Impulse);
    }
    private void changeTile(Vector3Int tilePosition) {
        StartCoroutine(changeTileDelay(tilePosition));
    }

    private IEnumerator changeTileDelay(Vector3Int tilePosition) {
        yield return new WaitForSeconds(GameController.GetInstance().delayChangeTile);
        enemyTileMap.SetTile(tilePosition, null);
        myTileMap.SetTile(tilePosition, myTile);
    }
}
