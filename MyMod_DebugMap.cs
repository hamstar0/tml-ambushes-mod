using Terraria.ModLoader;
using Terraria;
using HamstarHelpers.Helpers.HUD;
using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using System;


namespace Ambushes {
	public partial class AmbushesMod : Mod {
		internal int LatestMinibossWho = -1;



		////////////////

		public override void PostDrawFullscreenMap( ref string mouseText ) {
			if( !AmbushesMod.Config.DebugModeMapCheat ) { return; }

			var myworld = ModContent.GetInstance<AmbushesWorld>();

			if( !AmbushManager.IsLocked ) {
				foreach( Ambush ambush in myworld.AmbushMngr.GetAllAmbushes() ) {
					this.DrawAmbushOnFullscreenMap( ambush.TileX, ambush.TileY, ambush );
				}
			}
		}

		////

		private void DrawAmbushOnFullscreenMap( int tileX, int tileY, Ambush ambush ) {
			var wldPos = new Vector2( tileX * 16, tileY * 16 );
			var overMapData = HUDMapHelpers.GetFullMapScreenPosition( wldPos );

			if( overMapData.Item2 ) {
				Main.spriteBatch.DrawString(
					Main.fontMouseText,
					"X",
					overMapData.Item1,
					Color.Red
				);
			}

			if( this.LatestMinibossWho != -1 ) {
				NPC npc = Main.npc[this.LatestMinibossWho];

				if( npc == null || !npc.active ) {
					this.LatestMinibossWho = -1;
				} else {
					Tuple<Vector2, bool> minibossPosData = HUDMapHelpers.GetFullMapScreenPosition( npc.position );

					if( minibossPosData.Item2 ) {
						Main.spriteBatch.DrawString(
							Main.fontMouseText,
							"X",
							minibossPosData.Item1,
							Color.Green
						);
					}
				}
			}
		}
	}
}