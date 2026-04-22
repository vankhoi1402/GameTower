public class StateMachine
{
    public IState CurrentState { get; private set; }

    // Hàm để chuyển đổi trạng thái
    public void ChangeState(IState newState)
    {
        CurrentState?.OnExit(); // Thoát trạng thái cũ

        CurrentState = newState;

        CurrentState?.OnEnter(); // Vào trạng thái mới
    }

    // Gọi hàm này trong Update() của BaseUnit
    public void Update()
    {
        CurrentState?.OnUpdate();
    }
}