using UnityEngine;

public interface IState 
{
    void OnEnter();   // Chạy 1 lần duy nhất khi vừa vào trạng thái
    void OnUpdate();  // Chạy liên tục mỗi frame (giống Update của Unity)
    void OnExit();    // Chạy 1 lần duy nhất khi rời khỏi trạng thái
}