using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;


namespace Ambushes {
	abstract partial class Ambush {
		private void FlushDeadNPCs() {
			foreach( int npcWho in this.ClaimedNpcWhos.ToArray() ) {
				NPC npc = Main.npc[npcWho];

				if( npc == null || !npc.active ) {
					this.ClaimedNpcWhos.Remove( npcWho );
				}
			}
		}

		////

		public bool AttemptClaimNPC( NPC npc ) {
			int minDistSqr = 50 * 16;
			minDistSqr *= minDistSqr;

			var dist = new Vector2(
				npc.Center.X - ( this.TileX << 4 ),
				npc.Center.Y - ( this.TileY << 4 )
			);

			bool isNearby = dist.LengthSquared() < minDistSqr;
			if( isNearby ) {
				this.ClaimedNpcWhos.Add( npc.whoAmI );
			}

			return isNearby;
		}
	}
}
