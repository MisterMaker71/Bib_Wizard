using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public enum GameState { inMenu, inGame, In3D}
    public GameState state = GameState.inMenu;
    public static GameManager gameManager;
    bool newGame = false;

    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null)
            gameManager = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GameState.inMenu:
                if (Input.GetKeyDown(KeyCode.Escape))
                    Quit();
                break;
            case GameState.inGame:
            case GameState.In3D:
                if (Input.GetKeyDown(KeyCode.Tab))
                    GoToMenu();
                if (Input.GetKeyDown(KeyCode.Escape))
                    Quit();
                break;
        }
    }
    public void GoToGame(bool NewSave)
    {
        SceneManager.LoadScene("wizard");
        state = GameState.inGame;
        newGame = NewSave;
    }
    public void GoTo3D()
    {
        SceneManager.LoadScene("wizard 3D");
        state = GameState.In3D;
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
        Save("Save1");
        state = GameState.inMenu;
    }
    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void Save(string saveName)
    {
        if (!Directory.Exists(Application.dataPath + "/Saves"))
            Directory.CreateDirectory(Application.dataPath + "/Saves");

        if (!File.Exists(Application.dataPath + "/Saves/" + saveName + ".save"))
            File.Create(Application.dataPath + "/Saves/"+ saveName + ".save");

        MSave mS = new MSave(Wizard.wizard.skillPoints, Wizard.wizard.PointsInManaRegen, Wizard.wizard.PointsInMana, Wizard.wizard.PointsInHealth, Wizard.wizard.PointsInHealthRegen, Wizard.wizard.PointsInMovementSpeed, Wizard.wizard.PointsInSpellSpeed, Wizard.wizard.PointsInDamage,
            new Vector2(Wizard.wizard.transform.position.x, Wizard.wizard.transform.position.y), PointManager.points, Wizard.wizard.mana, Wizard.wizard.health
            );
        string j = JsonUtility.ToJson(mS);
        File.WriteAllText(Application.dataPath + "/Saves/" + saveName + ".save", j);

    }
    public void Load(string saveName, bool NewSave)
    {
        if (Directory.Exists(Application.dataPath + "/Saves"))
        {
            if (File.Exists(Application.dataPath + "/Saves/" + saveName + ".save"))
            {
                MSave ms = JsonUtility.FromJson<MSave>(File.ReadAllText(Application.dataPath + "/Saves/" + saveName + ".save"));
                ms.apply();
            }
        }
    }
    public class MSave
    {
        public int skillPoints = 0;
        public int PointsInManaRegen = 0;
        public int PointsInMana = 0;
        public int PointsInHealth = 0;
        public int PointsInHealthRegen = 0;
        public int PointsInMovementSpeed = 0;
        public int PointsInSpellSpeed = 0;
        public int PointsInDamage = 0;

        public Vector2 playPos;
        public int points;

        public float mana;
        public float health;
        public MSave(int _skillPoints,int _PointsInManaRegen,int _PointsInMana,int _PointsInHealth,int _PointsInHealthRegen,int _PointsInMovementSpeed,int _PointsInSpellSpeed,int _PointsInDamage, Vector2 _playerPos, int _points, float _mana, float _health)
        {
            skillPoints = _skillPoints;
            PointsInManaRegen = _PointsInManaRegen;
            PointsInMana = _PointsInMana;
            PointsInHealth = _PointsInHealth;
            PointsInHealthRegen = _PointsInHealthRegen;
            PointsInMovementSpeed = _PointsInMovementSpeed;
            PointsInSpellSpeed = _PointsInSpellSpeed;
            PointsInDamage = _PointsInDamage;

            playPos = _playerPos;
            points = _points;

            mana = _mana;
            health = _health;

        }
        public void apply()
        {
            if (Wizard.wizard == null)
                Wizard.wizard = FindObjectOfType<Wizard>();
            Wizard.wizard.skillPoints = skillPoints;
            Wizard.wizard.PointsInManaRegen = PointsInManaRegen;
            Wizard.wizard.PointsInMana = PointsInMana;
            Wizard.wizard.PointsInHealth = PointsInHealth;
            Wizard.wizard.PointsInHealthRegen = PointsInHealthRegen;
            Wizard.wizard.PointsInMovementSpeed = PointsInMovementSpeed;
            Wizard.wizard.PointsInSpellSpeed = PointsInSpellSpeed;
            Wizard.wizard.PointsInDamage = PointsInDamage;

            print(playPos);
            Wizard.wizard.transform.position = playPos;
            Wizard.wizard.health = health;
            Wizard.wizard.mana = mana;
            PointManager.points = points;
        }
    }
    private void OnLevelWasLoaded(int level)
    {
        if (state == GameState.inGame)
            Load("Save1", newGame);
        newGame = false;
    }
}
