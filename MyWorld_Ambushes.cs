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
			this.Ambushes.Clear();

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
				Ambush ambush;

				lock( AmbushesWorld.MyLock ) {
					if( this.GetRandomOpenAmbushPoint( out ambush, 1000 ) ) {
						this.SpawnAmbush( ambush );
					}
				}
			}, cts.Token );
		}


		////////////////

		private bool GetRandomOpenAmbushPoint( out Ambush ambush, int maxAttempts ) {
			int attempts = 0;

			do {
				ambush = this.GetRandomAmbushPoint( maxAttempts );

				if( !this.HasNearbyAmbushes(ambush.TileX, ambush.TileY) ) {
					return true;
				}
			} while( attempts++ < maxAttempts );

			return false;
		}


		private Ambush GetRandomAmbushPoint( int maxAttempts ) {
			int attempts = 0;
			int randTileX, randTileY;
			IList<(int TileX, int TileY)> edgeTiles;

			do {
				randTileX = Main.rand.Next( 64, Main.maxTilesX - 64 );
				randTileY = Main.rand.Next( (int)Main.worldSurface, Main.maxTilesY - 220 );

				if( !Ambush.CheckForAmbushElegibility( randTileX, randTileY, out edgeTiles ) ) {
					break;
				}
			} while( attempts++ < maxAttempts );

			return new Ambush( randTileX, randTileY, edgeTiles );
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

		private void SpawnAmbush( Ambush ambush ) {
			if( !this.Ambushes.ContainsKey(ambush.TileX) ) {
				this.Ambushes[ ambush.TileX ] = new Dictionary<int, Ambush>();
			}

			this.Ambushes[ ambush.TileX ][ ambush.TileY ] = ambush;
		}
	}
}
