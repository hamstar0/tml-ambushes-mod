using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using System;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;


namespace Ambushes {
	partial class AmbushesWorld : ModWorld {
private static int _CRWA = 0;
		private static Ambush CreateRandomWorldAmbush( int maxAttempts ) {
			int attempts = 0;
			int randTileX, randTileY;
			UnifiedRandom rand = TmlHelpers.SafelyGetRand();
			
LogHelpers.Log("1111 "+_CRWA);
try {
			do {
				randTileX = rand.Next( 64, Main.maxTilesX - 64 );
				randTileY = rand.Next( (int)Main.worldSurface, Main.maxTilesY - 220 );
				
				if( Ambush.CheckForAmbushElegibility( randTileX, randTileY ) ) {
					break;
				}
			} while( attempts++ < maxAttempts );
} catch( Exception e ) { LogHelpers.Log( "2222 a - " + e.ToString() ); return null; }
LogHelpers.Log("2222b "+_CRWA+", "+attempts);
			
			if( attempts >= maxAttempts ) {
LogHelpers.Log("2222c "+_CRWA);
				return null;
			}
			
			Ambush.AdjustAmbushTileCenter( randTileX, ref randTileY );
LogHelpers.Log("3333 "+_CRWA+", "+randTileX+","+randTileY);
			Ambush ambush = Ambush.CreateRandomType( randTileX, randTileY );
LogHelpers.Log("4444 "+_CRWA+", "+randTileX+","+randTileY);
			return ambush;
		}



		////////////////

		internal void InitializeAmbushesAsync( int maxAmbushes ) {
			this.AmbushMngr.UnarmAllAmbushes();
			
			//Task.Factory.StartNew( () => {
			//Parallel.For( 0, maxAmbushes, ( i ) => {
			Task.Run( () => {
				for( int i = 0; i < maxAmbushes; i++ ) {
					try {
						this.CreateRandomWorldAmbush();
					} catch( Exception e ) {
						LogHelpers.Warn( e.ToString() );
					}
					//this.CreateRandomWorldAmbushAsync();
					//Thread.Sleep( 10 );//AmbushesMod.Config.AmbushInitialGenerationSlowness
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
				try {
					this.CreateRandomWorldAmbush();
				} catch( Exception e ) {
					LogHelpers.Warn( e.ToString() );
				}
			} );
			//}, cts.Token );
		}
		
		private void CreateRandomWorldAmbush() {
LogHelpers.Log("-1111 "+_CRWA);
			lock( AmbushesWorld.MyLock ) {
				Ambush ambush = this.CreateNonNeighboringRandomWorldAmbush( 50, 10000 );

				if( ambush != null ) {
					this.SpawnAmbush( ambush );
				}
			}
LogHelpers.Log("6666 "+_CRWA);
_CRWA++;
		}


		////////////////

		private Ambush CreateNonNeighboringRandomWorldAmbush( int maxNonNeighborAttempts, int maxCreateAttempts ) {
			Ambush ambush = null;
			int attempts = 0;

LogHelpers.Log("0000 "+_CRWA);
			do {
				ambush = AmbushesWorld.CreateRandomWorldAmbush( maxCreateAttempts );
				if( ambush == null ) {
					continue;
				}

				if( !this.HasNearbyAmbushes(ambush.TileX, ambush.TileY) ) {
					break;
				}
			} while( attempts++ < maxNonNeighborAttempts );
LogHelpers.Log("5555 "+_CRWA+" ambush?"+(ambush!=null));

			return ambush;
		}

		////

		private bool HasNearbyAmbushes( int tileX, int tileY ) {
			int minTileDist = AmbushesMod.Config.MinimumAmbushTileSeparation;
			int minTileDistSqt = minTileDist * minTileDist;
			bool found = false;
			
LogHelpers.Log("4444bb "+_CRWA);
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
LogHelpers.Log("4444c "+_CRWA);

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
