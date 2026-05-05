using System.Collections;
using UnityEngine;

public class WorldSpaceUITextTimer : MonoBehaviour
{
    [SerializeField] private AnimationCurve spawnCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float duration = 0.5f;

    private Vector3 _startPos = new Vector3(-6.41349983f,-2.85855031f,0);
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(_Despawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    private IEnumerator _Despawn()
    {
        yield return new WaitForSeconds(1f);
            
        Vector3 target = new Vector3(0.1f, 0.1f, 0.1f);
        float shrinkSpeed = 5f; // units per second, adjust for faster/slower
            
        while (Vector3.Distance(transform.localScale, target) > 0.001f)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, target, shrinkSpeed * Time.deltaTime);
            yield return null;
        }
            
        transform.localScale = target;
        Destroy(gameObject);
    }
}
