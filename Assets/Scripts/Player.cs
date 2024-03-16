using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour {
    public Tilemap myTileMap, enemyTileMap;
    public Tile myTile, enemyTile;
    public float movementSpeed;
    private Rigidbody2D rb;
    [SerializeField] private Transform[] playerDirections;
    private Vector2 direction;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        //rb.AddForce(new Vector2(Random.Range(0, 100), Random.Range(0, 100)).normalized * movementSpeed, ForceMode2D.Impulse);
    }

    public void SetDirection(Vector2 dir) {
        direction = dir.normalized;
    }
    public void StartMovement() {
        rb.AddForce(direction * movementSpeed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        for(int i=0; i<collision.contactCount; i++) {
            if (collision.gameObject.CompareTag("bg")) {
                Vector2 offSet = calculateOffSetCollision(collision.GetContact(i).point);
                Vector2 hitTilePosition = collision.GetContact(i).point + offSet;
                Debug.Log(hitTilePosition);
                Grid tileMapGrid = enemyTileMap.layoutGrid;
                Vector3Int tilePosition = tileMapGrid.WorldToCell(hitTilePosition);
                changeTile(tilePosition);
            }
        }
        rb.AddForce(collision.GetContact(0).normal.normalized * movementSpeed, ForceMode2D.Impulse);    //O movimento será um pouco aleatório
    }
    private void changeTile(Vector3Int tilePosition) {
        StartCoroutine(changeTileDelay(tilePosition));
    }

    private IEnumerator changeTileDelay(Vector3Int tilePosition) {
        yield return new WaitForSeconds(GameController.GetInstance().delayChangeTile);
        enemyTileMap.SetTile(tilePosition, null);
        myTileMap.SetTile(tilePosition, myTile);
    }


    private Vector2 calculateOffSetCollision(Vector2 contactPoint) {
        Vector2 offSet = Vector2.zero;
        int[] validDirections = new int[2];
        float minMagnitude = 0, secondMinMagnitude = 0;
        for (int j = 0; j < playerDirections.Length; j++) {
            float dist = Vector2.Distance(contactPoint, (Vector2)playerDirections[j].position);
            if (j == 0) {
                minMagnitude = dist;
                secondMinMagnitude = minMagnitude;
                validDirections[0] = j;
                validDirections[1] = j;
            }
            else if (j == 1) {
                if (dist < minMagnitude) {
                    minMagnitude = dist;
                    validDirections[0] = j;
                }
                else {
                    secondMinMagnitude = dist;
                    validDirections[1] = j;
                }
            }
            else {
                if (dist < minMagnitude) {
                    minMagnitude = dist;
                    validDirections[0] = j;
                }
                else {
                    if (dist < secondMinMagnitude) {
                        secondMinMagnitude = dist;
                        validDirections[1] = j;
                    }
                }
            }
        }
        if (validDirections[0] == 0 || validDirections[1] == 0)
            offSet.y += 0.2f;
        if (validDirections[0] == 1 || validDirections[1] == 1)
            offSet.y -= 0.2f;
        if (validDirections[0] == 2 || validDirections[1] == 2)
            offSet.x -= 0.2f;
        if (validDirections[0] == 3 || validDirections[1] == 3)
            offSet.x += 0.2f;

        return offSet;
    }
}
