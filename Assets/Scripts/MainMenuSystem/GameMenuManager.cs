using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    public static GameMenuManager Instance { get; private set; }

    // Biến trung gian lưu giữ Data mà người chơi vừa click chọn ở Menu UI
    public LevelData SelectedLevelData { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ cho GameManager sống xuyên suốt các scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Hàm này được gọi khi người chơi bấm nút "PLAY" ngoài Menu UI Toolkit
    public void StartBattle(LevelData levelData, string battleSceneName)
    {
        SelectedLevelData = levelData;
        SceneManager.LoadScene(battleSceneName); // Chuyển sang màn chơi chiến đấu
    }

    // Hàm này gọi khi muốn thoát trận quay về lại Menu chính
    public void BackToMenu(string menuSceneName)
    {
        SelectedLevelData = null; // Reset dữ liệu tạm
        SceneManager.LoadScene(menuSceneName);
    }
}