using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleMovement : MonoBehaviour
{
    public PolygonCollider2D hole2DCollider;
    public PolygonCollider2D ground2DCollider;
    public MeshCollider GeneratedMeshCollider;
    public Collider Ground1Collider;
    public Collider Ground2Collider;

    public Transform Hole;
    public float moveSpeed;
    public Vector2 moveLimits1;
    public Vector2 moveLimits2;

    Mesh GeneratedMesh;
    private float x, y;
    private Vector3 touch, targetPos;


    private void Start()
    {
        GameObject[] AllGameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach(var go in AllGameObjects)
        {
            if (go.layer == LayerMask.NameToLayer("Obstacles"))
            {
                Physics.IgnoreCollision(go.GetComponent<Collider>(), GeneratedMeshCollider, true);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Physics.IgnoreCollision(other, Ground1Collider, true);
        Physics.IgnoreCollision(other, Ground2Collider, true);

        Physics.IgnoreCollision(other, GeneratedMeshCollider, false);
    }
    private void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(other, Ground1Collider, false);
        Physics.IgnoreCollision(other, Ground2Collider, false);

        Physics.IgnoreCollision(other, GeneratedMeshCollider, true);
    }

    //Player Controls
    private void Update()
    {
#if UNITY_EDITOR
        if (!GameController.GameOver && !GameController.NextStage&&!GameController.moveHoleToNext && Input.GetMouseButton(0))
        {
            x = Input.GetAxis("Mouse X");
            y = Input.GetAxis("Mouse Y");
            touch = Vector3.Lerp(Hole.position, Hole.position+new Vector3(x,0f,y),moveSpeed*Time.deltaTime);

            if (!GameController.isSecondStage)
            {
                targetPos = new Vector3(Mathf.Clamp(touch.x, -moveLimits1.x, moveLimits1.x), touch.y, Mathf.Clamp(touch.z, -moveLimits1.y, moveLimits1.y));
            }else if (GameController.isSecondStage)
            {
                targetPos = new Vector3(Mathf.Clamp(touch.x, -moveLimits1.x, moveLimits1.x), touch.y, Mathf.Clamp(touch.z, -moveLimits1.y + 36, moveLimits1.y + 36));
            }               
            Hole.position = targetPos;
        }
#else
        if (!GameController.GameOver && !GameController.NextStage&&!GameController.moveHoleToNext && Input.touchCount>0&&Input.GetTouch(0).phase==TouchPhase.Moved)
        {
            x = Input.GetAxis("Mouse X");
            y = Input.GetAxis("Mouse Y");
            touch = Vector3.Lerp(Hole.position, Hole.position+new Vector3(x,0f,y),moveSpeed*Time.deltaTime);

            if (!GameController.isSecondStage)
            {
                targetPos = new Vector3(Mathf.Clamp(touch.x, -moveLimits1.x, moveLimits1.x), touch.y, Mathf.Clamp(touch.z, -moveLimits1.y, moveLimits1.y));
            }else if (GameController.isSecondStage)
            {
                targetPos = new Vector3(Mathf.Clamp(touch.x, -moveLimits1.x, moveLimits1.x), touch.y, Mathf.Clamp(touch.z, -moveLimits1.y + 36, moveLimits1.y + 36));
            }               
            Hole.position = targetPos;
        }
#endif

    }

    //Creating Mesh with a hole
    private void FixedUpdate()
    {
        if (transform.hasChanged == true)
        {
            transform.hasChanged = false;
            hole2DCollider.transform.position = new Vector2(transform.position.x, transform.position.z);
            MakeHole2D();
            Make3DMeshCollider();
        }
    }
    private void MakeHole2D()
    {
        Vector2[] PointPositions = hole2DCollider.GetPath(0);
        for(int i = 0; i < PointPositions.Length; i++)
        {
            PointPositions[i] += (Vector2)hole2DCollider.transform.position;
        }
        ground2DCollider.pathCount = 2;
        ground2DCollider.SetPath(1, PointPositions);
    }
    private void Make3DMeshCollider()
    {
        if (GeneratedMesh != null)
        {
            Destroy(GeneratedMesh);
        }
            
        GeneratedMesh = ground2DCollider.CreateMesh(true, true);
        GeneratedMeshCollider.sharedMesh = GeneratedMesh;
    }
}
