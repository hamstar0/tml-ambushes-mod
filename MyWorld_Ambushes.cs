using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;


namespace Ambushes {
	partial class AmbushesWorld : ModWorld {
		internal void InitializeAmbushesAsync( int maxAmbushes ) {
			this.AmbushMngr.Clear();

			var cts = new CancellationTokenSource();

			Task.Factory.StartNew( () => {
				for( int i = 0; i < maxAmbushes; i++ ) {
					this.CreateRandomAmbushAsync();
					Thread.Sleep( AmbushesMod.Instance.Config.AmbushInitialGenerationSlowness );
				}
			}, cts.Token );
		}
		
		////////////////

		private void UpdateAmbushes( int maxAmbushes ) {
			var mymod = (AmbushesMod)this.mod;

			if( this.AmbushRegenDelay++ < mymod.Config.AmbushRegenTickRate ) {
				return;
			}

			this.AmbushRegenDelay = 0;

			if( this.AmbushMngr.Count < maxAmbushes ) {
				this.CreateRandomAmbushAsync();
			}
		}


		////////////////

		private void CreateRandomAmbushAsync() {
			lock( AmbushesWorld.MyLock ) { }

			var cts = new CancellationTokenSource();

			Task.Factory.StartNew( () => {
				lock( AmbushesWorld.MyLock ) {
					Ambush ambush = this.CreateNonNeighboringRandomAmbush( 1000 );

					if( ambush != null ) {
						this.SpawnAmbush( ambush );
					}
				}
			}, cts.Token );
		}


		////////////////

		private Ambush CreateNonNeighboringRandomAmbush( int maxAttempts ) {
			int attempts = 0;

			do {
				Ambush ambush = this.CreateRandomAmbush( maxAttempts );

				if( !this.HasNearbyAmbushes(ambush.TileX, ambush.TileY) ) {
					return ambush;
				}
			} while( attempts++ < maxAttempts );

			return null;
		}


		private Ambush CreateRandomAmbush( int maxAttempts ) {
			int attempts = 0;
			int randTileX, randTileY;
			IDictionary<int, ISet<int>> edgeTiles;
			UnifiedRandom rand = TmlHelpers.SafelyGetRand();

			do {
				randTileX = rand.Next( 64, Main.maxTilesX - 64 );
				randTileY = rand.Next( (int)Main.worldSurface, Main.maxTilesY - 220 );

				if( Ambush.CheckForAmbushElegibility( randTileX, randTileY, out edgeTiles ) ) {
					break;
				}
			} while( attempts++ < maxAttempts );

			if( attempts >= maxAttempts ) {
				return null;
			}

			Ambush.AdjustAmbushTileCenter( randTileX, ref randTileY );
			return new Ambush( randTileX, randTileY, edgeTiles );
		}


		private bool HasNearbyAmbushes( int tileX, int tileY ) {
			int minTileDist = AmbushesMod.Instance.Config.MinimumAmbushTileSpacing;
			int minTileDistSqt = minTileDist * minTileDist;

			foreach( Ambush ambush in this.AmbushMngr.GetAllAmbushes() ) {
				int xDist = ambush.TileX - tileX;
				int yDist = ambush.TileY - tileY;
				int xDistSqr = xDist * xDist;
				int yDistSqr = yDist * yDist;

				if( (xDistSqr + yDistSqr) < minTileDistSqt ) {
					return true;
				}
			}

			return false;
		}


		////////////////

		private void SpawnAmbush( Ambush ambush ) {
			var mymod = AmbushesMod.Instance;
			if( mymod.Config.DebugModeInfo ) {
				LogHelpers.Log( "Created ambush as " + ambush.TileX + "," + ambush.TileY + " ("+this.AmbushMngr.Count+")" );
			}

			this.AmbushMngr.AddAmbush( ambush );
		}
	}
}
