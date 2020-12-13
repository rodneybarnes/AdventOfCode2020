using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AdventOfCode2020.Day12
{
    public class Waypoint
    {
        public int UnitsNorth { get; private set; } = 1;
        public int UnitsEast { get; private set; } = 10;
        public int UnitsSouth { get; private set; } = 0;
        public int UnitsWest { get; private set; } = 0;

        public Waypoint() { }

        public Waypoint(int unitsNorth, int unitsEast, int unitsSouth, int unitsWest)
        {
            UnitsNorth = unitsNorth;
            UnitsEast = unitsEast;
            UnitsSouth = unitsSouth;
            UnitsWest = unitsWest;
        }

        public void MoveNorth(int value)
        {
            if (UnitsSouth >= value)
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

        public void MoveSouth(int value)
        {
            if (UnitsNorth >= value)
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

        public void MoveEast(int value)
        {
            if (UnitsWest >= value)
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

        public void MoveWest(int value)
        {
            if (UnitsEast >= value)
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

        public void RotateLeft(int degrees)
        {
            var waypoint = new Waypoint(UnitsNorth, UnitsEast, UnitsSouth, UnitsWest);  // Because we are mutating this object, we need to preserve the original values.
            var newDirections = new List<string>();

            var allPropertiesWithValue = this.GetType().GetProperties().Where(p => (int)p.GetValue(this) > 0);

            foreach (PropertyInfo property in allPropertiesWithValue)
            {
                var currentDirection = CardinalDirections.Points.FirstOrDefault(p => property.Name.Contains(p));
                string newDirection = CardinalDirections.RotateLeft(degrees, currentDirection);
                newDirections.Add(newDirection);
                this.GetType().GetProperties().FirstOrDefault(p => p.Name.Contains(newDirection))?.SetValue(this, property.GetValue(waypoint));
                if (!newDirections.Any(d => property.Name.Contains(d)))
                    property.SetValue(this, 0);
            }


        }

        public void RotateRight(int degrees)
        {
            var waypoint = new Waypoint(UnitsNorth, UnitsEast, UnitsSouth, UnitsWest);  // Because we are mutating this object, we need to preserve the original values.
            var newDirections = new List<string>();

            var allPropertiesWithValue = this.GetType().GetProperties().Where(p => (int)p.GetValue(this) > 0);

            foreach (PropertyInfo property in allPropertiesWithValue)
            {
                var currentDirection = CardinalDirections.Points.FirstOrDefault(p => property.Name.Contains(p));
                string newDirection = CardinalDirections.RotateRight(degrees, currentDirection);
                newDirections.Add(newDirection);
                this.GetType().GetProperties().FirstOrDefault(p => p.Name.Contains(newDirection))?.SetValue(this, property.GetValue(waypoint));
                if (!newDirections.Any(d => property.Name.Contains(d)))
                    property.SetValue(this, 0);
            }
        }
    }
}
