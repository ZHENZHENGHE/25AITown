using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BezierArrows : MonoBehaviour
{
    #region Public Fields
    [Tooltip("The Prefab of arrow head")]
    public GameObject ArrowHeadPrefab;
    [Tooltip("The Prefab of arrow node")]
    public GameObject ArrowNodePrefab;
    [Tooltip("the Number of arrow nodes")]
    public int arrowNodeNum;
    [Tooltip("The scale multiplier for arrow nodes")]
    public float scaleFactor = 1f;
    [Tooltip("Please do not assign values")]
    public Transform ShowObj;
    [Tooltip("Please do not assign values")]
    public bool IsActive = false;
    [Tooltip("Please do not assign values")]
    public Vector2 Position = Vector2.zero;
    #endregion
    #region Private Fields
    /// <summary>
    /// The position of P0 (The arrow emitter point)
    /// </summary>
    private RectTransform origin;
    /// <summary>
    /// The list of arrow nodes transform
    /// </summary>
    private List<RectTransform> arrowNodes = new List<RectTransform>();
    /// <summary>
    /// The list of control points
    /// </summary>
    private List<Vector2> controlPoints = new List<Vector2>();
    /// <summary>
    /// The factors to determine the position of control point P1，P2  控制曲线的形态
    /// </summary>
    private readonly List<Vector2> controlPointFactors = new List<Vector2>() { new Vector2(-0.3f, 0.8f), new Vector2(0.1f, 1.4f) };
    #endregion
    #region Private Methods
    /// <summary>
    /// Executes when the gameObject instantiates.
    /// </summary>
    private void Start()
    {
        //Gets position of the arrow emitter point
        this.origin = this.GetComponent<RectTransform>();
        for (int i = 0; i < this.arrowNodeNum; i++)
        {
            GameObject arrowNode = Instantiate(this.ArrowNodePrefab, ShowObj);
            arrowNode.SetActive(false);
            arrowNode.transform.SetAsFirstSibling();
            this.arrowNodes.Add(arrowNode.GetComponent<RectTransform>());
        }
        GameObject arrowHead = Instantiate(this.ArrowHeadPrefab, ShowObj);
        arrowHead.SetActive(false);
        arrowHead.transform.SetAsFirstSibling();
        this.arrowNodes.Add(arrowHead.GetComponent<RectTransform>());
        //Hides the arrow nodes
        this.arrowNodes.ForEach(a => a.GetComponent<RectTransform>().position = new Vector2(-1000, -1000));
        //Initializes the control points list
        for (int i = 0; i < 4; i++)
        {
            this.controlPoints.Add(Vector2.zero);
        }
    }
    /// <summary>
    /// Executes every frame
    /// </summary>
    private void Update()
    {
        //P0 is at the arrow emitter point
        //this.controlPoints[0] = new Vector2(this.origin.position.x, this.origin.position.y);
        this.controlPoints[0] = Position;
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //P3 is at the mouse position 
        this.controlPoints[3] = new Vector2(mouse.x, mouse.y);
        //P1,P2 determines by P0 and P3
        //P1 = P0 + (P3 - P0) * Vector2(-0.3f, 0.8f)
        //P2 = P0 + (P3 - P0) * Vector2(0.1f, 1.4f)
        this.controlPoints[1] = this.controlPoints[0] + (this.controlPoints[3] - this.controlPoints[0]) * this.controlPointFactors[0];
        this.controlPoints[2] = this.controlPoints[0] + (this.controlPoints[3] - this.controlPoints[0]) * this.controlPointFactors[1];
        for (int i = 0; i < this.arrowNodes.Count; i++)
        {
            // Calculates t.
            var t = Mathf.Log(1f * i / (this.arrowNodes.Count - 1) + 1f, 2f);
            //Cubic Bezier curve
            //B(T) = (1 - t) ^ * P0 + 3 * (1 - t) ^ 2 * t * P1 + 3 * (1 - t) * t ^ 2 * P2 + t ^ 3 * P3
            this.arrowNodes[i].position = Mathf.Pow(1 - t, 3) * this.controlPoints[0] + 3 * Mathf.Pow(1 - t, 2) * t * this.controlPoints[1] + 3 * (1 - t) * Mathf.Pow(t, 2) * this.controlPoints[2] + Mathf.Pow(t, 3) * this.controlPoints[3];
            //Calculates rotations for each arrow node
            if (i > 0)
            {
                var euler = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, this.arrowNodes[i].position - this.arrowNodes[i - 1].position));
                this.arrowNodes[i].rotation = Quaternion.Euler(euler);
            }
            this.arrowNodes[i].gameObject.SetActive(IsActive);
            // Calculates scales for each arrow node
            //var scale = this.scaleFactor * (1f - 0.03f * (this.arrowNodes.Count - 1 - i));
            //this.arrowNodes[i].localScale = new Vector3(scale, scale, 1f);
        }
        //The first arrow nodes rotation
        this.arrowNodes[0].transform.rotation = this.arrowNodes[1].transform.rotation;
    }
    #endregion
}