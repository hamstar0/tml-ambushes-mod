using Terraria.ModLoader;
using Terraria;
using HamstarHelpers.Helpers.HUD;
using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;


namespace Ambushes {
	public partial class AmbushesMod : Mod {
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
		}
	}
}