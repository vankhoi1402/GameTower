using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // Quản lý các State hiện có
    private GameBaseState currentState;
    public bool IsPlaying => currentState == PlayingState;

    public readonly PlayingState PlayingState = new PlayingState();
    public readonly PauseState PauseState = new PauseState();
    public readonly PrepareState PrepareState = new PrepareState();

    private void Awake()
    {
        // Khởi tạo Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        // Lắng nghe yêu cầu đổi State từ UI hoặc các hệ thống khác gửi về
        GameEvents.RequestStateChange += HandleStateChangeRequest;
    }

    private void OnDisable()
    {
        GameEvents.RequestStateChange -= HandleStateChangeRequest;
    }

    private void Start()
    {
        // Khởi tạo trạng thái ban đầu (Ví dụ: Vào thẳng PlayingState hoặc PrepareState)
        SwitchState(PrepareState);
    }

    private void Update()
    {
        // Chạy Update của State hiện tại nếu có logic liên tục
        currentState?.UpdateState(this);
    }

    // Tiếp nhận request đổi State từ cổng Event chung
    private void HandleStateChangeRequest(GameState newState)
    {
        switch (newState)
        {
            case GameState.Playing:
                if (currentState != PlayingState) SwitchState(PlayingState);
                break;
            case GameState.Paused:
                if (currentState != PauseState) SwitchState(PauseState);
                break;
            case GameState.Prepare:
                SwitchState(PrepareState);
                break;
        }
    }

    // Hàm lõi xử lý chuyển đổi giữa các State Class
    private void SwitchState(GameBaseState nextState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = nextState;
        currentState.EnterState(this);
    }
}