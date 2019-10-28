using Ambushes.NetProtocols;
using Ambushes.Tiles;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Players;
using HamstarHelpers.Services.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace Ambushes {
	class AmbushesPlayer : ModPlayer {
		public override void OnEnterWorld( Player player ) {
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			if( Main.netMode == 1 ) {
				this.OnCurrentClientConnect();
			}
		}


		////////////////

		private void OnCurrentClientConnect() {
			AmbushesProtocol.QuickRequest();
		}


		////////////////

		public override void PreUpdate() {
			var myworld = ModContent.GetInstance<AmbushesWorld>();
			IList<Ambush> ambushes = myworld.AmbushMngr.GetAmbushesNear(
				(int)this.player.position.X >> 4,
				(int)this.player.position.Y >> 4
			);

			if( ambushes.Count > 0 ) {
				myworld.AmbushMngr.TriggerAmbush( ambushes.First(), this.player );
			}
		}


		public override void PreUpdateMovement() {
			bool enbrambled = false;
			int brambleType = ModContent.TileType<CursedBrambleTile>();
			int begX = (int)this.player.position.X;
			int endX = begX + this.player.width;

			for( int i = begX; i < endX; i += 16 ) {
				int begY = (int)this.player.position.Y;
				int endY = begY + this.player.height;

				for( int j = begY; j < endY; j += 16 ) {
					if( Framing.GetTileSafely(i >> 4, j >> 4).type == brambleType ) {
						enbrambled = true;
						break;
					}
				}
			}

			if( enbrambled ) {
				this.ApplyBrambleEffects();
			}
		}


		////////////////

		private void ApplyBrambleEffects() {
			string timerName = "AmbushesCursedBrambleHurt_" + this.player.whoAmI;
			
			if( this.player.velocity.LengthSquared() > 0.1f ) {
				this.player.velocity *= 1f - AmbushesMod.Config.BrambleStickiness;
			}
			
			if( Timers.GetTimerTickDuration( timerName ) <= 0 ) {
				Timers.SetTimer( timerName, AmbushesMod.Config.BrambleTicksPerDamage, () => {
					PlayerHelpers.RawHurt(
						player: this.player,
						deathReason: PlayerDeathReason.ByCustomReason( " was devoured by cursed brambles" ),
						damage: AmbushesMod.Config.BrambleDamage,
						direction: 0,
						pvp: false,
						quiet: true,
						crit: false
					);
					return false;
				} );

				this.player.AddBuff( BuffID.Venom, 60 );
			}
		}
	}
}
