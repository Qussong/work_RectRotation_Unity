using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using static UIManager;
using System.Configuration;
using NUnit.Framework.Constraints;
using System.Threading;

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
    [SerializeField] GameEndUI gameEndUI;
    public Shape shape;

    [Header("Serial Ports")]
    [SerializeField] SerialPort serialPort;
    [SerializeField] bool[] sensors = new bool[61];
    int currentLastSensingIndex = 0;
    int currentStartSensingIndex = 0;
    int previousStartSensingIndex = 0;
    int previousLastSensingIndex = 0;
    byte[] buffer = new byte[1024];

    [Header("Game Status")]
    public bool isGameEnd = false;
    public bool IsTimeOut = false;
    public ShapeType shapeType;

    [Header("Shape Setting")]
    [Tooltip("하단의 값을 변경하면 센서 배열의 해당 인덱스 값이 감지된 결과를 바탕으로 설정된 도형으로 인식합니다.")]
    [SerializeField] int RectIndex = 1;
    [SerializeField] int HexagonIndex = 2;

    [Header("Test")]
    public bool TestStartButton = false;

    bool IsPlayTakeOutShapeCoroutine = false;


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
        if (TestStartButton && !isGameEnd)
        {
            TestFunction();
        }

        if (serialPort != null)
        {
            if (serialPort.IsOpen && !isGameEnd)
            {
                ReciveSignal();
            }
        }

        if (isGameEnd)
        {
            if (!IsPlayTakeOutShapeCoroutine)
            {
                StartCoroutine(CheckingTakeOutShape());
            }
        }
    }

    public void LoadTitleUI()
    {
        isGameEnd = false;
        shapeType = ShapeType.None;
        title.SetActive(true);
        gameEnd.SetActive(false);
        inGame.SetActive(false);
        StopAllCoroutines();

    }

    public void LoadGameEndUI() 
    {
        title.SetActive(false);
        gameEnd.SetActive(true);

        if (IsTimeOut)
        {
            IsTimeOut = false;
            gameEndUI.ShowUI(GameEndUI.UIType.MissionFail);
            StartCoroutine(WaitForSecondsToLoadTitleUI());
        }
        else
        {
            if (shapeType == ShapeType.Sphere) 
            {
                gameEndUI.ShowUI(GameEndUI.UIType.SphereEnd);
                inGame.SetActive(false);
                StartCoroutine(CheckNextShapeInsert());
            }
            else
            {
                gameEndUI.ShowUI(GameEndUI.UIType.MissionSucceed);
                StartCoroutine(WaitForSecondsToLoadTitleUI());
            }
        }

        TestStartButton = false;
    }

    IEnumerator CheckNextShapeInsert() 
    {
        yield return new WaitForSecondsRealtime(5.0f);

        float ResetTimer = 0;

        while (CheckingSensors() > 0) 
        {
            ResetTimer += 1.0f;

            yield return new WaitForSecondsRealtime(1.0f);

            if (ResetTimer <= 10.0f) 
            {
                LoadTitleUI();
                ResetTimer = 0.0f;
                yield break;
            }

            if (CheckingSensors() < 0) 
            {
                LoadInGameUI();
                yield break;
            }
        }

        LoadInGameUI();
    }

    IEnumerator WaitForSecondsToLoadTitleUI() 
    {
        yield return new WaitForSecondsRealtime(10.0f);
        LoadTitleUI();
    }

    void LoadInGameUI() 
    {
        SettingShape();
        gameManager.GameStart();
        isGameEnd = false;
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
        IsPlayTakeOutShapeCoroutine = true;

        yield return new WaitForSecondsRealtime(2.0f);

        while (CheckingSensors() > 0)
        {
            LoadPleaseTakeOutShapeUI();
            yield return new WaitForSecondsRealtime(1.0f);
        }

        isGameEnd = false;
        UnloadPleaseTakeOutShapeUI();
        LoadGameEndUI();
        IsPlayTakeOutShapeCoroutine = false;
    }

    public int CheckingSensors() 
    {
        if (serialPort == null)
            return -1;

        if (!serialPort.IsOpen)
        {
            Debug.Log("Serial Port Is Not Open");
            return -1;
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
                Debug.Log("Data Not Changed");
                return;
            }

            if (currentLastSensingIndex - currentStartSensingIndex > 8) 
            {
                Debug.Log("Insert at last two shape");
                return;
            }

            int Result = currentLastSensingIndex - previousLastSensingIndex;

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

    void TestFunction() 
    {
        currentLastSensingIndex = 1;

        if (shapeType == ShapeType.None) 
        {
            LoadInGameUI();
        }

        if (Input.GetKey(KeyCode.D))
        {
            shape.UpdateRotateAndLocation(55f);
        }
        else if (Input.GetKey(KeyCode.A)) 
        {
            shape.UpdateRotateAndLocation(-55f);
        }
    }

    void SettingShape() 
    {
        if (shapeType == ShapeType.Sphere)
        {
            if (currentLastSensingIndex == RectIndex)
                shapeType = ShapeType.Rect;
            else if(currentLastSensingIndex == HexagonIndex)
                shapeType = ShapeType.Hexagon;
            else
                shapeType = ShapeType.None;
        }
        else if (shapeType == ShapeType.None) 
        {
            shapeType = ShapeType.Sphere;
        }
    }
}
