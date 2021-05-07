using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Transform canvas;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private int multipleChildOccurence;
    private GameObject spawn;
    
    
    public List<GameObject> OverlappingNodes;

    private float _topLimit;
    private float _bottomLimit;
    private float _leftLimit;
    private float _rightLimit;
    
    public static MapGenerator instance;
 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    

    private void Start()
    {
        var pos = canvas.position;
        var scale = canvas.lossyScale;
        _topLimit = pos.y + scale.y / 2;
        _bottomLimit= pos.y-scale.y / 2;
        _leftLimit = pos.x - scale.x / 2;
        _rightLimit = pos.x + scale.x / 2-2;
        
        
        
        
        spawn= (GameObject) Resources.Load("Node");
        Vector2 currentPos = startPoint.position;
        GameObject startNode=Instantiate(spawn, currentPos, Quaternion.identity,canvasTransform);

        StartCoroutine(GeneratePath(currentPos, startNode));
    }

    private IEnumerator GeneratePath(Vector2 pos,GameObject node)
    
    {
        Debug.Log(pos.x+" "+pos.y);
        yield return new WaitForSecondsRealtime(0.25f);
        if (pos.x>_rightLimit)
        {
            Debug.Log("exceeded");
            yield return new WaitForSecondsRealtime(0.25f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            yield break;
        }
        if(node==null)yield break;
        
        bool up = (_topLimit-pos.y>2);
        bool down = (pos.y - _bottomLimit > 2);
        int nextY;
        if (up && down)
        {
            int cnt = Random.Range(-1, 1)*2;
            if (cnt == 0) cnt = 2;
            nextY=(int)(cnt+pos.y);
            int rand = Random.Range(1, 10);
            int count;
            if (rand > 7 && multipleChildOccurence != 0)
            {
                count = 2;
                multipleChildOccurence--;
            }
            else count = 1;

            Debug.Log(count);
            for (int i = 0; i < count; i++)
            {
                Configure();
                nextY = (int)((0-cnt)+pos.y);
            }
        }
        else if (up && !down)
        {
            nextY = (int)(Random.Range(0,  2) * 2+pos.y);
            Configure();
        }
        else
        {
            nextY=(int)(Random.Range(-2, 0)*2+pos.y);
            Configure();
               
        }

        void Configure()
        {
            Vector2 newPos=new Vector2(pos.x + 2, nextY);
            GameObject newNode=Instantiate(spawn, newPos,Quaternion.identity,canvasTransform);
            node.GetComponent<Node>().nextNodes.Add(newNode);
            newNode.GetComponent<Node>().parents.Add(node);
            StartCoroutine(GeneratePath(newPos,newNode));
        }
    }
    
    public void AddElimination(GameObject obj)
    {
        OverlappingNodes.Add(obj);
        if (OverlappingNodes.Count == 2)
        {
            var parent0 = OverlappingNodes[0].GetComponent<Node>().parents[0].GetComponent<Node>();
            parent0.nextNodes.Remove(OverlappingNodes[0]);
            parent0.nextNodes.Add(OverlappingNodes[1]);
            var node1 = OverlappingNodes[1].GetComponent<Node>();
            node1.parents.Add(parent0.gameObject);
            node1.SecondParent();
            Destroy(OverlappingNodes[0]);
            OverlappingNodes=new List<GameObject>();
        }
    }
    
}