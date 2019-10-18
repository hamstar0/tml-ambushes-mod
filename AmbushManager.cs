using Ambushes.Ambushes;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Terraria;
using Terraria.ModLoader.IO;


namespace Ambushes {
	partial class AmbushManager {
		private static object MyLock = new object();

		////////////////

		public static bool IsLocked => !Monitor.TryEnter( AmbushManager.MyLock );



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



		////////////////

		public int TotalAmbushes {
			get {
				lock( AmbushManager.MyLock ) {
					return this.ArmedAmbushes.Count2D();
				}
			}
		}



		////////////////

		public AmbushManager( WorldSize size ) { }


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
		
		internal void Update() {
			foreach( Ambush ambush in this.ActiveAmbushes.ToArray() ) {
				if( ambush.Run() ) {
					ambush.OnDeactivate();
					this.ActiveAmbushes.Remove( ambush );
				}
			}
		}
	}
}
