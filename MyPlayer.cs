using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;


namespace Ambushes {
	class AmbushesPlayer : ModPlayer {
		public override void PreUpdate() {
			if( Main.netMode == 1 ) { return; }

			var myworld = ModContent.GetInstance<AmbushesWorld>();
			IList<Ambush> ambushes = myworld.AmbushMngr.GetAmbushesNear( (int)this.player.position.X >> 4, (int)this.player.position.Y >> 4 );

			if( ambushes.Count > 0 ) {
				ambushes.First().Trigger( this.player );
			}
		}
	}
}
