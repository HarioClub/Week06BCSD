using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSystem : MonoBehaviour
{
    [SerializeField] private Wave[] waves;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private SystemTextViewer systemTextViewer;

    private int currentWaveIndex = -1;
    private WaveState state;
    public static WaveSystem Instance { get; private set; }
    public int CurrentWave => currentWaveIndex + 1;
    public int MaxWave => waves.Length;
    public string StateName => state.Name;
    public bool IsFinished => state is ResultState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        ChangeState(new ReadyState());
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void StartWave() => state.StartWave(this);

    public void EnemyRemoved()
    {
        if (enemySpawner.CurrentEnemyCount == 0) state.WaveCleared(this);
    }

    public void GameOver() => Finish(false);

    private void ChangeState(WaveState next)
    {
        state = next;
        Debug.Log($"[웨이브 {CurrentWave}/{MaxWave}] {state.Name}");
    }

    private void Finish(bool won)
    {
        if (IsFinished) return;
        ChangeState(new ResultState(won));
        enemySpawner.StopWave();
        systemTextViewer.ShowResult(won);
    }

    private abstract class WaveState
    {
        public abstract string Name { get; }
        public virtual void StartWave(WaveSystem game) { }
        public virtual void WaveCleared(WaveSystem game) { }
    }

    private sealed class ReadyState : WaveState
    {
        public override string Name => "대기";

        public override void StartWave(WaveSystem game)
        {
            if (game.CurrentWave >= game.MaxWave) return;
            game.currentWaveIndex++;
            game.ChangeState(new BattleState());
            game.enemySpawner.StartWave(game.waves[game.currentWaveIndex]);
        }
    }

    private sealed class BattleState : WaveState
    {
        public override string Name => "전투";

        public override void WaveCleared(WaveSystem game)
        {
            if (game.CurrentWave == game.MaxWave) game.Finish(true);
            else game.ChangeState(new ReadyState());
        }
    }

    private sealed class ResultState : WaveState
    {
        private readonly bool won;
        private bool restarting;
        public override string Name => won ? "클리어" : "패배";

        public ResultState(bool won) => this.won = won;

        public override void StartWave(WaveSystem game)
        {
            if (restarting) return;
            restarting = true;
            SceneManager.LoadScene(game.gameObject.scene.name);
        }
    }
}

[System.Serializable]
public struct Wave
{
    public float spawnTime;
    public int maxEnemyCount;
    public GameObject[] enemyPrefabs;
}
