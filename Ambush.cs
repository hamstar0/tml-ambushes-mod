using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Helpers.Tiles;
using HamstarHelpers.Helpers.TModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;


namespace Ambushes {
	abstract partial class Ambush {
		protected static List<Ambush> StaticInstances;
		protected static float TotalWeight = 0f;



		////////////////

		internal static void OnModsLoad() {
			IEnumerable<Type> ambushTypes = ReflectionHelpers.GetAllAvailableSubTypesFromMods( typeof(Ambush) )
				.Where( t => !t.IsAbstract );

			Ambush.StaticInstances = ambushTypes.SafeSelect(
				ambushType => (Ambush)Activator.CreateInstance(ambushType, true)
			).SafeOrderBy( a => {
				Ambush.TotalWeight += a.SpawnWeight;
				return a.SpawnWeight;
			} ).ToList();
		}


		////////////////

		public static Ambush CreateRandomType( int tileX, int tileY ) {
			float rand = TmlHelpers.SafelyGetRand().NextFloat() * (float)Ambush.TotalWeight;
			Ambush randAmbush = null;

			float counted = 0f;
			foreach( Ambush ambush in Ambush.StaticInstances ) {
				if( rand >= counted && rand < ( ambush.SpawnWeight + counted ) ) {
					randAmbush = ambush;
					break;
				}
				counted += ambush.SpawnWeight;
			}

			return randAmbush?.Clone( tileX, tileY );
		}



		////////////////

		public int TileX { get; }
		public int TileY { get; }

		////

		public int TriggeringPlayer { get; private set; } = -1;

		////

		public Vector2 WorldPosition => new Vector2( this.TileX << 4, this.TileY << 4 );



		////////////////

		public Ambush( int tileX, int tileY ) {
			this.TileX = tileX;
			this.TileY = tileY;
		}


		////////////////

		public bool Trigger( Player player ) {
			(int x, int y)? point = TileFinderHelpers.GetNearestTile( this.TileX, this.TileY, TilePattern.AbsoluteAir, 8 );
			if( !point.HasValue ) {
				LogHelpers.Warn( "No empty air for ambush to trigger." );
				return false;
			}

			this.TriggeringPlayer = player.whoAmI;

			return this.OnActivate( point.Value.x, point.Value.y );
		}
	}
}
