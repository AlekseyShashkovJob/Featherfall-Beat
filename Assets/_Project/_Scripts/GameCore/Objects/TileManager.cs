using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Objects
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField] private ObjectPool _tilePool;
        [SerializeField] private Transform _spawnParent;
        [SerializeField] private RectTransform _containerRect;

        private readonly float _baseTileSpeed = 1000f;
        private readonly int _tilesPerRow = 3;
        private readonly int _tilesPerColumn = 4;

        private float _tileSpeed;
        private float _tileWidth;
        private float _tileHeight;
        private float _spawnInterval;
        private float _spawnTimer;

        private readonly List<Tile> _activeTiles = new();

        private void Start()
        {
            float containerWidth = _containerRect.rect.width;
            float containerHeight = _containerRect.rect.height;

            _tileWidth = containerWidth / _tilesPerRow;
            _tileHeight = containerHeight / _tilesPerColumn;
        }

        public void OnGameStart()
        {
            _spawnTimer = 0f;

            float difficultyMultiplier = GameManager.Instance.GetSpawnIntervalMultiplier();

            _tileSpeed = _baseTileSpeed * difficultyMultiplier;

            _spawnInterval = _tileHeight / _tileSpeed;
            _spawnInterval *= 0.985f;
        }

        private void Update()
        {
            if (!GameManager.Instance.IsGameActive) return;

            _spawnTimer += Time.deltaTime;

            while (_spawnTimer >= _spawnInterval)
            {
                _spawnTimer -= _spawnInterval;
                SpawnRow();
            }

            foreach (var tile in _activeTiles)
            {
                tile.MoveDown(_tileSpeed);
            }

            _activeTiles.RemoveAll(t => t == null || !t.gameObject.activeSelf);
        }

        private void SpawnRow()
        {
            float startY = _containerRect.rect.height / 2 + _tileHeight / 2;

            for (int i = 0; i < _tilesPerRow; i++)
            {
                GameObject tileObj = _tilePool.GetObject(_spawnParent);

                float x = -_containerRect.rect.width / 2 + _tileWidth / 2 + i * _tileWidth;
                tileObj.transform.localPosition = new Vector3(x, startY, 0);

                if (tileObj.TryGetComponent(out RectTransform rt))
                {
                    rt.sizeDelta = new Vector2(_tileWidth, _tileHeight);
                    rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
                }

                Tile tile = tileObj.GetComponent<Tile>();
                tile.Initialize(GetRandomTileType(), _tilePool, _tileHeight);
                _activeTiles.Add(tile);
            }
        }

        private TileType GetRandomTileType()
        {
            int rand = Random.Range(0, 9);

            return rand switch
            {
                0 => TileType.ChickenBlack,
                1 => TileType.ChickenGray,
                2 => TileType.ChickenWhite,
                3 => TileType.Empty1,
                4 => TileType.Empty2,
                5 => TileType.Empty3,
                _ => TileType.Empty1
            };
        }
    }
}