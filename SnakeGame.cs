using System.Collections.Generic;

namespace Snake
{
    class SnakeGame
    {
        public GameState State { get; }

        public SnakeGame(GameState state)
        {
            State = state;
        }

        public void Update()
        {
            Position nextHead = MoveSnake(State.Head, State.Direction);

            if (HitWall(nextHead) || HitBody(nextHead))
            {
                State.GameOver = true;
                return;
            }

            State.Body.Add(State.Head);
            State.Head = nextHead;

            if (State.Head.X == State.Berry.X && State.Head.Y == State.Berry.Y)
            {
                State.Score++;
                SpawnBerry();
            }
            else if (State.Body.Count > State.Score)
            {
                State.Body.RemoveAt(0);
            }
        }

        private bool HitWall(Position head)
        {
            return head.X == State.ScreenWidth - 1 || head.X == 0 || head.Y == State.ScreenHeight - 1 || head.Y == 0;
        }

        private bool HitBody(Position head)
        {
            for (int i = 0; i < State.Body.Count; i++)
            {
                if (State.Body[i].X == head.X && State.Body[i].Y == head.Y)
                {
                    return true;
                }
            }

            return false;
        }

        private void TryEatBerry()
        {
            if (State.Head.X == State.Berry.X && State.Head.Y == State.Berry.Y)
            {
                State.Score++;
                SpawnBerry();
            }
        }

        public void SpawnBerry()
        {
            Position newBerry;

            do
            {
                int x = State.Random.Next(1, State.ScreenWidth - 2);
                int y = State.Random.Next(1, State.ScreenHeight - 2);
                newBerry = new Position(x, y);
            }
            while (IsSnakePosition(newBerry));

            State.Berry = newBerry;
        }

        private Position MoveSnake(Position head, Direction direction)
        {
            return direction switch
            {
                Direction.Up => new Position(head.X, head.Y - 1),
                Direction.Down => new Position(head.X, head.Y + 1),
                Direction.Left => new Position(head.X - 1, head.Y),
                Direction.Right => new Position(head.X + 1, head.Y),
                _ => head
            };
        }

        private bool IsSnakePosition(Position position)
        {
            if (position.X == State.Head.X && position.Y == State.Head.Y)
            {
                return true;
            }

            for (int i = 0; i < State.Body.Count; i++)
            {
                if (State.Body[i].X == position.X && State.Body[i].Y == position.Y)
                {
                    return true;
                }
            }
            return false;
        }
    }
}