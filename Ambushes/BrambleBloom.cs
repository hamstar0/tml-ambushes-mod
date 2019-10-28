using Ambushes.Tiles;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;


namespace Ambushes.Ambushes {
	class BrambleBloomAmbush : BrambleAmbush {
		private int CurrentReach = 0;
		private int Cooldown = 0;

		private IList<BrambleBloomTendril> Tendrils;


		////////////////

		public override float WorldGenWeight => AmbushesMod.Config.BrambleBloomAmbushPriorityWeight;

		public override bool PlaysMusic => false;



		////////////////

		private BrambleBloomAmbush() : base( 0, 0 ) {
		}

		protected BrambleBloomAmbush( int tileX, int tileY ) : base( tileX, tileY ) {
		}

		////

		protected override Ambush CloneRandomized( int tileX, int tileY ) {
			return new BrambleBloomAmbush( tileX, tileY );
		}


		////////////////

		protected override bool OnActivate( int clearTileX, int clearTileY ) {
			float deg;
			int tendrils = AmbushesMod.Config.BrambleBloomThickness;
			this.Tendrils = new List<BrambleBloomTendril>( tendrils );

			Main.NewText( "Run!", Color.Red );

			int tileY = this.TileY;
			Player plr = Main.player[this.TriggeringPlayer];

			if( plr != null && plr.active ) {
				tileY = this.SetToSafeStartPositionRelativeToPlayer( plr );
			}

			for( int i = 0; i < tendrils; i++ ) {
				deg = ( (float)i / (float)tendrils ) * 360f;

				this.Tendrils.Add( new BrambleBloomTendril( deg, this.TileX, tileY ) );
			}

			return true;
		}

		protected override void OnDeactivate() {
		}

		////////////////

		public override int GetBrambleDuration() {
			return 0;
		}


		////////////////

		protected override bool RunUntil() {
			if( this.CurrentReach < AmbushesMod.Config.BrambleBloomRadius ) {
				if( this.RunBloom() ) {
					this.CurrentReach++;
				}
				return false;
			}

			return base.RunUntil();
		}


		////

		private bool RunBloom() {
			if( this.Cooldown++ > 20 ) {
				this.Cooldown = 0;
			} else {
				return false;
			}

			int nearby;
			UnifiedRandom rand = TmlHelpers.SafelyGetRand();
			
			foreach( BrambleBloomTendril tendril in this.Tendrils.ToArray() ) {
				if( tendril.IsSplit() ) {
					this.Tendrils.Add( tendril.CloneAsSplit() );
				}

				(int x, int y) growAt = tendril.Grow();

				nearby = this.CountBramblesNear( growAt.x, growAt.y );
				if( nearby <= 1 || rand.Next(nearby) == 0 ) {
					CursedBrambleTile.CreateBrambleAt( growAt.x, growAt.y );
				}
			}

			return true;
		}


		////////////////

		private int SetToSafeStartPositionRelativeToPlayer( Player player ) {
			int newTileY = this.TileY;
			int safeDistance = 7;
			int plrTileY = (int)( player.Center.Y / 16 );
			int proximity = plrTileY - this.TileY;

			if( proximity >= 0 && proximity < safeDistance ) {
				newTileY = plrTileY + safeDistance;
			} else if( proximity < 0 && proximity > -safeDistance ) {
				newTileY = plrTileY - safeDistance;
			}

			return newTileY;
		}

		////////////////

		private int CountBramblesNear( int tileX, int tileY ) {
			int brambleType = ModContent.TileType<CursedBrambleTile>();
			int nearbyBrambles = 0;

			if( Framing.GetTileSafely( tileX - 1, tileY - 1 ).type == brambleType ) {
				nearbyBrambles++;
			}
			if( Framing.GetTileSafely( tileX, tileY - 1 ).type == brambleType ) {
				nearbyBrambles++;
			}
			if( Framing.GetTileSafely( tileX + 1, tileY - 1 ).type == brambleType ) {
				nearbyBrambles++;
			}
			if( Framing.GetTileSafely( tileX - 1, tileY ).type == brambleType ) {
				nearbyBrambles++;
			}
			if( Framing.GetTileSafely( tileX, tileY ).type == brambleType ) {
				nearbyBrambles++;
			}
			if( Framing.GetTileSafely( tileX + 1, tileY ).type == brambleType ) {
				nearbyBrambles++;
			}
			if( Framing.GetTileSafely( tileX - 1, tileY + 1 ).type == brambleType ) {
				nearbyBrambles++;
			}
			if( Framing.GetTileSafely( tileX, tileY + 1 ).type == brambleType ) {
				nearbyBrambles++;
			}
			if( Framing.GetTileSafely( tileX + 1, tileY + 1 ).type == brambleType ) {
				nearbyBrambles++;
			}

			return nearbyBrambles;
		}
	}
}
