using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using static UIManager;
using System.Configuration;
using NUnit.Framework.Constraints;
using System.Threading;
using DG.Tweening.Core.Easing;
using NUnit.Framework.Interfaces;
using System;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;

public class UIManager : MonoBehaviour
{
    public enum ShapeType
    {
        None,
        Sphere,
        Rect,
        Hexagon,
        Max
    }

    public enum GameState
    {
        Title,
        InGame,
        End
    }

    public static UIManager Instance;

    [Header("UIs")]
    [SerializeField] GameObject title;
    [SerializeField] GameObject gameEnd;
    [SerializeField] GameObject inGame;
    [SerializeField] GameObject pleaseTakeOutShapeUI;
    [SerializeField] GameObject insertNewShapeUI;
    [SerializeField] GameObject WrongShapeUI;
    [SerializeField] InGameManager gameManager;
    [SerializeField] GameEndUI gameEndUI;
    [SerializeField] GameState currnetGameState;
    public Shape shape;

    [Header("Serial Ports")]
    [SerializeField] SerialPort serialPort;
    [SerializeField] bool[] sensors = new bool[61];
    int currentLastSensingIndex = 0;
    int currentStartSensingIndex = 0;
    int previousStartSensingIndex = 0;
    int previousLastSensingIndex = 0;
    public int baudRate = 9600; // 시리얼 통신 속도
    private byte[] receivedBytes = new byte[19]; // 5바이트 데이터 저장용
    private byte[] latestData = new byte[19];    // 최신 데이터 저장용

    [Header("Game Status")]
    public bool isGameEnd = false;
    public bool IsTimeOut = false;
    float CheckingTime = 0.0f;
    public ShapeType shapeType;
    bool isCheckingShape;

    [Header("Shape Setting")]
    [Tooltip("하단의 값을 변경하면 센서 배열의 해당 인덱스 값이 감지된 결과를 바탕으로 설정된 도형으로 인식합니다.")]
    [SerializeField] int RectIndex = 1;
    [SerializeField] int HexagonIndex = 2;
    [SerializeField] string[] portNames;
    [SerializeField] string portName;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        portNames = SerialPort.GetPortNames();

        if (portNames.Length > 0)
        {
            string selectedPort = "";

            foreach (var port in portNames)
            {
                Debug.Log("Port Name : " + port);

                if (port != "COM1" && port != "LP1")
                {
                    selectedPort = port;
                    Debug.Log(port);
                    break;
                }
            }

            if (!string.IsNullOrEmpty(selectedPort))
                portName = selectedPort;
            else
                Debug.Log("Not Connection");

        }
        else
            Debug.LogWarning("No serial ports found.");

        LoadTitleUI();


    }
    async void Start()
    {
        if (string.IsNullOrEmpty(portName))
        {
            Debug.LogError("Port name is empty. Cannot initialize serial port.");
            return;
        }

        InitializeSerialPort();
        await StartListening(); // 비동기 데이터 수신 시작
    }

    private void Update()
    {
        //센서값이 넘어오면 알아서 호출되는 함수
        //에디터에서 테스트용으로 Update에서 초기.
        CheckArray();
        
    }


    public void LoadTitleUI()
    {

        currnetGameState = GameState.Title;
        isGameEnd = false;
        shapeType = ShapeType.None;
        title.SetActive(true);
        gameEnd.SetActive(false);
        inGame.SetActive(false);
        IsTimeOut = false;
        StopAllCoroutines();
    }

    public void LoadGameEndUI()
    {
        StartCoroutine(CheckingTakeOutShape());
        isGameEnd = true;
        currnetGameState = GameState.End;
        title.SetActive(false);
        gameEnd.SetActive(true);
        inGame.SetActive(false);

        if (IsTimeOut)
        {
            gameEndUI.ShowUI(GameEndUI.UIType.MissionFail);
        }
        else if(shapeType == ShapeType.Sphere)
        {
            gameEndUI.ShowUI(GameEndUI.UIType.SphereEnd);
        }
        else
        {
            gameEndUI.ShowUI(GameEndUI.UIType.MissionSucceed);
        }
    }

    void LoadInsertNewShapeUI()
    {
        insertNewShapeUI.SetActive(true);
        isGameEnd = false;
        IsTimeOut = false;
    }

    void UnloadInsertNewShapeUI() 
    {
        insertNewShapeUI.SetActive(false);
    }

    void LoadInGameUI()
    {
        if (shapeType == ShapeType.Max)
            return;

        SendData(true);
        UnloadInsertNewShapeUI();
        currnetGameState = GameState.InGame;
        SettingShape();
        gameManager.GameStart();
        isGameEnd = false;
        gameEnd.SetActive(false);
        title.SetActive(false);
        inGame.SetActive(true);
    }

    void LoadPleaseTakeOutShapeUI()
    {
        pleaseTakeOutShapeUI.SetActive(true);
    }

    void UnloadPleaseTakeOutShapeUI()
    {
        SendData(false);
        pleaseTakeOutShapeUI.SetActive(false);
    }

    void LoadWrongShapeUI()
    {
        WrongShapeUI.SetActive(true);
        StartCoroutine(CheckingTakeOutShape());
    }

    void UnloadWrongShapeUI()
    {
        WrongShapeUI.SetActive(false);
    }

    IEnumerator CheckingTakeOutShape()
    {
        isCheckingShape = true;

        yield return new WaitForSecondsRealtime(5.0f);

        while (true)
        {
            bool isShapeDetected = false;

            foreach (var sensor in sensors)
            {
                if (sensor)
                {
                    isShapeDetected = true;

                    LoadPleaseTakeOutShapeUI();
                    break;

                }
                
            }

            if (!isShapeDetected)
            {
                if (shapeType == ShapeType.Sphere && !IsTimeOut)
                {
                    UnloadPleaseTakeOutShapeUI();
                    LoadInsertNewShapeUI();
                    isCheckingShape = false;
                    yield break;
                }
                else
                {
                    UnloadPleaseTakeOutShapeUI();
                    LoadTitleUI();
                    yield break;
                }
            }

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    void CheckArray()
    {

        previousStartSensingIndex = currentStartSensingIndex;
        previousLastSensingIndex = currentLastSensingIndex;

        bool isAnySensorActive = false;
        int Result = -1;

        for (int i = 0; i < sensors.Length; i++)
        {
            if (sensors[i])
            {
                isAnySensorActive = true;
                currentLastSensingIndex = i;
            }
        }

        Result = previousLastSensingIndex - currentLastSensingIndex;

        if (isGameEnd)
            return;

        if (currnetGameState == GameState.Title && isAnySensorActive)
        {
            UnloadInsertNewShapeUI();
            LoadInGameUI();
        }
        else if (currnetGameState == GameState.InGame)
        {

            if (Result != 0)
            {
                shape.UpdateRotateAndLocation(Result);
            }
        }
        else if (insertNewShapeUI.activeSelf && isAnySensorActive)
        {
            LoadInGameUI();
        }

    }

    void SettingShape()
    {
        if (shapeType == ShapeType.Sphere)
        {
            shapeType = ShapeType.Rect;

            //육각형이 추가되는 경우
            //센싱된 첫 인덱스에 따라 사각형 또는 육각형으로 정하는 코드
            //문제는 현재 센싱되는 인덱스가 서로 너무 유사하고 꽂는 위치에 따라 인덱스가 달라지므로 정확히 판별할수가 없음
            
/*            if (currentLastSensingIndex <= RectIndex)
                shapeType = ShapeType.Rect;
            else
            {
                shapeType = ShapeType.Hexagon;
            }*/
        }
        else if (shapeType == ShapeType.None)
        {
            shapeType = ShapeType.Sphere;
        }
    }

    void InitializeSerialPort()
    {
        try
        {
            serialPort = new SerialPort(portName, baudRate)
            {
                DataBits = 8, //한번에 전송하는 비트 수(Default : 8)
                Parity = Parity.None, //패리티 검출 방식을 사용하지 않음
                StopBits = StopBits.One, //정지비트(비트의 끝을 알림), 정지비트는 1비트 사용
                ReadTimeout = 500, //읽기시간, 500ms 동안 응답이 없다면 예외발생
                WriteTimeout = 500 //쓰기시간, 500ms 동안 응답이 없다면 예외발생
            };

            serialPort.Open();
            Debug.Log("Serial port opened successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to open serial port: {e.Message}");
        }
    }

    async Task StartListening()
    {
        while (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                if (serialPort.BytesToRead >= receivedBytes.Length)
                {
                    // 비동기로 데이터 읽기
                    await Task.Run(() => serialPort.Read(receivedBytes, 0, receivedBytes.Length));
                    SaveData(receivedBytes); // 데이터 저장
                }
                else
                {
                    await Task.Delay(50); // 데이터를 기다리는 동안 CPU 사용량 최소화
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error reading from serial port: {e.Message}");
                break; // 오류 발생 시 루프 종료
            }
        }
    }

    void SaveData(byte[] data)
    {
        if (data == null || data.Length < 9)
        {
            Debug.LogError("수신된 데이터 오류(데이터가 짧습니다.)");
            return;
        }

        Array.Copy(data, latestData, data.Length);

        if (latestData[0] == 0xFA)
        {
            int bitIndex = 0;

            for (int i = 9; i > 0; i--)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (i == 1) 
                    {
                        if (j > 4)
                            continue;
                    }

                    bool bBit = (latestData[i] & (1 << j)) != 0;

                    if (bitIndex < sensors.Length)
                    {
                        sensors[bitIndex] = bBit;

                        //처음 센싱된 인덱스를 확인하는 코드
                        //필요하면 사용
/*                        if (bBit && bitIndex < currentStartSensingIndex)
                        {
                            currentStartSensingIndex = bitIndex;
                        }*/
                    }

                    bitIndex++;
                }
                
            }

            CheckArray();
        }
    }

    void SendData(bool IsStart) 
    {
        byte[] Data = new byte[2];

        if (serialPort != null && serialPort.IsOpen)
        {
            Data[0] = 0xFA;

            if (IsStart)
                Data[1] = 0x41;
            else
                Data[1] = 0X42;

            try
            {
                serialPort.Write(Data, 0, Data.Length);
                Debug.Log($"Sent {Data.Length} bytes: {BitConverter.ToString(Data)}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to send data: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning("Serial port is not open or not connected.");
        }
    }
}
