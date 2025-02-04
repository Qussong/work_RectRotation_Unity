using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class UIManager : MonoBehaviour
{

    [Header("UIs")]
    [SerializeField] GameObject title;
    [SerializeField] GameObject gameEnd;
    [SerializeField] GameObject inGame;
    [SerializeField] GameObject pleaseTakeOutShapeUI;
    Shape shape;

    [Header("Serial Ports")]
    [SerializeField] SerialPort serialPort;
    bool[] sensors = new bool[60];
    int currentSensingIndex = 0;
    int previousSensingIndex = 0;
    byte[] buffer = new byte[1024];

    [Header("Game Succed")]
    [Tooltip("test")]
    [SerializeField] bool isGameSucced = false;

    [SerializeField] bool TestBool = false;


    private void Start()
    {
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
        shape = inGame.GetComponentInChildren<Shape>();
    }

    private void Update()
    {

        if (serialPort != null)
        {
            if (serialPort.IsOpen && isGameSucced == false)
            {
                ReciveSignal();
            }
        }

        if (isGameSucced)
        {
           StartCoroutine(CheckingTakeOutShape());
        }
    }

    void LoadTitleUI()
    {
        isGameSucced = false;
        title.SetActive(true);
        gameEnd.SetActive(false);
        inGame.SetActive(false);
    }

    void LoadGameEndUI() 
    {
        gameEnd.SetActive(true);
        title.SetActive(false);
        inGame.SetActive(false);
        isGameSucced = true;
    }

    void LoadInGameUI() 
    {
        isGameSucced = false;
        gameEnd.SetActive(false);
        title.SetActive(false);
        inGame.SetActive(true);
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

        if (CheckingSensors() == 0)
        {
            LoadPleaseTakeOutShapeUI();
        }
        else
        {
            UnloadPleaseTakeOutShapeUI();
            LoadTitleUI();
            StopAllCoroutines();
        }
    }

    int CheckingSensors() 
    {
        if (TestBool)
            return 1;

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

        previousSensingIndex = currentSensingIndex;

        int Bytes = CheckingSensors();

        if (Bytes > 0) 
        {
            serialPort.Read(buffer, 0, Bytes);

            for (int i = 0; i < Bytes; ++i) 
            {
                if (buffer[i] == 0 || buffer[i] != 1)
                {
                    sensors[i] = false;
                }
                else if(buffer[i] == 1) 
                {
                    sensors[i] = true;
                    currentSensingIndex = i;
                }
            }

            if (shape.Type == Shape.ShapeType.None)
            {
                switch (currentSensingIndex) 
                {
                    case 4:
                        shape.Type = Shape.ShapeType.Sphere;
                        break;
                    case 5:
                        shape.Type = Shape.ShapeType.Rect;
                        break;
                    case 8:
                        shape.Type = Shape.ShapeType.Hexagon;
                        break;
                    default:
                        shape.Type = Shape.ShapeType.None;
                        Debug.Log("Sensing Error : Can Not Check Shape");
                        break;
                }
            }
        }
    }
}
