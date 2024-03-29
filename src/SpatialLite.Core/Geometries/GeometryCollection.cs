﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpatialLite.Core.API;

namespace SpatialLite.Core.Geometries {
	/// <summary>
	/// Represents generic collection of geometry objects.
	/// </summary>
	/// <remarks>All objects should be in the same spatial reference system, but it isn't enforced by this class.</remarks>
	/// <typeparam name="T">The type of objects in the collection</typeparam>
	public class GeometryCollection<T> : Geometry, IGeometryCollection<T> where T : IGeometry {
		#region Private Fields

		private List<T> _geometries;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <c>GeometryCollection</c> class that is empty and has assigned WSG84 coordinate reference system.
		/// </summary>
		public GeometryCollection()
			: base() {
			_geometries = new List<T>();
		}

		/// <summary>
		/// Initializes a new instance of the <c>GeometryCollection</c> class that is empty and has assigned specified coordinate reference system.
		/// </summary>
		/// <param name="srid">The <c>SRID</c> of the coordinate reference system<c>GeometryCollection</c>.</param>
		public GeometryCollection(int srid)
			: base(srid) {
			_geometries = new List<T>();
		}

		/// <summary>
		/// Initializes a new instance of the <c>GeometryCollection</c> class in WSG84 coordinate reference system and fills it with specified geometries.
		/// </summary>
		/// <param name="geometries">Geometry objects to be added to the collection</param>
		public GeometryCollection(IEnumerable<T> geometries)
			: base() {
			_geometries = new List<T>(geometries);
		}

		/// <summary>
		/// Initializes a new instance of the <c>GeometryCollection</c> class in specified coordinate reference system and fills it with specified geometries.
		/// </summary>
		/// <param name="srid">The <c>SRID</c> of the coordinate reference system.</param> 
		/// <param name="geometries">Geometry objects to be added to the collection</param>
		public GeometryCollection(int srid, IEnumerable<T> geometries)
			: base(srid) {
			_geometries = new List<T>(geometries);
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets a value indicating whether the this <see cref="GeometryCollectionBase"/> has Z ordinates set.
		/// </summary>
		/// <remarks>
		/// Is3D returns <c>true</c> if any of the geometries contained in this <c>GeometryCollection</c> has Z ordinate set.
		/// </remarks>
		public override bool Is3D {
			get {
				return _geometries.Any(geometry => geometry.Is3D);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Geometry"/> has M values.
		/// </summary>
		/// <remarks>
		/// IsMeasured returns <c>true</c> if any of the geometries in this <c>GeometryCollection</c> has M value set.
		/// </remarks>
		public override bool IsMeasured {
			get { return _geometries.Any(c => c.IsMeasured); }
		}

		/// <summary>
		/// Gets the list of IGeometry objects in this collection
		/// </summary>
		public List<T> Geometries {
			get {
				return _geometries;
			}
		}

		/// <summary>
		/// Gets collection of geometry obejcts from this GeometryCollection as the collection of IGeometry objects.
		/// </summary>
		IEnumerable<T> IGeometryCollection<T>.Geometries {
			get { return _geometries; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Computes envelope of the <c>GeometryCollection</c> object. The envelope is defined as a minimal bounding box for a geometry.
		/// </summary>
		/// <returns>
		/// Returns an <see cref="Envelope"/> object that specifies the minimal bounding box of the <c>GeometryCollection</c> object.
		/// </returns>
		public override Envelope GetEnvelope() {
			Envelope result = new Envelope();
			foreach (var item in _geometries) {
				result.Extend(item.GetEnvelope());
			}

			return result;
		}

		/// <summary>
		/// Returns  the  closure  of  the  combinatorial  boundary  of  this  geometric  object
		/// </summary>
		/// <returns> the  closure  of  the  combinatorial  boundary  of  this  GeometryCollection</returns>
		public override IGeometry GetBoundary() {
			GeometryCollection<IGeometry> boundary = new GeometryCollection<IGeometry>(this.Srid);
			foreach (var geometry in this.Geometries) {
				boundary.Geometries.Add(geometry.GetBoundary());
			}

			return boundary;
		}

		#endregion
	}
}
