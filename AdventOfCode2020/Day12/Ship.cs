using System;
using System.Linq;
using System.Reflection;

namespace AdventOfCode2020.Day12
{
    public class Ship
    {
        public string Facing { get; private set; } = CardinalDirections.East;
        public int Degrees { get; private set; } = 0;
        public int UnitsNorth { get; private set; } = 0;
        public int UnitsSouth { get; private set; } = 0;
        public int UnitsEast { get; private set; } = 0;
        public int UnitsWest { get; private set; } = 0;
        public Waypoint Waypoint { get; private set; } = new Waypoint();
        public int ManhattanDistance => CalculateManhattanDistance();

        private int CalculateManhattanDistance()
        {
            var northSouth = Math.Abs(UnitsNorth - UnitsSouth);
            var eastWest = Math.Abs(UnitsEast - UnitsWest);
            return northSouth + eastWest;
        }

        public void NavigateWithCorrectInstructions(string instruction)
        {
            char action = instruction[0];
            int value = int.Parse(instruction.Substring(1));

            switch (action)
            {
                case 'N':
                    Waypoint.MoveNorth(value);
                    break;
                case 'S':
                    Waypoint.MoveSouth(value);
                    break;
                case 'E':
                    Waypoint.MoveEast(value);
                    break;
                case 'W':
                    Waypoint.MoveWest(value);
                    break;
                case 'F':
                    MoveShipToWaypoint(value);
                    break;
                case 'R':
                    Waypoint.RotateRight(value);
                    break;
                case 'L':
                    Waypoint.RotateLeft(value);
                    break;
                default:
                    break;
            }
        }

        public void NavigateWithOriginalInstructions(string instruction)
        {
            char action = instruction[0];
            int value = int.Parse(instruction.Substring(1));

            switch (action)
            {
                case 'N':
                    UnitsNorth += value;
                    break;
                case 'S':
                    UnitsSouth += value;
                    break;
                case 'E':
                    UnitsEast += value;
                    break;
                case 'W':
                    UnitsWest += value;
                    break;
                case 'F':
                    MoveShipForwardNaively(value);
                    break;
                case 'R':
                    RotateRight(value);
                    break;
                case 'L':
                    RotateLeft(value);
                    break;
                default:
                    break;
            }
        }

        private void MoveShipToWaypoint(int value)
        {
            var waypointUnitsWithValue = Waypoint.GetType().GetProperties().Where(p => (int)p.GetValue(Waypoint) > 0);

            foreach (PropertyInfo property in waypointUnitsWithValue)
            {
                string directionToMove = CardinalDirections.Points.FirstOrDefault(p => property.Name.Contains(p));
                int valueToMove = (int)property.GetValue(Waypoint) * value;

                switch (directionToMove)
                {
                    case CardinalDirections.North:
                        MoveNorth(valueToMove);
                        break;
                    case CardinalDirections.East:
                        MoveEast(valueToMove);
                        break;
                    case CardinalDirections.South:
                        MoveSouth(valueToMove);
                        break;
                    case CardinalDirections.West:
                        MoveWest(valueToMove);
                        break;
                    default:
                        break;
                }
            }
        }

        private void MoveNorth(int value)
        {
            if (UnitsSouth > value)
            {
                UnitsSouth -= value;
                return;
            }

            if (UnitsSouth > 0 && UnitsSouth < value)
            {
                UnitsNorth = value - UnitsSouth;
                UnitsSouth = 0;
                return;
            }

            UnitsNorth += value;
        }

        private void MoveSouth(int value)
        {
            if (UnitsNorth > value)
            {
                UnitsNorth -= value;
                return;
            }

            if (UnitsNorth > 0 && UnitsNorth < value)
            {
                UnitsSouth = value - UnitsNorth;
                UnitsNorth = 0;
                return;
            }

            UnitsSouth += value;
        }

        private void MoveEast(int value)
        {
            if (UnitsWest > value)
            {
                UnitsWest -= value;
                return;
            }

            if (UnitsWest > 0 && UnitsWest < value)
            {
                UnitsEast = value - UnitsWest;
                UnitsWest = 0;
                return;
            }

            UnitsEast += value;
        }

        private void MoveWest(int value)
        {
            if (UnitsEast > value)
            {
                UnitsEast -= value;
                return;
            }

            if (UnitsEast > 0 && UnitsEast < value)
            {
                UnitsWest = value - UnitsEast;
                UnitsEast = 0;
                return;
            }

            UnitsWest += value;
        }

        private void MoveShipForwardNaively(int value)
        {
            switch (Facing)
            {
                case CardinalDirections.North:
                    UnitsNorth += value;
                    break;
                case CardinalDirections.South:
                    UnitsSouth += value;
                    break;
                case CardinalDirections.East:
                    UnitsEast += value;
                    break;
                case CardinalDirections.West:
                    UnitsWest += value;
                    break;
                default:
                    break;
            }
        }

        private void RotateRight(int degrees)
        {
            Facing = CardinalDirections.RotateRight(degrees, Facing);
        }

        private void RotateLeft(int degrees)
        {
            Facing = CardinalDirections.RotateLeft(degrees, Facing);
        }
    }
}
