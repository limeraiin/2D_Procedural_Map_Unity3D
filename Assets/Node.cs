using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<GameObject> nextNodes;
    public List<GameObject> parents;
    [SerializeField] private LineRenderer line1;
    [SerializeField] private LineRenderer line2;

    private void Start()
    {
        if (parents.Count==1)
        {
            
            line1.SetPosition(0,transform.localPosition);
            line1.SetPosition(1,parents[0].transform.localPosition);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        MapGenerator.instance.AddElimination(gameObject);
    }

    public void SecondParent()
    {
        
        line2.SetPosition(0,transform.localPosition);
        line2.SetPosition(1,parents[1].transform.localPosition);
    }
}
