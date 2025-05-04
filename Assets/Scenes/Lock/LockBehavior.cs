using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockBehavior : MonoBehaviour
{
    private Camera mainCamera;
    public float moveDistance = 2f;
    public float moveSpeed = 2f;
    public int maxSelectedObjects = 3;
    public bool isPuzzeleSolved;
    private List<GameObject> selectedObjects = new List<GameObject>();
    private Vector3[] originalPositions;
    private GameObject[] gameObjects;

    [SerializeField] private GameObject Key1;
    [SerializeField] private GameObject Key2;
    [SerializeField] private GameObject Key3;
    private int currentHighlightedIndex = 0;
    [SerializeField] private GameObject Locker;
    [SerializeField] private GameObject Lock;
    [SerializeField] private GameObject leftGate;
    [SerializeField] private GameObject rightGate;
    private Vector3 key1OriginalPosition;
    private Vector3 key2OriginalPosition;
    private Vector3 key3OriginalPosition;
    [SerializeField]  private float activationDistance;
    private float rotationAngle = 90f;
    public float rotationNewSpeed = 2f;
    private bool isOpening;
    private bool isClosing;
    private List<GameObject> generatedSequence = new List<GameObject>();
    private Vector3 offset = new Vector3(0.5f, 0, 0);
    private Vector3 LeftGateOffset = new Vector3(0.5f, 0, 0);
    private Vector3 RightGateOffset = new Vector3(-0.5f, 0, 0);
    public float rotationSpeed = 15f;


    private void Start()
    {
        key1OriginalPosition = Key1.transform.position;
        key2OriginalPosition = Key2.transform.position;
        key3OriginalPosition = Key3.transform.position;
        mainCamera = Camera.main;
        gameObjects = new GameObject[] { Key1, Key2, Key3 };
        originalPositions = new Vector3[gameObjects.Length];
        for (int i = 0; i < gameObjects.Length; i++)
        {
            originalPositions[i] = gameObjects[i].transform.position;
        }
        GenerateNewSequence();
    }


    private void Update()
    {
        BeginGame();
        SelectObject();
        MoveHighlight();
        EndGame();

    }

    private void GenerateNewSequence()
    {
        generatedSequence = GenerateRandomSequence(maxSelectedObjects);

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

 

    private void MoveObjectUp(GameObject obj)
    {
        Vector3 targetPosition = obj.transform.position + Vector3.up * moveDistance;
        while (Vector3.Distance(obj.transform.position, targetPosition) > 0.01f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        obj.transform.position = targetPosition;
    }
    private IEnumerator UnLock()
    {
        Vector3 targetPosition = Locker.transform.position + Vector3.up * moveDistance;
        Vector3 pivotPoint = Locker.transform.position + offset;

        while (Vector3.Distance(Locker.transform.position, targetPosition) > 0.01f)
        {
            Locker.transform.RotateAround(pivotPoint, Vector3.up, rotationSpeed * Time.deltaTime / 3);
            Locker.transform.position = Vector3.MoveTowards(Locker.transform.position, targetPosition, moveSpeed * Time.deltaTime / 3);
            yield return null;
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
            StartCoroutine(UnLock());
            isPuzzeleSolved = true;
            ResetSelectedObjects();
        }
        else
        {
            Debug.Log("Не совпадает. Попробуйте еще раз.");
            ResetSelectedObjects();
        }
    }



    private void BeginGame()
    {

        if (Vector3.Distance(transform.position, Lock.transform.position) <= activationDistance)
        {
            // Проверяем нажатие клавиши X
            if (Input.GetKeyDown(KeyCode.X))
            {
                Key1.SetActive(true);
                Key2.SetActive(true);
                Key3.SetActive(true);

            }
        }
    }
    private void EndGame()
    {
        if (isPuzzeleSolved)
        {
            Key1.SetActive(false);
            Key2.SetActive(false);
            Key3.SetActive(false);
            Lock.SetActive(false);
            isOpening = true;
        }
        else
        {
            isOpening = false;
        }
        if (isOpening)
        {
            OpenGates();
        }
    }
    private void OpenGates()
    {
        Vector3 targetLeftGatePosition = leftGate.transform.position + Vector3.up * moveDistance;
        Vector3 targetRightGatePosition = rightGate.transform.position + Vector3.up * moveDistance;

        Vector3 pivotLeftGatePoint = leftGate.transform.position + LeftGateOffset;
        Vector3 pivotRightGatePoint = rightGate.transform.position + RightGateOffset;

        float totalRotationAngle = 90f;
        float currentRotationAngleLeft = 0f;
        float currentRotationAngleRight = 0f;

        while (currentRotationAngleLeft < totalRotationAngle || currentRotationAngleRight < totalRotationAngle)
        {
            float rotationStep = rotationSpeed * Time.deltaTime;

            if (currentRotationAngleLeft < totalRotationAngle)
            {
                currentRotationAngleLeft += rotationStep;
                leftGate.transform.RotateAround(pivotLeftGatePoint, Vector3.up, rotationStep);
                leftGate.transform.position = Vector3.MoveTowards(leftGate.transform.position, targetLeftGatePosition, moveSpeed * Time.deltaTime);
            }

            if (currentRotationAngleRight < totalRotationAngle)
            {
                currentRotationAngleRight += rotationStep;
                rightGate.transform.RotateAround(pivotRightGatePoint, Vector3.down, rotationStep);
                rightGate.transform.position = Vector3.MoveTowards(rightGate.transform.position, targetRightGatePosition, moveSpeed * Time.deltaTime);
            }

            
        }

        // Установка окончательной ориентации ворот
        leftGate.transform.rotation = Quaternion.Euler(0, currentRotationAngleLeft, 0);
        rightGate.transform.rotation = Quaternion.Euler(0, -currentRotationAngleRight, 0);

        isPuzzeleSolved = true;
    }
    private void MoveHighlight()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentHighlightedIndex = (currentHighlightedIndex + 1) % gameObjects.Length; 
            HighlightCurrentObject();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            currentHighlightedIndex = (currentHighlightedIndex - 1 + gameObjects.Length) % gameObjects.Length; 
            HighlightCurrentObject();
        }
    }
    

    private void HighlightCurrentObject()
    {
        // Сбросить подсветку всех объектов
        foreach (GameObject obj in gameObjects)
        {
            ResetHighlight(obj);
        }

        // Подсветить текущий объект
        Highlight(gameObjects[currentHighlightedIndex]);
    }

    private void Highlight(GameObject obj)
    {
        // Пример подсветки: изменяем цвет объекта
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.blue; 
        }
    }

    private void ResetHighlight(GameObject obj)
    {
        // Сброс цвета объекта
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white; 
        }
    }

    private void SelectObject()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameObject selectedObject = gameObjects[currentHighlightedIndex];
            if (!selectedObjects.Contains(selectedObject))
            {
                if (selectedObjects.Count < maxSelectedObjects)
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
   

    private void ResetSelectedObjects()
    {
        selectedObjects.Clear();
        foreach (GameObject obj in gameObjects)
        {
            ResetHighlight(obj); // Сбросить подсветку всех объектов
        }
        Key1.transform.position = key1OriginalPosition;
        Key2.transform.position = key2OriginalPosition;
        Key3.transform.position = key3OriginalPosition;
        

    }
   

    
}


