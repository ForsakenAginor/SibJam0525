using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script : MonoBehaviour 
{


    private Camera mainCamera;
    [SerializeField] private LayerMask movableMask;
    public float moveDistance = 2f;
    public float moveSpeed = 2f;
    public int maxSelectedObjects = 3; 

    private List<GameObject> selectedObjects = new List<GameObject>();
    private Vector3[] originalPositions;
    private GameObject[] gameObjects;

    [SerializeField] private GameObject gameObject1;
    [SerializeField] private GameObject gameObject2;
    [SerializeField] private GameObject gameObject3;

    [SerializeField] private GameObject Loker;
   
    private List<GameObject> generatedSequence = new List<GameObject>();
    

    private void Start()
    {
        
        mainCamera = Camera.main;
        gameObjects = new GameObject[] { gameObject1, gameObject2, gameObject3 };
        originalPositions = new Vector3[gameObjects.Length]; 
        for (int i = 0; i < gameObjects.Length; i++)
        {
            originalPositions[i] = gameObjects[i].transform.position; 
        }
        GenerateNewSequence(); 
    }
    

    private void Update()
    {

        SelectObject();
        
        
    }

    private void GenerateNewSequence()
    {
        generatedSequence = GenerateRandomSequence(maxSelectedObjects);

        
        foreach (GameObject obj in generatedSequence)
        {
            Debug.Log("Сгенерированный объект: " + obj.name);
        }
    }

    private List<GameObject> GenerateRandomSequence(int length)
    {
        List<GameObject> objects = new List<GameObject>();
        HashSet<int> uniqueIndices = new HashSet<int>();
        System.Random random = new System.Random();

        while (uniqueIndices.Count < length)
        {
            int index = random.Next(0, gameObjects.Length);
            uniqueIndices.Add(index);
        }

        foreach (int index in uniqueIndices)
        {
            objects.Add(gameObjects[index]);
        }

        return objects;
    }

    private void SelectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, movableMask))
        {
            GameObject selectedObject = hit.collider.gameObject;
            if(Input.GetMouseButtonDown(0))
            {
                if (!selectedObjects.Contains(selectedObject))
                {
                    

                    if (selectedObjects.Count <= maxSelectedObjects)
                    {

                        selectedObjects.Add(selectedObject);

                        MoveObjectUp(selectedObject);
                    }

                    

                
                    if (selectedObjects.Count == maxSelectedObjects)
                    { 

                        CheckSequences();
                    }
                }

            }      
        }
    }

    private void MoveObjectUp(GameObject obj)
    {
        Vector3 targetPosition = obj.transform.position + Vector3.up * moveDistance;
     

        while (Vector3.Distance(obj.transform.position, targetPosition) > 0.01f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
        }

        obj.transform.position = targetPosition;
    }
    private void UnLock()
    {
        Vector3 targetPosition = Loker.transform.position + Vector3.up * moveDistance;
        while (Vector3.Distance(Loker.transform.position, targetPosition) > 0.01f)
        {
            Loker.transform.position = Vector3.MoveTowards(Loker.transform.position, targetPosition, moveSpeed * Time.deltaTime);

        }
        
    }


    private void CheckSequences()
    {
        bool sequencesMatch = true;
        
        for (int i = 0; i < maxSelectedObjects; i++)
        {
            if (selectedObjects[i] != generatedSequence[i])
            {
                sequencesMatch = false;
                break;
            }
        }
        

        if (sequencesMatch)
        {
            Debug.Log("Получилось! Последовательности совпадают.");
            UnLock();
            ResetSelectedObjects(); 
            
        }
        else
        {
            Debug.Log("Не совпадает. Попробуйте еще раз.");
            
            ResetSelectedObjects(); 
            
        }
    }

    private void ResetSelectedObjects()
    {
        foreach (GameObject obj in selectedObjects)
        {
            
            int index = System.Array.IndexOf(gameObjects, obj);
            if (index >= 0 && index < originalPositions.Length)
            {
                obj.transform.position = originalPositions[index];
            }
        }  
        selectedObjects.Clear();
        
    }
    
}





