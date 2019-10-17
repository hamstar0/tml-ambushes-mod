using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using HamstarHelpers.Helpers.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using Terraria;


namespace Ambushes {
	class AmbushManager {
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

		private IDictionary<int, IDictionary<int, Ambush>> Ambushes
			= new Dictionary<int, IDictionary<int, Ambush>>();
		private IDictionary<int, IDictionary<int, IList<Ambush>>> AmbushSegs
			= new Dictionary<int, IDictionary<int, IList<Ambush>>>();



		////////////////

		public int Count {
			get {
				lock( AmbushManager.MyLock ) {
					return this.Ambushes.Count2D();
				}
			}
		}



		////////////////

		public AmbushManager( WorldSize size ) { }


		////////////////

		public IEnumerable<Ambush> GetAllAmbushes() {
			lock( AmbushManager.MyLock ) {
				foreach( IDictionary<int, Ambush> otherTileYs in this.Ambushes.Values ) {
					foreach( Ambush ambush in otherTileYs.Values ) {
						yield return ambush;
					}
				}
			}
		}

		public IList<Ambush> GetAmbushesNear( int tileX, int tileY ) {
			int radius = AmbushesMod.Config.AmbushTriggerRadiusTiles;
			int segX = tileX / radius;
			int segY = tileY / radius;
			IList<Ambush> ambushes = new List<Ambush>();

			lock( AmbushManager.MyLock ) {
				if( !this.AmbushSegs.ContainsKey( segX - 1 ) &&
					!this.AmbushSegs.ContainsKey( segX ) &&
					!this.AmbushSegs.ContainsKey( segX + 1 ) ) {
					return ambushes;
				}

				int radiusSqr = radius * radius;

				for( int i = segX - 1; i <= segX + 1; i++ ) {
					if( !this.AmbushSegs.ContainsKey(i) ) { continue; }

					for( int j = segY - 1; j <= segY + 1; j++ ) {
						if( !this.AmbushSegs[i].ContainsKey(j) ) { continue; }

						AmbushManager.GetAmbushesNearInSeg( tileX, tileY, radiusSqr, this.AmbushSegs[i][j], ref ambushes );
					}
				}
			}

			return ambushes;
		}


		////////////////

		public void AddAmbush( Ambush ambush ) {
			int radius = AmbushesMod.Config.AmbushTriggerRadiusTiles;

			lock( AmbushManager.MyLock ) {
				this.Ambushes.Set2D( ambush.TileX, ambush.TileY, ambush );

				IList<Ambush> segAmbushes;
				if( this.AmbushSegs.TryGetValue2D(ambush.TileX / radius, ambush.TileY / radius, out segAmbushes) ) {
					segAmbushes.Add( ambush );
				} else {
					this.AmbushSegs.Set2D( ambush.TileX / radius, ambush.TileY / radius, new List<Ambush> { ambush } );
				}
			}
		}

		////

		public void RemoveAmbush( Ambush ambush ) {
			lock( AmbushManager.MyLock ) {
				bool foundY = false;
				bool foundX = this.Ambushes.ContainsKey( ambush.TileX );
				int radius = AmbushesMod.Config.AmbushTriggerRadiusTiles;

				if( foundX ) {
					foundY = this.Ambushes[ambush.TileX].Remove( ambush.TileY );
				}

				if( !foundX || !foundY ) {
					if( AmbushesMod.Config.DebugModeInfo ) {
						LogHelpers.WarnOnce( "Isolated ambush triggered at " + ambush.TileX + ":" + ambush.TileY + " (" + foundX + "," + foundY + ")" );
						Main.NewText( "Isolated ambush triggered at " + ambush.TileX + ":" + ambush.TileY, Color.Yellow );
					}
				}

				int segX = ambush.TileX / radius;
				int segY = ambush.TileY / radius;

				if( this.AmbushSegs.ContainsKey(segX) ) {
					this.AmbushSegs[segX].Remove2D( segY, ambush );
				}
			}
		}

		public void ClearAllAmbushes() {
			this.Ambushes.Clear();
			this.AmbushSegs.Clear();
		}


		////////////////

		public void TriggerAmbush( Ambush ambush, Player player ) {
			ambush.Trigger( player );

			this.RemoveAmbush( ambush );
		}
	}
}
