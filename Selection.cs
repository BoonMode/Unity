using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    private static Selection _instance;
    public static Selection Instance { get { return _instance; } }

    List<GameObject> Selected = new List<GameObject>();

    [SerializeField] RectTransform SelectionBox;
    Vector3 SelectionBoxStart;
    [SerializeField] float MinBoxSelectDistance = 5;
    [SerializeField] LayerMask Interactable;
    const int SelectionBoxHeight = 200;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    private void Start()
    {
        SelectionBox.gameObject.SetActive(false);
    }
    private void Update()
    {
        BoxSelect();
    }
    void BoxSelect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectionBoxStart = Input.mousePosition;
        }
        if (Input.GetMouseButton(0) && IsDragging())
        {
            SelectionBox.gameObject.SetActive(true);
            float width = Input.mousePosition.x - SelectionBoxStart.x;
            float height = Input.mousePosition.y - SelectionBoxStart.y;
            SelectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
            SelectionBox.position = new Vector3(SelectionBoxStart.x + width / 2, SelectionBoxStart.y + height / 2, 0);
        }
        if (Input.GetMouseButtonUp(0) && IsDragging())
        {
            Vector3 TopRight = Input.mousePosition;
            Vector3 BottomLeft = SelectionBoxStart;
            Vector3 BottomRight = new Vector3(TopRight.x, BottomLeft.y, 0);
            Vector3 TopLeft = new Vector3(BottomLeft.x, TopRight.y, 0);

            Physics.Raycast(Camera.main.ScreenPointToRay(TopRight), out RaycastHit TopRightHit, Mathf.Infinity, Interactable);
            Physics.Raycast(Camera.main.ScreenPointToRay(BottomLeft), out RaycastHit BottomLeftHit, Mathf.Infinity, Interactable);
            Physics.Raycast(Camera.main.ScreenPointToRay(BottomRight), out RaycastHit BottomRightHit, Mathf.Infinity, Interactable);
            Physics.Raycast(Camera.main.ScreenPointToRay(TopLeft), out RaycastHit TopLeftHit, Mathf.Infinity, Interactable);

            if (AllRaysHit(TopRightHit, BottomRightHit, BottomLeftHit, TopLeftHit))
            {
                GameObject SelectionCollider = CreateSelectionMesh(BottomLeftHit.point, BottomRightHit.point, TopLeftHit.point, TopRightHit.point);

                Destroy(SelectionCollider, .1f);
            }
            SelectionBox.gameObject.SetActive(false);
        }
        else if (Input.GetMouseButtonUp(0) && !IsDragging())
        {
            Deselect();
        }
    }
    GameObject CreateSelectionMesh(Vector3 BottomLeft, Vector3 BottomRight, Vector3 TopLeft, Vector3 TopRight)
    {
        Vector3[] vectices = new Vector3[8];
        Vector2[] uv = new Vector2[8];
        int[] triangles = new int[12];

        vectices[0] = TopLeft;
        vectices[1] = TopRight;
        vectices[2] = BottomLeft;
        vectices[3] = BottomRight;
        vectices[4] = new Vector3(TopLeft.x, TopLeft.y += SelectionBoxHeight, TopLeft.z);
        vectices[5] = new Vector3(TopRight.x, TopRight.y += SelectionBoxHeight, TopRight.z);
        vectices[6] = new Vector3(BottomLeft.x, BottomLeft.y += SelectionBoxHeight, BottomLeft.z);
        vectices[7] = new Vector3(BottomRight.x, BottomRight.y += SelectionBoxHeight, BottomRight.z);

        uv[0] = new Vector2(TopLeft.x, TopLeft.y);
        uv[1] = new Vector2(TopRight.x, TopRight.y);
        uv[2] = new Vector2(BottomLeft.x, BottomLeft.y);
        uv[3] = new Vector2(BottomRight.x, BottomRight.y);
        uv[4] = new Vector2(TopLeft.x, TopLeft.y);
        uv[5] = new Vector2(TopRight.x, TopRight.y );
        uv[6] = new Vector2(BottomLeft.x, BottomLeft.y);
        uv[7] = new Vector2(BottomRight.x, BottomRight.y);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        triangles[3] = 2;
        triangles[4] = 1;
        triangles[5] = 3;

        triangles[6] = 4;
        triangles[7] = 5;
        triangles[8] = 6;

        triangles[9] = 6;
        triangles[10] = 5;
        triangles[11] = 7;

        Mesh mesh = new Mesh();
        mesh.vertices = vectices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        GameObject SelectionCollider = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshCollider), typeof(SelectionBox), typeof(Rigidbody));

        SelectionCollider.GetComponent<Rigidbody>().useGravity = false;

        MeshCollider meshCollider = SelectionCollider.GetComponent<MeshCollider>();
        meshCollider.GetComponent<MeshFilter>().mesh = mesh;
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;
        meshCollider.isTrigger = true;


        return SelectionCollider;
    }
    bool IsDragging()
    {
        if (Mathf.Abs(Input.mousePosition.x - SelectionBoxStart.x) > MinBoxSelectDistance && Mathf.Abs(Input.mousePosition.y - SelectionBoxStart.y) > MinBoxSelectDistance)
        {
            return true;
        }
        return false;
    }
    bool AllRaysHit(RaycastHit Ray1, RaycastHit Ray2, RaycastHit Ray3, RaycastHit Ray4)
    {
        if (Ray1.collider != null && Ray2.collider != null && Ray3.collider != null && Ray4.collider != null)
        {
            return true;
        }
        return false;
    }
    public void CheckAndAdd(GameObject gameObject)
    {
        if (gameObject.tag == "Selectable")
        {
            if (!Selected.Contains(gameObject))
            {
                Selected.Add(gameObject);
                gameObject.GetComponent<MeshRenderer>().materials[0].color = Color.red;
            }
        }
    }
    void Deselect()
    {
        foreach(GameObject selectable in Selected)
        {
            selectable.GetComponent<MeshRenderer>().materials[0].color = Color.green;
        }
        Selected.Clear();
    }
}
