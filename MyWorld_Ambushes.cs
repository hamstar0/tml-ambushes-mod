using HamstarHelpers.Helpers.DotNET.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes {
	partial class AmbushesWorld : ModWorld {
		private void InitializeAmbushes() {
			for( int i=0; i<this.MaxAmbushes; i++ ) {
				this.CreateRandomAmbushAsync();
			}
		}

		////////////////

		private void UpdateAmbushes() {
			var mymod = (AmbushesMod)this.mod;

			if( this.AmbushRegenDelay++ < mymod.Config.AmbushRegenTickRate ) {
				return;
			}

			this.AmbushRegenDelay = 0;

			if( this.Ambushes.Count < this.MaxAmbushes ) {
				this.CreateRandomAmbushAsync();
			}
		}


		////////////////

		private void CreateRandomAmbushAsync() {
			var cts = new CancellationTokenSource();

			Task.Factory.StartNew( () => {

			}, cts.Token );
		}


		////////////////

		private bool GetRandomOpenAmbushPoint( out (int TileX, int TileY) randTileCenter, int maxAttempts ) {
			int attempts = 0;

			do {
				randTileCenter = this.GetRandomAmbushPoint( maxAttempts );

				if( !this.HasNearbyAmbushes( randTileCenter.TileX, randTileCenter.TileY) ) {
					return true;
				}
			} while( attempts++ < maxAttempts );

			return false;
		}


		private (int TileX, int TileY) GetRandomAmbushPoint( int maxAttempts ) {
			int attempts = 0;
			int randTileX, randTileY;

			do {
				randTileX = Main.rand.Next( 64, Main.maxTilesX - 64 );
				randTileY = Main.rand.Next( (int)Main.worldSurface, Main.maxTilesY - 220 );

				if( !Ambush.CheckForAmbushElegibility( ref randTileX, ref randTileY ) ) {
					break;
				}
			} while( attempts++ < maxAttempts );

			return (randTileX, randTileY);
		}


		private bool HasNearbyAmbushes( int tileX, int tileY ) {
			int minTileDist = AmbushesMod.Instance.Config.MinimumAmbushTileSpacing;
			int minTileDistSqt = minTileDist * minTileDist;

			foreach( (int otherTileX, IDictionary<int, Ambush> otherTileYs) in this.Ambushes ) {
				foreach( (int otherTileY, Ambush ambush) in otherTileYs ) {
					int xDist = otherTileX - tileX;
					int yDist = otherTileY - tileY;
					int xDistSqr = xDist * xDist;
					int yDistSqr = yDist * yDist;

					if( (xDistSqr + yDistSqr) < minTileDistSqt ) {
						return true;
					}
				}
			}

			return false;
		}


		////////////////

		private void SpawnAmbush( int tileX, int tileY ) {
			var mymod = AmbushesMod.Instance;

			lock( AmbushesWorld.MyLock ) {
				if( !this.Ambushes.ContainsKey( tileX ) ) {
					this.Ambushes[ tileX ] = new Dictionary<int, Ambush>();
				}

				this.Ambushes[ tileX ][ tileY ] = new Ambush( tileX, tileY );
			}
		}
	}
}
