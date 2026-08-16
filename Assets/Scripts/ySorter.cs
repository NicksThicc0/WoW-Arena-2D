using UnityEngine;
using UnityEngine.Rendering;

public class ySorter : MonoBehaviour
{
    [SerializeField] private bool sortOnce;
    [Header("CHOSE ONE OF THE TWO BELOW")]
    [SerializeField] private SortingGroup sortingGroup;
    [SerializeField] private SpriteRenderer spriteRenderer;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (sortOnce)
        {
            sortGroup();
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        sortGroup();
    }



    void sortGroup()
    {


        if (sortingGroup != null)
            sortingGroup.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
        else if (spriteRenderer != null)
            spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
        else
            Debug.LogError("No Sorting Group or Sprite Renderer assigned to ySorter on " + gameObject.name);


    }

    private void OnDrawGizmos()
    {
        sortGroup();
    }
}
