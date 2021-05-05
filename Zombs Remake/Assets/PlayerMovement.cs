using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 dir;
    public Vector2 worldTilePos;
    public Vector2 mousePos;
    public Vector3 clickPos;
    public Vector3 point;
    public Vector3 camOffset;
    public Vector3Int tilePos;
    public float speed = 2;
    public float offset;
    public float worldTileOffset;
    public float cost;
    public float smoothSpeed = 0.5f;
    public float gold = 1000;
    public float desiredZoom = 5;
    public Camera cam;
    public GameObject place;
    public GameObject destroy;
    public ParticleSystem speedEffect;
    public Rigidbody2D rb;
    public Text text;
    public Tilemap map;
    public Tile tile;
    private void FixedUpdate()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, desiredZoom, smoothSpeed);
        Vector3 desiredPos = transform.position + camOffset;
        Vector3 smoothedPos = Vector3.Lerp(cam.transform.position, desiredPos, smoothSpeed);
        cam.transform.position = smoothedPos;
    }
    void Update()
    {
        if (Input.mouseScrollDelta.y == 1 && desiredZoom > 1)
        {
            desiredZoom /= 1.3f;
        }
        if (Input.mouseScrollDelta.y == -1 && desiredZoom < 25)
        {
            desiredZoom *= 1.3f;
        }
        text.text = "Gold: "+gold.ToString();
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        mousePos = Input.mousePosition;

        point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        point = point + new Vector3(offset,offset,offset);
        tilePos = new Vector3Int(Mathf.RoundToInt(point.x * 2), Mathf.RoundToInt(point.y * 2),0);
        worldTilePos = new Vector2(tilePos.x + worldTileOffset, tilePos.y + worldTileOffset) / 2;


        dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = speed * 2;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = speed / 2;
        }
        rb.AddForce(dir * speed * Time.deltaTime);
        if (!(Input.GetButton("Fire1") && Input.GetButton("Fire2")))
        {
            if (Input.GetButton("Fire1"))
            {
                if (map.GetTile(tilePos) == null && gold >= cost)
                {
                    gold -= cost;
                    Debug.Log(tilePos);
                    map.SetTile(tilePos, tile);
                    Instantiate(place, new Vector3(worldTilePos.x,worldTilePos.y,1), new Quaternion(0, 0, 0, 0));
                }
            }
            if (Input.GetButton("Fire2"))
            {
                if (map.GetTile(tilePos) != null)
                {
                    gold += Mathf.Floor(cost/2);
                    Debug.Log(tilePos);
                    map.SetTile(tilePos, null);
                    Instantiate(destroy, new Vector3(worldTilePos.x, worldTilePos.y, 1), new Quaternion(0, 0, 0, 0));
                }
            }
        }
    }
}