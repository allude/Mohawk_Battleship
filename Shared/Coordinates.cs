﻿using System;

namespace MBC.Shared
{
    /// <summary>
    /// Used to represent X and Y components. Has all of the most common operators available for manipulation
    /// and comparison.
    /// </summary>
    public struct Coordinates : IEquatable<Coordinates>, IComparable<Coordinates>
    {
        private int x;
        private int y;

        /// <summary>
        /// Sets the X and Y component to <paramref name="x"/> and <paramref name="y"/> respectively.
        /// </summary>
        /// <param name="x">The X value.</param>
        /// <param name="y">The Y value.</param>
        public Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Gets the X component.
        /// </summary>
        public int X
        {
            get
            {
                return x;
            }
        }

        /// <summary>
        /// Gets the Y component.
        /// </summary>
        public int Y
        {
            get
            {
                return y;
            }
        }

        /// <summary>
        /// Subtracts the value of a set of <see cref="Coordinates"/> from another set of <see cref="Coordinates"/>.
        /// </summary>
        /// <param name="coord1">The <see cref="Coordinates"/> to subtract values from.</param>
        /// <param name="coord2">The <see cref="Coordinates"/> to subtract.</param>
        /// <returns>The resulting <see cref="Coordinates"/>.</returns>
        public static Coordinates operator -(Coordinates coord1, Coordinates coord2)
        {
            return new Coordinates(coord1.x - coord2.x, coord1.y - coord2.y);
        }

        /// <summary>
        /// Compares two <see cref="Coordinates"/> for inequality.
        /// </summary>
        /// <param name="coord1"><see cref="Coordinates"/> to compare.</param>
        /// <param name="coord2"><see cref="Coordinates"/> to compare to the other set.</param>
        /// <returns>true if the given <see cref="Coordinates"/> are inequal, false otherwise.</returns>
        public static bool operator !=(Coordinates coord1, Coordinates coord2)
        {
            return !(coord1 == coord2);
        }

        /// <summary>
        /// Adds the values of two <see cref="Coordinates"/>.
        /// </summary>
        /// <param name="coord1">The <see cref="Coordinates"/> to add to.</param>
        /// <param name="coord2">The <see cref="Coordinates"/> to add.</param>
        /// <returns>The resulting <see cref="Coordinates"/>.</returns>
        public static Coordinates operator +(Coordinates coord1, Coordinates coord2)
        {
            return new Coordinates(coord1.x + coord2.x, coord1.y + coord2.y);
        }

        /// <summary>
        /// Compares two <see cref="Coordinates"/>, determining if a set of <see cref="Coordinates"/> is smaller than the other
        /// in both components.
        /// </summary>
        /// <param name="coord1">The <see cref="Coordinates"/> to compare.</param>
        /// <param name="coord2">The <see cref="Coordinates"/> to compare with.</param>
        /// <returns>true if coord2 has both components smaller than coord1, false otherwise.</returns>
        public static bool operator <(Coordinates coord1, Coordinates coord2)
        {
            return coord1.x < coord2.x && coord1.y < coord2.y;
        }

        /// <summary>
        /// Compares two <see cref="Coordinates"/>, determining if a set of <see cref="Coordinates"/> is smaller than or equal to the other
        /// in both components.
        /// </summary>
        /// <param name="coord1">The <see cref="Coordinates"/> to compare.</param>
        /// <param name="coord2">The <see cref="Coordinates"/> to compare with.</param>
        /// <returns>true if coord2 has both components smaller than or equal to coord1, false otherwise.</returns>
        public static bool operator <=(Coordinates coord1, Coordinates coord2)
        {
            return coord1.x <= coord2.x && coord1.y <= coord2.y;
        }

        /// <summary>
        /// Compares two <see cref="Coordinates"/> for equality.
        /// </summary>
        /// <param name="coord1"><see cref="Coordinates"/> to compare.</param>
        /// <param name="coord2"><see cref="Coordinates"/> to compare to the other set.</param>
        /// <returns>true if the given <see cref="Coordinates"/> are equal, false otherwise.</returns>
        public static bool operator ==(Coordinates coord1, Coordinates coord2)
        {
            return coord1.x == coord2.x && coord1.y == coord2.y;
        }

        /// <summary>
        /// Compares two <see cref="Coordinates"/>, determining if a set of <see cref="Coordinates"/> is greater than the other
        /// in both components.
        /// </summary>
        /// <param name="coord1">The <see cref="Coordinates"/> to compare.</param>
        /// <param name="coord2">The <see cref="Coordinates"/> to compare with.</param>
        /// <returns>true if coord1 has both components larger than coord2, false otherwise.</returns>
        public static bool operator >(Coordinates coord1, Coordinates coord2)
        {
            return coord1.x > coord2.x && coord1.y > coord2.y;
        }

        /// <summary>
        /// Compares two <see cref="Coordinates"/>, determining if a set of <see cref="Coordinates"/> is greater than or equal to the other
        /// in both components.
        /// </summary>
        /// <param name="coord1">The Coordinates to compare.</param>
        /// <param name="coord2">The Coordinates to compare with.</param>
        /// <returns>true if coord1 has both components larger than or equal to coord2, false otherwise.</returns>
        public static bool operator >=(Coordinates coord1, Coordinates coord2)
        {
            return coord1.x >= coord2.x && coord1.y >= coord2.y;
        }

        /// <summary>
        /// Compares the order between these <see cref="Coordinates"/> with another set of <see cref="Coordinates"/>.
        /// </summary>
        /// <param name="coords"><see cref="Coordinates"/> to compare to.</param>
        /// <returns>1 if these <see cref="Coordinates"/> are ordered higher, 0 if they are equal, -1 if these <see cref="Coordinates"/> preceed the other.</returns>
        public int CompareTo(Coordinates coords)
        {
            if (coords == null)
            {
                return 1;
            }
            return (y - coords.y) + (x - coords.x);
        }

        /// <summary>
        /// Determines if these <see cref="Coordinates"/> are equal to another set of <see cref="Coordinates"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Coordinates"/> to compare to.</param>
        /// <returns>true if the <see cref="Coordinates"/> are equal.</returns>
        public bool Equals(Coordinates obj)
        {
            return this == obj;
        }

        /// <summary>
        /// Determines if these <see cref="Coordinates"/> are equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>true if these <see cref="Coordinates"/> are equal to the object.</returns>
        public override bool Equals(object obj)
        {
            return obj is Coordinates && Equals((Coordinates)obj);
        }

        /// <summary>
        /// Gets a hash code for these <see cref="Coordinates"/>.
        /// </summary>
        /// <returns>A hash code integer.</returns>
        public override int GetHashCode()
        {
            int hash = 23;
            hash = hash * 37 + X;
            hash = hash * 37 + Y;
            return hash;
        }

        /// <summary>
        /// Creates a formatted string representation of these <see cref="Coordinates"/> in the format: (x, y)
        /// </summary>
        /// <returns>The string representation of these <see cref="Coordinates"/>.</returns>
        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }
    }
}