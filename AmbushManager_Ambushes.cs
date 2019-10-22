using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;


namespace Ambushes {
	partial class AmbushManager {
		public IEnumerable<Ambush> GetAllAmbushes() {
			IList<Ambush> ambushes;

			lock( AmbushManager.MyLock ) {
				ambushes = new List<Ambush>( this.ArmedAmbushes.Count );

				foreach( IDictionary<int, Ambush> otherTileYs in this.ArmedAmbushes.Values ) {
					foreach( Ambush ambush in otherTileYs.Values ) {
						ambushes.Add( ambush );
					}
				}
			}

			return ambushes;
		}

		public IList<Ambush> GetAmbushesNear( int tileX, int tileY ) {
			int radius = AmbushesMod.Config.AmbushTriggerRadiusTiles;
			int segX = tileX / radius;
			int segY = tileY / radius;
			IList<Ambush> ambushes = new List<Ambush>();
			
			lock( AmbushManager.MyLock ) {
				if( !this.ArmedAmbushSegs.ContainsKey( segX - 1 ) &&
					!this.ArmedAmbushSegs.ContainsKey( segX ) &&
					!this.ArmedAmbushSegs.ContainsKey( segX + 1 ) ) {
					return ambushes;
				}

				int radiusSqr = radius * radius;

				for( int i = segX - 1; i <= segX + 1; i++ ) {
					if( !this.ArmedAmbushSegs.ContainsKey(i) ) { continue; }

					for( int j = segY - 1; j <= segY + 1; j++ ) {
						if( !this.ArmedAmbushSegs[i].ContainsKey(j) ) { continue; }

						AmbushManager.GetAmbushesNearInSeg( tileX, tileY, radiusSqr, this.ArmedAmbushSegs[i][j], ref ambushes );
					}
				}
			}

			return ambushes;
		}


		////////////////

		public void ArmAmbush( Ambush ambush ) {
			int radius = AmbushesMod.Config.AmbushTriggerRadiusTiles;
			
			lock( AmbushManager.MyLock ) {
				this.ArmedAmbushes.Set2D( ambush.TileX, ambush.TileY, ambush );

				IList<Ambush> segAmbushes;
				if( this.ArmedAmbushSegs.TryGetValue2D(ambush.TileX / radius, ambush.TileY / radius, out segAmbushes) ) {
					segAmbushes.Add( ambush );
				} else {
					this.ArmedAmbushSegs.Set2D( ambush.TileX / radius, ambush.TileY / radius, new List<Ambush> { ambush } );
				}
			}
		}

		////

		public void UnarmAmbush( Ambush ambush ) {
			lock( AmbushManager.MyLock ) {
				bool foundY = false;
				bool foundX = this.ArmedAmbushes.ContainsKey( ambush.TileX );
				int radius = AmbushesMod.Config.AmbushTriggerRadiusTiles;

				if( foundX ) {
					foundY = this.ArmedAmbushes[ambush.TileX]
						.Remove( ambush.TileY );
				}

				if( !foundX || !foundY ) {
					if( AmbushesMod.Config.DebugModeInfo ) {
						LogHelpers.WarnOnce( "Isolated ambush found at " + ambush.TileX + ":" + ambush.TileY + " (" + foundX + "," + foundY + ")" );
						Main.NewText( "Isolated ambush found at " + ambush.TileX + ":" + ambush.TileY, Color.Yellow );
					}
				}

				int segX = ambush.TileX / radius;
				int segY = ambush.TileY / radius;

				if( this.ArmedAmbushSegs.ContainsKey(segX) ) {
					this.ArmedAmbushSegs[segX].Remove2D( segY, ambush );
				}
			}
		}

		public void UnarmAllAmbushes() {
			this.ArmedAmbushes.Clear();
			this.ArmedAmbushSegs.Clear();
		}


		////////////////

		public void TriggerAmbush( Ambush ambush, Player player ) {
			if( ambush.Trigger( player ) ) {
				lock( AmbushManager.MyLock ) {
					this.ActiveAmbushes.Add( ambush );
				}
			}
			
			this.UnarmAmbush( ambush );
		}
	}
}
