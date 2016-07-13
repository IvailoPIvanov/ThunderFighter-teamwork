namespace ThunderFighter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Media;
    using System.Threading;
    using Sidebar;
    using ThunderFighter.Common.Enums;
    using ThunderFighter.Common.Utils;
    using ThunderFighter.Controls;
    using ThunderFighter.Models.Buildings.Abstract;
    using ThunderFighter.Models.Common;
    using ThunderFighter.Models.Enemies.Abstract;
    using ThunderFighter.Models.Fighters;
    using ThunderFighter.Models.Fighters.Abstract;
    using ThunderFighter.Models.Screens;    

    internal class Engine
    {
        private readonly WelcomeScreen welcomeScreen;
        private readonly PauseScreen pauseScreen;
        private readonly GameOverScreen gameOverScreen;

        private Field field;
        private Fighter player;
        private GameLevelType gameLevel;

        private Type[] playerClassTypes;
        private Type[] enemyClassTypes;
        private Type[] buildingClassTypes;

        private List<Fighter> players;
        private List<Enemy> enemies;
        private List<Building> buildings;

        private ulong counter;
        private DateTime startTime;

        private SoundPlayer soundPlayer;

        public Engine(Field field)
        {
            this.Field = field;
            this.GameStatus = GameStatusType.Welcome;

            this.Player = new ThunderFighterOne(this.Field, new Point2D(10, 5));

            this.PlayerClassTypes = ReflectiveArray.GetTypeOfDerivedClasses<Fighter>();
            this.EnemyClassTypes = ReflectiveArray.GetTypeOfDerivedClasses<Enemy>();
            this.BuildingClassTypes = ReflectiveArray.GetTypeOfDerivedClasses<Building>();

            this.players = new List<Fighter>();
            this.enemies = new List<Enemy>();
            this.buildings = new List<Building>();

            this.counter = 0;

            this.Scores = new ScoreBoard();
            this.GameCounter = -1;
            this.Timer = TimeSpan.Zero;

            this.welcomeScreen = new WelcomeScreen(this);
            this.pauseScreen = new PauseScreen(this);
            this.gameOverScreen = new GameOverScreen(this);

            this.Menu = new Menu(this.Field, this);
            this.Menu.CreateBase();

            string dirName = AppDomain.CurrentDomain.BaseDirectory;

            this.soundPlayer = new SoundPlayer();
            this.soundPlayer.SoundLocation = dirName + "Common\\Music\\backgroundMusic.wav";
            this.soundPlayer.PlayLooping();

            ConsoleKeyboardHandler.Instance.KeyDown += this.Instance_KeyDown;
        }

        public ScoreBoard Scores { get; set; }

        public TimeSpan Timer { get; set; }

        public int GameCounter { get; set; }

        public Menu Menu { get; set; }

        public GameStatusType GameStatus { get; internal set; }

        public Field Field
        {
            get
            {
                return this.field;
            }

            private set
            {
                this.field = value;
            }
        }

        public Fighter Player
        {
            get
            {
                return this.player;
            }

            private set
            {
                this.player = value;
            }
        }

        public GameLevelType GameLevel
        {
            get
            {
                return this.gameLevel;
            }

            private set
            {
                this.gameLevel = value;
            }
        }

        public Type[] PlayerClassTypes
        {
            get
            {
                return this.playerClassTypes;
            }

            private set
            {
                this.playerClassTypes = value;
            }
        }

        public Type[] EnemyClassTypes
        {
            get
            {
                return this.enemyClassTypes;
            }

            private set
            {
                this.enemyClassTypes = value;
            }
        }

        public Type[] BuildingClassTypes
        {
            get
            {
                return this.buildingClassTypes;
            }

            private set
            {
                this.buildingClassTypes = value;
            }
        }

        public DateTime StartTime
        {
            get
            {
                return this.startTime;
            }

            set
            {
                this.startTime = value;
            }
        }

        public void Start()
        {
            while (true)
            {
                ConsoleKeyboardHandler.Instance.HandleKeys();

                switch (this.GameStatus)
                {
                    case GameStatusType.Welcome:
                        this.Welcome();
                        break;

                    case GameStatusType.Play:
                        this.Play();
                        break;

                    case GameStatusType.Pause:
                        this.Pause();
                        break;

                    case GameStatusType.GameOver:
                        this.GameOver();
                        break;

                    case GameStatusType.TopScores:
                        break;

                    case GameStatusType.Idle:
                        break;

                    default:
                        break;
                }

                Thread.Sleep(50);
            }
        }

        private void Instance_KeyDown(object sender, ConsoleKeyDownEventArgs e)
        {
            if (this.GameStatus == GameStatusType.Play)
            {
                if (e.KeyInfo.Key == ConsoleKey.P)
                {
                    this.GameStatus = GameStatusType.Pause;
                }
            }
        }

        private void Welcome()
        {
            this.Clear();
            this.ResetGame();
            this.welcomeScreen.Show();
            this.GameStatus = GameStatusType.Idle;
        }

        private void ResetGame()
        {
            this.enemies.Clear();
            this.buildings.Clear();

            this.Player = new ThunderFighterOne(this.Field, new Point2D(10, 5), EntityStateType.Strong);

            this.Scores.Lives = 1;
            this.Scores.Score = 0;
            this.GameLevel = GameLevelType.Easy;
            this.GameCounter++;
            this.Timer = TimeSpan.Zero;

            this.Menu.DrawInfo();
        }

        private void Play()
        {
            this.Clear();
            this.Move();
            this.CollisionDetection();
            this.Draw();
            this.counter++;
        }

        private void Clear()
        {
            this.Player.Clear();
            this.EnemiesClear();
            this.BuildingsClear();
            this.BulletsClear();
            this.BombsClear();
            
            // TODO: this.MissilesClear();
        }

        private void Move()
        {
            this.Player.Move();
            this.EnemiesMove();
            this.BuildingsMove();
            this.BulletsMove();
            this.BombsMove();
            
            // TODO: this.MissilesMove();
        }

        private void CollisionDetection()
        {
            this.DetectPlayerBulletCollisions();
            this.DetectPlayerEnemyCollisions();
            this.DetectEnemyBulletCollisions();
            this.DetectEnemyBombCollisions();
            this.DetectBulletBulletCollisions();
            this.DetectPlayerBuildingCollisions();
            
            // TODO: this.DetectEnemyMissileCollisions();
            this.DetectBuildingBombCollisions();
        }

        private void Draw()
        {
            this.PlayerDraw();
            this.EnemiesDraw();
            this.BuildingsDraw();
            this.BulletsDraw();
            this.BombsDraw();
            this.CalculateElapsedTime();

            ScreenBuffer.DrawRectangle(0, 0, this.Field.PlayWidth - 1, this.Field.PlayHeight - 1);

            this.Menu.DrawInfo();
            
            // TODO: this.MissilesDraw();
        }

        private void Pause()
        {
            this.pauseScreen.Show();
            this.GameStatus = GameStatusType.Idle;
        }

        private void GameOver()
        {
            if (this.Scores.HighestScore <= this.Scores.Score)
            {
                (new HighScoreScreen(this)).Show();
            }

            this.gameOverScreen.Show();
            this.GameStatus = GameStatusType.Idle;
        }

        private void CalculateElapsedTime()
        {
            this.Timer = DateTime.Now - this.StartTime;
        }

        private void OnEnemyKilled() // TODO: call it from base class of enemies, buildings, ...
        {
            if (this.Scores.Score > this.Scores.HighestScore)
            {
                this.Scores.HighestScore = this.Scores.Score;
            }

            if (this.GameLevel != GameLevelType.Hard && this.Scores.Score >= 1000)
            {
                this.GameLevel = GameLevelType.Hard;
            }
            else if (this.GameLevel != GameLevelType.Normal && this.Scores.Score >= 500)
            {
                this.GameLevel = GameLevelType.Normal;
            }
        }

        private void DetectEnemyBulletCollisions()
        {
            for (int i = 0; i < this.enemies.Count; i++)
            {
                for (int j = 0; j < this.Player.Bullets.Count; j++)
                {
                    if (this.enemies[i].State == (int)EntityStateType.Strong &&
                        this.Player.Bullets[j].State == (int)EntityStateType.Strong &&
                        this.enemies[i].Body
                            .Exists(enemyPixel => this.Player.Bullets[j].Body.Exists(bulletPixel =>
                                enemyPixel.Coordinate.Y == bulletPixel.Coordinate.Y &&
                                0 <= (bulletPixel.Coordinate.X - enemyPixel.Coordinate.X) &&
                                (bulletPixel.Coordinate.X - enemyPixel.Coordinate.X) <= this.Player.Bullets[j].DeltaX)))
                    {
                        this.enemies[i].State = (int)EntityStateType.HalfDestroyed;
                        this.enemies[i].DeltaX = 0;
                        this.enemies[i].DeltaY = 0;
                        this.enemies[i].IsShootingEnabled = false;

                        this.Player.Bullets[j].State = (int)EntityStateType.HalfDestroyed;
                        this.Player.Bullets[j].DeltaX = 0;
                        this.Player.Bullets[j].DeltaY = 0;

                        this.Scores.Score += this.enemies[i].PointsGainOnDie();
                        this.OnEnemyKilled();

                        break;
                    }
                }
            }
        }

        private void DetectEnemyBombCollisions()
        {
            for (int i = 0; i < this.enemies.Count; i++)
            {
                for (int j = 0; j < this.Player.Bombs.Count; j++)
                {
                    if (this.enemies[i].State == (int)EntityStateType.Strong &&
                        this.Player.Bombs[j].State == (int)EntityStateType.Strong &&
                        this.enemies[i].Body
                            .Exists(enemyPixel => this.Player.Bombs[j].Body.Exists(bombPixel =>
                                enemyPixel.Coordinate.Y == bombPixel.Coordinate.Y &&
                                0 <= (bombPixel.Coordinate.X - enemyPixel.Coordinate.X) &&
                                (bombPixel.Coordinate.X - enemyPixel.Coordinate.X) <= this.Player.Bombs[j].DeltaX)))
                    {
                        this.enemies[i].State = (int)EntityStateType.HalfDestroyed;
                        this.enemies[i].DeltaX = 0;
                        this.enemies[i].DeltaY = 0;
                        this.enemies[i].IsShootingEnabled = false;

                        this.Player.Bombs[j].State = (int)EntityStateType.HalfDestroyed;
                        this.Player.Bombs[j].DeltaX = 0;
                        this.Player.Bombs[j].DeltaY = 0;

                        this.Scores.Score += this.enemies[i].PointsGainOnDie();
                        this.OnEnemyKilled();

                        break;
                    }
                }
            }
        }

        private void DetectPlayerEnemyCollisions()
        {
            for (int i = 0; i < this.enemies.Count; i++)
            {
                if (this.Player.State == (int)EntityStateType.Strong &&
                    this.enemies[i].State == (int)EntityStateType.Strong &&
                    this.enemies[i].Body
                        .Exists(enemyPixel => this.Player.Body.Exists(playerPixel =>
                                enemyPixel.Coordinate.Y == playerPixel.Coordinate.Y &&
                                0 <= (playerPixel.Coordinate.X - enemyPixel.Coordinate.X) &&
                                (playerPixel.Coordinate.X - enemyPixel.Coordinate.X) <= Math.Abs(this.enemies[i].DeltaX))))
                {
                    this.Player.State = (int)EntityStateType.HalfDestroyed;
                    this.Scores.Lives = 0;

                    this.enemies[i].State = (int)EntityStateType.HalfDestroyed;
                    this.enemies[i].DeltaX = 0;
                    this.enemies[i].DeltaY = 0;

                    break;
                }
            }
        }

        private void DetectPlayerBuildingCollisions()
        {
            for (int i = 0; i < this.buildings.Count; i++)
            {
                if (this.Player.State == (int)EntityStateType.Strong &&
                    this.buildings[i].State == (int)EntityStateType.Strong &&
                    this.buildings[i].Body
                        .Exists(buildingPixel => this.Player.Body.Exists(playerPixel =>
                                buildingPixel.Coordinate.Y == playerPixel.Coordinate.Y &&
                                0 <= (playerPixel.Coordinate.X - buildingPixel.Coordinate.X) &&
                                (playerPixel.Coordinate.X - buildingPixel.Coordinate.X) <= Math.Abs(this.enemies[i].DeltaX))))
                {
                    this.Player.State = (int)EntityStateType.HalfDestroyed;
                    this.Scores.Lives = 0;

                    this.buildings[i].State = (int)EntityStateType.HalfDestroyed;

                    break;
                }
            }
        }

        private void DetectPlayerBulletCollisions()
        {
            for (int i = 0; i < this.enemies.Count; i++)
            {
                for (int j = 0; j < this.enemies[i].Bullets.Count; j++)
                {
                    if (this.Player.State == (int)EntityStateType.Strong &&
                    this.enemies[i].Bullets[j].State == (int)EntityStateType.Strong &&
                    this.enemies[i].Bullets[j].Body
                        .Exists(enemyPixel => this.Player.Body.Exists(playerPixel =>
                                enemyPixel.Coordinate.Y == playerPixel.Coordinate.Y &&
                                0 <= (playerPixel.Coordinate.X - enemyPixel.Coordinate.X) &&
                                (playerPixel.Coordinate.X - enemyPixel.Coordinate.X) <= Math.Abs(this.enemies[i].Bullets[j].DeltaX))))
                    {
                        this.Player.State = (int)EntityStateType.HalfDestroyed;
                        this.Scores.Lives = 0;

                        this.enemies[i].Bullets[j].State = (int)EntityStateType.HalfDestroyed;
                        this.enemies[i].Bullets[j].DeltaX = 0;
                        this.enemies[i].Bullets[j].DeltaY = 0;

                        break;
                    }
                }
            }
        }

        private void DetectBulletBulletCollisions()
        {
            for (int i = 0; i < this.enemies.Count; i++)
            {
                for (int j = 0; j < this.enemies[i].Bullets.Count; j++)
                {
                    for (int l = 0; l < this.Player.Bullets.Count; l++)
                    {
                        if (this.Player.Bullets[l].State == (int)EntityStateType.Strong &&
                        this.enemies[i].Bullets[j].State == (int)EntityStateType.Strong &&
                        this.enemies[i].Bullets[j].Body
                            .Exists(enemyPixel => this.Player.Bullets[l].Body.Exists(playerPixel =>
                                    enemyPixel.Coordinate.Y == playerPixel.Coordinate.Y &&
                                    0 <= (playerPixel.Coordinate.X - enemyPixel.Coordinate.X) &&
                                    (playerPixel.Coordinate.X - enemyPixel.Coordinate.X) <= Math.Abs(this.enemies[i].Bullets[j].DeltaX))))
                        {
                            this.Player.Bullets[l].State = (int)EntityStateType.HalfDestroyed;

                            this.enemies[i].Bullets[j].State = (int)EntityStateType.HalfDestroyed;
                            this.enemies[i].Bullets[j].DeltaX = 0;
                            this.enemies[i].Bullets[j].DeltaY = 0;

                            this.Scores.Score += (int)PointsGainType.Ultimate;
                            this.OnEnemyKilled();

                            break;
                        }
                    }
                }
            }
        }

        private void DetectBuildingBombCollisions()
        {
            for (int i = 0; i < this.buildings.Count; i++)
            {
                for (int j = 0; j < this.Player.Bombs.Count; j++)
                {
                    if (this.buildings[i].State == (int)EntityStateType.Strong &&
                        this.Player.Bombs[j].State == (int)EntityStateType.Strong &&
                        this.buildings[i].Body
                            .Exists(buildingPixel => this.Player.Bombs[j].Body.Exists(bombPixel =>
                                (buildingPixel.Coordinate.X == bombPixel.Coordinate.X &&
                                0 <= (bombPixel.Coordinate.Y - buildingPixel.Coordinate.Y) &&
                                (bombPixel.Coordinate.Y - buildingPixel.Coordinate.Y) <= this.buildings[i].Height))))
                    {
                        this.buildings[i].State = (int)EntityStateType.HalfDestroyed;

                        this.Player.Bombs[j].State = (int)EntityStateType.HalfDestroyed;
                        this.Player.Bombs[j].DeltaY = 0;

                        this.Scores.Score += this.buildings[i].PointsGainOnDestroy();
                        this.OnEnemyKilled();

                        break;
                    }
                }
            }
        }

        private void PlayerDraw()
        {
            this.Player.Draw();

            if (this.Player.IsDestroyed)
            {
                this.GameStatus = GameStatusType.GameOver;
            }
        }

        private void EnemiesClear()
        {
            for (int i = 0; i < this.enemies.Count; i++)
            {
                this.enemies[i].Clear();

                if (this.enemies[i].IsDestroyed)
                {
                    if (this.enemies[i].Bullets.All(bullet => bullet.Body.All(pixel => pixel.Coordinate.X < 0)))
                    {
                        Enemy.BulletsEngaged -= (uint)this.enemies[i].Bullets.Count;

                        this.enemies.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        private void EnemiesMove()
        {
            for (int i = 0; i < this.enemies.Count; i++)
            {
                this.enemies[i].Move();

                if (this.enemies[i].Body.All(pixel => pixel.Coordinate.X < 0))
                {
                    this.enemies.RemoveAt(i);
                    i--;
                }
            }

            this.SpawnNewEnemy();
        }

        private void EnemiesDraw()
        {
            for (int i = 0; i < this.enemies.Count; i++)
            {
                this.enemies[i].Draw();
            }
        }

        private void SpawnNewEnemy()
        {
            int indexOfRandomEnemyClass = RandomProvider.Instance.Next(0, this.EnemyClassTypes.Count());

            // used game level
            while (this.enemies.Count < (int)this.GameLevel)
            {
                // TODO: use enemy width and height
                int x = RandomProvider.Instance.Next(this.Field.PlayWidth, 2 * this.Field.PlayWidth);
                int y = RandomProvider.Instance.Next(2, this.Field.PlayHeight - 10);

                var randomEnemy = (Enemy)Activator.CreateInstance(
                    this.EnemyClassTypes[indexOfRandomEnemyClass],
                    this.Field,
                    new Point2D(x, y),
                    EntityStateType.Strong);

                if (this.enemies.Exists(enemy => enemy.Body.Exists(pixel => randomEnemy.Body.Exists(newEnemyPixel => newEnemyPixel.Coordinate == pixel.Coordinate))))
                {
                    continue;
                }
                else
                {
                    this.enemies.Add(randomEnemy);
                    indexOfRandomEnemyClass = RandomProvider.Instance.Next(0, this.EnemyClassTypes.Count());
                }
            }
        }

        private void BuildingsClear()
        {
            for (int i = 0; i < this.buildings.Count; i++)
            {
                this.buildings[i].Clear();

                if (this.buildings[i].IsDestroyed)
                {
                    this.buildings.RemoveAt(i);
                    i--;
                }
            }
        }

        private void BuildingsMove()
        {
            for (int i = 0; i < this.buildings.Count; i++)
            {
                this.buildings[i].Move();

                if (this.buildings[i].Body.All(pixel => pixel.Coordinate.X < 0))
                {
                    this.buildings.RemoveAt(i);
                    i--;
                }
            }

            this.SpawnNewBuilding();
        }

        private void BuildingsDraw()
        {
            for (int i = 0; i < this.buildings.Count; i++)
            {
                this.buildings[i].Draw();
            }
        }

        private void SpawnNewBuilding()
        {
            int indexOfRandomBuildingClass = RandomProvider.Instance.Next(0, this.BuildingClassTypes.Count());

            // used game level, screen flickering avoided
            while (this.buildings.Count < (int)this.GameLevel &&
                this.counter % (ulong)Math.Ceiling(1 / Math.Abs(Building.DeltaX)) == 1)
            {
                // TODO: use building width
                int x = RandomProvider.Instance.Next(
                    this.Field.PlayWidth,
                    this.Field.PlayWidth + (int)(0.5 * this.Field.PlayWidth));
                int y = this.Field.PlayHeight - 1;

                var randomBuilding = (
                    Building)Activator.CreateInstance(
                    this.BuildingClassTypes[indexOfRandomBuildingClass],
                    this.Field,
                    new Point2D(x, y),
                    EntityStateType.Strong);

                if (this.buildings.Exists(building => building.Body.Exists(pixel => randomBuilding.Body.Exists(newBuildingPixel => (newBuildingPixel.Coordinate.Y == pixel.Coordinate.Y) && (newBuildingPixel.Coordinate.X - pixel.Coordinate.X) <= 0))))
                {
                    break;
                }
                else
                {
                    this.buildings.Add(randomBuilding);
                    indexOfRandomBuildingClass = RandomProvider.Instance.Next(0, this.BuildingClassTypes.Count());
                }
            }
        }

        private void BulletsClear()
        {
            for (int i = 0; i < this.Player.Bullets.Count; i++)
            {
                this.Player.Bullets[i].Clear();

                if (this.Player.Bullets[i].IsDestroyed)
                {
                    this.Player.Bullets.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < this.enemies.Count; i++)
            {
                for (int j = 0; j < this.enemies[i].Bullets.Count; j++)
                {
                    this.enemies[i].Bullets[j].Clear();

                    if (this.enemies[i].Bullets[j].IsDestroyed)
                    {
                        this.enemies[i].Bullets.RemoveAt(j);
                        j--;

                        Enemy.BulletsEngaged--;
                    }
                }
            }
        }

        private void BulletsMove()
        {
            for (int i = 0; i < this.Player.Bullets.Count; i++)
            {
                this.Player.Bullets[i].Move();

                if (this.Player.Bullets[i].Body.All(pixel => pixel.Coordinate.X >= this.Field.PlayWidth))
                {
                    this.Player.Bullets.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < this.enemies.Count; i++)
            {
                for (int j = 0; j < this.enemies[i].Bullets.Count; j++)
                {
                    this.enemies[i].Bullets[j].Move();

                    if (this.enemies[i].Bullets[j].Body.All(pixel => pixel.Coordinate.X < 0))
                    {
                        this.enemies[i].Bullets.RemoveAt(j);
                        j--;

                        Enemy.BulletsEngaged--;
                    }
                }
            }
        }

        private void BulletsDraw()
        {
            for (int i = 0; i < this.Player.Bullets.Count; i++)
            {
                this.Player.Bullets[i].Draw();
            }

            for (int i = 0; i < this.enemies.Count; i++)
            {
                for (int j = 0; j < this.enemies[i].Bullets.Count; j++)
                {
                    this.enemies[i].Bullets[j].Draw();
                }
            }
        }

        private void BombsClear()
        {
            for (int i = 0; i < this.Player.Bombs.Count; i++)
            {
                this.Player.Bombs[i].Clear();

                if (this.Player.Bombs[i].IsDestroyed)
                {
                    this.Player.Bombs.RemoveAt(i);
                    i--;
                }
            }
        }

        private void BombsMove()
        {
            for (int i = 0; i < this.Player.Bombs.Count; i++)
            {
                this.Player.Bombs[i].Move();

                if (this.Player.Bombs[i].Body.All(pixel => pixel.Coordinate.Y >= this.Field.PlayWidth || pixel.Coordinate.X >= this.Field.PlayWidth))
                {
                    this.Player.Bombs.RemoveAt(i);
                    i--;
                }
            }
        }

        private void BombsDraw()
        {
            for (int i = 0; i < this.Player.Bombs.Count; i++)
            {
                this.Player.Bombs[i].Draw();
            }
        }
    }
}
