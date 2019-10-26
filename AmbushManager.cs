using Ambushes.Ambushes;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.Tiles;
using HamstarHelpers.Helpers.World;
using HamstarHelpers.Services.OverlaySounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;


namespace Ambushes {
	partial class AmbushManager {
		private static object MyLock = new object();

		////////////////

		public static bool IsLocked {
			get {
				if( Monitor.TryEnter( AmbushManager.MyLock ) ) {
					Monitor.Exit( AmbushManager.MyLock );
					return false;
				}
				return true;
			}
		}


		////////////////

		private static void GetAmbushesNearInSeg(
				int tileX,
				int tileY,
				int radiusSqr,
				IList<Ambush> segAmbushes,
				ref IList<Ambush> foundAmbushes ) {
			foreach( Ambush ambush in segAmbushes ) {
				int distX = ambush.TileX - tileX;
				int distY = ambush.TileY - tileY;
				int distSqr = ( distX * distX ) + ( distY * distY );

				if( distSqr < radiusSqr ) {
					foundAmbushes.Add( ambush );
				}
			}
		}



		////////////////

		private IDictionary<int, IDictionary<int, Ambush>> ArmedAmbushes
			= new Dictionary<int, IDictionary<int, Ambush>>();
		private IDictionary<int, IDictionary<int, IList<Ambush>>> ArmedAmbushSegs
			= new Dictionary<int, IDictionary<int, IList<Ambush>>>();

		private ISet<Ambush> ActiveAmbushes = new HashSet<Ambush>();

		private OverlaySound RecentMusic = null;


		////////////////

		public TilePattern ViableAmbushTile { get; }



		////////////////

		public AmbushManager( WorldSize size ) {
			var dungeonWalls = new HashSet<int>( TileWallHelpers.UnsafeDungeonWallTypes );
			dungeonWalls.Add( WallID.LihzahrdBrickUnsafe );

			this.ViableAmbushTile = new TilePattern( new TilePatternBuilder {
				IsSolid = false,
				IsActuated = false,
				MaximumBrightness = 0.25f,
				CustomCheck = ( x, y ) => !dungeonWalls.Contains( Main.tile[x, y].wall )
			} );
		}


		////////////////

		internal void Load( TagCompound tag ) {
			this.ActiveAmbushes.Clear();

			if( !tag.ContainsKey( "active_ambush_count" ) ) {
				return;
			}

			int count = tag.GetInt( "active_ambush_count" );

			for( int i=0; i<count; i++ ) {
				int x = tag.GetInt( "active_ambush_" + i + "_x" );
				int y = tag.GetInt( "active_ambush_" + i + "_y" );
				//bool isEntrapping = tag.GetBool( "active_ambush_" + i + "_istrap" );

				this.ActiveAmbushes.Add( new BrambleCleanupOnlyAmbush( x, y) );
			}
		}

		internal void Save( TagCompound tag ) {
			tag["active_ambush_count"] = this.ActiveAmbushes.Count;

			int i = 0;
			foreach( Ambush ambush in this.ActiveAmbushes ) {
				tag[ "active_ambush_"+i+"_x" ] = ambush.TileX;
				tag[ "active_ambush_"+i+"_y" ] = ambush.TileY;
				//tag[ "active_ambush_"+i+"_istrap" ] = ambush.IsEntrapping;
				i++;
			}
		}


		////////////////

		public int CountTotalAmbushes() {
			lock( AmbushManager.MyLock ) {
				return this.ArmedAmbushes.Count2D();
			}
		}


		////////////////

		internal void Update() {
			foreach( Ambush ambush in this.ActiveAmbushes.ToArray() ) {
				bool isDone;
				ambush.InternalRun( out isDone );

				if( isDone ) {
					ambush.Deactivate();
					this.ActiveAmbushes.Remove( ambush );
				}
			}
		}


		////////////////

		public void PlayMusic( Ambush ambush ) {
			if( this.RecentMusic != null ) {
				if( !this.RecentMusic.IsFadingOut ) {
					return;
				}
			}

			this.RecentMusic = OverlaySound.Create(
				sourceMod: AmbushesMod.Instance,
				soundPath: "Sounds/LowAmbushBGM",
				fadeTicks: 60,
				playDurationTicks: -1,
				customCondition: () => (0.8f, ambush.IsEnded)
			);
			this.RecentMusic.Play();
		}
	}
}
