using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using System;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;


namespace Ambushes {
	partial class AmbushesWorld : ModWorld {
		private static Ambush CreateRandomWorldAmbush( int maxAttempts ) {
			int attempts = 0;
			int randTileX, randTileY;
			UnifiedRandom rand = TmlHelpers.SafelyGetRand();
			
			do {
				randTileX = rand.Next( 64, Main.maxTilesX - 64 );
				randTileY = rand.Next( (int)Main.worldSurface, Main.maxTilesY - 220 );
				
				if( Ambush.CheckForAmbushElegibility( randTileX, randTileY ) ) {
					break;
				}
			} while( attempts++ < maxAttempts );
			
			if( attempts >= maxAttempts ) {
				return null;
			}
			
			Ambush.AdjustAmbushTileCenter( randTileX, ref randTileY );
			return Ambush.CreateRandomType( randTileX, randTileY );
		}



		////////////////

		internal void InitializeAmbushesAsync( int maxAmbushes ) {
			this.AmbushMngr.UnarmAllAmbushes();
			
			//Task.Factory.StartNew( () => {
			//Parallel.For( 0, maxAmbushes, ( i ) => {
			Task.Run( () => {
				int ambushCount = 0;

				for( int i = 0; i < maxAmbushes; i++ ) {
					if( this.CreateRandomWorldAmbush() ) {
						ambushCount++;
					}
					//this.CreateRandomWorldAmbushAsync();
					//Thread.Sleep( 10 );//AmbushesMod.Config.AmbushInitialGenerationSlowness
				}

				if( AmbushesMod.Config.DebugModeInfo ) {
					Main.NewText( "Initialization complete. "+ambushCount+" of "+maxAmbushes+" ambushes initialized." );
				}
			} );
		}

		////////////////

		private void UpdateAmbushesRegen( int maxAmbushes ) {
			if( this.AmbushRegenDelay++ >= AmbushesMod.Config.AmbushRegenTickRate ) {
				this.AmbushRegenDelay = 0;

				if( this.AmbushMngr.CountTotalAmbushes() < maxAmbushes ) {
					this.CreateRandomWorldAmbushAsync();
				}
			}
		}


		////////////////

		private void CreateRandomWorldAmbushAsync() {
			//var cts = new CancellationTokenSource();
			//Task.Factory.StartNew( () => {

			Task.Run( () => {
				this.CreateRandomWorldAmbush();
			} );
			//}, cts.Token );
		}
		
		private bool CreateRandomWorldAmbush() {
			lock( AmbushesWorld.MyLock ) {
				Ambush ambush = this.CreateNonNeighboringRandomWorldAmbush( 50, 10000 );

				if( ambush != null ) {
					this.SpawnAmbush( ambush );
					return true;
				}
			}
			return false;
		}


		////////////////

		private Ambush CreateNonNeighboringRandomWorldAmbush( int maxNonNeighborAttempts, int maxCreateAttempts ) {
			Ambush ambush = null;
			int attempts = 0;

			do {
				ambush = AmbushesWorld.CreateRandomWorldAmbush( maxCreateAttempts );
				if( ambush == null ) {
					continue;
				}

				if( !this.HasNearbyAmbushes(ambush.TileX, ambush.TileY) ) {
					break;
				}

				ambush = null;
			} while( attempts++ < maxNonNeighborAttempts );

			return ambush;
		}

		////

		private bool HasNearbyAmbushes( int tileX, int tileY ) {
			int minTileDist = AmbushesMod.Config.MinimumAmbushTileSeparation;
			int minTileDistSqt = minTileDist * minTileDist;
			bool found = false;
			
			foreach( Ambush ambush in this.AmbushMngr.GetAllAmbushes() ) {
				int xDist = ambush.TileX - tileX;
				int yDist = ambush.TileY - tileY;
				int xDistSqr = xDist * xDist;
				int yDistSqr = yDist * yDist;
				
				if( (xDistSqr + yDistSqr) < minTileDistSqt ) {
					found = true;
					break;
				}
			}

			return found;
		}


		////////////////

		private void SpawnAmbush( Ambush ambush ) {
			if( AmbushesMod.Config.DebugModeInfo ) {
				LogHelpers.Log( "Created ambush at " + ambush.TileX + "," + ambush.TileY + " ("+this.AmbushMngr.CountTotalAmbushes()+")" );
			}

			this.AmbushMngr.ArmAmbush( ambush );
		}
	}
}
