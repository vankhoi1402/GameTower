// Định danh tất cả âm thanh trong game để tránh dùng chuỗi String
public enum SoundType
{
    None = 0,
    // UI Sounds (2D)
    UI_Click = 1,
    UI_OpenWindow = 2,
    UI_CloseWindow = 3,


    // Battle Sounds (3D)
    Battle_Place_Unit = 9,
    Battle_SwordSlash = 10,
    Battle_ArrowShoot = 11,
    Battle_MagicFireball = 12,
    Battle_UnitDie = 13,

    //
    SO_BGM_Menu= 14,
    SO_BGM_Battle= 15,
    SO_BGM_GameWin= 16,
}

// Định danh các kênh âm thanh trong bảng Cài đặt Settings
public enum AudioChannel
{
    Master,
    BGM,
    UI_SFX,
    Battle_SFX
}