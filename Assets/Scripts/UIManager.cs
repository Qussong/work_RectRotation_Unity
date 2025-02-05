using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using static UIManager;

public class UIManager : MonoBehaviour
{
    public enum ShapeType
    {
        None,
        Sphere,
        Rect,
        Hexagon
    }

    public static UIManager Instance;

    [Header("UIs")]
    [SerializeField] GameObject title;
    [SerializeField] GameObject gameEnd;
    [SerializeField] GameObject inGame;
    [SerializeField] GameObject pleaseTakeOutShapeUI;
    [SerializeField] InGameManager gameManager;
    public Shape shape;

    [Header("Serial Ports")]
    [SerializeField] SerialPort serialPort;
    [SerializeField] bool[] sensors = new bool[61];
    int currentLastSensingIndex = 0;
    int currentStartSensingIndex = 0;
    int previousStartSensingIndex = 0;
    int previousLastSensingIndex = 0;
    byte[] buffer = new byte[1024];

    [Header("Game Succed")]
    public bool isGameSucced = false;
    public ShapeType shapeType;

    [Header("Test")]
    public bool TestStartButton = false;

    Coroutine LoadTitleUICoroutine;
    Coroutine TakeOutShapeCoroutine;


    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(Instance);
        }
        
        LoadTitleUI();

        foreach (var Name in SerialPort.GetPortNames()) 
        {
            if (Name == "COM3") 
            {
                serialPort = new SerialPort("COM3", 9600);
            }
        }

        if (serialPort == null || serialPort.PortName != "COM3") 
        {
            Debug.Log("Serial Port Is Null");
            return;
        }

        serialPort.Open();
    }

    private void Update()
    {
        if (TestStartButton && !isGameSucced)
            TestFunction();

        if (serialPort != null)
        {
            if (serialPort.IsOpen && !isGameSucced)
            {
                ReciveSignal();
            }
        }

        if (isGameSucced)
        {
            TakeOutShapeCoroutine = StartCoroutine(CheckingTakeOutShape());
        }
    }

    public void LoadTitleUI()
    {
        isGameSucced = false;
        shapeType = ShapeType.None;
        title.SetActive(true);
        gameEnd.SetActive(false);
        inGame.SetActive(false);

    }

    public void LoadGameEndUI() 
    {
        title.SetActive(false);
        inGame.SetActive(false);
        isGameSucced = true;
        gameEnd.SetActive(true);
        TestStartButton = false;

        LoadTitleUICoroutine = StartCoroutine(WaitForSecondsToLoadTitleUI());
    }

    IEnumerator WaitForSecondsToLoadTitleUI() 
    {
        yield return new WaitForSecondsRealtime(10.0f);
        LoadTitleUI();
        StopCoroutine(LoadTitleUICoroutine);
    }

    void LoadInGameUI() 
    {
        isGameSucced = false;
        gameEnd.SetActive(false);
        title.SetActive(false);
        inGame.SetActive(true);
        gameManager.GameStart();
    }

    void LoadPleaseTakeOutShapeUI() 
    {
        if (!pleaseTakeOutShapeUI.activeSelf)
        {
            pleaseTakeOutShapeUI.SetActive(true);
        }
    }

    void UnloadPleaseTakeOutShapeUI() 
    {
        if (pleaseTakeOutShapeUI.activeSelf)
        {
            pleaseTakeOutShapeUI.SetActive(false);
        }
    }

    IEnumerator CheckingTakeOutShape() 
    {
        yield return new WaitForSecondsRealtime(2.0f);

        if (CheckingSensors() > 0)
        {
            LoadPleaseTakeOutShapeUI();
        }
        else
        {
            LoadTitleUI();
            UnloadPleaseTakeOutShapeUI();
            StopCoroutine(TakeOutShapeCoroutine);
        }
    }

    public int CheckingSensors() 
    {

        if (serialPort == null)
            return 0;

        if (!serialPort.IsOpen)
        {
            Debug.Log("Serial Port Is Not Open");
            return 0;
        }

        return serialPort.ReadByte();
    }

    void ReciveSignal() 
    {
        previousLastSensingIndex = currentLastSensingIndex;
        previousStartSensingIndex = currentStartSensingIndex;

        int Bytes = CheckingSensors();

        if (Bytes > 0)
        {
            serialPort.Read(buffer, 0, Bytes);

            int bitIndex = 0;

            for (int i = 1; i < 9; ++i)
            {
                for (int j = 7; j <= 0; --j)
                {
                    if (i == 1)
                    {
                        j = 4;
                    }

                    bool bit = (buffer[i] & (1 << j)) != 0;

                    if (bitIndex < sensors.Length)
                    {
                        sensors[bitIndex] = bit;

                        if (bit) 
                        {

                            if (currentStartSensingIndex > bitIndex) 
                            {
                                currentStartSensingIndex = bitIndex;
                            }

                            currentLastSensingIndex = bitIndex;
                        }
                    }

                    bitIndex++;
                }
            }

            if (previousLastSensingIndex == currentLastSensingIndex) 
            {
                return;
            }

            if (shapeType == ShapeType.None)
            {
                SettingShape();
            }
            else 
            {
                int Result = currentSensingIndex - previousLastSensingIndex;

                if (Result < 0)
                {
                    shape.UpdateRotateAndLocation(-10);
                }
                else 
                {
                    shape.UpdateRotateAndLocation(10);
                }
            }
        }
    }

    void TestFunction() 
    {
        currentSensingIndex = 1;

        if (shapeType == ShapeType.None)
        {
            SettingShape();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            shape.UpdateRotateAndLocation(50f);
        }
        else if (Input.GetKeyDown(KeyCode.A)) 
        {
            shape.UpdateRotateAndLocation(-50f);
        }
    }

    void SettingShape() 
    {
        switch (currentSensingIndex)
        {
            case 1:
                shapeType = ShapeType.Sphere;
                break;
            case 4:
                shapeType = ShapeType.Rect;
                break;
            case 6:
                shapeType = ShapeType.Hexagon;
                break;
            default:
                shapeType = ShapeType.None;
                break;
        }

        if (shapeType != ShapeType.None)
        {
            LoadInGameUI();
        }
       
    }
}
