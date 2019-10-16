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
		private static Ambush CreateRandomWorldAmbush( int maxAttempts ) {
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
			return Ambush.CreateRandomType( randTileX, randTileY, edgeTiles );
		}



		////////////////

		internal void InitializeAmbushesAsync( int maxAmbushes ) {
			this.AmbushMngr.Clear();

			//Task.Factory.StartNew( () => {
			Task.Run( () => {
				for( int i = 0; i < maxAmbushes; i++ ) {
					this.CreateRandomWorldAmbushAsync();
					Thread.Sleep( AmbushesMod.Config.AmbushInitialGenerationSlowness );
				}
			} );
		}
		
		////////////////

		private void UpdateAmbushes( int maxAmbushes ) {
			if( this.AmbushRegenDelay++ < AmbushesMod.Config.AmbushRegenTickRate ) {
				return;
			}

			this.AmbushRegenDelay = 0;

			if( this.AmbushMngr.Count < maxAmbushes ) {
				this.CreateRandomWorldAmbushAsync();
			}
		}


		////////////////

		private void CreateRandomWorldAmbushAsync() {
			lock( AmbushesWorld.MyLock ) { }

			//var cts = new CancellationTokenSource();

			//Task.Factory.StartNew( () => {
			Task.Run( () => {
				lock( AmbushesWorld.MyLock ) {
					Ambush ambush = this.CreateNonNeighboringRandomWorldAmbush( 10000 );
					if( ambush != null ) {
						this.SpawnAmbush( ambush );
					}
				}
			} );
			//}, cts.Token );
		}


		////////////////

		private Ambush CreateNonNeighboringRandomWorldAmbush( int maxAttempts ) {
			int attempts = 0;

			do {
				Ambush ambush = AmbushesWorld.CreateRandomWorldAmbush( maxAttempts );
				if( ambush == null ) {
					continue;
				}

				if( !this.HasNearbyAmbushes(ambush.TileX, ambush.TileY) ) {
					return ambush;
				}
			} while( attempts++ < maxAttempts );

			return null;
		}


		private bool HasNearbyAmbushes( int tileX, int tileY ) {
			int minTileDist = AmbushesMod.Config.MinimumAmbushTileSpacing;
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
			if( AmbushesMod.Config.DebugModeInfo ) {
				LogHelpers.Log( "Created ambush as " + ambush.TileX + "," + ambush.TileY + " ("+this.AmbushMngr.Count+")" );
			}

			this.AmbushMngr.AddAmbush( ambush );
		}
	}
}
