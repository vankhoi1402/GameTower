using UnityEngine;

public class ResetSave : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("Đã xóa toàn bộ save!");
    }
}